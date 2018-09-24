namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformDefaults
    {
        public const int InterpolationTargetBufferSize = 10;
        public const int InterpolationMaxBufferSize = 20;

        public const int MaxTickSmearFactor = 1;

        public const float MaxTransformUpdateRateHz = 30.0f;
        public const float MaxPositionUpdateRateHz = 1.0f;

        public const float MaxSquareDistanceChangedWithoutUpdate = 0.01f;
        public const float MaxSquareVelocityChangedWithoutUpdate = 0.01f;
        public const float MaxRotationWithoutUpdateDegrees = 0.1f;
        public const float MaxTimeForStaleTransformWithoutUpdateS = 0.5f;

        public const float MaxSquarePositionChangeWithoutUpdate = 1.0f;
        public const float MaxTimeForStalePositionWithoutUpdateS = 1.0f;
    }
}
