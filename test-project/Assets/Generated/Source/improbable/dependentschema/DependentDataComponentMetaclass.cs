// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 198801;
            public string Name => "DependentDataComponent";

            public Type Data { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.DependentDataComponentViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.DependentDataComponentDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
                new BarCommandMetaclass(),
            };
        }

        public class BarCommandMetaclass : ICommandMetaclass
        {
            public uint CommandIndex => 1;
            public string Name => "BarCommand";

            public Type DiffDeserializer { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.BarCommandDiffCommandDeserializer);
            public Type Serializer { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.BarCommandCommandSerializer);

            public Type MetaDataStorage { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.BarCommandCommandMetaDataStorage);
            public Type SendStorage { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.BarCommandCommandsToSendStorage);
            public Type DiffStorage { get; } = typeof(global::Improbable.DependentSchema.DependentDataComponent.DiffBarCommandCommandStorage);
        }
    }
}
