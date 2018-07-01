using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Timing
{
    public struct ActiveTimer : IComponentData
    {
        private uint handle;

        public PriorityQueue<ComponentToAddAtLocalTime> Queue =>
            LocalTimerLifecycleSystem.GetOrCreateComponentTimer(handle);

        public ActiveTimer(uint handle)
        {
            this.handle = handle;
        }
    }

    public struct PersistantLocalTimerHandle : ISystemStateComponentData
    {
        public uint Handle;
    }

    public struct HasHadLocalTimerTag : IComponentData
    {
    }

    public struct CheckForEmptyTimer : IComponentData
    {
    }
}

