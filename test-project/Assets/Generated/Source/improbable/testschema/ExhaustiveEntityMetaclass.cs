// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveEntity
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197720;
            public string Name => "ExhaustiveEntity";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.ExhaustiveEntityViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveEntity.ExhaustiveEntityDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
