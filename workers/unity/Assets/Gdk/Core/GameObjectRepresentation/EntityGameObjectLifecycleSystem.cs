using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class EntityGameObjectLifecycleSystem : ComponentSystem
    {
        private uint currentHandle;
        private readonly Dictionary<uint, GameObject> entityGameObjectCache = new Dictionary<uint, GameObject>();

        public struct AddedEntitiesData
        {
            public int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentArray<GameObjectReference> GameObjectReferences;
            public SubtractiveComponent<GameObjectReferenceHandle> NoGameObjectReferenceHandle;
        }

        public struct RemovedEntitiesData
        {
            public int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var handle = currentHandle++;
                var gameObject = addedEntitiesData.GameObjectReferences[i].GameObject;
                entityGameObjectCache[handle] = gameObject;
                var gameObjectReferenceHandleComponent = new GameObjectReferenceHandle { GameObjectHandle = handle };
                PostUpdateCommands.AddComponent(addedEntitiesData.Entities[i], gameObjectReferenceHandleComponent);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var handle = removedEntitiesData.GameObjectReferenceHandles[i].GameObjectHandle;
                GameObject gameObject;
                if (!entityGameObjectCache.TryGetValue(handle, out gameObject))
                {
                    Debug.LogErrorFormat(Errors.GameObjectNotFound, handle);
                    continue;
                }

                entityGameObjectCache.Remove(handle);
                UnityObjectDestroyer.Destroy(gameObject);
                PostUpdateCommands.RemoveSystemStateComponent<GameObjectReferenceHandle>(
                    removedEntitiesData.Entities[i]);
            }
        }

        internal static class Errors
        {
            public const string GameObjectNotFound = "EntityGameObject with handle {0} not found.";
        }
    }
}
