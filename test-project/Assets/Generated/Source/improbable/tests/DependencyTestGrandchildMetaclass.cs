// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Tests
{
    public partial class DependencyTestGrandchild
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 11113;
            public string Name => "DependencyTestGrandchild";

            public Type Data { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.DependencyTestGrandchildViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.Tests.DependencyTestGrandchild.DependencyTestGrandchildDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
