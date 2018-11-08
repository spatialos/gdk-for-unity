using System;
using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class DynamicSnapshot
    {
        public delegate T SnapshotDeserializer<out T>(ComponentData update, World world) where T : ISpatialComponentSnapshot;

        public delegate void SnapshotSerializer<T>(T snapshot, ComponentData data) where T : ISpatialComponentSnapshot;

        public interface ISnapshotHandler
        {
            void Accept<T>(uint componentId,
                SnapshotDeserializer<T> deserializeComponentData,
                    SnapshotSerializer<T> serializeSnapshot)
                where T : ISpatialComponentSnapshot;
        }
        
        public static void ForEachSnapshotComponent(ISnapshotHandler handler)
        {
            foreach (var component in ComponentDatabase.IdsToDynamicInvokers.Values)
            {
                component.InvokeSnapshotHandler(handler);
            }
        }

        internal static void ForSnapshotComponent(uint componentId, ISnapshotHandler handler)
        {
            if (!ComponentDatabase.IdsToDynamicInvokers.TryGetValue(componentId, out var component))
            {
                throw new ArgumentException($"Unknown component ID {componentId}.");
            }

            component.InvokeSnapshotHandler(handler);
        }

        public static uint GetSnapshotComponentId<T>() where T : ISpatialComponentSnapshot
        {
            if (!ComponentDatabase.SnapshotsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component {nameof(T)}.");
            }

            return id;
        }
    }
}
