using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public delegate TComponentSnapshot SnapshotDeserializer<out TComponentSnapshot>(ComponentData update)
        where TComponentSnapshot : struct, ISpatialComponentSnapshot;

    public delegate void SnapshotSerializer<TComponentSnapshot>(TComponentSnapshot snapshot, ComponentData data)
        where TComponentSnapshot : struct, ISpatialComponentSnapshot;

    public delegate TComponentSnapshot SnapshotDeserializerRaw<out TComponentSnapshot>(SchemaObject obj)
        where TComponentSnapshot : struct, ISpatialComponentSnapshot;

    public delegate void SnapshotSerializeRaw<TComponentSnapshot>(TComponentSnapshot snapshot, SchemaObject obj)
        where TComponentSnapshot : struct, ISpatialComponentSnapshot;

    public delegate TUpdate SnapshotToUpdate<TSnapshot, out TUpdate>(in TSnapshot snapshot)
        where TSnapshot : struct, ISpatialComponentSnapshot
        where TUpdate : struct, ISpatialComponentUpdate;

    public static class Dynamic
    {
        private static readonly Dictionary<uint, IDynamicInvokable> DynamicInvokers;

        static Dynamic()
        {
            DynamicInvokers = ComponentDatabase.Metaclasses.ToDictionary(
                pair => pair.Key,
                pair => (IDynamicInvokable) Activator.CreateInstance(pair.Value.DynamicInvokable));
        }

        public struct VTable<TUpdate, TSnapshot>
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            public SnapshotDeserializer<TSnapshot> DeserializeSnapshot;
            public SnapshotSerializer<TSnapshot> SerializeSnapshot;
            public SnapshotDeserializerRaw<TSnapshot> DeserializeSnapshotRaw;
            public SnapshotSerializeRaw<TSnapshot> SerializeSnapshotRaw;
            public SnapshotToUpdate<TSnapshot, TUpdate> ConvertSnapshotToUpdate;
        }

        public interface IHandler
        {
            void Accept<TUpdate, TSnapshot>(uint componentId, VTable<TUpdate, TSnapshot> vtable)
                where TUpdate : struct, ISpatialComponentUpdate
                where TSnapshot : struct, ISpatialComponentSnapshot;
        }

        public static void ForEachComponent(IHandler handler)
        {
            foreach (var component in DynamicInvokers.Values)
            {
                component.InvokeHandler(handler);
            }
        }

        public static void ForComponent(uint componentId, IHandler handler)
        {
            if (!DynamicInvokers.TryGetValue(componentId, out var component))
            {
                throw new ArgumentException($"Unknown component ID {componentId}.");
            }

            component.InvokeHandler(handler);
        }
    }
}
