using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public struct CameraInput : IComponentData
    {
        public float X;
        public float Y;
        public float Distance;
    }

    public struct CameraTransform : IComponentData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public static class CameraComponentDefaults
    {
        public static readonly CameraInput Input = new CameraInput
        {
            X = 0,
            Y = 40f,
            Distance = 5f
        };

        public static readonly CameraTransform Transform = new CameraTransform
        {
            Position = Vector3.zero,
            Rotation = Quaternion.identity
        };
    }
}
