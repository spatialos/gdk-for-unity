// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public static class ReceivedEvents
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> Events
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> Events
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
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
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> Events
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }
    }
}
