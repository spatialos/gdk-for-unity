using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public struct SpanId : IEquatable<SpanId>
    {
        internal static UIntPtr SpanIdSize = new UIntPtr(16);
        public unsafe fixed byte Data[16];

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

        public static SpanId GetNullSpanId()
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
        public int CauseCount;
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

        public void AddToStorage(IOStorage storage, Item? item = null)
        {
            unsafe
            {
                var traceItem = ConvertItem(storage, item ?? this);
                CEventTrace.ItemCreate(storage.Storage, traceItem);
            }
        }

        public void SerializeToStream(IOStorage storage, IOStream stream)
        {
            unsafe
            {
                var nativeItem = ConvertItem(storage, this);
                var itemSize = CEventTrace.GetSerializedItemSize(nativeItem);

                var serializedItemSize = CEventTrace.SerializeItemToStream(stream.Stream, nativeItem, itemSize);
                if (serializedItemSize == 1)
                {
                    return;
                }

                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }
        }

        private long GetSerializedSizeInBytes(IOStorage storage)
        {
            unsafe
            {
                var itemSize = CEventTrace.GetSerializedItemSize(ConvertItem(storage, this));
                if (itemSize != 0)
                {
                    return itemSize;
                }

                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }
        }

        public static Item DeserializeNextItemFromStream(IOStorage storage, IOStream stream)
        {
            unsafe
            {
                var itemContainer = CEventTrace.ItemCreate(storage.Storage, null);
                var itemSize = CEventTrace.GetNextSerializedItemSize(stream.Stream);

                var deserializeStatus = CEventTrace.DeserializeItemFromStream(stream.Stream, itemContainer, itemSize);
                if (deserializeStatus != 1)
                {
                    var errorMessage = CEventTrace.GetLastError();
                    throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
                }

                return ConvertItem(itemContainer);
            }
        }

        internal static unsafe Item ConvertItem(CEventTrace.Item* itemContainer)
        {
            var newItem = new Item();
            newItem.ItemType = (ItemType) itemContainer->ItemType;
            switch (newItem.ItemType)
            {
                case ItemType.Span:
                    newItem.Span = new Span();

                    var newSpan = newItem.Span.GetValueOrDefault();
                    ApiInterop.Memcpy(newSpan.Id.Data, itemContainer->ItemUnion.Span.Id.Data, SpanId.SpanIdSize);

                    newSpan.CauseCount = (int) itemContainer->ItemUnion.Span.CauseCount;
                    for (var i = 0; i < newSpan.CauseCount; i++)
                    {
                        newSpan.Causes[i] = new SpanId();
                        fixed (byte* spanIdDest = newSpan.Causes[i].Data)
                        {
                            ApiInterop.Memcpy(spanIdDest, itemContainer->ItemUnion.Span.Causes[i].Data, SpanId.SpanIdSize);
                        }
                    }

                    break;
                case ItemType.Event:
                    newItem.Event = new Event();

                    var newEvent = newItem.Event.GetValueOrDefault();
                    newEvent.Id = new SpanId();
                    ApiInterop.Memcpy(newEvent.Id.Data, itemContainer->ItemUnion.Event.Id.Data, SpanId.SpanIdSize);

                    newEvent.UnixTimestampMillis = itemContainer->ItemUnion.Event.UnixTimestampMillis;
                    newEvent.Type = ApiInterop.FromUtf8Cstr(itemContainer->ItemUnion.Event.Type);
                    newEvent.Message = ApiInterop.FromUtf8Cstr(itemContainer->ItemUnion.Event.Message);

                    newEvent.Data = Marshal.PtrToStructure<TraceEventData>(itemContainer->ItemUnion.Event.Data);
                    break;
                default:
                    throw new NotSupportedException("Invalid Item Type provided.");
            }

            return newItem;
        }

        internal static unsafe CEventTrace.Item* ConvertItem(IOStorage storage, Item item)
        {
            var newItem = CEventTrace.ItemCreate(storage.Storage, null);
            newItem->ItemUnion = new CEventTrace.Item.Union();
            newItem->ItemType = (CEventTrace.ItemType) item.ItemType;

            switch (item.ItemType)
            {
                case ItemType.Span:
                    var spanItem = item.Span.GetValueOrDefault();
                    newItem->ItemUnion.Span = new CEventTrace.Span();
                    newItem->ItemUnion.Span.Id = ParameterConversion.ConvertSpanId(spanItem.Id);
                    newItem->ItemUnion.Span.CauseCount = (uint) spanItem.CauseCount;
                    for (var i = 0; i < newItem->ItemUnion.Span.CauseCount; i++)
                    {
                        newItem->ItemUnion.Span.Causes[i] = ParameterConversion.ConvertSpanId(spanItem.Causes[i]);
                    }

                    break;
                case ItemType.Event:
                    var eventItem = item.Event.GetValueOrDefault();
                    ParameterConversion.ConvertEvent(eventItem, internalEvent =>
                    {
                        newItem->ItemUnion.Event = internalEvent;
                    });

                    break;
                default:
                    throw new NotSupportedException("Invalid Item Type provided.");
            }

            return newItem;
        }
    }

    public class EventTracer : IDisposable
    {
        private CEventTrace.EventTracer eventTracer;
        private List<ParameterConversion.WrappedGcHandle> handleList;

        public bool IsEnabled { get; private set; }

        public EventTracer(EventTracerParameters[] parameters)
        {
            unsafe
            {
                ParameterConversion.ConvertEventTracer(parameters, (internalParameters, handles) =>
                {
                    eventTracer = CEventTrace.EventTracerCreate(internalParameters);
                    handleList = handles;
                });
            }
        }


        public void Dispose()
        {
            eventTracer.Dispose();
            handleList.ForEach(handle => handle.Dispose());
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

        public void ClearActiveSpanId(SpanId spanId)
        {
            // Uncomment once Worker SDK updated to 14.8.0 (version where EventTracerClearActiveSpanId is added)
            // CEventTrace.EventTracerClearActiveSpanId(eventTracer, ParameterConversion.ConvertSpanId(spanId));
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

        public SpanId AddSpan()
        {
            var newSpanId = new SpanId();
            unsafe
            {
                var createdSpanId = CEventTrace.EventTracerAddSpan(eventTracer, null, 1);
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
        internal CEventTrace.EventData eventData;

        public TraceEventData()
        {
            eventData = CEventTrace.EventDataCreate();
        }

        public void AddFields(IEnumerable<KeyValuePair<string, string>> fields)
        {
            unsafe
            {
                foreach (var kvp in fields)
                {
                    fixed (byte* key = ApiInterop.ToUtf8Cstr(kvp.Key))
                    fixed (byte* value = ApiInterop.ToUtf8Cstr(kvp.Value))
                    {
                        CEventTrace.EventDataAddStringFields(eventData, 1, &key, &value);
                    }
                }
            }
        }

        public Dictionary<string, string> GetAllFields()
        {
            unsafe
            {
                var numberOfFields = CEventTrace.EventDataGetFieldCount(eventData);
                var nativeKeys = new byte*[numberOfFields];
                var nativeValues = new byte*[numberOfFields];

                var fields = new Dictionary<string, string>();
                fixed (byte** keys = nativeKeys)
                fixed (byte** values = nativeValues)
                {
                    CEventTrace.EventDataGetStringFields(eventData, keys, values);

                    for (var i = 0; i < numberOfFields; i++)
                    {
                        fields.Add(ApiInterop.FromUtf8Cstr(nativeKeys[i]), ApiInterop.FromUtf8Cstr(nativeValues[i]));
                    }
                }

                return fields;
            }
        }

        public string GetFieldValue(string key)
        {
            unsafe
            {
                byte* value;
                fixed (byte* nativeKey = ApiInterop.ToUtf8Cstr(key))
                {
                    value = CEventTrace.EventDataGetFieldValue(eventData, nativeKey);
                }

                return ApiInterop.FromUtf8Cstr(value);
            }
        }
    }

    public delegate void TraceCallback(object userData, Item item);

    public class EventTracerParameters
    {
        public TraceCallback TraceCallback;
        public object UserData;
    }
}
