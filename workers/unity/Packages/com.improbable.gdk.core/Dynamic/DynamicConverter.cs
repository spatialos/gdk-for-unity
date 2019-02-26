using System;

namespace Improbable.Gdk.Core
{
    public static class DynamicConverter
    {
        public delegate TUpdate SnapshotToUpdate<TSnapshot, out TUpdate>(in TSnapshot snapshot)
            where TSnapshot : struct, ISpatialComponentSnapshot
            where TUpdate : struct, ISpatialComponentUpdate;

        public interface IConverterHandler
        {
            void Accept<TSnapshot, TUpdate>(uint componentId, SnapshotToUpdate<TSnapshot, TUpdate> snapshotToUpdate)
                where TSnapshot : struct, ISpatialComponentSnapshot
                where TUpdate : struct, ISpatialComponentUpdate;
        }

        public static void ForEachComponent(IConverterHandler handler)
        {
            foreach (var component in ComponentDatabase.IdsToDynamicInvokers.Values)
            {
                component.InvokeConvertHandler(handler);
            }
        }

        public static void ForComponent(uint componentId, IConverterHandler handler)
        {
            if (!ComponentDatabase.IdsToDynamicInvokers.TryGetValue(componentId, out var component))
            {
                throw new ArgumentException($"Unknown component ID {componentId}.");
            }

            component.InvokeConvertHandler(handler);
        }
    }
}
