using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using Improbable.Worker.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class RequestSenderBehaviourActivationTests : ActivationManagerTestBase
    {
        private static readonly uint ComponentId = new ComponentWithNoFieldsWithCommands.Component().ComponentId;

        public class TestBehaviourWithCommandSender : MonoBehaviour
        {
            [Require] public ComponentWithNoFieldsWithCommands.Requirable.CommandRequestSender CommandRequestSender;

            [Require] public ComponentWithNoFieldsWithCommands.Requirable.CommandResponseHandler CommandResponseHandler;

            [Require] public WorldCommands.Requirable.WorldCommandRequestSender WorldCommandRequestSender;
            [Require] public WorldCommands.Requirable.WorldCommandResponseHandler WorldCommandResponseHandler;
        }

        protected override void PopulateBehaviours()
        {
            TestGameObject.AddComponent<TestBehaviourWithCommandSender>();
        }

        private void AssertRequirablesNotNull()
        {
            var testBehaviour = TestGameObject.GetComponent<TestBehaviourWithCommandSender>();
            Assert.IsNotNull(testBehaviour.CommandRequestSender);
            Assert.IsNotNull(testBehaviour.CommandResponseHandler);
            Assert.IsNotNull(testBehaviour.WorldCommandRequestSender);
            Assert.IsNotNull(testBehaviour.WorldCommandResponseHandler);
        }

        [Test]
        public void Activates_behaviour_when_component_is_not_present()
        {
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called" +
                " even if it has no PlayerCreator component");
            AssertRequirablesNotNull();
        }

        [Test]
        public void Activate_behaviour_when_authority_is_not_present()
        {
            ActivationManager.AddComponent(ComponentId);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called" +
                " even if authority is not present");
            AssertRequirablesNotNull();
        }

        [Test]
        public void Activates_behaviour_when_authority_is_present()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called");
            AssertRequirablesNotNull();
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_lost()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.ChangeAuthority(ComponentId, Authority.NotAuthoritative);
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            AssertRequirablesNotNull();
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled if authority is not lost");
            AssertRequirablesNotNull();
        }
    }
}
