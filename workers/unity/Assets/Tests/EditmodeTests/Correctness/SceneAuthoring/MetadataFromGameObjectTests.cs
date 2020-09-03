using System.Security.Cryptography;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.SceneAuthoring.AuthoringComponents;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.SceneAuthoring
{
    [TestFixture]
    public class MetadataFromGameObjectTests
    {
        [Test]
        public void WriteTo_uses_the_GameObject_name()
        {
            var gameObject = new GameObject();
            gameObject.name = "My Entity";
            var metadataFromGameObject = gameObject.AddComponent<MetadataFromGameObjectAuthoringComponent>();

            var entityTemplate = new EntityTemplate();
            metadataFromGameObject.WriteTo(entityTemplate);

            Assert.IsTrue(entityTemplate.HasComponent<Metadata.Snapshot>());

            var metadata = entityTemplate.GetComponent<Metadata.Snapshot>().Value;
            Assert.AreEqual(gameObject.name, metadata.EntityType);
        }
    }
}
