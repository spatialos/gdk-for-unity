using Improbable;
using NUnit.Framework;
using UnityEngine;

namespace EditmodeTests.Conversions
{
    public class CoordinatesTests
    {
        [Test]
        public void Coordinates_from_Vector3_has_equal_component_values()
        {
            var v1 = new Vector3(10, 20, 30);
            var c1 = Coordinates.FromUnityVector(v1);

            Assert.IsTrue(c1.X == v1.x);
            Assert.IsTrue(c1.Y == v1.y);
            Assert.IsTrue(c1.Z == v1.z);
        }

        [Test]
        public void Coordinates_to_Vector3_has_equal_component_values()
        {
            var c1 = new Coordinates(10, 20, 30);
            var v1 = c1.ToUnityVector();

            Assert.IsTrue(c1.X == v1.x);
            Assert.IsTrue(c1.Y == v1.y);
            Assert.IsTrue(c1.Z == v1.z);
        }
    }
}
