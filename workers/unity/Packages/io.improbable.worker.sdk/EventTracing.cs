using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public interface ITraceItem
    {
    }

    public unsafe struct SpanId
    {
        public fixed byte Data[16];
    }

    public struct Span : ITraceItem
    {
        public SpanId Id;
        public int CauseCount;
        public SpanId[] Causes;
    }

    public struct Event : ITraceItem
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
        public ITraceItem TraceItem;
    }

    public static class TraceSpan
    {
        public static byte SpanIdHash(SpanId spanId)
        {
            return CEventTrace.SpanIdHash(ParameterConversion.ConvertSpanId(spanId));
        }

        public static bool SpanIdEqual(SpanId spanId1, SpanId spanId2)
        {
            return CEventTrace.SpanIdEqual(
                ParameterConversion.ConvertSpanId(spanId1),
                ParameterConversion.ConvertSpanId(spanId2)) > 0;
        }

        public static unsafe SpanId GetNullSpanId()
        {
            var nativeSpanId = new SpanId();
            var nullSpanId = CEventTrace.SpanIdNull();
            ApiInterop.Memcpy(nativeSpanId.Data, nullSpanId.Data, (UIntPtr) 16);
            return nativeSpanId;
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

        public void AddCollectionOfFields(IEnumerable<KeyValuePair<string, string>> fields)
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

            fixed (byte** keys = nativeKeys)
            fixed (byte** values = nativeValues)
            {
                CEventTrace.EventDataGetStringFields(eventData.AddrOfPinnedObject(), keys, values);
            }

            var fields = new Dictionary<string, string>();
            for (var i = 0; i < numberOfFields; i++)
            {
                fields.Add(ApiInterop.FromUtf8Cstr(nativeKeys[i]), ApiInterop.FromUtf8Cstr(nativeValues[i]));
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

    public unsafe class TraceItem
    {
        private readonly CIO.StorageHandle storage;
        private readonly CIO.StreamHandle stream;

        public TraceItem(IOStorage storageWrapper, IOStream streamWrapper)
        {
            storage = storageWrapper.Storage;
            stream = streamWrapper.Stream;
        }

        public void AddItem(Item item)
        {
            var traceItem = ConvertItem(item);
            CEventTrace.ItemCreate(storage, traceItem);
        }

        public void SerializeItemToStream(Item item)
        {
            var nativeItem = ConvertItem(item);
            var itemSize = CEventTrace.GetSerializedItemSize(nativeItem);

            var serializedItemSize = CEventTrace.SerializeItemToStream(stream, nativeItem, itemSize);
            if (serializedItemSize == 1)
            {
                return;
            }

            var errorMessage = CEventTrace.GetLastError();
            throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
        }

        public long GetSerializedItemSizeInBytes(Item item)
        {
            var itemSize = CEventTrace.GetSerializedItemSize(ConvertItem(item));
            if (itemSize != 0)
            {
                return itemSize;
            }

            var errorMessage = CEventTrace.GetLastError();
            throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
        }

        public Item DeserializeNextItemFromStream()
        {
            var itemContainer = CEventTrace.ItemCreate(storage, null);
            var itemSize = CEventTrace.GetNextSerializedItemSize(stream);

            var deserializeStatus = CEventTrace.DeserializeItemFromStream(stream, itemContainer, itemSize);
            if (deserializeStatus != 1)
            {
                var errorMessage = CEventTrace.GetLastError();
                throw new IOException(ApiInterop.FromUtf8Cstr(errorMessage));
            }

            var newItem = new Item();
            switch (itemContainer->ItemType)
            {
                case (byte) ItemType.Span:
                    newItem.ItemType = ItemType.Span;
                    newItem.TraceItem = new Span();
                    var newSpan = (Span) newItem.TraceItem;

                    newSpan.Id = new SpanId();
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
                case (byte) ItemType.Event:
                    newItem.ItemType = ItemType.Event;
                    newItem.TraceItem = new Event();
                    var newEvent = (Event) newItem.TraceItem;

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

        private CEventTrace.Item* ConvertItem(Item item)
        {
            var newItem = CEventTrace.ItemCreate(storage, null);
            newItem->ItemUnion = new CEventTrace.Item.Union();

            switch (item.ItemType)
            {
                case ItemType.Span:
                    var spanItem = (Span) item.TraceItem;
                    newItem->ItemUnion.Span = new CEventTrace.Span();
                    newItem->ItemUnion.Span.Id = ParameterConversion.ConvertSpanId(spanItem.Id);
                    newItem->ItemUnion.Span.CauseCount = (uint) spanItem.CauseCount;
                    for (var i = 0; i < newItem->ItemUnion.Span.CauseCount; i++)
                    {
                        newItem->ItemUnion.Span.Causes[i] = ParameterConversion.ConvertSpanId(spanItem.Causes[i]);
                    }

                    break;
                case ItemType.Event:
                    var eventItem = (Event) item.TraceItem;
                    newItem->ItemUnion.Event = ParameterConversion.ConvertEvent(eventItem);
                    break;
                default:
                    throw new NotSupportedException("Invalid Item Type provided.");
            }

            newItem->ItemType = (byte) item.ItemType;

            return newItem;
        }
    }

    public unsafe delegate void TraceCallback(
        UIntPtr userData, IntPtr item);

    public class EventTracerParameters
    {
        public TraceCallback TraceCallback;
        public UIntPtr UserData;
    }
}
