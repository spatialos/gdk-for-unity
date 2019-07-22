// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

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


#if !DISABLE_REACTIVE_COMPONENTS
        public static class ReceivedEvents
        {
            public struct FooEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.TestSchema.SomeType> Events
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.FooEventProvider.Get(handle);
                    internal set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.FooEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct FooEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.TestSchema.SomeType> Events
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.FooEventProvider.Get(handle);
                    internal set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.FooEventProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
