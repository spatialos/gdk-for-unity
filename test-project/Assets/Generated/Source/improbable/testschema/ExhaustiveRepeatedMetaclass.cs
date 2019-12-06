// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveRepeated
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197717;
            public string Name => "ExhaustiveRepeated";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.ExhaustiveRepeatedViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveRepeated.ExhaustiveRepeatedDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
