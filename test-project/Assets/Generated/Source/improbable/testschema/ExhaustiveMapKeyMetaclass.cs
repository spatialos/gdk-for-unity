// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveMapKey
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197719;
            public string Name => "ExhaustiveMapKey";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.ExhaustiveMapKeyViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapKey.ExhaustiveMapKeyDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[] 
            { 
            };
        }
    }
}
