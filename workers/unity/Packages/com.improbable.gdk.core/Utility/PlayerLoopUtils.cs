using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.Core
{
    public static class PlayerLoopUtils
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class UpdateInSubSystemAttribute : Attribute
        {
            public UpdateInSubSystemAttribute(Type subSystemType)
            {
                SubSystemType = subSystemType;
            }

            public Type SubSystemType { get; }
        }

        // HACK: We need to call this specific private method to ensure the EntityDebugger shows our groups in the PlayerLoop
        // No need to cache these methods, as each type will almost always be called once.
        private static void InsertManagerIntoSubsystemList(PlayerLoopSystem[] subsystemList, int insertIndex,
            ComponentSystemBase mgr, Type type)
        {
            var method = typeof(ScriptBehaviourUpdateOrder).GetMethod("InsertManagerIntoSubsystemList",
                BindingFlags.Static | BindingFlags.NonPublic);
            var genericMethod = method.MakeGenericMethod(type);
            genericMethod.Invoke(null, new object[] { subsystemList, insertIndex, mgr });
        }

        public static void ResolveSystemGroups(World world)
        {
            var systems = world.Systems.ToList();
            var uniqueGroup = new HashSet<Type>(systems.Select(s => s.GetType()));

            // create presentation system and simulation system
            var simulationSystemGroup = world.GetOrCreateSystem<SimulationSystemGroup>();

            // Add systems to their groups, based on the [UpdateInGroup] attribute.
            for (var i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                var type = system.GetType();

                // Skip the root-level systems
                if (type == typeof(InitializationSystemGroup) ||
                    type == typeof(SimulationSystemGroup) ||
                    type == typeof(PresentationSystemGroup) ||
                    type.GetCustomAttribute<UpdateInSubSystemAttribute>() != null)
                {
                    uniqueGroup.Add(type);
                    continue;
                }

                // systems without a group will be added to the SimulationSystemGroup
                var groups = type.GetCustomAttributes(typeof(UpdateInGroupAttribute), true);
                if (groups.Length == 0)
                {
                    simulationSystemGroup.AddSystemToUpdateList(system as ComponentSystemBase);
                }


                foreach (var group in groups)
                {
                    if (!(group is UpdateInGroupAttribute groupAttribute))
                    {
                        continue;
                    }

                    if (!typeof(ComponentSystemGroup).IsAssignableFrom(groupAttribute.GroupType))
                    {
                        Debug.LogError(
                            $"Invalid [UpdateInGroup] attribute for {type}: {groupAttribute.GroupType} must be derived from ComponentSystemGroup.");
                        continue;
                    }

                    var groupMgr = world.GetOrCreateSystem(groupAttribute.GroupType);
                    if (groupMgr is ComponentSystemGroup groupSys)
                    {
                        groupSys.AddSystemToUpdateList(world.GetOrCreateSystem(type) as ComponentSystemBase);
                        if (!uniqueGroup.Contains(groupAttribute.GroupType))
                        {
                            uniqueGroup.Add(groupAttribute.GroupType);
                            systems.Add(groupMgr);
                        }
                    }
                }
            }

            // Sort all root groups, sorts depth first
            foreach (var system in systems)
            {
                var type = system.GetType();
                if (type == typeof(InitializationSystemGroup) ||
                    type == typeof(SimulationSystemGroup) ||
                    type == typeof(PresentationSystemGroup) ||
                    type.GetCustomAttribute<UpdateInSubSystemAttribute>() != null)
                {
                    var groupSystem = system as ComponentSystemGroup;
                    groupSystem.SortSystemUpdateList();
                }
            }
        }

        public static void AddToPlayerLoop(World world)
        {
            var systemGroups = world.Systems.OfType<ComponentSystemGroup>();
            var subSystemToGroup = new Dictionary<Type, List<ComponentSystemGroup>>();

            // Build lookup for PlayerLoop
            foreach (var systemGroup in systemGroups)
            {
                var type = systemGroup.GetType();
                Type subSystemType;

                // HACK: Hardcode the subSystem types for Unity root-systems
                if (type == typeof(InitializationSystemGroup))
                {
                    subSystemType = typeof(Initialization);
                }
                else if (type == typeof(SimulationSystemGroup))
                {
                    subSystemType = typeof(Update);
                }
                else if (type == typeof(PresentationSystemGroup))
                {
                    subSystemType = typeof(PreLateUpdate);
                }
                else
                {
                    var attributes = type.GetCustomAttributes(typeof(UpdateInSubSystemAttribute), true);
                    if (attributes.Length == 0)
                    {
                        continue;
                    }

                    subSystemType = ((UpdateInSubSystemAttribute) attributes[0]).SubSystemType;
                }

                // Add to lookup table
                if (!subSystemToGroup.TryGetValue(subSystemType, out var groupList))
                {
                    groupList = new List<ComponentSystemGroup>();
                    subSystemToGroup.Add(subSystemType, groupList);
                }

                groupList.Add(systemGroup);
            }

            // Insert groups into PlayerLoop
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            if (playerLoop.subSystemList == null)
            {
                playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            }

            for (var i = 0; i < playerLoop.subSystemList.Length; ++i)
            {
                ref var subSystem = ref playerLoop.subSystemList[i];
                if (!subSystemToGroup.TryGetValue(subSystem.type, out var groupList))
                {
                    // No groups to add for this subsystem
                    continue;
                }

                var originalSize = subSystem.subSystemList.Length;
                var newSize = originalSize + groupList.Count;
                Array.Resize(ref subSystem.subSystemList, newSize);
                for (var groupIndex = 0; groupIndex < groupList.Count; groupIndex++)
                {
                    var systemGroup = groupList[groupIndex];
                    InsertManagerIntoSubsystemList(subSystem.subSystemList, originalSize + groupIndex,
                        systemGroup, systemGroup.GetType());
                }
            }

            // Set as new PlayerLoop
            ScriptBehaviourUpdateOrder.SetPlayerLoop(playerLoop);
        }

        public static void PrintPlayerLoop()
        {
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            if (playerLoop.subSystemList == null)
            {
                playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            }

            foreach (var subSystem in playerLoop.subSystemList)
            {
                Debug.Log($"{subSystem.type}");

                for (var i = 0; i < subSystem.subSystemList.Length; ++i)
                {
                    Debug.Log($"-- {subSystem.subSystemList[i].type}");
                }
            }
        }
    }
}
