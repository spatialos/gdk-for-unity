// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public static class ReceivedEvents
        {
            public struct Evt : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> Events
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct Evt : IComponentData
            {
                internal uint handle;

                public List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> Events
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Get(handle);
                    internal set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.Set(handle, value);
                }
            }

        }
    }
}
