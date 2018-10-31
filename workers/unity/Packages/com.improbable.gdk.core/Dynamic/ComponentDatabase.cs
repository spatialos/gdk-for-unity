using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal static class ComponentDatabase
    {
        public static Dictionary<uint, IDynamicInvokable> IdsToDynamicInvokers { get; }

        public static Dictionary<Type, uint> ComponentsToIds { get; }

        public static Dictionary<Type, uint> SnapshotsToIds { get; }

        static ComponentDatabase()
        {
            IdsToDynamicInvokers = new Dictionary<uint, IDynamicInvokable>();
            ComponentsToIds = new Dictionary<Type, uint>();
            SnapshotsToIds = new Dictionary<Type, uint>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IDynamicInvokable).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IDynamicInvokable) Activator.CreateInstance(type);
                        IdsToDynamicInvokers.Add(instance.ComponentId, instance);
                    }

                    if (typeof(ISpatialComponentData).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ISpatialComponentData) Activator.CreateInstance(type);
                        ComponentsToIds.Add(type, instance.ComponentId);
                    }

                    if (typeof(ISpatialComponentSnapshot).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ISpatialComponentSnapshot) Activator.CreateInstance(type);
                        SnapshotsToIds.Add(type, instance.ComponentId);
                    }
                }
            }
        }
    }
}
