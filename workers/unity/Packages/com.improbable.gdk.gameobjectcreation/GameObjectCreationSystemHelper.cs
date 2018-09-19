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
            world.CreateManager<GameObjectInitializationSystem>(creator);
            if (workerGameObject != null)
            {
                world.CreateManager<WorkerEntityGameObjectLinkerSystem>(workerGameObject);
            }
        }
    }
}
