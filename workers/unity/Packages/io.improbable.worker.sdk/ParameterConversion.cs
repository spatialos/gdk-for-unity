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
            ApiInterop.Memcpy(internalSpanId.Data, spanId.Data, (UIntPtr) 16);
            return internalSpanId;
        }

        public static unsafe CEventTrace.Event ConvertEvent(Event eventToConvert)
        {
            var internalEvent = new CEventTrace.Event();
            internalEvent.UnixTimestampMillis = eventToConvert.UnixTimestampMillis;
            internalEvent.Id = ConvertSpanId(eventToConvert.Id);
            internalEvent.Data = eventToConvert.Data?.eventData.AddrOfPinnedObject() ?? IntPtr.Zero;

            fixed (byte* eventType = ApiInterop.ToUtf8Cstr(eventToConvert.Type))
            fixed (byte* eventMessage = ApiInterop.ToUtf8Cstr(eventToConvert.Message))
            {
                internalEvent.Type = eventType;
                internalEvent.Message = eventMessage;
            }

            return internalEvent;
        }

        public static unsafe void ConvertEventTracer(EventTracerParameters[] parameters, out CEventTrace.EventTracerParameters[] internalParameters)
        {
            List<WrappedGcHandle> handles = new List<WrappedGcHandle>(
                parameters.Length);

            // ConvertTracerParameters(handles, ...)
            internalParameters = new CEventTrace.EventTracerParameters[parameters.Length];
        }

        public static unsafe WrappedGcHandle ConvertTracerParameter(EventTracerParameters parameter, ref CEventTrace.EventTracerParameters internalParameters)
        {
            WrappedGcHandle wrappedParameterObject = new WrappedGcHandle(parameter);

            internalParameters.UserData = (void*) wrappedParameterObject.Get();
            internalParameters.TraceCallback = parameter.TraceCallback == null
                ? IntPtr.Zero
                : Marshal.GetFunctionPointerForDelegate(CallbackThunkDelegates.traceCallbackThunkDelegate);

            return wrappedParameterObject;
        }

        public static void FreeTracerParameterHandles(IEnumerable<WrappedGcHandle> tracerParameterHandles)
        {
            foreach (var tracerParameterHandle in tracerParameterHandles)
            {
                tracerParameterHandle.Dispose();
            }
        }

        private static void ConvertTracerParameters(List<WrappedGcHandle> handles,
            Dictionary<uint, EventTracerParameters> parameterDict, EventTracerParameters defaultParameters,
            out CEventTrace.EventTracerParameters[] internalParameters, out CEventTrace.EventTracerParameters internalDefaultParameters)
        {
            internalParameters = new CEventTrace.EventTracerParameters[parameterDict.Count];
            int i = 0;
            foreach (var parameter in parameterDict)
            {
                var parameterHandle = ConvertTracerParameter(parameter.Value, ref internalParameters[i]);
                if (parameterHandle != null)
                {
                    handles.Add(parameterHandle);
                }

                ++i;
            }

            internalDefaultParameters = new CEventTrace.EventTracerParameters();
            if (defaultParameters != null)
            {
                var defaultParameterHandle = ConvertTracerParameter(defaultParameters, ref internalDefaultParameters);
                handles.Add(defaultParameterHandle);
            }
        }

        private unsafe class CallbackThunkDelegates
        {
            public static readonly CEventTrace.TraceCallback traceCallbackThunkDelegate = TraceCallbackThunk;

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
