using Improbable.Gdk.Core;
using Improbable.Gdk.Core.SceneAuthoring.AuthoringComponents;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.SceneAuthoring
{
    [TestFixture]
    public class PositionFromGameObjectTests
    {
        [Test]
        public void WriteTo_uses_the_GameObject_position()
        {
            var gameObject = new GameObject();
            gameObject.transform.position = new Vector3(100, 100, 100);
            var positionFromGameObject = gameObject.AddComponent<PositionFromGameObjectAuthoringComponent>();

            var entityTemplate = new EntityTemplate();
            positionFromGameObject.WriteTo(entityTemplate);

            Assert.IsTrue(entityTemplate.HasComponent<Position.Snapshot>());

            var position = entityTemplate.GetComponent<Position.Snapshot>().Value;
            Assert.AreEqual(gameObject.transform.position, position.Coords.ToUnityVector());
        }
    }
}
