using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    // Should really be split out into transform and position send strategies
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Send Strategies/Rate Limited")]
    public class RateLimitedSendStrategy : TransformSynchronizationSendStrategy
    {
        public float MaxTransformUpdateRateHz = TransformDefaults.MaxTransformUpdateRateHz;
        public float MaxPositionUpdateRateHz = TransformDefaults.MaxPositionUpdateRateHz;

        public float MaxSquareDistanceChangedWithoutUpdate = TransformDefaults.MaxSquareDistanceChangedWithoutUpdate;
        public float MaxSquareVelocityChangedWithoutUpdate = TransformDefaults.MaxSquareVelocityChangedWithoutUpdate;
        public float MaxRotationWithoutUpdateDegrees = TransformDefaults.MaxPositionUpdateRateHz;
        public float MaxTimeForStaleTransformWithoutUpdateS = TransformDefaults.MaxTimeForStaleTransformWithoutUpdateS;

        public float MaxSquarePositionChangeWithoutUpdate = TransformDefaults.MaxSquarePositionChangeWithoutUpdate;
        public float MaxTimeForStalePositionWithoutUpdateS = TransformDefaults.MaxTimeForStalePositionWithoutUpdateS;

        internal override void Apply(Entity entity, World world)
        {
            var manager = world.GetExistingManager<EntityManager>();

            manager.AddSharedComponentData(entity, new RateLimitedSendConfig
            {
                MaxTransformUpdateRateHz = MaxTransformUpdateRateHz,
                MaxPositionUpdateRateHz = MaxPositionUpdateRateHz,
                MaxSquareDistanceChangedWithoutUpdate = MaxSquareDistanceChangedWithoutUpdate,
                MaxSquareVelocityChangedWithoutUpdate = MaxSquareVelocityChangedWithoutUpdate,
                MaxRotationWithoutUpdateDegrees = MaxRotationWithoutUpdateDegrees,
                MaxTimeForStaleTransformWithoutUpdateS = MaxTimeForStaleTransformWithoutUpdateS,
                MaxSquarePositionChangeWithoutUpdate = MaxSquarePositionChangeWithoutUpdate,
                MaxTimeForStalePositionWithoutUpdateS = MaxTimeForStalePositionWithoutUpdateS
            });
        }
    }
}
