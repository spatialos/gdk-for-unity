using Improbable.Transform;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformUtils
    {
        // Can avoid these distance methods by having them at the intermediate layer rather than a custom send system
        // Should leave the custom send system for purely time based things
        public static float SquareDistance(Coordinates a, Coordinates b)
        {
            return (float) ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z));
        }

        public static float SquareDistance(Location a, Location b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);
        }

        public static float SquareDistance(Velocity a, Velocity b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);
        }

        public static Quaternion ToUnityQuaternion(this Improbable.Transform.Quaternion quaternion)
        {
            return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }

        public static Improbable.Transform.Quaternion ToImprobableQuaternion(this Quaternion quaternion)
        {
            return new Improbable.Transform.Quaternion(quaternion.w, quaternion.x, quaternion.y,
                quaternion.z);
        }

        public static Vector3 ToUnityVector3(this Location location)
        {
            return new Vector3(location.X, location.Y, location.Z);
        }

        public static Location ToImprobableLocation(this Vector3 vector)
        {
            return new Location(vector.x, vector.y, vector.z);
        }

        public static Vector3 ToUnityVector3(this Velocity velocity)
        {
            return new Vector3(velocity.X, velocity.Y, velocity.Z);
        }

        public static Velocity ToImprobableVelocity(this Vector3 velocity)
        {
            return new Velocity(velocity.x, velocity.y, velocity.z);
        }
    }
}
