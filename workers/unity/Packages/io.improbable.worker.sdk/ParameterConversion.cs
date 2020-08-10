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
            return new CEventTrace.SpanId { Data = spanId.Data };
        }

        public static unsafe void ConvertEventTracer(EventTracerParameters[] parameters, out CEventTrace.EventTracerParameters[] internalParameters)
        {
            List<WrappedGcHandle> handles = new List<WrappedGcHandle>(
                parameters.Length);

            // ConvertTracerParameters(handles, )
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
            private static unsafe void TraceCallbackThunk(void* callbackHandlePtr, CEventTrace.Item* item)
            {
                var callbackHandle = GCHandle.FromIntPtr((IntPtr) callbackHandlePtr);
                var callback = (Action<Item>) callbackHandle.Target;

                var callbackItem = new Item();
                // Todo: Add logic to convert from internal item to public-facing item

                callback(callbackItem);
            }
        }
    }
}
