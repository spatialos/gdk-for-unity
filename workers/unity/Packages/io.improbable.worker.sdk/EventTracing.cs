using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public struct SpanId : IEquatable<SpanId>
    {
        internal static UIntPtr SpanIdSize = new UIntPtr(16);
        public unsafe fixed byte Data[16];

        public static SpanId Null => GetNullSpanId();

        public override bool Equals(object obj)
        {
            return obj is SpanId && Equals(obj);
        }

        public bool Equals(SpanId spanId)
        {
            return CEventTrace.SpanIdEqual(
                ParameterConversion.ConvertSpanId(this),
                ParameterConversion.ConvertSpanId(spanId)) > 0;
        }

        public static bool operator ==(SpanId lhs, SpanId rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SpanId lhs, SpanId rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return CEventTrace.SpanIdHash(ParameterConversion.ConvertSpanId(this));
        }

        private static SpanId GetNullSpanId()
        {
            var nativeSpanId = new SpanId();
            var nullSpanId = CEventTrace.SpanIdNull();
            unsafe
            {
                ApiInterop.Memcpy(nativeSpanId.Data, nullSpanId.Data, SpanIdSize);
            }

            return nativeSpanId;
        }
    }

    public struct Span
    {
        public SpanId Id;
        public SpanId[] Causes;
    }

    public struct Event
    {
        public SpanId Id;
        public ulong UnixTimestampMillis;
        public string Message;
        public string Type;
        public TraceEventData Data;
    }

    public enum ItemType
    {
        Span = 1,
        Event = 2
    }

    public struct Item
    {
        public ItemType ItemType;
        public Span? Span;
        public Event? Event;

        public static Item Create(IOStorage storage, Item? itemToConvert = null)
        {
            unsafe
            {
                var newItem = new Item();
                if (itemToConvert != null)
                {
                    ParameterConversion.ConvertItem(itemToConvert.Value, nativeItem =>
                    {
                        newItem = ParameterConversion.ConvertItem(CEventTrace.ItemCreate(storage.Storage, nativeItem));
                    });
                }
                else
                {
                    newItem = ParameterConversion.ConvertItem(CEventTrace.ItemCreate(storage.Storage, null));
                }

                return newItem;
            }
        }

        public void SerializeToStream(IOStream stream)
        {
            unsafe
            {
                var serializedItemResult = 0;
                ParameterConversion.ConvertItem(this, nativeItem =>
                {
                    var itemSize = CEventTrace.GetSerializedItemSize(nativeItem);
                    serializedItemResult = CEventTrace.SerializeItemToStream(stream.Stream, nativeItem, itemSize);
                });

                if (serializedItemResult == 1)
                {
                    return;
                }

                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }
        }

        private long GetSerializedSizeInBytes()
        {
            unsafe
            {
                var itemSize = 0L;
                ParameterConversion.ConvertItem(this, nativeItem =>
                {
                    itemSize = CEventTrace.GetSerializedItemSize(nativeItem);
                });

                if (itemSize != 0)
                {
                    return itemSize;
                }

                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }
        }

        public static Item DeserializeNextItemFromStream(IOStream stream)
        {
            unsafe
            {
                var itemContainer = GetThreadLocalItem();
                var itemSize = CEventTrace.GetNextSerializedItemSize(stream.Stream);

                var deserializeStatus = CEventTrace.DeserializeItemFromStream(stream.Stream, itemContainer, itemSize);
                if (deserializeStatus != 1)
                {
                    var errorMessage = CEventTrace.GetLastError();
                    throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
                }

                return ParameterConversion.ConvertItem(itemContainer);
            }
        }

        internal static unsafe CEventTrace.Item* GetThreadLocalItem()
        {
            var item = CEventTrace.ItemGetThreadLocal();
            switch (item->ItemType)
            {
                case CEventTrace.ItemType.Span:
                    item->ItemUnion.Span.Id = CEventTrace.SpanIdNull();
                    item->ItemUnion.Span.Causes = null;
                    item->ItemUnion.Span.CauseCount = 0;
                    break;
                case CEventTrace.ItemType.Event:
                    item->ItemUnion.Event.Data = IntPtr.Zero;
                    item->ItemUnion.Event.Id = CEventTrace.SpanIdNull();
                    item->ItemUnion.Event.Message = null;
                    item->ItemUnion.Event.Type = null;
                    break;
            }

            item->ItemType = 0;
            return item;
        }
    }

    public class EventTracer : IDisposable
    {
        private CEventTrace.EventTracer eventTracer;
        private IList<ParameterConversion.WrappedGcHandle> handleList;

        public bool IsEnabled { get; private set; }

        public EventTracer(EventTracerParameters[] parameters)
        {
            unsafe
            {
                ParameterConversion.ConvertEventTracerParameters(parameters, (internalParameters, handles) =>
                {
                    eventTracer = CEventTrace.EventTracerCreate(internalParameters);
                    handleList = handles;
                });
            }
        }

        public void Dispose()
        {
            eventTracer.Dispose();
            foreach (var handle in handleList)
            {
                handle.Dispose();
            }
        }

        public void Enable()
        {
            CEventTrace.EventTracerEnable(eventTracer);
            IsEnabled = true;
        }

        public void Disable()
        {
            CEventTrace.EventTracerDisable(eventTracer);
            IsEnabled = false;
        }

        public void SetActiveSpanId(SpanId spanId)
        {
            CEventTrace.EventTracerSetActiveSpanId(eventTracer, ParameterConversion.ConvertSpanId(spanId));
        }

        public void ClearActiveSpanId()
        {
            CEventTrace.EventTracerClearActiveSpanId(eventTracer);
        }

        public SpanId GetActiveSpanId()
        {
            var nativeSpanId = CEventTrace.EventTracerGetActiveSpanId(eventTracer);
            var spanId = new SpanId();
            unsafe
            {
                ApiInterop.Memcpy(spanId.Data, nativeSpanId.Data, SpanId.SpanIdSize);
            }

            return spanId;
        }

        // Returns the SpanId of the newly-created Span in the EventTracer
        public SpanId AddSpan(SpanId[] causes)
        {
            unsafe
            {
                if (causes == null)
                {
                    return AddSpan();
                }

                var causeIds = new CEventTrace.SpanId[causes.Length];
                for (var i = 0; i < causes.Length; i++)
                {
                    causeIds[i] = ParameterConversion.ConvertSpanId(causes[i]);
                }

                CEventTrace.SpanId createdSpanId;
                fixed (CEventTrace.SpanId* fixedCauseIds = causeIds)
                {
                    createdSpanId = CEventTrace.EventTracerAddSpan(eventTracer, fixedCauseIds, (uint) causeIds.Length);
                }

                var newSpanId = new SpanId();
                ApiInterop.Memcpy(newSpanId.Data, createdSpanId.Data, SpanId.SpanIdSize);

                return newSpanId;
            }
        }

        public SpanId AddSpan(SpanId spanId)
        {
            var newSpanId = new SpanId();
            unsafe
            {
                var internalSpanId = ParameterConversion.ConvertSpanId(spanId);
                var createdSpanId = CEventTrace.EventTracerAddSpan(eventTracer, &internalSpanId, 1);
                ApiInterop.Memcpy(newSpanId.Data, createdSpanId.Data, SpanId.SpanIdSize);
            }

            return newSpanId;
        }

        public SpanId AddSpan()
        {
            var newSpanId = new SpanId();
            unsafe
            {
                var createdSpanId = CEventTrace.EventTracerAddSpan(eventTracer, null, 0);
                ApiInterop.Memcpy(newSpanId.Data, createdSpanId.Data, SpanId.SpanIdSize);
            }

            return newSpanId;
        }

        public void AddEvent(Event @event)
        {
            ParameterConversion.ConvertEvent(@event, internalEvent =>
            {
                CEventTrace.EventTracerAddEvent(eventTracer, internalEvent);
            });
        }

        public bool ShouldSampleEvent(Event @event)
        {
            bool shouldSampleEvent = false;
            ParameterConversion.ConvertEvent(@event, internalEvent =>
            {
                shouldSampleEvent = CEventTrace.EventTracerShouldSampleEvent(eventTracer, internalEvent) > 0;
            });

            return shouldSampleEvent;
        }
    }

    public class TraceEventData
    {
        internal readonly CEventTrace.EventDataHandle EventData;

        internal TraceEventData(IntPtr handle)
        {
            EventData = new CEventTrace.EventDataHandle(handle);
        }

        public TraceEventData()
        {
            EventData = CEventTrace.EventDataCreate();
        }

        public string this[string key]
        {
            get => GetValue(key);
            set => AddField(key, value);
        }

        public int Count => GetFieldCount();

        public void AddAll(IEnumerable<KeyValuePair<string, string>> fields)
        {
            foreach (var kvp in fields)
            {
                AddField(kvp.Key, kvp.Value);
            }
        }

        public Dictionary<string, string> GetAll()
        {
            unsafe
            {
                var numberOfFields = CEventTrace.EventDataGetFieldCount(EventData);
                var nativeKeys = new byte*[numberOfFields];
                var nativeValues = new byte*[numberOfFields];

                var fields = new Dictionary<string, string>();
                fixed (byte** keys = nativeKeys)
                fixed (byte** values = nativeValues)
                {
                    CEventTrace.EventDataGetStringFields(EventData, keys, values);

                    for (var i = 0; i < numberOfFields; i++)
                    {
                        fields.Add(ApiInterop.FromUtf8Cstr(nativeKeys[i]), ApiInterop.FromUtf8Cstr(nativeValues[i]));
                    }
                }

                return fields;
            }
        }

        private void AddField(string key, string value)
        {
            unsafe
            {
                fixed (byte* keyPointer = ApiInterop.ToUtf8Cstr(key))
                fixed (byte* valuePointer = ApiInterop.ToUtf8Cstr(value))
                {
                    CEventTrace.EventDataAddStringFields(EventData, 1, &keyPointer, &valuePointer);
                }
            }
        }

        private string GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            unsafe
            {
                byte* valuePointer;
                fixed (byte* keyPointer = ApiInterop.ToUtf8Cstr(key))
                {
                    valuePointer = CEventTrace.EventDataGetFieldValue(EventData, keyPointer);
                }

                var value = ApiInterop.FromUtf8Cstr(valuePointer);
                if (value.Equals("null"))
                {
                    throw new KeyNotFoundException($"Key '{key}' is not found.");
                }

                return value;
            }
        }

        private int GetFieldCount()
        {
            return (int) CEventTrace.EventDataGetFieldCount(EventData);
        }
    }

    public delegate void TraceCallback(object userData, Item item);

    public class EventTracerParameters
    {
        public TraceCallback TraceCallback;
        public object UserData;
    }
}
