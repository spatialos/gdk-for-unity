using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Creates and removes MonoBehaviourActivationManager object for EntityGameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectInitialisationGroup))]
    public class MonoBehaviourActivationManagerInitializationSystem : ComponentSystem
    {
        public struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentArray<GameObjectReference> GameObjectReferences;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            [ReadOnly] public ComponentDataArray<RequiresMonoBehaviourActivationManager> RequiresSpatialOSBehaviourManagerTags;
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
                var entity = addedEntitiesData.Entities[i];
                gameObjectDispatcherSystem.CreateActivationManagerAndReaderWriterStore(entity);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                gameObjectDispatcherSystem.RemoveActivationManagerAndReaderWriterStore(entityIndex);
            }
        }
    }
}
