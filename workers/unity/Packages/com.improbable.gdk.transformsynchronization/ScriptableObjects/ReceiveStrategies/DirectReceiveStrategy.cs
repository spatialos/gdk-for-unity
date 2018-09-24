using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Receive Strategies/Direct")]
    public class DirectReceiveStrategy : TransformSynchronizationReceiveStrategy
    {
        internal override void Apply(Entity entity, World world)
        {
            var manager = world.GetOrCreateManager<EntityManager>();
            manager.AddComponent(entity, typeof(DirectReceiveTag));
        }
    }
}
