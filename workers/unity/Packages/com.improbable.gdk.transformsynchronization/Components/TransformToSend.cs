using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct TransformToSend : IComponentData
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public UnityEngine.Quaternion Orientation;
    }
}
