using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Send Strategies/Teleport Only")]
    public class TeleportOnlySendStategy : TransformSynchronizationSendStrategy
    {
        internal override void Apply(Entity entity, World world)
        {
            var manager = world.GetOrCreateManager<EntityManager>();
            manager.AddComponent(entity, typeof(TeleportOnlySendTag));
        }
    }
}
