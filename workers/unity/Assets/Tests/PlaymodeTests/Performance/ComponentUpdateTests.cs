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

        [Performance, Test]
        public void SinglePositionUpdate()
        {
            var markers = new[] { "GatherAllChunks", "ExecuteReplication", "Position.SendUpdates" };

            var currentState = World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetEntityTemplate());
                world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            });

            ActionMeasurement.Measure(() =>
                {
                    currentState.Step(world =>
                    {
                        var workerSystem = world.Worker.World.GetExistingSystem<WorkerSystem>();
                        var entity = workerSystem.GetEntity(new EntityId(EntityId));
                        var position = workerSystem.EntityManager.GetComponentData<Position.Component>(entity);

                        position.Coords = new Coordinates(3006, 42, 2020);
                        workerSystem.EntityManager.SetComponentData(entity, position);
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
