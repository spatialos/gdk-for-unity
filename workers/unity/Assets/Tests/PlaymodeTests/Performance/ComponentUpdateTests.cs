using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.PlaymodeTests
{
    [TestFixture]
    public class ComponentUpdateTests : MockBase
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
        public void SinglePositionUpdate()
        {
            var random = new System.Random(42);
            var markers = new[] { "GatherAllChunks", "ExecuteReplication", "Position" };

            var currentState = World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetEntityTemplate());
                world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            });

            ActionMeasurement.Measure(() =>
                {
                    currentState.Step(world =>
                    {
                        world.Connection.UpdateComponent(EntityId, Position.ComponentId, new Position.Update
                        {
                            Coords = new Coordinates(random.NextDouble(), random.NextDouble(), random.NextDouble())
                        });
                    });
                })
                .ProfilerMarkers(markers)
                .WarmupCount(100)
                .MeasurementCount(1000)
                .Run();
        }

        private static EntityTemplate GetEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), WorkerType);
            return template;
        }
    }
}
