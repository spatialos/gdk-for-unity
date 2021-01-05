using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using Improbable.Generated;
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
                EnableSerialization = true
            };
        }

        [Performance, Test]
        public void SinglePositionUpdate()
        {
            var markers = new[]
            {
                "SpatialOSSendSystem.CompletingAllJobs",
                "SpatialOSSendSystem.QueueSerializedMessages",
                "WorkerSystem.SendMessages",
                "PositionSerializationJob"
            };

            var currentState = World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetEntityTemplate());
                world.Connection.ChangeAuthority(EntityId, ComponentSets.WellKnownComponentSet, Authority.Authoritative);
            });

            ActionMeasurement.Measure(() =>
                {
                    currentState.Step(world =>
                    {
                        var workerSystem = world.GetSystem<WorkerSystem>();
                        var entity = workerSystem.GetEntity(new EntityId(EntityId));
                        var position = workerSystem.EntityManager.GetComponentData<Position.Component>(entity);

                        position.Coords = new Coordinates(3006, 42, 2020);
                        workerSystem.EntityManager.SetComponentData(entity, position);
                    });
                })
                .ProfilerMarkers(markers)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        private static EntityTemplate GetEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            return template;
        }
    }
}
