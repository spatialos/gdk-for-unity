using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Receive Strategies/Direct")]
    public class DirectReceiveStrategy : TransformSynchronizationReceiveStrategy
    {
        internal override void Apply(Entity entity, World world, EntityCommandBuffer commandBuffer)
        {
            commandBuffer.AddComponent(entity, new DirectReceiveTag());
        }

        internal override void Remove(Entity entity, World world, EntityCommandBuffer commandBuffer)
        {
            commandBuffer.RemoveComponent<DirectReceiveTag>(entity);
        }
    }
}
