using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    public static class GameObjectCreationSystemHelper
    {
        public static void EnableStandardGameObjectCreation(World world)
        {
            var workerSystem = world.GetOrCreateManager<WorkerSystem>();
            EnableStandardGameObjectCreation(world, new GameObjectCreatorFromMetadata(workerSystem.WorkerType,
                workerSystem.Origin, workerSystem.LogDispatcher));
        }

        public static void EnableStandardGameObjectCreation(World world, IEntityGameObjectCreator creator)
        {
            world.CreateManager<GameObjectInitializationSystem>(creator);
        }
    }
}
