namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformDefaults
    {
        public const int InterpolationTargetBufferSize = 10;
        public const int InterpolationMaxBufferSize = 20;

        public const int MaxTickSmearFactor = 1;

        public const float MaxTransformUpdateRateHz = 30.0f;
        public const float MaxPositionUpdateRateHz = 1.0f;
    }
}
