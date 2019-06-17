using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct BufferedTransform : IBufferElementData
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public UnityEngine.Quaternion Orientation;
        public uint PhysicsTick;
    }
}
