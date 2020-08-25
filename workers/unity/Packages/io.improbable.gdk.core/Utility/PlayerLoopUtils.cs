using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Improbable.Gdk.Core
{
    internal static class PlayerLoopUtils
    {
        public static void ResolveSystemGroups(World world)
        {
            // Create simulation system for the default group
            var simulationSystemGroup = world.GetOrCreateSystem<SimulationSystemGroup>();

            var systems = new List<ComponentSystemBase>();
            foreach (var system in world.Systems)
            {
                systems.Add(system);
            }

            var uniqueSystemTypes = new HashSet<Type>(systems.Select(s => s.GetType()));

            // Add systems to their groups, based on the [UpdateInGroup] attribute.
            for (var i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                var type = system.GetType();

                // Skip the root-level systems
                if (type == typeof(InitializationSystemGroup) ||
                    type == typeof(SimulationSystemGroup) ||
                    type == typeof(PresentationSystemGroup))
                {
                    continue;
                }

                // Add to default group if none is defined
                var groupAttributes = type.GetCustomAttributes(typeof(UpdateInGroupAttribute), true);
                if (groupAttributes.Length == 0)
                {
                    simulationSystemGroup.AddSystemToUpdateList(system);
                }

                foreach (UpdateInGroupAttribute groupAttr in groupAttributes)
                {
                    if (!typeof(ComponentSystemGroup).IsAssignableFrom(groupAttr.GroupType))
                    {
                        Debug.LogError(
                            $"Invalid [UpdateInGroup] attribute for {type}: {groupAttr.GroupType} must be derived from ComponentSystemGroup.");
                        continue;
                    }

                    var systemGroup = (ComponentSystemGroup) world.GetOrCreateSystem(groupAttr.GroupType);
                    systemGroup.AddSystemToUpdateList(world.GetOrCreateSystem(type));
                    if (!uniqueSystemTypes.Contains(groupAttr.GroupType))
                    {
                        uniqueSystemTypes.Add(groupAttr.GroupType);
                        systems.Add(systemGroup);
                    }
                }
            }

            // Sort all root groups, sorts depth first
            foreach (var system in systems)
            {
                var type = system.GetType();
                if (type == typeof(InitializationSystemGroup) ||
                    type == typeof(SimulationSystemGroup) ||
                    type == typeof(PresentationSystemGroup))
                {
                    var groupSystem = system as ComponentSystemGroup;
                    groupSystem.SortSystems();
                }
            }
        }
    }
}
