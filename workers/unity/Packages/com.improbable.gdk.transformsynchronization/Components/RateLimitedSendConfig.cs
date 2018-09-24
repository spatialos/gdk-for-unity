using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct RateLimitedSendConfig : ISharedComponentData
    {
        public float MaxTransformUpdateRateHz;
        public float MaxPositionUpdateRateHz;

        public float MaxSquareDistanceChangedWithoutUpdate;
        public float MaxSquareVelocityChangedWithoutUpdate;
        public float MaxRotationWithoutUpdateDegrees;
        public float MaxTimeForStaleTransformWithoutUpdateS;

        public float MaxSquarePositionChangeWithoutUpdate;
        public float MaxTimeForStalePositionWithoutUpdateS;
    }
}
