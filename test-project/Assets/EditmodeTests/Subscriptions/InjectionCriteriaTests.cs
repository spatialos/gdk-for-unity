using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using Improbable.TestSchema;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This tests the injection criteria for a reader or writer.
    /// </summary>
    [TestFixture]
    public class ReaderWriterInjectionCriteriaTests : MockBase
    {
        private const long EntityId = 100;

        [Test]
        public void Reader_is_not_injected_when_entity_component_is_not_checked_out()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<ExhaustiveSingularReaderBehaviour>(EntityId);
                    return readerBehaviour;
                })
                .Step((world, reader) =>
                {
                    Assert.IsFalse(reader.enabled);
                    Assert.IsNull(reader.Reader);
                });
        }

        [Test]
        public void Reader_is_injected_when_entity_component_is_checked_out()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<PositionReaderBehaviour>(EntityId);
                    return readerBehaviour;
                })
                .Step((world, readerBehaviour) =>
                {
                    Assert.IsTrue(readerBehaviour.enabled);
                    Assert.IsNotNull(readerBehaviour.Reader);
                });
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_not_checked_out()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, writerBehaviour) = world.CreateGameObject<ExhaustiveSingularWriterBehaviour>(EntityId);
                    return writerBehaviour;
                })
                .Step((world, writerBehaviour) =>
                {
                    Assert.IsFalse(writerBehaviour.enabled);
                    Assert.IsNull(writerBehaviour.Writer);
                });
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_checked_out_and_unauthoritative()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, writerBehaviour) = world.CreateGameObject<PositionWriterBehaviour>(EntityId);
                    return writerBehaviour;
                })
                .Step((world, writerBehaviour) =>
                {
                    Assert.IsFalse(writerBehaviour.enabled);
                    Assert.IsNull(writerBehaviour.Writer);
                });
        }

        [Test]
        public void Writer_is_injected_when_entity_component_is_checked_out_and_authoritative()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, BasicEntity());
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, writerBehaviour) = world.CreateGameObject<PositionWriterBehaviour>(EntityId);
                    return writerBehaviour;
                })
                .Step((world, writerBehaviour) =>
                {
                    Assert.IsTrue(writerBehaviour.enabled);
                    Assert.IsNotNull(writerBehaviour.Writer);
                });
        }

        [Test]
        public void Injection_does_not_happen_if_not_all_constraints_are_satisfied()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, compositeBehaviour) = world.CreateGameObject<CompositeBehaviour>(EntityId);
                    return compositeBehaviour;
                })
                .Step((world, compositeBehaviour) =>
                {
                    Assert.IsFalse(compositeBehaviour.enabled);
                    Assert.IsNull(compositeBehaviour.Reader);
                    Assert.IsNull(compositeBehaviour.Writer);
                });
        }

        [Test]
        public void Injection_happens_if_all_constraints_are_satisfied()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, BasicEntity());
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, compositeBehaviour) = world.CreateGameObject<CompositeBehaviour>(EntityId);
                    return compositeBehaviour;
                })
                .Step((world, compositeBehaviour) =>
                {
                    Assert.IsTrue(compositeBehaviour.enabled);
                    Assert.IsNotNull(compositeBehaviour.Reader);
                    Assert.IsNotNull(compositeBehaviour.Writer);
                });
        }

        [Test]
        public void Injection_happens_if_inherited_constraints_are_satisfied()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, BasicEntity()); })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<InheritanceBehaviour>(EntityId);
                    return readerBehaviour;
                })
                .Step((world, readerBehaviour) =>
                {
                    Assert.IsTrue(readerBehaviour.enabled);
                    Assert.IsNotNull(readerBehaviour.OwnReader);
                });
        }

        private static EntityTemplate BasicEntity()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            return template;
        }

#pragma warning disable 649
        private class InheritanceBehaviour : BaseBehaviour
        {
            public PositionReader OwnReader => Reader;
        }

        private class BaseBehaviour : MonoBehaviour
        {
            [Require] protected PositionReader Reader;
        }

        private class PositionReaderBehaviour : MonoBehaviour
        {
            [Require] public PositionReader Reader;
        }

        private class PositionWriterBehaviour : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

        private class ExhaustiveSingularReaderBehaviour : MonoBehaviour
        {
            [Require] public ExhaustiveSingularReader Reader;
        }

        private class ExhaustiveSingularWriterBehaviour : MonoBehaviour
        {
            [Require] public ExhaustiveSingularWriter Writer;
        }

        private class CompositeBehaviour : MonoBehaviour
        {
            [Require] public PositionReader Reader;
            [Require] public PositionWriter Writer;
        }
#pragma warning restore 649
    }
}
