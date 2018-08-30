using Packages.com.improbable.gdk.core.GameObjectRepresentation.GameObjectCreation;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectSystemHelper
    {
        public static IEntityGameObjectCreator EntityGameObjectCreator;

        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<MonoBehaviourActivationManagerInitializationSystem>();
            world.GetOrCreateManager<EntityGameObjectLinkerSystem>();
            world.GetOrCreateManager<GameObjectInitializationSystem>();
        }
    }
}
