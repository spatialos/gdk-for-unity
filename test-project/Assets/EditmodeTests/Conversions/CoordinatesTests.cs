using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Conversions
{
    public class CoordinatesTests
    {
        [Test]
        public void Coordinates_from_Vector3_has_equal_component_values()
        {
            var vector = new Vector3(10f, 20f, 30f);
            var coords = Coordinates.FromUnityVector(vector);

            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.X, vector.x);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.Y, vector.y);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.Z, vector.z);
        }

        [Test]
        public void Coordinates_to_Vector3_has_equal_component_values()
        {
            var coords = new Coordinates(10f, 20f, 30f);
            var vector = coords.ToUnityVector();

            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.X, vector.x);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.Y, vector.y);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual((float) coords.Z, vector.z);
        }
    }
}
