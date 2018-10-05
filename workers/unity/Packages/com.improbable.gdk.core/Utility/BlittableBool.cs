using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A blittable bool implementation to use in Unity's ECS.
    /// </summary>
    /// <remarks>
    ///     Can be used in place of bool where a blittable type is needed.
    /// </remarks>
    public struct BlittableBool : IEquatable<BlittableBool>
    {
        private readonly byte value;

        public BlittableBool(bool value)
        {
            this.value = Convert.ToByte(value);
        }

        public static implicit operator bool(BlittableBool blittableBool)
        {
            return blittableBool.value != 0;
        }

        public static implicit operator BlittableBool(bool value)
        {
            return new BlittableBool(value);
        }

        public bool Equals(BlittableBool other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is BlittableBool b && Equals(b);
        }

        public override int GetHashCode()
        {
            return value;
        }

        public static bool operator ==(BlittableBool left, BlittableBool right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlittableBool left, BlittableBool right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return ((bool) this).ToString();
        }
    }
}
