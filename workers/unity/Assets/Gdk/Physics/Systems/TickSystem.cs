using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(TransformSynchronizationGroup))]
    public class TickSystem : ComponentSystem
    {
        public uint GlobalTick = 0;

        protected override void OnUpdate()
        {
            GlobalTick++;
        }
    }
}
