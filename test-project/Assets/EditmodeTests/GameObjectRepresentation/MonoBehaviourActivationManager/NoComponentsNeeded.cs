using Generated.Improbable.Gdk.Tests.ComponentsWithNoFields;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class NoComponentsNeeded : ActivationManagerTestBase<NoComponentsNeeded.TestBehaviourWithCommandSender>
    {
        private static readonly uint ComponentId = new ComponentWithNoFieldsWithCommands.Component().ComponentId;

        public class TestBehaviourWithCommandSender : MonoBehaviour
        {
            [Require] private ComponentWithNoFieldsWithCommands.Requirable.CommandRequestSender commandRequestSender;

            [Require]
            private ComponentWithNoFieldsWithCommands.Requirable.CommandResponseHandler commandResponseHandler;

            [Require] private WorldCommands.Requirable.WorldCommandRequestSender worldCommandRequestSender;
            [Require] private WorldCommands.Requirable.WorldCommandResponseHandler worldCommandResponseHandler;
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
                " even if it has no authority");
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
                " even if it has lost authority");
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);

            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled if it has not lost authority");
        }
    }
}