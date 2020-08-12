using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public struct SpanId
    {
        public unsafe fixed byte Data[16];

        public override bool Equals(object obj)
        {
            if (!(obj is SpanId))
            {
                return false;
            }

            return CEventTrace.SpanIdEqual(
                ParameterConversion.ConvertSpanId(this),
                ParameterConversion.ConvertSpanId((SpanId) obj)) > 0;
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
                ApiInterop.Memcpy(nativeSpanId.Data, nullSpanId.Data, (UIntPtr) 16);
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


        public void AddItemToStorage(IOStorage storage, Item item)
        {
            unsafe
            {
                var traceItem = ConvertItem(storage, item);
                CEventTrace.ItemCreate(storage.Storage, traceItem);
            }
        }

        public void SerializeItemToStream(IOStorage storage, IOStream stream, Item item)
        {
            unsafe
            {
                var nativeItem = ConvertItem(storage, item);
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

        public long GetSerializedItemSizeInBytes(IOStorage storage, Item item)
        {
            unsafe
            {
                var itemSize = CEventTrace.GetSerializedItemSize(ConvertItem(storage, item));
                if (itemSize != 0)
                {
                    return itemSize;
                }

                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }
        }

        public Item DeserializeNextItemFromStream(IOStorage storage, IOStream stream)
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
                    ApiInterop.Memcpy(newSpan.Id.Data, itemContainer->ItemUnion.Span.Id.Data, (UIntPtr) 16);

                    newSpan.CauseCount = (int) itemContainer->ItemUnion.Span.CauseCount;
                    for (var i = 0; i < newSpan.CauseCount; i++)
                    {
                        newSpan.Causes[i] = new SpanId();
                        fixed (byte* spanIdDest = newSpan.Causes[i].Data)
                        {
                            ApiInterop.Memcpy(spanIdDest, itemContainer->ItemUnion.Span.Causes[i].Data, (UIntPtr) 16);
                        }
                    }

                    break;
                case ItemType.Event:
                    newItem.Event = new Event();

                    var newEvent = newItem.Event.GetValueOrDefault();
                    newEvent.Id = new SpanId();
                    ApiInterop.Memcpy(newEvent.Id.Data, itemContainer->ItemUnion.Event.Id.Data, (UIntPtr) 16);

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
                    newItem->ItemUnion.Event = ParameterConversion.ConvertEvent(eventItem);
                    break;
                default:
                    throw new NotSupportedException("Invalid Item Type provided.");
            }

            return newItem;
        }
    }

    public unsafe class EventTracer : IDisposable
    {
        private readonly CEventTrace.EventTracer eventTracer;

        public bool IsEnabled { get; private set; }

        public EventTracer(EventTracerParameters[] parameters)
        {
            ParameterConversion.ConvertEventTracer(parameters, out var tracerParameters);

            fixed (CEventTrace.EventTracerParameters* fixedParameters = tracerParameters)
            {
                eventTracer = CEventTrace.EventTracerCreate(fixedParameters);
            }
        }

        public void Dispose()
        {
            eventTracer.Dispose();
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
            ApiInterop.Memcpy(spanId.Data, nativeSpanId.Data, (UIntPtr) 16);
            return spanId;
        }

        public void AddSpanToCauses(int causeCount, SpanId[] causes)
        {
            var createdSpan = CEventTrace.EventTracerAddSpan(eventTracer, null, (uint) causeCount);
        }

        public void AddEvent(Event @event)
        {
            CEventTrace.EventTracerAddEvent(eventTracer, ParameterConversion.ConvertEvent(@event));
        }

        public bool ShouldSampleEvent(Event @event)
        {
            return CEventTrace.EventTracerShouldSampleEvent(eventTracer, ParameterConversion.ConvertEvent(@event)) > 0;
        }
    }

    public unsafe class TraceEventData : IDisposable
    {
        internal GCHandle eventData;

        public TraceEventData()
        {
            var internalEventData = CEventTrace.EventDataCreate();
            eventData = GCHandle.Alloc(internalEventData, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            if (eventData.IsAllocated)
            {
                eventData.Free();
            }
        }

        public void AddFields(IEnumerable<KeyValuePair<string, string>> fields)
        {
            foreach (var kvp in fields)
            {
                fixed (byte* key = ApiInterop.ToUtf8Cstr(kvp.Key))
                fixed (byte* value = ApiInterop.ToUtf8Cstr(kvp.Value))
                {
                    CEventTrace.EventDataAddStringFields(eventData.AddrOfPinnedObject(), 1, &key, &value);
                }
            }
        }

        public Dictionary<string, string> GetAllFields()
        {
            var numberOfFields = CEventTrace.EventDataGetFieldCount(eventData.AddrOfPinnedObject());
            var nativeKeys = new byte*[numberOfFields];
            var nativeValues = new byte*[numberOfFields];

            var fields = new Dictionary<string, string>();
            fixed (byte** keys = nativeKeys)
            fixed (byte** values = nativeValues)
            {
                CEventTrace.EventDataGetStringFields(eventData.AddrOfPinnedObject(), keys, values);

                for (var i = 0; i < numberOfFields; i++)
                {
                    fields.Add(ApiInterop.FromUtf8Cstr(nativeKeys[i]), ApiInterop.FromUtf8Cstr(nativeValues[i]));
                }
            }

            return fields;
        }

        public string GetFieldValue(string key)
        {
            byte* value;
            fixed (byte* nativeKey = ApiInterop.ToUtf8Cstr(key))
            {
                value = CEventTrace.EventDataGetFieldValue(eventData.AddrOfPinnedObject(), nativeKey);
            }

            return ApiInterop.FromUtf8Cstr(value);
        }
    }

    public delegate void TraceCallback(object userData, Item item);

    public class EventTracerParameters
    {
        public TraceCallback TraceCallback;
        public object UserData;
    }
}
