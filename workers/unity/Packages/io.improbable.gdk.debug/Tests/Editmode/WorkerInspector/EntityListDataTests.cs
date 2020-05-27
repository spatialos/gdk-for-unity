using Improbable.Gdk.Core;
using Improbable.Gdk.Debug.WorkerInspector;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;

namespace Improbable.Gdk.Debug.EditmodeTests.WorkerInspector
{
    [TestFixture]
    public class EntityListDataTests : MockBase
    {
        [TestCase]
        public void Data_is_empty_intially()
        {
            var data = new EntityListData();
            Assert.IsEmpty(data.Data);
        }

        [TestCase]
        public void SetNewWorld_should_not_throw()
        {
            var data = new EntityListData();
            Assert.DoesNotThrow(() => data.SetNewWorld(null));
        }

        [TestCase]
        public void RefreshData_does_not_throw_if_no_world()
        {
            var data = new EntityListData();
            Assert.DoesNotThrow(() => data.RefreshData());
        }

        [TestCase]
        public void ApplySearch_does_not_throw_if_no_world()
        {
            var data = new EntityListData();
            Assert.DoesNotThrow(() => data.ApplySearch(EntitySearchParameters.FromSearchString("")));
        }

        [TestCase]
        public void RefreshData_finds_entities_after_setting_world()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(1, GetTemplate("some-entity"));
                })
                .Step(world =>
                {
                    var data = new EntityListData();
                    data.SetNewWorld(world.Worker.World); // Yikes
                    return data;
                })
                .Step((world, data) =>
                {
                    data.RefreshData();
                    Assert.AreEqual(1, data.Data.Count);

                    var entityData = data.Data[0];
                    Assert.AreEqual(1, entityData.EntityId.Id);
                    Assert.AreEqual("some-entity", entityData.Metadata);
                });
        }

        [TestCase]
        public void ApplySearchFilter_immediately_filters_data()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(1, GetTemplate("some-entity"));
                })
                .Step(world =>
                {
                    var data = new EntityListData();
                    data.SetNewWorld(world.Worker.World); // Yikes
                    return data;
                })
                .Step((world, data) =>
                {
                    data.RefreshData();
                    Assert.AreEqual(1, data.Data.Count);
                })
                .Step((world, data) =>
                {
                    data.ApplySearch(EntitySearchParameters.FromSearchString("2")); // Entity ID = 2
                    Assert.IsEmpty(data.Data);
                });
        }

        [TestCase]
        public void SetNewWorld_resets_data()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(1, GetTemplate("some-entity"));
                })
                .Step(world =>
                {
                    var data = new EntityListData();
                    data.SetNewWorld(world.Worker.World); // Yikes
                    return data;
                })
                .Step((world, data) =>
                {
                    data.RefreshData();
                    Assert.AreEqual(1, data.Data.Count);
                })
                .Step((world, data) =>
                {
                    data.SetNewWorld(null);
                    Assert.IsEmpty(data.Data);
                });
        }

        [TestCase]
        public void SearchFilter_persists_through_RefreshData()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(1, GetTemplate("some-entity"));
                })
                .Step(world =>
                {
                    var data = new EntityListData();
                    data.ApplySearch(EntitySearchParameters.FromSearchString("2")); // Entity ID = 2
                    data.SetNewWorld(world.Worker.World); // Yikes
                    return data;
                })
                .Step((world, data) =>
                {
                    data.RefreshData();
                    Assert.IsEmpty(data.Data);
                });
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
