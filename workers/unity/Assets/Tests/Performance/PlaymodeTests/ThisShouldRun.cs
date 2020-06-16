using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Improbable.Gdk.PlaymodePerformanceTests
{
    public class ThisShouldRun : MockBase
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

        [Performance, Test]
        public void MeasurementTest()
        {
            var markers = new[]
            {
                "WorkerSystem.Tick",
                "View.ApplyDiff",
                "EntitySystem.ApplyDiff",
                "EcsViewSystem.ApplyDiff",
                "EcsViewSystem.OnAddEntity"
            };

            var i = EntityId;
            ActionMeasurement.Measure(() =>
                {
                    World.Step(world =>
                        {
                            world.Connection.CreateEntity(++i, GetEntityTemplate());
                        })
                        .Step(world =>
                        {
                            var (_, behaviour) = world.CreateGameObject<TestBehaviour>(i);
                            return behaviour;
                        })
                        .Step((world, behaviour) =>
                        {
                            Assert.IsTrue(behaviour.enabled);
                        });
                })
                .ProfilerMarkers(markers)
                .WarmupCount(5)
                .MeasurementCount(100)
                .Run();

            Assert.Pass();
        }

        private static EntityTemplate GetEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            return template;
        }

#pragma warning disable 649

        [WorkerType(WorkerType)]
        private class TestBehaviour : MonoBehaviour
        {
        }
#pragma warning restore 649
    }
}
