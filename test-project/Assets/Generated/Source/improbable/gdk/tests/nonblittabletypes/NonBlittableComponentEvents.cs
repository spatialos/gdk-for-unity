// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public static class FirstEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload Payload;

                public Event(global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload payload)
                {
                    Payload = payload;
                }
            }
        }

        public static class SecondEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload Payload;

                public Event(global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload payload)
                {
                    Payload = payload;
                }
            }
        }


#if !DISABLE_REACTIVE_COMPONENTS
        public static class ReceivedEvents
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
