using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class WorkerTypeCriteriaTests : MockBase
    {
        private const string WorkerType = "TestWorkerType";
        private const long EntityId = 100;

        protected override MockWorld.Options GetOptions()
        {
            return new MockWorld.Options
            {
                WorkerType = WorkerType
            };
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetEntityTemplate()); })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<MatchingWorkerType>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsTrue(behaviour.enabled);
                });
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetEntityTemplate()); })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<NonMatchingWorkerType>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsFalse(behaviour.enabled);
                });
        }

        [Test]
        public void Monobehaviour_is_disabled_with_matching_WorkerType_but_unsatisfied_requireable()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetEntityTemplate()); })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<MatchingWorkerTypeWithRequireable>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsFalse(behaviour.enabled);
                    Assert.IsNull(behaviour.Writer);
                });
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType_and_satisfied_requireable()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetEntityTemplate());
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<MatchingWorkerTypeWithRequireable>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsTrue(behaviour.enabled);
                    Assert.IsNotNull(behaviour.Writer);
                });
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_unsatisfied_requireable()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetEntityTemplate()); })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<NonMatchingWorkerTypeWithRequireable>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsFalse(behaviour.enabled);
                    Assert.IsNull(behaviour.Writer);
                });
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_satisfied_requireable()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetEntityTemplate());
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<NonMatchingWorkerTypeWithRequireable>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsFalse(behaviour.enabled);
                    Assert.IsNull(behaviour.Writer);
                });
        }

        private static EntityTemplate GetEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            return template;
        }

#pragma warning disable 649

        [WorkerType(WorkerType)]
        private class MatchingWorkerType : MonoBehaviour
        {
        }

        [WorkerType("NotMyWorker")]
        private class NonMatchingWorkerType : MonoBehaviour
        {
        }

        [WorkerType(WorkerType)]
        private class MatchingWorkerTypeWithRequireable : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

        [WorkerType("NotMyWorker")]
        private class NonMatchingWorkerTypeWithRequireable : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

#pragma warning restore 649
    }
}
