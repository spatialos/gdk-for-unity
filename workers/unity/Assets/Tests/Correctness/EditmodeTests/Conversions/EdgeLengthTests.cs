using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Improbable.Gdk.EditmodeTests.Conversions
{
    public class EdgeLengthTests
    {
        [Test]
        public void EdgeLength_from_Vector3_has_equal_component_values()
        {
            var vector = new Vector3(10f, 20f, 30f);
            var edgeLength = EdgeLength.FromUnityVector(vector);

            Assert.AreApproximatelyEqual((float) edgeLength.X, vector.x);
            Assert.AreApproximatelyEqual((float) edgeLength.Y, vector.y);
            Assert.AreApproximatelyEqual((float) edgeLength.Z, vector.z);
        }

        [Test]
        public void EdgeLength_to_Vector3_has_equal_component_values()
        {
            var edgeLength = new EdgeLength(10f, 20f, 30f);
            var vector = edgeLength.ToUnityVector();

            Assert.AreApproximatelyEqual((float) edgeLength.X, vector.x);
            Assert.AreApproximatelyEqual((float) edgeLength.Y, vector.y);
            Assert.AreApproximatelyEqual((float) edgeLength.Z, vector.z);
        }
    }
}
