using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public static class ReactiveComponentsHelper
    {
        public static void AddClientSystems(World world)
        {
            AddCommonSystems(world);
        }

        public static void AddServerSystems(World world)
        {
            AddCommonSystems(world);
        }

        internal static void AddCommonSystems(World world)
        {
            world.GetOrCreateManager<ReactiveComponentSendSystem>();
            world.GetOrCreateManager<ReactiveCommandComponentSystem>();
            world.GetOrCreateManager<ReactiveComponentSystem>();
            world.GetOrCreateManager<CommandSenderComponentSystem>();
            world.GetOrCreateManager<AcknowledgeAuthorityLossSystem>();
            world.GetOrCreateManager<CleanReactiveComponentsSystem>();
            world.GetOrCreateManager<WorldCommandsCleanSystem>();
            world.GetOrCreateManager<WorldCommandsSendSystem>();
        }
    }
}
