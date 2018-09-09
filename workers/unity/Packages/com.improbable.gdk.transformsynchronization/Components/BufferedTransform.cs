using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [InternalBufferCapacity(TransformSynchronizationConfig.MaxLoadMatchedBufferSize)]
    public struct BufferedTransform : IBufferElementData
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Quaternion Orientation;
        public uint PhysicsTick;
    }
}
