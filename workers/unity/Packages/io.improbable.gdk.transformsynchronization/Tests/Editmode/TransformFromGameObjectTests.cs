using Improbable.Gdk.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization.EditmodeTests
{
    [TestFixture]
    public class TransformFromGameObjectTests
    {
        [Test]
        public void WriteTo_uses_the_GameObject_position_and_rotation()
        {
            var gameObject = new GameObject();
            gameObject.transform.position = new Vector3(100, 100, 100);
            gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            var transformFromGameObject = gameObject.AddComponent<TransformFromGameObjectAuthoringComponent>();

            var entityTemplate = new EntityTemplate();
            transformFromGameObject.WriteTo(entityTemplate);

            Assert.IsTrue(entityTemplate.TryGetComponent<TransformInternal.Snapshot>(out var transform));

            var position = transform.Location.ToUnityVector();
            var positionDifference = Vector3.Distance(gameObject.transform.position, position);
            Assert.AreEqual(0.0, positionDifference, float.Epsilon);

            var rotation = transform.Rotation.ToUnityQuaternion();
            var rotationDifference = Quaternion.Angle(gameObject.transform.rotation, rotation);
            Assert.AreEqual(0.0, rotationDifference, float.Epsilon);
        }
    }
}
