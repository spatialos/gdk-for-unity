// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public static class FirstEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload Payload;

                public Event(global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
                {
                    Payload = payload;
                }
            }
        }

        public static class SecondEvent
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload Payload;

                public Event(global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
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

                public List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> Events
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
