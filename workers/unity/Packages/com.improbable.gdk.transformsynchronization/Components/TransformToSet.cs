using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct TransformToSet : IComponentData
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Quaternion Orientation;
        public uint ApproximateRemoteTick;
    }
}
