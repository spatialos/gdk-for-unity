// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Tests
{
    public partial class DependencyTestChild
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 11112;
            public string Name => "DependencyTestChild";

            public Type Data { get; } = typeof(global::Improbable.Tests.DependencyTestChild.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.Tests.DependencyTestChild.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.Tests.DependencyTestChild.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.Tests.DependencyTestChild.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.Tests.DependencyTestChild.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.Tests.DependencyTestChild.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.Tests.DependencyTestChild.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.Tests.DependencyTestChild.DependencyTestChildViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.Tests.DependencyTestChild.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.Tests.DependencyTestChild.DependencyTestChildDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
