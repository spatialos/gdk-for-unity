using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class CoreSystemHelper
    {
        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<SpatialOSSendSystem>();
            world.GetOrCreateManager<SpatialOSReceiveSystem>();
            world.GetOrCreateManager<CleanReactiveComponentsSystem>();
            world.GetOrCreateManager<WorldCommandsCleanSystem>();
            world.GetOrCreateManager<WorldCommandsSendSystem>();
            world.GetOrCreateManager<CommandRequestTrackerSystem>();
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<MonoBehaviourActivationManagerInitializationSystem>();
        }
    }
}
