using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
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
        private SpatialOSBehaviourLibrary behaviourLibrary;

        private ILogDispatcher logger;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            gameObjectDispatcherSystem = World.GetOrCreateManager<GameObjectDispatcherSystem>();
            logger = WorkerRegistry.GetWorkerForWorld(World).View.LogDispatcher;
            behaviourLibrary = new SpatialOSBehaviourLibrary(logger);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entityIndex = addedEntitiesData.Entities[i].Index;
                var spatialOSBehaviourManager = new SpatialOSBehaviourManager(
                    addedEntitiesData.GameObjectReferences[i].GameObject, behaviourLibrary, logger);
                gameObjectDispatcherSystem.AddSpatialOSBehaviourManager(entityIndex, spatialOSBehaviourManager);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                gameObjectDispatcherSystem.RemoveSpatialOSBehaviourManager(entityIndex);
            }
        }
    }
}
