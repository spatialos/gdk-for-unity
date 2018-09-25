using Improbable.Worker.Core;
using NUnit.Framework;


namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    using ReaderBehaviour = ReaderBehaviourActivationTests.TestBehaviourWithReader;
    using WriterBehaviour = WriterBehaviourActivationTests.TestBehaviourWithWriter;

    [TestFixture]
    // A fork of the ReaderBehaviourActivationTests, with an additional authority-requiring behaviour.
    public class MultipleBehaviourActivationTests : ActivationManagerTestBase
    {
        private static readonly uint PositionComponentId = new Position.Component().ComponentId;

        protected override void PopulateBehaviours()
        {
            TestGameObject.AddComponent<ReaderBehaviour>();
            TestGameObject.AddComponent<WriterBehaviour>();
        }

        [Test]
        public void Does_not_activate_either_behaviour_when_component_is_not_present()
        {
            Assert.IsFalse(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsFalse(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it has no components");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it has no components");
        }

        [Test]
        public void Activate_only_reader_behaviour_when_authority_is_not_present()
        {
            ActivationManager.AddComponent(PositionComponentId);
            Assert.IsFalse(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should be enabled after EnableSpatialOSBehaviours is called" +
                " even if authority is not present");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be enabled after EnableSpatialOSBehaviours is called" +
                " because authority is not present");
        }

        [Test]
        public void Activates_both_behaviours_when_authority_is_present()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            Assert.IsFalse(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be enabled before EnableSpatialOSBehaviours is called");
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should be enabled after EnableSpatialOSBehaviours is called");
            Assert.IsTrue(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should be enabled after EnableSpatialOSBehaviours is called");
        }

        [Test]
        public void Does_not_deactivate_either_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be disabled if authority is not lost");
            Assert.IsTrue(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be disabled if authority is not lost");
        }

        [Test]
        public void Does_not_deactivate_reader_behaviour_when_authority_is_lost()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.Authoritative);
            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.ChangeAuthority(PositionComponentId, Authority.NotAuthoritative);
            Assert.IsTrue(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be disabled before DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            Assert.IsTrue(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be disabled before DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            ActivationManager.DisableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<ReaderBehaviour>().enabled,
                "Reader Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");
            Assert.IsFalse(TestGameObject.GetComponent<WriterBehaviour>().enabled,
                "Writer Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " after authority is lost");
        }
    }
}
