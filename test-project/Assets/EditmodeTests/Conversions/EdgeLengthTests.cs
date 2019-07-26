using System;
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

            Assert.IsTrue(Math.Abs(edgeLength.X - vector.x) < float.Epsilon);
            Assert.IsTrue(Math.Abs(edgeLength.Y - vector.y) < float.Epsilon);
            Assert.IsTrue(Math.Abs(edgeLength.Z - vector.z) < float.Epsilon);
        }

        [Test]
        public void EdgeLength_to_Vector3_has_equal_component_values()
        {
            var edgeLength = new EdgeLength(10f, 20f, 30f);
            var vector = edgeLength.ToUnityVector();

            Assert.IsTrue(Math.Abs(edgeLength.X - vector.x) < float.Epsilon);
            Assert.IsTrue(Math.Abs(edgeLength.Y - vector.y) < float.Epsilon);
            Assert.IsTrue(Math.Abs(edgeLength.Z - vector.z) < float.Epsilon);
        }
    }
}
