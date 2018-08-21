// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public static class ReceivedEvents
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> Events
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> Events
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct FirstEvent : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> Events
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.Set(handle, value);
                }
            }

            public struct SecondEvent : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> Events
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.Set(handle, value);
                }
            }

        }
    }
}
