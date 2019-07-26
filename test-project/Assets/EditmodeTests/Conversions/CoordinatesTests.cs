using System;
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

            Assert.IsTrue(Math.Abs(coords.X - vector.x) < float.Epsilon);
            Assert.IsTrue(Math.Abs(coords.Y - vector.y) < float.Epsilon);
            Assert.IsTrue(Math.Abs(coords.Z - vector.z) < float.Epsilon);
        }

        [Test]
        public void Coordinates_to_Vector3_has_equal_component_values()
        {
            var coords = new Coordinates(10f, 20f, 30f);
            var vector = coords.ToUnityVector();

            Assert.IsTrue(Math.Abs(coords.X - vector.x) < float.Epsilon);
            Assert.IsTrue(Math.Abs(coords.Y - vector.y) < float.Epsilon);
            Assert.IsTrue(Math.Abs(coords.Z - vector.z) < float.Epsilon);
        }
    }
}
