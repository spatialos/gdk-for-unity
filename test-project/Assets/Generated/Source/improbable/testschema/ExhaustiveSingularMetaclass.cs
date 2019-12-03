// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveSingular
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197715;
            public string Name => "ExhaustiveSingular";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.ExhaustiveSingularViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveSingular.ExhaustiveSingularDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]
            {
            };
        }
    }
}
