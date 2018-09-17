// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        public static class ReceivedEvents
        {
            public struct MyEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> Events
                {
                    get => Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.MyEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.MyEventProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct MyEvent : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> Events
                {
                    get => Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.MyEventProvider.Get(handle);
                    internal set => Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.MyEventProvider.Set(handle, value);
                }
            }

        }
    }
}
