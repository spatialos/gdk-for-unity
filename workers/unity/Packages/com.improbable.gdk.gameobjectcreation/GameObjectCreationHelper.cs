using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    /// This class sets up the systems required for game object creation. You can pass your own creator
    /// or use an opinionated helper for setting up standard game object creation.
    /// </summary>
    public static class GameObjectCreationHelper
    {
        private static readonly string WorkerNotCreatedErrorMessage = $"{nameof(EnableStandardGameObjectCreation)} should be called only after a worker has been initialised for the world.";
        private static readonly Dictionary<string, GameObject> cachedPrefabs = new Dictionary<string, GameObject>();
        private static string workerType;
        private static Vector3 workerOrigin;

        public static void EnableGameObjectCreation(World world, IEntityGameObjectCreator creator,
            GameObject workerGameObject = null)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new InvalidOperationException(WorkerNotCreatedErrorMessage);
            }

            var entityManager = world.GetOrCreateManager<EntityManager>();

            if (world.GetExistingManager<GameObjectInitializationSystem>() != null)
            {
                workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "You should only call EnableStandardGameObjectCreation() once on worker setup")
                    .WithField(LoggingUtils.LoggerName, nameof(GameObjectCreationHelper)));
                return;
            }

            world.CreateManager<GameObjectInitializationSystem>(creator);

            if (workerGameObject != null)
            {
                if (!entityManager.HasComponent<OnConnected>(workerSystem.WorkerEntity))
                {
                    workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent("You cannot set the Worker " +
                            "GameObject once the World has already started running")
                        .WithField(LoggingUtils.LoggerName, nameof(GameObjectCreationHelper)));
                }
                else
                {
                    world.CreateManager<WorkerEntityGameObjectLinkerSystem>(workerGameObject);
                }
            }
        }

        public static void EnableStandardGameObjectCreation(World world, GameObject workerGameObject = null)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new InvalidOperationException(WorkerNotCreatedErrorMessage);
            }

            workerType = workerSystem.WorkerType;
            workerOrigin = workerSystem.Origin;

            EnableGameObjectCreation(world, StandardGameObjectCreator(), workerGameObject);
        }

        private static IEntityGameObjectCreator StandardGameObjectCreator()
        {
            IEntityGameObjectCreator creator = new GameObjectCreator()
                .PrefabsFrom(MetadataWithCache)
                .PositionFrom(RuntimeCoordinateTranslatedByWorkerOrigin)
                .RotationFrom((e) => Quaternion.identity);

            return new NamedGameObjectCreator(creator, GameObjectNameWithSpatialOSEntityIdAndWorkerType);
        }

        private static GameObject MetadataWithCache(SpatialOSEntity entity)
        {
            if (!entity.HasComponent<Metadata.Component>())
            {
                return null;
            }

            var prefabName = entity.GetComponent<Metadata.Component>().EntityType;
            var workerSpecificPath = Path.Combine("Prefabs", workerType, prefabName);
            var commonPath = Path.Combine("Prefabs", "Common", prefabName);

            if (!cachedPrefabs.TryGetValue(prefabName, out var prefab))
            {
                prefab = Resources.Load<GameObject>(workerSpecificPath);
                if (prefab == null)
                {
                    prefab = Resources.Load<GameObject>(commonPath);
                }

                cachedPrefabs[prefabName] = prefab;
            }

            return prefab;
        }

        private static Vector3 RuntimeCoordinateTranslatedByWorkerOrigin(SpatialOSEntity entity)
        {
            var runtimeCoordinate = entity.GetComponent<Position.Component>();

            var runtimeCoordinateVector = new Vector3(
                (float) runtimeCoordinate.Coords.X,
                (float) runtimeCoordinate.Coords.Y,
                (float) runtimeCoordinate.Coords.Z);

            return workerOrigin + runtimeCoordinateVector;
        }

        private static string GameObjectNameWithSpatialOSEntityIdAndWorkerType(SpatialOSEntity entity, GameObject prefab)
        {
            return $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";
        }
    }
}
