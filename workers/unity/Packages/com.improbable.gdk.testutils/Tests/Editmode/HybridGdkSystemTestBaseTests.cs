using System;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TestUtils.EditmodeTests
{
    public class HybridGdkSystemTestBaseTests
    {
        private struct TestPreparation : IComponentData
        {
            public int Value;
        }

        [DisableAutoCreation]
        private class ExampleHybridSystem : ComponentSystem
        {
            private EntityQuery testGroup;

            protected override void OnCreate()
            {
                base.OnCreate();

                testGroup = GetEntityQuery(
                    ComponentType.ReadWrite<Rigidbody>(),
                    ComponentType.ReadWrite<TestPreparation>()
                );
            }

            protected override void OnUpdate()
            {
                Entities.With(testGroup).ForEach((ref TestPreparation testPreparation) =>
                {
                    testPreparation.Value = 1;
                });
            }
        }

        [TestFixture]
        private class FixtureImplementingHybridTestBase
        {
            [Test]
            public static void ExampleHybridSystem_can_be_created_and_updated()
            {
                using (var world = new World("test-world"))
                {
                    var entityManager = world.EntityManager;

                    var entity = entityManager.CreateEntity(
                        typeof(Rigidbody)
                    );

                    entityManager.AddComponentData(entity, new TestPreparation { Value = 0 });

                    var testSystem = world.GetOrCreateSystem<ExampleHybridSystem>();

                    testSystem.Update();

                    Assert.AreEqual(1, entityManager.GetComponentData<TestPreparation>(entity).Value,
                        "The system did not process the TestPreparation component of the entity.");
                }
            }
        }
    }
}
