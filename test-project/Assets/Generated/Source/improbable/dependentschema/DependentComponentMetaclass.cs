// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 198800;
            public string Name => "DependentComponent";

            public Type Data { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.DependentComponentViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.DependentSchema.DependentComponent.DependentComponentDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
