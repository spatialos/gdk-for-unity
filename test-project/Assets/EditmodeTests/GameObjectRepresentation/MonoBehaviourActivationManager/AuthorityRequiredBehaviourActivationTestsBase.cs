using Improbable.Worker.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    public abstract class TestForBehaviourThatNeedsAuthority<TBehaviour> : ActivationManagerTestBase<TBehaviour>
        where TBehaviour : MonoBehaviour
    {
        protected abstract uint ComponentId { get; }

        [Test]
        public void Does_not_activate_behaviour_when_component_is_not_present()
        {
            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");

            ActivationManager.EnableSpatialOSBehaviours();

            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it has no transform component");
        }

        [Test]
        public void Does_not_activate_behaviour_when_authority_is_not_present()
        {
            ActivationManager.AddComponent(ComponentId);

            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");

            ActivationManager.EnableSpatialOSBehaviours();

            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it has no authority");
        }

        [Test]
        public void Activates_behaviour_when_authority_is_present()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);

            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");

            ActivationManager.EnableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called");
        }

        [Test]
        public void Deactivates_behaviour_when_authority_is_lost()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);

            ActivationManager.EnableSpatialOSBehaviours();

            ActivationManager.ChangeAuthority(ComponentId, Authority.NotAuthoritative);

            Assert.IsTrue(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");

            ActivationManager.DisableSpatialOSBehaviours();

            Assert.IsFalse(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should be disabled after DisableSpatialOSBehaviours is called");
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(ComponentId);
            ActivationManager.ChangeAuthority(ComponentId, Authority.Authoritative);

            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TBehaviour>().enabled,
                "Behaviour should not be disabled if it has not lost authority");
        }
    }
}