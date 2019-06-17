using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct InterpolationConfig : ISharedComponentData
    {
        public int TargetBufferSize;
        public int MaxLoadMatchedBufferSize;
    }
}
