using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal static class ComponentDatabase
    {
        public static Dictionary<uint, IDynamicInvokable> IdsToDynamicInvokers { get; }

        public static Dictionary<Type, uint> ComponentsToIds { get; }

        public static Dictionary<Type, uint> SnapshotsToIds { get; }

        public static Dictionary<uint, Type> IdsToComponents { get; }

        static ComponentDatabase()
        {
            IdsToDynamicInvokers = new Dictionary<uint, IDynamicInvokable>();
            ComponentsToIds = new Dictionary<Type, uint>();
            SnapshotsToIds = new Dictionary<Type, uint>();
            IdsToComponents = new Dictionary<uint, Type>();

            var dynamicTypes = ReflectionUtility.GetNonAbstractTypes(typeof(IDynamicInvokable));
            var componentTypes = ReflectionUtility.GetNonAbstractTypes(typeof(ISpatialComponentData));
            var snapshotTypes = ReflectionUtility.GetNonAbstractTypes(typeof(ISpatialComponentSnapshot));

            foreach (var type in dynamicTypes)
            {
                var instance = (IDynamicInvokable) Activator.CreateInstance(type);
                IdsToDynamicInvokers.Add(instance.ComponentId, instance);
            }

            foreach (var type in componentTypes)
            {
                var instance = (ISpatialComponentData) Activator.CreateInstance(type);
                ComponentsToIds.Add(type, instance.ComponentId);
                IdsToComponents.Add(instance.ComponentId, type);
            }

            foreach (var type in snapshotTypes)
            {
                var instance = (ISpatialComponentSnapshot) Activator.CreateInstance(type);
                SnapshotsToIds.Add(type, instance.ComponentId);
            }
        }
    }
}
