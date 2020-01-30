// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class RecursiveComponent
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 18800;
            public string Name => "RecursiveComponent";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.RecursiveComponentViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.RecursiveComponent.RecursiveComponentDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
