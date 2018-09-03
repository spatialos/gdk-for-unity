using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectRepresentationSystemHelper
    {
        public static void AddSystems(World world, IEntityGameObjectCreator gameObjectCreator)
        {
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<MonoBehaviourActivationManagerInitializationSystem>();
            world.GetOrCreateManager<EntityGameObjectLinkerSystem>();
            world.CreateManager<GameObjectInitializationSystem>(gameObjectCreator);
        }
    }
}
