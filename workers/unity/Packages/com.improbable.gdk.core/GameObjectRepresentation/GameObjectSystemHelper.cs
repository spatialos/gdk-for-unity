using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectSystemHelper
    {
        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<MonoBehaviourActivationManagerInitializationSystem>();
            world.GetOrCreateManager<EntityGameObjectLinkerSystem>();
        }
    }
}
