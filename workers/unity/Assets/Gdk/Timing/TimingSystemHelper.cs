using Unity.Entities;

namespace Improbable.Gdk.Timing
{
    public static class TimingSystemHelper
    {
        public static void RegisterClientSystems(World world)
        {
            world.GetOrCreateManager<LocalTimerLifecycleSystem>();
            world.GetOrCreateManager<AddComponentAtLocalTimeSystem>();
        }

        public static void RegisterServerSystems(World world)
        {
            world.GetOrCreateManager<LocalTimerLifecycleSystem>();
            world.GetOrCreateManager<AddComponentAtLocalTimeSystem>();
        }
    }
}

