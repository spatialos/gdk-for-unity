using Unity.Entities;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Helper methods for the GameObjectRepresentation module.
    /// </summary>
    public static class GameObjectRepresentationHelper
    {
        /// <summary>
        ///     Adds all systems for the GameObjectRepresentation module to an ECS World.
        /// </summary>
        /// <param name="world">The world to add the systems to.</param>
        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<EntityGameObjectLinkerSystem>();
            world.GetOrCreateManager<GameObjectWorldCommandSystem>();
        }
    }
}
