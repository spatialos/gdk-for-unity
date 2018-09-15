using Generated.Improbable;
using Improbable.Worker.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class ReaderBehaviourActivationTests : ActivationManagerTestBase<ReaderBehaviourActivationTests.TestBehaviourWithReader>
    {
        private static readonly uint PositionComponentId = new Position.Component().ComponentId;

        public class TestBehaviourWithReader : MonoBehaviour
        {
            [Require] private Position.Requirable.Reader positionReader;
        }

        [Test]
        public void Does_not_activate_behaviour_when_component_is_not_present()
        {
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it has no transform component");
        }

        [Test]
        public void Activate_behaviour_when_authority_is_not_present()
        {
            ActivationManager.AddComponent(PositionComponentId);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called" +
                " even if it has no authority");
        }

        [Test]
        public void Activates_behaviour_when_authority_is_present()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called");
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_lost()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.NotAuthoritative);
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if it has lost authority");
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithReader>().enabled,
                "Behaviour should not be disabled if it has not lost authority");
        }
    }
}
