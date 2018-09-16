// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public static class ReceivedEvents
        {
            public struct Evt : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> Events
                {
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
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
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
                }
            }

        }
    }
}
