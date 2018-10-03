using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Receive Strategies/Teleport Only")]
    public class TeleportOnlyReceiveStategy : TransformSynchronizationReceiveStrategy
    {
        internal override void Apply(Entity entity, World world)
        {
            var manager = world.GetOrCreateManager<EntityManager>();
            manager.AddComponent(entity, typeof(TeleportOnlyReceiveTag));
        }
    }
}
