using Generated.Improbable;
using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    public abstract class ActivationManagerTestBase<TBehaviour> where TBehaviour : Component
    {
        private World world;
        protected GameObject TestGameObject;
        private EntityManager entityManager;
        protected MonoBehaviourActivationManager ActivationManager;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            TestGameObject = new GameObject();

            TestGameObject.AddComponent<TBehaviour>();
            TestGameObject.AddComponent<SpatialOSComponent>().Entity = entityManager.CreateEntity();

            var loggingDispatcher = new LoggingDispatcher();
            var injectableStore = new InjectableStore();
            var requiredFieldInjector = new RequiredFieldInjector(entityManager, loggingDispatcher);

            ActivationManager = new MonoBehaviourActivationManager(TestGameObject,
                requiredFieldInjector, injectableStore, loggingDispatcher);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(TestGameObject);

            world?.Dispose();
            world = null;
        }
    }

    [TestFixture]
    public class NoComponentsNeeded : ActivationManagerTestBase<NoComponentsNeeded.TestBehaviourWithCommandSender>
    {
        private static readonly uint PlayerCreatorComponentId = new PlayerCreator.Component().ComponentId;

        public class TestBehaviourWithCommandSender : MonoBehaviour
        {
            [Require] private PlayerCreator.Requirable.CommandRequestSender commandRequestSender;
            [Require] private PlayerCreator.Requirable.CommandResponseHandler commandResponseHandler;
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

            Object.DestroyImmediate(TestGameObject);
        }

        [Test]
        public void Activate_behaviour_when_authority_is_not_present()
        {
            ActivationManager.AddComponent(PlayerCreatorComponentId);

            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");

            ActivationManager.EnableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called" +
                " even if it has no authority");

            Object.DestroyImmediate(TestGameObject);
        }

        [Test]
        public void Activates_behaviour_when_authority_is_present()
        {
            ActivationManager.AddComponent(PlayerCreatorComponentId);
            ActivationManager.ChangeAuthority(PlayerCreatorComponentId, Authority.Authoritative);

            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be enabled before EnableSpatialOSBehaviours is called");

            ActivationManager.EnableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should be enabled after EnableSpatialOSBehaviours is called");

            Object.DestroyImmediate(TestGameObject);
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_lost()
        {
            ActivationManager.AddComponent(PlayerCreatorComponentId);
            ActivationManager.ChangeAuthority(PlayerCreatorComponentId, Authority.Authoritative);

            ActivationManager.EnableSpatialOSBehaviours();

            ActivationManager.ChangeAuthority(PlayerCreatorComponentId, Authority.NotAuthoritative);

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if authority is lost");

            ActivationManager.DisableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled after DisableSpatialOSBehaviours is called" +
                " even if it has lost authority");

            Object.DestroyImmediate(TestGameObject);
        }

        [Test]
        public void Does_not_deactivate_behaviour_when_authority_is_not_lost()
        {
            ActivationManager.AddComponent(PlayerCreatorComponentId);
            ActivationManager.ChangeAuthority(PlayerCreatorComponentId, Authority.Authoritative);

            ActivationManager.EnableSpatialOSBehaviours();
            ActivationManager.DisableSpatialOSBehaviours();

            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourWithCommandSender>().enabled,
                "Behaviour should not be disabled if it has not lost authority");

            Object.DestroyImmediate(TestGameObject);
        }
    }

    [TestFixture]
    public class Readers : ActivationManagerTestBase<Readers.TestBehaviourWithReader>
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
        }
    }

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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
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

            Object.DestroyImmediate(TestGameObject);
        }
    }

    [TestFixture]
    public class Writers :
        TestForBehaviourThatNeedsAuthority<Writers.TestBehaviourWithWriter>
    {
        protected override uint ComponentId => new Position.Component().ComponentId;

        public class TestBehaviourWithWriter : MonoBehaviour
        {
            [Require] private Position.Requirable.Writer positionWriter;
        }
    }

    [TestFixture]
    public class ReadersAndWriters :
        TestForBehaviourThatNeedsAuthority<ReadersAndWriters.TestBehaviourWithReaderAndWriter>
    {
        protected override uint ComponentId => new Position.Component().ComponentId;

        public class TestBehaviourWithReaderAndWriter : MonoBehaviour
        {
            [Require] private Position.Requirable.Reader positionReader;
            [Require] private Position.Requirable.Writer positionWriter;
        }
    }

    [TestFixture]
    public class CommandHandlers :
        TestForBehaviourThatNeedsAuthority<CommandHandlers.TestBehaviourWithCommandHandlers>
    {
        protected override uint ComponentId => new PlayerCreator.Component().ComponentId;

        public class TestBehaviourWithCommandHandlers : MonoBehaviour
        {
            [Require] private PlayerCreator.Requirable.CommandRequestHandler commandRequestHandler;
        }
    }
}
