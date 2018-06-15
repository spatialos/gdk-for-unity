using Improbable.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private MutableView view;
        private ComponentGroup newlyCreatedEntityComponentGroup;
        private ComponentGroup onConnectedComponentGroup;
        private ComponentGroup onDisconnectedComponentGroup;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            GenerateComponentGroups();

            newlyCreatedEntityComponentGroup = GetComponentGroup(typeof(NewlyCreatedEntity));
            onConnectedComponentGroup = GetComponentGroup(typeof(OnConnected));
            onDisconnectedComponentGroup = GetComponentGroup(typeof(OnDisconnected));
        }

        private void GenerateComponentGroups()
        {
            foreach (var translationUnit in view.TranslationUnits.Values)
            {
                translationUnit.CleanUpComponentGroups = new List<ComponentGroup>();
                foreach (ComponentType componentType in translationUnit.CleanUpComponentTypes)
                {
                    translationUnit.CleanUpComponentGroups.Add(GetComponentGroup(componentType));
                }
            }
        }

        protected override void OnUpdate()
        {
            var commandBuffer = PostUpdateCommands;

            foreach (var translationUnit in view.TranslationUnits.Values)
            {
                translationUnit.CleanUpComponents(ref commandBuffer);
            }

            var newlyCreatedEntities = newlyCreatedEntityComponentGroup.GetEntityArray();
            for (var i = 0; i < newlyCreatedEntities.Length; i++)
            {
                var entity = newlyCreatedEntities[i];
                commandBuffer.RemoveComponent<NewlyCreatedEntity>(entity);
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
    }
}
