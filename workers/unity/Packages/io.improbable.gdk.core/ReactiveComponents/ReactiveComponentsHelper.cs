#if !DISABLE_REACTIVE_COMPONENTS
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
            world.GetOrCreateSystem<ReactiveComponentSendSystem>();
            world.GetOrCreateSystem<ReactiveCommandComponentSystem>();
            world.GetOrCreateSystem<ReactiveComponentSystem>();
            world.GetOrCreateSystem<CommandSenderComponentSystem>();
            world.GetOrCreateSystem<AcknowledgeAuthorityLossSystem>();
            world.GetOrCreateSystem<CleanReactiveComponentsSystem>();
            world.GetOrCreateSystem<WorldCommandsCleanSystem>();
            world.GetOrCreateSystem<WorldCommandsSendSystem>();
        }
    }
}
#endif
