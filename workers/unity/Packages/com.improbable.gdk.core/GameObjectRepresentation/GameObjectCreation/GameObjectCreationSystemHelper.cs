using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectCreationSystemHelper
    {
        public static void AddStandardGameObjectCreation(World world)
        {
            string workerType = world.GetOrCreateManager<WorkerSystem>().WorkerType;
            AddStandardGameObjectCreation(world, new GameObjectCreatorFromMetadata(workerType));
        }

        public static void AddStandardGameObjectCreation(World world, IEntityGameObjectCreator creator)
        {
            world.CreateManager<GameObjectInitializationSystem>(creator);
        }
    }
}
