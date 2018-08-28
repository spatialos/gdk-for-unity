using Generated.Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [InternalBufferCapacity(TransformSynchronizationSystemHelper.MaxBufferSize)]
    public struct BufferedTransform : IBufferElementData
    {
        public SpatialOSTransform transformUpdate;
    }
}
