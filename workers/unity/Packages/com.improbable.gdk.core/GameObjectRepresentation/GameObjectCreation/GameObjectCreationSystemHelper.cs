using Unity.Entities;

namespace Playground
{
    public static class GameObjectCreationSystemHelper
    {
        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<GameObjectInitializationSystem>();
        }
    }
}
