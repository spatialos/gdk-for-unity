// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ComponentUsingNestedTypeSameName
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 198730;
            public string Name => "ComponentUsingNestedTypeSameName";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.ComponentUsingNestedTypeSameNameViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.ComponentUsingNestedTypeSameNameDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
