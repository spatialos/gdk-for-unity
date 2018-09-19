using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    public static class GameObjectCreationSystemHelper
    {
        public static void EnableStandardGameObjectCreation(World world, IEntityGameObjectCreator creator = null,
            GameObject workerGameObject = null)
        {
            if (creator == null)
            {
                var workerSystem = world.GetOrCreateManager<WorkerSystem>();
                creator = new GameObjectCreatorFromMetadata(workerSystem.WorkerType, workerSystem.Origin,
                    workerSystem.LogDispatcher);
            }

            world.CreateManager<GameObjectInitializationSystem>(creator);
            if (workerGameObject != null)
            {
                world.CreateManager<WorkerEntityGameObjectLinkerSystem>(workerGameObject);
            }
        }
    }
}
