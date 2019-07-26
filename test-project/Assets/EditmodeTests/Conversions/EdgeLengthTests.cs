using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Conversions
{
    public class EdgeLengthTests
    {
        [Test]
        public void EdgeLength_from_Vector3_has_equal_component_values()
        {
            var vector = new Vector3(10f, 20f, 30f);
            var edgeLength = EdgeLength.FromUnityVector(vector);

            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.X, vector.x);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.Y, vector.y);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.Z, vector.z);
        }

        [Test]
        public void EdgeLength_to_Vector3_has_equal_component_values()
        {
            var edgeLength = new EdgeLength(10f, 20f, 30f);
            var vector = edgeLength.ToUnityVector();

            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.X, vector.x);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.Y, vector.y);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) edgeLength.Z, vector.z);
        }
    }
}
