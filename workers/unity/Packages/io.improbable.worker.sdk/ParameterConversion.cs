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
        public delegate void EventParametersCallback(CEventTrace.Event internalEvent);

        public unsafe delegate void EventTracerParametersCallback(CEventTrace.EventTracerParameters* parameters, List<WrappedGcHandle> handles);

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

            if (eventToConvert.Data.EventData != null && !eventToConvert.Data.EventData.IsClosed)
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

        public static unsafe void ConvertEventTracerParameters(EventTracerParameters[] parameters, EventTracerParametersCallback callback)
        {
            var handles = new List<WrappedGcHandle>(parameters.Length);
            ConvertTracerParameters(handles, parameters, out var internalParameters);

            fixed (CEventTrace.EventTracerParameters* parameterBuffer = internalParameters)
            {
                callback(parameterBuffer, handles);
            }
        }

        private static unsafe WrappedGcHandle ConvertTracerParameter(EventTracerParameters parameter, ref CEventTrace.EventTracerParameters internalParameters)
        {
            WrappedGcHandle wrappedParameterObject = new WrappedGcHandle(parameter);

            internalParameters.UserData = (void*) wrappedParameterObject.Get();
            internalParameters.TraceCallback = parameter.TraceCallback == null
                ? IntPtr.Zero
                : Marshal.GetFunctionPointerForDelegate(CallbackThunkDelegates.TraceCallbackThunkDelegate);

            return wrappedParameterObject;
        }

        public static void FreeTracerParameterHandles(IEnumerable<WrappedGcHandle> tracerParameterHandles)
        {
            foreach (var tracerParameterHandle in tracerParameterHandles)
            {
                tracerParameterHandle.Dispose();
            }
        }

        private static void ConvertTracerParameters(List<WrappedGcHandle> handles, EventTracerParameters[] parameters,
            out CEventTrace.EventTracerParameters[] internalParameters)
        {
            internalParameters = new CEventTrace.EventTracerParameters[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterHandle = ConvertTracerParameter(parameters[i], ref internalParameters[i]);
                if (parameterHandle != null)
                {
                    handles.Add(parameterHandle);
                }
            }
        }

        private unsafe class CallbackThunkDelegates
        {
            public static readonly CEventTrace.TraceCallback TraceCallbackThunkDelegate = TraceCallbackThunk;

            [MonoPInvokeCallback(typeof(CEventTrace.TraceCallback))]
            private static void TraceCallbackThunk(void* callbackHandlePtr, CEventTrace.Item* item)
            {
                var callbackHandle = GCHandle.FromIntPtr((IntPtr) callbackHandlePtr);
                var callback = (Action<Item>) callbackHandle.Target;

                callback(Item.ConvertItem(item));
            }
        }
    }
}
