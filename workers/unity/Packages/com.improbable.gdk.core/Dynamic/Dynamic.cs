using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public delegate T ComponentDeserializer<out T>(ComponentData data, World world) where T : ISpatialComponentData;

    public delegate T UpdateDeserializer<out T>(ComponentUpdate update, World world) where T : ISpatialComponentUpdate;

    public static class Dynamic
    {
        public interface IHandler
        {
            void Accept<TData, TUpdate>(uint componentId,
                ComponentDeserializer<TData> deserializeComponentData,
                UpdateDeserializer<TUpdate> deserializeComponentUpdate)
                where TData : ISpatialComponentData
                where TUpdate : ISpatialComponentUpdate;
        }

        public static void ForEachComponent(IHandler handler)
        {
            foreach (var component in ComponentDatabase.IdsToDynamicInvokers.Values)
            {
                component.InvokeHandler(handler);
            }
        }

        public static void ForComponent(uint componentId, IHandler handler)
        {
            if (!ComponentDatabase.IdsToDynamicInvokers.TryGetValue(componentId, out var component))
            {
                throw new ArgumentException($"Unknown component ID {componentId}.");
            }

            component.InvokeHandler(handler);
        }

        public static uint GetComponentId<T>() where T : ISpatialComponentData
        {
            if (!ComponentDatabase.ComponentsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component {nameof(T)}.");
            }

            return id;
        }
    }
}
