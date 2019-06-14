using System;
using Unity.Mathematics;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformUtils
    {
        // Checking for no change, so exact equality is fine
        public static bool HasChanged(Coordinates a, Coordinates b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        // Checking for no change, so exact equality is fine
        public static bool HasChanged(Location a, Location b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        // Checking for no change, so exact equality is fine
        public static bool HasChanged(CompressedQuaternion a, CompressedQuaternion b)
        {
            return a.Data != b.Data;
        }

        public static bool HasChanged(Velocity a, Velocity b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static UnityEngine.Quaternion ToUnityQuaternion(this CompressedQuaternion quaternion)
        {
            var compressedValue = quaternion.Data;
            var q = new float[4];

            const uint mask = (1u << 9) - 1u;

            int largestIndex = (int) (compressedValue >> 30);
            float sumSquares = 0;
            for (var i = 3; i >= 0; --i)
            {
                if (i != largestIndex)
                {
                    uint magnitude = compressedValue & mask;
                    uint signBit = (compressedValue >> 9) & 0x1;
                    compressedValue = compressedValue >> 10;
                    q[i] = SqrtHalf * ((float) magnitude) / mask;
                    if (signBit == 1)
                    {
                        q[i] *= -1;
                    }

                    sumSquares += Mathf.Pow(q[i], 2);
                }
            }

            q[largestIndex] = Mathf.Sqrt(1f - sumSquares);

            return new Quaternion(q[0], q[1], q[2], q[3]);
        }

        private const float SqrtHalf = 0.70710678118f;

        public static CompressedQuaternion ToImprobableQuaternion(this UnityEngine.Quaternion quaternion)
        {
            quaternion = quaternion.normalized;

            var q = new float[4] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };

            uint largestIndex = 0;
            var largestValue = Mathf.Abs(q[largestIndex]);
            for (uint i = 1; i < 4; ++i)
            {
                var componentAbsolute = Mathf.Abs(q[i]);
                if (componentAbsolute > largestValue)
                {
                    largestIndex = i;
                    largestValue = componentAbsolute;
                }
            }

            uint negativeBit = quaternion[(int) largestIndex] < 0 ? 1u : 0u;

            uint compressedQuaternion = largestIndex;
            for (uint i = 0; i < 4; i++)
            {
                if (i != largestIndex)
                {
                    uint signBit = (q[i] < 0 ? 1u : 0u) ^ negativeBit;
                    uint magnitude = (uint) (((1u << 9) - 1u) * (Mathf.Abs(q[i]) / SqrtHalf) + 0.5f);
                    compressedQuaternion = (compressedQuaternion << 10) | (signBit << 9) | magnitude;
                }
            }

            return new CompressedQuaternion(compressedQuaternion);
        }

        public static UnityEngine.Vector3 ToUnityVector3(this Location location)
        {
            return new Vector3(location.X, location.Y, location.Z);
        }

        public static Location ToImprobableLocation(this Vector3 vector)
        {
            return new Location(vector.x, vector.y, vector.z);
        }

        public static UnityEngine.Vector3 ToUnityVector3(this Velocity velocity)
        {
            return new Vector3(velocity.X, velocity.Y, velocity.Z);
        }

        public static Velocity ToImprobableVelocity(this Vector3 velocity)
        {
            return new Velocity(velocity.x, velocity.y, velocity.z);
        }

        public static Coordinates ToCoordinates(this Location location)
        {
            return new Coordinates(location.X, location.Y, location.Z);
        }
    }
}
