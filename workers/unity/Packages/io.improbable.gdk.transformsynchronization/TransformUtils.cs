﻿using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    /// <summary>
    ///     A collection of utility functions for use with the Transform Synchronization Feature Module.
    /// </summary>
    public static class TransformUtils
    {
        /// <summary>
        ///     Utility method for creating a TransformInternal Snapshot.
        /// </summary>
        /// <param name="location">
        ///     The location of an entity, given as a Unity Vector3.
        /// </param>
        /// <param name="rotation">
        ///     The rotation of an entity, given as a Unity Quaternion.
        /// </param>
        /// <param name="velocity">
        ///     The velocity of an entity, given as a Unity Vector3.
        /// </param>
        /// <remarks>
        ///     This method populates a TransformInternal with compressed representations of the given arguments.
        /// </remarks>
        public static TransformInternal.Snapshot CreateTransformSnapshot(
            Vector3 location = default,
            Quaternion rotation = default,
            Vector3 velocity = default)
        {
            return new TransformInternal.Snapshot
            {
                Location = FixedPointVector3.FromUnityVector(location),
                Rotation = ToCompressedQuaternion(rotation),
                Velocity = FixedPointVector3.FromUnityVector(velocity),
                TicksPerSecond = 1f / Time.fixedDeltaTime
            };
        }

        /// <summary>
        ///     Utility method for creating a TransformInternal Snapshot.
        /// </summary>
        /// <param name="location">
        ///     The location of an entity, given as Improbable Coordinates.
        /// </param>
        /// <param name="rotation">
        ///     The rotation of an entity, given as a Unity Quaternion.
        /// </param>
        /// <param name="velocity">
        ///     The velocity of an entity, given as a Unity Vector3.
        /// </param>
        /// <remarks>
        ///     This method populates a TransformInternal with compressed representations of the given arguments.
        /// </remarks>
        public static TransformInternal.Snapshot CreateTransformSnapshot(
            Coordinates location = default,
            Quaternion rotation = default,
            Vector3 velocity = default)
        {
            return new TransformInternal.Snapshot
            {
                Location = FixedPointVector3.FromCoordinates(location),
                Rotation = ToCompressedQuaternion(rotation),
                Velocity = FixedPointVector3.FromUnityVector(velocity),
                TicksPerSecond = 1f / Time.fixedDeltaTime
            };
        }

        /// <summary>
        ///     Returns whether two CompressedQuaternion variables are different.
        /// </summary>
        internal static bool HasChanged(CompressedQuaternion a, CompressedQuaternion b)
        {
            return a.Data != b.Data;
        }

        /// <summary>
        ///     Decompresses a quaternion from a packed uint32 to an unpacked Unity Quaternion.
        /// </summary>
        /// <param name="compressedQuaternion">
        ///     The CompressedQuaternion to decompress.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         The underlying uint contains the "smallest three" components of a quaternion. The first two bits of
        ///         of the uint encode which component is the largest value. This component's value is then computed as
        ///         1 minus the sum of the squares of the smallest three components.
        ///     </para>
        ///     <para>
        ///         This method is marked as unsafe due to the use of stack allocation.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     A decompressed Unity Quaternion.
        /// </returns>
        internal static unsafe UnityEngine.Quaternion ToUnityQuaternion(CompressedQuaternion compressedQuaternion)
        {
            // The raw uint representing a compressed quaternion.
            var compressedValue = compressedQuaternion.Data;

            // Stack allocate float array to ensure it's discarded when the method returns.
            // q[x, y, z, w]
            var q = stackalloc float[4];

            // Mask of 23 0's and 9 1's.
            const uint mask = (1u << 9) - 1u;

            // Only need the two leftmost bits to find the index of the largest component.
            int largestIndex = (int) (compressedValue >> 30);
            float sumSquares = 0;
            for (var i = 3; i >= 0; --i)
            {
                if (i != largestIndex)
                {
                    // Apply mask to return the 9 bits representing a component's value.
                    uint magnitude = compressedValue & mask;

                    // Get the 10th bit from the right (the signbit of the component).
                    uint signBit = (compressedValue >> 9) & 0x1;

                    // Convert back from the range [0,1] to [0, 1/sqrt(2)].
                    q[i] = SqrtHalf * ((float) magnitude) / mask;

                    // If signbit is set, negate the value.
                    if (signBit == 1)
                    {
                        q[i] *= -1;
                    }

                    // Add to the rolling sum of each component's square value.
                    sumSquares += Mathf.Pow(q[i], 2);

                    // Shift right by 10 so that the next component's bits are evaluated in the next loop iteration.
                    compressedValue >>= 10;
                }
            }

            // The value of the largest component is 1 - the sum of the squares of the smallest three components.
            q[largestIndex] = Mathf.Sqrt(1f - sumSquares);

            return new Quaternion(q[0], q[1], q[2], q[3]);
        }

        private const float SqrtHalf = 0.70710678118f;

        /// <summary>
        ///     Compresses a quaternion from an unpacked Unity Quaternion to a packed uint32.
        /// </summary>
        /// <param name="quaternion">
        ///     The Unity Quaternion to compress.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         A quaternion's length must equal 1, so the largest value can be dropped and recalculated later using
        ///         the "smallest three" trick. The target type is a uint, therefore 2 bits are budgeted for the index
        ///         of the largest component and 10 bits per smallest component (including sign bit).
        ///     </para>
        ///     <para>
        ///         This method is marked as unsafe due to the use of stack allocation.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     A uint32 representation of a quaternion.
        /// </returns>
        internal static unsafe CompressedQuaternion ToCompressedQuaternion(UnityEngine.Quaternion quaternion)
        {
            // Ensure we have a unit quaternion before compression.
            quaternion = quaternion.normalized;

            // Stack allocate float array to ensure it's discarded when the method returns.
            var q = stackalloc float[4] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };

            // Iterate through quaternion to find the index of the largest component.
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

            // Since -q == q, transform the quaternion so that the largest component is positive. This means the sign
            // bit of the largest component does not need to be sent.
            uint negativeBit = quaternion[(int) largestIndex] < 0 ? 1u : 0u;

            // Initialise a uint with the index of the largest component. The variable is shifted left after each
            // section of the uint is populated. At the end of the loop, the uint has the following structure:
            // |largest index (2)|signbit (1) + component (9)|signbit (1) + component (9)|signbit (1) + component (9)|
            uint compressedQuaternion = largestIndex;
            for (uint i = 0; i < 4; i++)
            {
                if (i != largestIndex)
                {
                    // If quaternion needs to be transformed, flip the sign bit.
                    uint signBit = (q[i] < 0 ? 1u : 0u) ^ negativeBit;

                    // The maximum possible value of the second largest component in a unit quaternion is 1/sqrt(2), so
                    // translate the value from the range [0, 1/sqrt(2)] to [0, 1] for higher precision. Add 0.5f for
                    // rounding up the value before casting to a uint.
                    uint magnitude = (uint) (((1u << 9) - 1u) * (Mathf.Abs(q[i]) / SqrtHalf) + 0.5f);

                    // Shift uint by 10 bits then place the component's sign bit and 9-bit magnitude in the gap.
                    compressedQuaternion = (compressedQuaternion << 10) | (signBit << 9) | magnitude;
                }
            }

            return new CompressedQuaternion(compressedQuaternion);
        }
    }
}
