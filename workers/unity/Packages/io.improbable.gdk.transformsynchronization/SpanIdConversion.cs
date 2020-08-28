using System;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class SpanIdConversion
    {
        public static Worker.CInterop.SpanId FromSchema(this SpanId spanId)
        {
            var workerSpanId = new Worker.CInterop.SpanId();
            unsafe
            {
                ApiInterop.Memcpy(workerSpanId.Data, (byte*) &spanId.SpanIdLower, (UIntPtr) 8);
                ApiInterop.Memcpy(workerSpanId.Data + 8 * sizeof(byte), (byte*) &spanId.SpanIdUpper, (UIntPtr) 8);
            }

            return workerSpanId;
        }

        public static SpanId ToSchema(this Worker.CInterop.SpanId spanId)
        {
            var schemaSpanId = new SpanId();
            unsafe
            {
                ApiInterop.Memcpy((byte*) &schemaSpanId.SpanIdLower, spanId.Data, (UIntPtr) 8);
                ApiInterop.Memcpy((byte*) &schemaSpanId.SpanIdUpper, spanId.Data + 8 * sizeof(byte), (UIntPtr) 8);
            }

            return schemaSpanId;
        }
    }
}
