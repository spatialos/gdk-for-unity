using Improbable.Gdk.Core;
using Improbable.Gdk.Debug.WorkerInspector;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.EditmodeTests.WorkerInspector
{
    [TestFixture]
    public class EntityListDataTests : MockBase
    {
        [TestCase(10)]
        [TestCase(1000)]
        [TestCase(100000)]
        [Performance]
        public void RefreshData_and_ApplySearch_with_increasing_number_of_entities(int entityCount)
        {
            var profilerMarkers = new[] { "EntityList.RefreshData", "EntityList.ApplySearch" };

            var currentState = World
                .Step(world =>
                {
                    for (var i = 1; i < entityCount + 1; i++)
                    {
                        world.Connection.CreateEntity(1, GetTemplate("some-entity"));
                    }
                })
                .Step(world =>
                {
                    var data = new EntityListData();
                    data.SetNewWorld(world.Worker.World); // Yikes
                    return data;
                });

            ActionMeasurement.Measure(() =>
                {
                    currentState.Step((world, data) =>
                    {
                        data.RefreshData();
                        return data;
                    }).Step((world, data) =>
                    {
                        data.ApplySearch(EntitySearchParameters.FromSearchString("2"));
                    });
                })
                .WarmupCount(2)
                .MeasurementCount(10)
                .ProfilerMarkers(profilerMarkers)
                .Run();
        }

        private EntityTemplate GetTemplate(string metadata)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot(metadata));
            return template;
        }
    }
}
