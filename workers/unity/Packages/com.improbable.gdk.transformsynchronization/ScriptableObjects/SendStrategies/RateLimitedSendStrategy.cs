using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Send Strategies/Rate Limited")]
    public class RateLimitedSendStrategy : TransformSynchronizationSendStrategy
    {
        public float MaxTransformUpdateRateHz = TransformDefaults.MaxTransformUpdateRateHz;
        public float MaxPositionUpdateRateHz = TransformDefaults.MaxPositionUpdateRateHz;

        internal override void Apply(Entity entity, World world, EntityCommandBuffer commandBuffer)
        {
            var manager = world.GetExistingManager<EntityManager>();
            manager.AddSharedComponentData(entity, new RateLimitedSendConfig
            {
                MaxTransformUpdateRateHz = MaxTransformUpdateRateHz,
                MaxPositionUpdateRateHz = MaxPositionUpdateRateHz,
            });
        }
    }
}
