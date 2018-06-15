using System.Collections.Generic;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    /// <summary>
    ///     Creates a companion gameobject for newly spawned entities according to a prefab definition.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.EntityInitialisationGroup))]
    internal class GameObjectInitializationSystem : ComponentSystem
    {
        private readonly Dictionary<string, GameObject> cachedPrefabs = new Dictionary<string, GameObject>();

        public struct Data
        {
            public int Length;
            [ReadOnly] public ComponentArray<SpatialOSPrefab> PrefabNames;
            [ReadOnly] public ComponentDataArray<SpatialOSTransform> Transforms;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyCreatedEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        private MutableView view;
        private WorkerBase worker;
        private Vector3 origin;
        private readonly ViewCommandBuffer viewCommandBuffer = new ViewCommandBuffer();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;
            origin = worker.Origin;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var prefabMapping = PrefabConfig.PrefabMappings[data.PrefabNames[i].Prefab];
                var transform = data.Transforms[i];
                var entity = data.Entities[i];
                var spatialEntityId = data.SpatialEntityIds[i].EntityId;

                if (!(worker is UnityClient) && !(worker is UnityGameLogic))
                {
                    Debug.LogErrorFormat(Errors.UnknownWorkerType, World.Name);
                }

                var prefabName = worker is UnityGameLogic
                    ? prefabMapping.UnityGameLogic
                    : prefabMapping.UnityClient;

                InstantiateAndLinkGameObject(ref entity, ref prefabName, ref transform, spatialEntityId);
            }

            viewCommandBuffer.FlushBuffer(view);
        }

        private void InstantiateAndLinkGameObject(ref Entity entity, ref string prefabName,
            ref SpatialOSTransform transform,
            long spatialEntityId)
        {
            GameObject prefab;
            if (!cachedPrefabs.TryGetValue(prefabName, out prefab))
            {
                prefab = Resources.Load<GameObject>(prefabName);
                if (prefab == null)
                {
                    Debug.LogErrorFormat(Errors.PrefabNotFound, prefabName);
                    return;
                }

                cachedPrefabs[prefabName] = prefab;
            }

            var spawningPosition =
                new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) + origin;
            var spawningRotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                transform.Rotation.Z, transform.Rotation.W);
            var gameObject = GameObject.Instantiate(prefab, spawningPosition, spawningRotation);
            gameObject.name = $"{prefab.name}(SpatialOS: {spatialEntityId}, Unity: {entity.Index}/{World.Name})";

            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = spatialEntityId;

            foreach (var component in gameObject.GetComponents<Component>())
            {
                viewCommandBuffer.AddComponent(entity, component.GetType(), component);
            }

            view.AddGameObjectEntity(entity, gameObject);
        }

        internal static class Errors
        {
            public const string PrefabNotFound =
                "Prefab for prefabPath {0} not found.";

            public const string UnknownWorkerType =
                "Unknown workerType for world name {0}.";
        }
    }
}
