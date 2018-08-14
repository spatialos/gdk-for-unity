using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Creates and removes SpatialOSBehaviourManager object for EntityGameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectInitialisationGroup))]
    public class SpatialOSBehaviourManagerInitializationSystem : ComponentSystem
    {
        public struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentArray<GameObjectReference> GameObjectReferences;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            [ReadOnly] public ComponentDataArray<RequiresSpatialOSBehaviourManager> RequiresSpatialOSBehaviourManagerTags;
        }

        public struct RemovedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        private GameObjectDispatcherSystem gameObjectDispatcherSystem;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            gameObjectDispatcherSystem = World.GetOrCreateManager<GameObjectDispatcherSystem>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entityIndex = addedEntitiesData.Entities[i].Index;
                gameObjectDispatcherSystem.CreateBehaviourManager(addedEntitiesData.Entities[i]);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                gameObjectDispatcherSystem.RemoveSpatialOSBehaviourManager(entityIndex);
            }
        }


    }
}
