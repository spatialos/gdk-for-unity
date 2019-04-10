// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public static class Evt
        {
            public readonly struct Event : IEvent
            {
                public readonly global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty Payload;

                public Event(global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
                {
                    Payload = payload;
                }
            }
        }


#if !DISABLE_REACTIVE_COMPONENTS
        public static class ReceivedEvents
        {
            public struct Evt : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> Events
                {
                    get => global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct Evt : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> Events
                {
                    get => global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
                }
            }

        }
#endif
    }
}
