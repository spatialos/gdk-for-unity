// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public static class FooEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.TestSchema.SomeType Payload;

                public Event(global::Improbable.TestSchema.SomeType payload)
                {
                    Payload = payload;
                }
            }
        }

    }
}
