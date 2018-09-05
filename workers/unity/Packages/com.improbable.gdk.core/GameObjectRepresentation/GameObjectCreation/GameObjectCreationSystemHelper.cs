using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectCreationSystemHelper
    {
        public static void EnableStandardGameObjectCreation(World world)
        {
            string workerType = world.GetOrCreateManager<WorkerSystem>().WorkerType;
            EnableStandardGameObjectCreation(world, new GameObjectCreatorFromMetadata(workerType));
        }

        public static void EnableStandardGameObjectCreation(World world, IEntityGameObjectCreator creator)
        {
            world.CreateManager<GameObjectInitializationSystem>(creator);
        }
    }
}
