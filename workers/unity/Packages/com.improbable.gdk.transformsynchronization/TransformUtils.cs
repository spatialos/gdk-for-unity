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
        public static bool HasChanged(Quaternion a, Quaternion b)
        {
            return a.W != b.W || a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static UnityEngine.Quaternion ToUnityQuaternion(this Quaternion quaternion)
        {
            return new UnityEngine.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }

        public static Quaternion ToImprobableQuaternion(this UnityEngine.Quaternion quaternion)
        {
            return new Quaternion(quaternion.w, quaternion.x, quaternion.y,
                quaternion.z);
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
