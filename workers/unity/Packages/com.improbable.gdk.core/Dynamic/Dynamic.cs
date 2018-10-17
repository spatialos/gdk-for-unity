using System;
using System.Collections.Generic;
using Improbable.Worker.Core;
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

        private static readonly Dictionary<uint, IDynamicInvokable> idsToComponents =
            new Dictionary<uint, IDynamicInvokable>();

        private static readonly Dictionary<Type, uint> componentsToIds = new Dictionary<Type, uint>();

        static Dynamic()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IDynamicInvokable).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IDynamicInvokable) Activator.CreateInstance(type);
                        idsToComponents.Add(instance.ComponentId, instance);
                    }

                    if (typeof(ISpatialComponentData).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ISpatialComponentData) Activator.CreateInstance(type);
                        componentsToIds.Add(type, instance.ComponentId);
                    }
                }
            }
        }

        public static void ForEachComponent(IHandler handler)
        {
            foreach (var component in idsToComponents.Values)
            {
                component.InvokeHandler(handler);
            }
        }

        public static void ForComponent(uint componentId, IHandler handler)
        {
            if (!idsToComponents.TryGetValue(componentId, out var component))
            {
                throw new ArgumentException($"Unknown component ID {componentId}.");
            }

            component.InvokeHandler(handler);
        }

        public static uint GetComponentId<T>() where T : ISpatialComponentData
        {
            if (!componentsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component {nameof(T)}.");
            }

            return id;
        }
    }
}
