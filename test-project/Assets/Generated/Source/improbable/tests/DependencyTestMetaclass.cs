// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Tests
{
    public partial class DependencyTest
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 11111;
            public string Name => "DependencyTest";

            public Type Data { get; } = typeof(global::Improbable.Tests.DependencyTest.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.Tests.DependencyTest.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.Tests.DependencyTest.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.Tests.DependencyTest.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.Tests.DependencyTest.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.Tests.DependencyTest.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.Tests.DependencyTest.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.Tests.DependencyTest.DependencyTestViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.Tests.DependencyTest.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.Tests.DependencyTest.DependencyTestDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
