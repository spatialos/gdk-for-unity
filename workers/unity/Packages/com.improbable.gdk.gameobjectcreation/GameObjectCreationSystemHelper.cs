using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    public static class GameObjectCreationSystemHelper
    {
        public static void EnableStandardGameObjectCreation(World world, GameObject workerGameObject = null)
        {
            var workerSystem = world.GetOrCreateManager<WorkerSystem>();
            var creator = new GameObjectCreatorFromMetadata(workerSystem.WorkerType, workerSystem.Origin,
                workerSystem.LogDispatcher);
            EnableStandardGameObjectCreation(world, creator, workerGameObject);
        }

        public static void EnableStandardGameObjectCreation(World world, IEntityGameObjectCreator creator,
            GameObject workerGameObject = null)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            var entityManager = world.GetOrCreateManager<EntityManager>();
            world.CreateManager<GameObjectInitializationSystem>(creator);
            if (workerGameObject != null)
            {
                if (world.GetExistingManager<WorkerEntityGameObjectLinkerSystem>() != null)
                {
                    workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "You have already added a worker GameObject, calling it the second time will be ignored")
                        .WithField(LoggingUtils.LoggerName, nameof(GameObjectCreationSystemHelper)));
                }
                else if (!entityManager.HasComponent<OnConnected>(workerSystem.WorkerEntity))
                {
                    workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent("You cannot set the Worker " +
                            "GameObject once the World has already started running")
                        .WithField(LoggingUtils.LoggerName, nameof(GameObjectCreationSystemHelper)));
                }
                else
                {
                    world.CreateManager<WorkerEntityGameObjectLinkerSystem>(workerGameObject);
                }
            }
        }
    }
}
