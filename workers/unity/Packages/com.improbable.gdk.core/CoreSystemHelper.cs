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
<<<<<<< HEAD
            world.GetOrCreateManager<GameObjectDispatcherSystem>();
            world.GetOrCreateManager<MonoBehaviourActivationManagerInitializationSystem>();
            world.GetOrCreateManager<ReactiveComponentAdditionSystem>();
=======
>>>>>>> 6ed01da8... Revert "Add ComponentUpdated when a component is added."
        }
    }
}
