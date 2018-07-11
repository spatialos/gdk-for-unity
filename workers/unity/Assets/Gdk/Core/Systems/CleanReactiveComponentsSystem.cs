using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private MutableView view;
        private ComponentGroup newlyAddedSpatialOSEntityComponentGroup;
        private ComponentGroup onConnectedComponentGroup;
        private ComponentGroup onDisconnectedComponentGroup;

        private List<ComponentCleanupData> cleaners = new List<ComponentCleanupData>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            GenerateComponentGroups();

            newlyAddedSpatialOSEntityComponentGroup = GetComponentGroup(typeof(NewlyAddedSpatialOSEntity));
            onConnectedComponentGroup = GetComponentGroup(typeof(OnConnected));
            onDisconnectedComponentGroup = GetComponentGroup(typeof(OnDisconnected));
        }

        private void GenerateComponentGroups()
        {
            var cleanupers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentCleanup).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var cleanuper in cleanupers)
            {
                var c = (ComponentCleanup) Activator.CreateInstance(cleanuper, this);
                var cGroups = c.CleanUpComponentTypes.Select(type => GetComponentGroup(type)).ToList();

                cleaners.Add(new ComponentCleanupData
                {
                    cleanupObj = c,
                    cleanupGroups = cGroups
                });
            }
        }

        protected override void OnUpdate()
        {
            var commandBuffer = PostUpdateCommands;

            foreach (var cleaner in cleaners)
            {
                cleaner.cleanupObj.CleanupComponents(cleaner.cleanupGroups, ref commandBuffer);
            }

            var newlyCreatedEntities = newlyAddedSpatialOSEntityComponentGroup.GetEntityArray();
            for (var i = 0; i < newlyCreatedEntities.Length; i++)
            {
                var entity = newlyCreatedEntities[i];
                commandBuffer.RemoveComponent<NewlyAddedSpatialOSEntity>(entity);
            }

            var onConnectedEntities = onConnectedComponentGroup.GetEntityArray();
            for (var i = 0; i < onConnectedEntities.Length; i++)
            {
                var entity = onConnectedEntities[i];
                commandBuffer.RemoveComponent<OnConnected>(entity);
            }

            var onDisconnectedEntities = onDisconnectedComponentGroup.GetEntityArray();
            for (var i = 0; i < onDisconnectedEntities.Length; i++)
            {
                var entity = onDisconnectedEntities[i];
                commandBuffer.RemoveComponent<OnDisconnected>(entity);
            }
        }

        private struct ComponentCleanupData
        {
            public ComponentCleanup cleanupObj;
            public List<ComponentGroup> cleanupGroups;
        }
    }
}
