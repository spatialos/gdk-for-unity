using System;
using Unity.Entities;

namespace Improbable.Gdk.Timing
{
    public struct ComponentToAddAtLocalTime : IComparable<ComponentToAddAtLocalTime>
    {
        public Action<EntityCommandBuffer> AddComponentAction;
        public float TimerEndTime;

        public int CompareTo(ComponentToAddAtLocalTime other)
        {
            return TimerEndTime.CompareTo(other.TimerEndTime);
        }

        public static ComponentToAddAtLocalTime Create<T>(T component, Entity entity, float time)
            where T : struct, IComponentData
        {
            return new ComponentToAddAtLocalTime
            {
                AddComponentAction = commandBuffer => commandBuffer.AddComponent(entity, component),
                TimerEndTime = time
            };
        }
    }
}
