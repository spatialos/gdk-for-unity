using Improbable.Gdk.Core;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestUtils;
using Improbable.Generated;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class Test : MockBase
    {
        private const long EntityId = 1;

        [Test]
        public void Authority_changes_are_kept_in_order_and_applied_in_order()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    world.Connection.ChangeComponentAuthority(EntityId, 54, Authority.Authoritative);
                    world.Connection.ChangeComponentAuthority(EntityId, 54, Authority.NotAuthoritative);
                })
                .Step(world =>
                {
                    var authChanges = world
                        .GetSystem<ComponentUpdateSystem>()
                        .GetAuthorityChangesReceived(new EntityId(EntityId), 54);

                    Assert.AreEqual(2, authChanges.Count);
                    Assert.AreEqual(Authority.Authoritative, authChanges[0].Authority);
                    Assert.AreEqual(Authority.NotAuthoritative, authChanges[1].Authority);

                    var ecsEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(EntityId));

                    Assert.IsFalse(world.Worker.World.EntityManager.HasComponent<Position.HasAuthority>(ecsEntity));
                });
        }

        [Test]
        public void Component_updates_are_kept_in_order()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    world.Connection.UpdateComponent(EntityId, 54, new Position.Update
                    {
                        Coords = new Coordinates(1, 0, 0)
                    });
                    world.Connection.UpdateComponent(EntityId, 54, new Position.Update
                    {
                        Coords = new Coordinates(-1, 0, 0)
                    });
                })
                .Step(world =>
                {
                    var componentUpdates = world
                        .GetSystem<ComponentUpdateSystem>()
                        .GetComponentUpdatesReceived<Position.Update>();

                    Assert.AreEqual(2, componentUpdates.Count);
                    Assert.AreEqual(1, componentUpdates[0].Update.Coords.Value.X);
                    Assert.AreEqual(-1, componentUpdates[1].Update.Coords.Value.X);

                    var ecsEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(EntityId));

                    Assert.AreEqual(-1, world.Worker.World.EntityManager.GetComponentData<Position.Component>(ecsEntity).Coords.X);
                });
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            return template;
        }
    }
}
