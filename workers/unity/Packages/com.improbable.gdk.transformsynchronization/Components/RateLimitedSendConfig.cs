using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct RateLimitedSendConfig : ISharedComponentData
    {
        public float MaxTransformUpdateRateHz;
        public float MaxPositionUpdateRateHz;
    }
}
