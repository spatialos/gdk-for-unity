using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Improbable.Worker.CInterop.Internal
{
    // Merge with /worker_sdk/csharp_cinterop/sdk/worker/internal/ParameterConversion.cs
    // when changes upstreamed to Worker SDK
    internal static class ParameterConversion
    {
        public unsafe delegate void ItemCallback(CEventTrace.Item* ptr);

        public delegate void EventParametersCallback(CEventTrace.Event internalEvent);

        public unsafe delegate void EventTracerParametersCallback(CEventTrace.EventTracerParameters* parameters, WrappedGcHandle[] handles);

        internal class WrappedGcHandle : CriticalFinalizerObject, IDisposable
        {
            private GCHandle handle;

            public WrappedGcHandle(object obj)
            {
                handle = GCHandle.Alloc(obj);
            }

            ~WrappedGcHandle()
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }

            public void Dispose()
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }

                GC.SuppressFinalize(this);
            }

            public IntPtr Get()
            {
                return GCHandle.ToIntPtr(handle);
            }
        }

        public static unsafe CEventTrace.SpanId ConvertSpanId(SpanId spanId)
        {
            var internalSpanId = new CEventTrace.SpanId();
            ApiInterop.Memcpy(internalSpanId.Data, spanId.Data, SpanId.SpanIdSize);
            return internalSpanId;
        }

        public static unsafe void ConvertEvent(Event eventToConvert, EventParametersCallback callback)
        {
            CEventTrace.Event internalEvent = new CEventTrace.Event();
            internalEvent.UnixTimestampMillis = eventToConvert.UnixTimestampMillis;
            internalEvent.Id = ConvertSpanId(eventToConvert.Id);

            if (eventToConvert.Data != null && !eventToConvert.Data.EventData.IsClosed)
            {
                internalEvent.Data = eventToConvert.Data.EventData.GetUnderlying();
            }

            fixed (byte* eventType = ApiInterop.ToUtf8Cstr(eventToConvert.Type))
            fixed (byte* eventMessage = ApiInterop.ToUtf8Cstr(eventToConvert.Message))
            {
                internalEvent.Type = eventType;
                internalEvent.Message = eventMessage;

                callback(internalEvent);
            }
        }

        public static unsafe Item ConvertItem(CEventTrace.Item* itemContainer)
        {
            var newItem = new Item();
            if (itemContainer->ItemType == 0)
            {
                // The item is newly initialized so return an empty Item
                return newItem;
            }

            newItem.ItemType = (ItemType) itemContainer->ItemType;
            switch (newItem.ItemType)
            {
                case ItemType.Span:
                    newItem.Span = new Span();

                    var newSpan = newItem.Span.Value;
                    ApiInterop.Memcpy(newSpan.Id.Data, itemContainer->ItemUnion.Span.Id.Data, SpanId.SpanIdSize);

                    newSpan.Causes = new SpanId[(int) itemContainer->ItemUnion.Span.CauseCount];
                    for (var i = 0; i < newSpan.Causes.Length; i++)
                    {
                        fixed (byte* spanIdDest = newSpan.Causes[i].Data)
                        {
                            ApiInterop.Memcpy(spanIdDest, itemContainer->ItemUnion.Span.Causes[i].Data, SpanId.SpanIdSize);
                        }
                    }

                    newItem.Span = newSpan;
                    break;
                case ItemType.Event:
                    newItem.Event = new Event();

                    var newEvent = newItem.Event.GetValueOrDefault();
                    newEvent.Id = new SpanId();
                    ApiInterop.Memcpy(newEvent.Id.Data, itemContainer->ItemUnion.Event.Id.Data, SpanId.SpanIdSize);

                    newEvent.UnixTimestampMillis = itemContainer->ItemUnion.Event.UnixTimestampMillis;
                    newEvent.Type = ApiInterop.FromUtf8Cstr(itemContainer->ItemUnion.Event.Type);
                    newEvent.Message = ApiInterop.FromUtf8Cstr(itemContainer->ItemUnion.Event.Message);

                    newEvent.Data = new TraceEventData(itemContainer->ItemUnion.Event.Data);
                    var fields = newEvent.Data.GetAll();
                    // Release memory allocated to the underlying event data in the itemContainer
                    newEvent.Data.EventData.Dispose();

                    // Add the data to the newly initialized event data struct
                    newEvent.Data = new TraceEventData();
                    newEvent.Data.AddAll(fields);

                    newItem.Event = newEvent;
                    break;
                default:
                    throw new NotSupportedException("Invalid Item Type provided.");
            }

            return newItem;
        }

        public static unsafe void ConvertItem(Item item, ItemCallback callback)
        {
            var newItem = Item.GetThreadLocalItem();

            newItem->ItemUnion = new CEventTrace.Item.Union();
            newItem->ItemType = (CEventTrace.ItemType) item.ItemType;
            switch (item.ItemType)
            {
                case ItemType.Span:
                    var spanItem = item.Span.Value;
                    newItem->ItemUnion.Span = new CEventTrace.Span();
                    newItem->ItemUnion.Span.Id = ConvertSpanId(spanItem.Id);
                    newItem->ItemUnion.Span.CauseCount = (uint) spanItem.Causes.Length;
                    var causesPointer = stackalloc CEventTrace.SpanId[spanItem.Causes.Length];
                    newItem->ItemUnion.Span.Causes = causesPointer;
                    for (var i = 0; i < newItem->ItemUnion.Span.CauseCount; i++)
                    {
                        newItem->ItemUnion.Span.Causes[i] = ConvertSpanId(spanItem.Causes[i]);
                    }

                    callback(newItem);

                    break;
                case ItemType.Event:
                    var eventItem = item.Event.Value;
                    ConvertEvent(eventItem, internalEvent =>
                    {
                        newItem->ItemUnion.Event = internalEvent;
                        callback(newItem);
                    });

                    break;
            }
        }

        public static unsafe void ConvertEventTracerParameters(EventTracerParameters[] parameters, EventTracerParametersCallback callback)
        {
            var internalParameters = ConvertTracerParameters(parameters, out var handles);

            fixed (CEventTrace.EventTracerParameters* parameterBuffer = internalParameters)
            {
                callback(parameterBuffer, handles);
            }
        }

        private static unsafe WrappedGcHandle ConvertTracerParameter(EventTracerParameters parameter, ref CEventTrace.EventTracerParameters internalParameters)
        {
            var wrappedParameterObject = new WrappedGcHandle(parameter);

            internalParameters.UserData = wrappedParameterObject.Get().ToPointer();
            internalParameters.TraceCallback = parameter.TraceCallback == null
                ? IntPtr.Zero
                : Marshal.GetFunctionPointerForDelegate(CallbackThunkDelegates.TraceCallbackThunkDelegate);

            return wrappedParameterObject;
        }

        private static CEventTrace.EventTracerParameters[] ConvertTracerParameters(EventTracerParameters[] parameters, out WrappedGcHandle[] handles)
        {
            handles = new WrappedGcHandle[parameters.Length];
            var internalParameters = new CEventTrace.EventTracerParameters[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterHandle = ConvertTracerParameter(parameters[i], ref internalParameters[i]);
                if (parameterHandle != null)
                {
                    handles[i] = parameterHandle;
                }
            }

            return internalParameters;
        }

        private unsafe class CallbackThunkDelegates
        {
            public static readonly CEventTrace.TraceCallback TraceCallbackThunkDelegate = TraceCallbackThunk;

            [MonoPInvokeCallback(typeof(CEventTrace.TraceCallback))]
            private static void TraceCallbackThunk(void* callbackPtr, CEventTrace.Item* responseItem)
            {
                var callbackHandle = (EventTracerParameters) GCHandle.FromIntPtr((IntPtr) callbackPtr).Target;
                callbackHandle.TraceCallback(callbackHandle.UserData, ConvertItem(responseItem));
            }
        }
    }
}
