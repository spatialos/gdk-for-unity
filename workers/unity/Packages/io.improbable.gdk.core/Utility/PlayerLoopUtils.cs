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
    internal static class PlayerLoopUtils
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class UpdateInSubSystemAttribute : Attribute
        {
            public Type SubSystemType { get; }

            public UpdateInSubSystemAttribute(Type subSystemType)
            {
                SubSystemType = subSystemType;
            }
        }

        // UTY-2059: We need to call this specific private method to ensure the EntityDebugger shows our groups in the PlayerLoop
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
            // Create simulation system for the default group
            var simulationSystemGroup = world.GetOrCreateSystem<SimulationSystemGroup>();

            var systems = world.Systems.ToList();
            var uniqueSystemTypes = new HashSet<Type>(systems.Select(s => s.GetType()));

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

                    var systemGroup = world.GetOrCreateSystem(groupAttr.GroupType) as ComponentSystemGroup;
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

                // Hardcode the subSystem types for Unity root-systems
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

        public static void RemoveFromPlayerLoop(World world)
        {
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            if (playerLoop.subSystemList == null)
            {
                Debug.LogWarning("Cannot remove a world from default PlayerLoop.");
                return;
            }

            //Reflection to get world from PlayerLoopSystem
            var wrapperType =
                typeof(ScriptBehaviourUpdateOrder).Assembly.GetType(
                    "Unity.Entities.ScriptBehaviourUpdateOrder+DummyDelegateWrapper");
            var systemField = wrapperType.GetField("m_System", BindingFlags.NonPublic | BindingFlags.Instance);

            for (var i = 0; i < playerLoop.subSystemList.Length; ++i)
            {
                ref var playerLoopSubSystem = ref playerLoop.subSystemList[i];
                playerLoopSubSystem.subSystemList = playerLoopSubSystem.subSystemList.Where(s =>
                {
                    if (s.updateDelegate != null && s.updateDelegate.Target.GetType() == wrapperType)
                    {
                        var targetSystem = systemField.GetValue(s.updateDelegate.Target) as ComponentSystemBase;
                        return targetSystem.World != world;
                    }

                    return true;
                }).ToArray();
            }

            // Update PlayerLoop
            ScriptBehaviourUpdateOrder.SetPlayerLoop(playerLoop);
        }
    }
}
