using Unity.Entities;

namespace Improbable.Gdk.Timing
{
    public static class LocalTimer
    {
        public static void AddComponentAtLocalTime<T>(float time, Entity entity, World world)
            where T : struct, IComponentData
        {
            world.GetExistingManager<LocalTimerLifecycleSystem>().AddComponentAtLocalTime(default(T), time, entity);
        }

        public static void AddComponentAtLocalTime<T>(T component, float time, Entity entity, World world)
            where T : struct, IComponentData
        {
            world.GetExistingManager<LocalTimerLifecycleSystem>().AddComponentAtLocalTime(component, time, entity);
        }
    }
}
