using Generated.Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct BufferedTransform : IBufferElementData
    {
        public SpatialOSTransform transformUpdate;
    }
}
