// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveMapValue
    {
        public class ComponentMetaclass : IComponentMetaclass
        {
            public uint ComponentId => 197718;
            public string Name => "ExhaustiveMapValue";

            public Type Data { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.Component);
            public Type Snapshot { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.Snapshot);
            public Type Update { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.Update);

            public Type ReplicationHandler { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.ComponentReplicator);
            public Type Serializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.ComponentSerializer);
            public Type DiffDeserializer { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.DiffComponentDeserializer);

            public Type DiffStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.DiffComponentStorage);
            public Type ViewStorage { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.ExhaustiveMapValueViewStorage);
            public Type EcsViewManager { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.EcsViewManager);
            public Type DynamicInvokable { get; } = typeof(global::Improbable.TestSchema.ExhaustiveMapValue.ExhaustiveMapValueDynamic);

            public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[] 
            { 
            };
        }
    }
}
