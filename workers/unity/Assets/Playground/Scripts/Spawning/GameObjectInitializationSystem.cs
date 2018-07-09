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
        public struct Data
        {
            public int Length;
            [ReadOnly] public ComponentArray<SpatialOSPrefab> PrefabNames;
            [ReadOnly] public ComponentDataArray<SpatialOSTransform> Transforms;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        private MutableView view;
        private WorkerBase worker;
        private Vector3 origin;
        private readonly ViewCommandBuffer viewCommandBuffer = new ViewCommandBuffer();
        private EntityGameObjectCreator entityGameObjectCreator;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;
            origin = worker.Origin;
            entityGameObjectCreator = new EntityGameObjectCreator(World, new Dictionary<string, GameObject>());
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

                var position = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) + origin;
                var rotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                    transform.Rotation.Z, transform.Rotation.W);

                var gameObject =
                    entityGameObjectCreator.CreateEntityGameObject(entity, prefabName, position, rotation,
                        spatialEntityId);
                worker.EntityGameObjectManager.LinkGameObjectToEntity(gameObject, entity, spatialEntityId,
                    viewCommandBuffer);
            }

            viewCommandBuffer.FlushBuffer(view);
        }

        internal static class Errors
        {
            public const string UnknownWorkerType =
                "Unknown workerType for world name {0}.";
        }
    }
}
