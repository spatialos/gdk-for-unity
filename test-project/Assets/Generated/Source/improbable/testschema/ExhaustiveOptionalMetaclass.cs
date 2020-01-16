// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveOptional
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197716;
            public string Name => "ExhaustiveOptional";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.ExhaustiveOptionalViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveOptional.ExhaustiveOptionalDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
