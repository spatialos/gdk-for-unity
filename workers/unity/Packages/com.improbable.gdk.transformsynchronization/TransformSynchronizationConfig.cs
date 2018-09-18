namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSynchronizationConfig
    {
        public const int MaxLoadMatchedBufferSize = 15;
        public const int TargetLoadMatchedBufferSize = 10;
        public const int MaxTickSmearFactor = 1;

        public const float MaxTransformUpdateRateHz = 30.0f;
        public const float MaxPositionUpdateRateHz = 2.0f;

        public const float MaxSquareDisatanceChangedWithoutUpdate = 0.01f;
        public const float MaxRotationWithoutUpdateDegrees = 0.1f;
        public const float MaxTimeForStaleTransformWithoutUpdateS = 0.5f;

        public const float MaxSquarePositionChangeWithoutUpdate = 1.0f;
        public const float MaxTimeForStalePositionWithoutUpdateS = 1.0f;
    }
}
