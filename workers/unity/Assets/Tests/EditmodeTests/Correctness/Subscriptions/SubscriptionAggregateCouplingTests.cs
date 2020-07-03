using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    public class SubscriptionAggregateCouplingTests : MockBase
    {
        protected override MockWorld.Options GetOptions()
        {
            var opts = base.GetOptions();

            // Required for the LinkedGameObjectMap subscription to be fulfilled.
            opts.AdditionalSystems = (world) =>
            {
                world.AddSystem(
                    new GameObjectInitializationSystem(
                        new GameObjectCreatorFromMetadata(opts.WorkerType, Vector3.zero, null), null));
            };

            return opts;
        }

        [Test]
        public void All_non_generated_subscription_managers_have_a_test()
        {
            // Find all non-generated subscription manager implementations.
            var subscriptionManagersTypes =
                ReflectionUtility.GetNonAbstractTypesWithBlacklist(typeof(SubscriptionManagerBase),
                    new[] { "Improbable.Gdk.Generated" });

            // Get the subscription payload type.
            var subscriptionManagerContainedTypes = subscriptionManagersTypes
                .Select(type => type.BaseType.GenericTypeArguments[0])
                .ToList();

            var method = typeof(SubscriptionAggregateCouplingTests)
                .GetMethod("DifferentAggregateSubscriptions_should_not_couple_together",
                    BindingFlags.Instance | BindingFlags.Public);

            Assert.NotNull(method);

            // Get the types of the MonoBehaviours from the TestCase attributes
            var typesWithTestCase = method.GetCustomAttributes<TestCaseAttribute>()
                .Where(attr => attr.Arguments.Length == 1 && attr.Arguments[0] is Type)
                .Select(attr => (Type) attr.Arguments[0])
                .ToList();

            // Get the dummy classes defined in TestMonoBehaviours
            var testMonobehaviourTypes = typeof(TestMonoBehaviours)
                .GetNestedTypes()
                .Where(type => !type.IsAbstract)
                .ToList();

            // Filter out the MonoBehaviour types that we don't have a corresponding TestCase attribute for
            // and get the contained type (subscription payload).
            var testMonobehaviourContainedTypes = testMonobehaviourTypes
                .Where(typesWithTestCase.Contains)
                .Select(type => type.BaseType.GenericTypeArguments[0])
                .ToList();

            // Get symmetric difference (everything except for the intersection) of the types.
            var difference = new HashSet<Type>(subscriptionManagerContainedTypes);
            difference.SymmetricExceptWith(testMonobehaviourContainedTypes);

            Assert.IsEmpty(difference, "Subscription manager defined for the following types, but no test found");
        }

        // Workaround due to the lack of support for generic MonoBehaviours. Instead have a concrete implementation
        // that we use. Little bit more boilerplate, but still less than manually writing _everything_.
        // Alternative would be to use Reflection.Emit, but this would fail on IL2CPP platforms (notably iOS).
        private static class TestMonoBehaviours
        {
            public abstract class Base<T> : MonoBehaviour
            {
#pragma warning disable 649
                [Require] public T Requireable;
                [Require] public PositionReader Reader;
#pragma warning restore 649
            }

            public class EntityId : Base<Improbable.Gdk.Core.EntityId>
            {
            }

            public class Entity : Base<Unity.Entities.Entity>
            {
            }

            public class LinkedGameObjectMap : Base<Improbable.Gdk.Subscriptions.LinkedGameObjectMap>
            {
            }

            public class LogDispatcher : Base<ILogDispatcher>
            {
            }

            public class WorkerFlagReader : Base<Improbable.Gdk.Core.WorkerFlagReader>
            {
            }

            public class WorkerId : Base<Improbable.Gdk.Subscriptions.WorkerId>
            {
            }

            public class WorldCommandSender : Base<Improbable.Gdk.Core.WorldCommandSender>
            {
            }

            public class World : Base<Unity.Entities.World>
            {
            }
        }

        [TestCase(typeof(TestMonoBehaviours.EntityId))]
        [TestCase(typeof(TestMonoBehaviours.Entity))]
        [TestCase(typeof(TestMonoBehaviours.LinkedGameObjectMap))]
        [TestCase(typeof(TestMonoBehaviours.LogDispatcher))]
        [TestCase(typeof(TestMonoBehaviours.WorkerFlagReader))]
        [TestCase(typeof(TestMonoBehaviours.WorkerId))]
        [TestCase(typeof(TestMonoBehaviours.WorldCommandSender))]
        [TestCase(typeof(TestMonoBehaviours.World))]
        public void DifferentAggregateSubscriptions_should_not_couple_together(Type requireableType)
        {
            var method = typeof(SubscriptionAggregateCouplingTests)
                .GetMethod("DifferentAggregateSubscriptions_should_not_couple_together_impl",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(method);

            method.MakeGenericMethod(requireableType).Invoke(this, new object[] { });
        }

        // This tests a bug we encountered where two different GameObjects that both had subscriptions that
        // were cached incorrectly would cause the subscriptions to become coupled.
        private void DifferentAggregateSubscriptions_should_not_couple_together_impl<T>() where T : MonoBehaviour
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(1, GetEntityTemplate());
                    world.Connection.CreateEntity(2, GetEntityTemplate());
                })
                .Step(world =>
                {
                    world.CreateGameObject<T>(1);
                    world.CreateGameObject<T>(2);
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(1, Position.ComponentId);
                    world.Connection.RemoveEntity(1);
                    world.Connection.RemoveComponent(2, Position.ComponentId);
                    world.Connection.RemoveEntity(2);
                })
                .Step(world =>
                {
                    world.Connection.CreateEntity(3, GetEntityTemplate());
                    world.Connection.CreateEntity(4, GetEntityTemplate());
                })
                .Step(world =>
                {
                    world.CreateGameObject<T>(3);
                    var (_, behaviour) = world.CreateGameObject<T>(4);
                    return behaviour;
                }).Step(world =>
                {
                    world.Connection.RemoveComponent(3, Position.ComponentId);
                    world.Connection.RemoveEntity(3);
                }).Step((world, behaviour) =>
                {
                    Assert.IsTrue(behaviour.enabled);
                });
        }

        private EntityTemplate GetEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            return template;
        }
    }
}
