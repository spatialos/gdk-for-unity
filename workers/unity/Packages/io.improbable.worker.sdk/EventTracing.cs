using System;
using Improbable.Worker.CInterop.Internal;

namespace Packages.io.improbable.worker.sdk
{
    public unsafe class EventTracing
    {
        public struct SpanId
        {
            public fixed byte Data[16];
        }

        public struct Span
        {
            public SpanId Id;
            public UInt32 CauseCount;
            public SpanId Causes;
        }
    }
}
