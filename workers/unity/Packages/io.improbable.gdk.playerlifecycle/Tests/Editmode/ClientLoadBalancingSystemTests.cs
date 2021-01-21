#if GDK_LOAD_BALANCING
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TestUtils;
using Improbable.Generated;
using NUnit.Framework;

namespace Improbable.Gdk.LoadBalancing.EditmodeTests
{
    public class ClientLoadBalancingSystemTests : MockBase
    {
        protected override MockWorld.Options GetOptions()
        {
            var opts = base.GetOptions();
            opts.AdditionalSystems = world =>
            {
                world.AddSystem(new ClientLoadBalancingSystem
                {
                    PlayerEntityType = "Player", ClientComponentSet = ComponentSets.PlayerClientSet
                });
            };
            return opts;
        }


        [Test]
        public void Only_specified_set_is_delegated_to_client()
        {
            World
                .Step(world =>
                {
                    // Will be the worker partition.
                    world.Connection.CreateEntity(1);
                    world.Connection.CreateEntity(2, GetPlayer(new EntityId(1)));
                })
                .Step(world =>
                {
                    var workerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(1));
                    world.EntityManager.AddComponentData(workerEntity,
                        new RegisteredWorker { PartitionEntityId = new EntityId(3) });
                }).Step(world =>
                {
                    var playerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(2));
                    var authComponent =
                        world.EntityManager.GetComponentData<AuthorityDelegation.Component>(playerEntity);

                    Assert.AreEqual(1, authComponent.Delegations.Count);
                    Assert.AreEqual(3, authComponent.Delegations[ComponentSets.PlayerClientSet.ComponentSetId]);
                });
        }

        [Test]
        public void Non_player_entities_are_ignored()
        {
            World
                .Step(world =>
                {
                    // Will be the worker partition.
                    world.Connection.CreateEntity(1);
                    world.Connection.CreateEntity(2, GetNonPlayer());
                })
                .Step(world =>
                {
                    var workerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(1));
                    world.EntityManager.AddComponentData(workerEntity,
                        new RegisteredWorker { PartitionEntityId = new EntityId(3) });
                }).Step(world =>
                {
                    var nonPlayerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(2));
                    var authComponent =
                        world.EntityManager.GetComponentData<AuthorityDelegation.Component>(nonPlayerEntity);

                    Assert.AreEqual(0, authComponent.Delegations.Count);
                });
        }

        private static EntityTemplate GetPlayer(EntityId owningWorkerEntityId)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Metadata.Snapshot("Player"));
            template.AddComponent(new AuthorityDelegation.Snapshot(new Dictionary<uint, long>()));
            template.AddComponent(new OwningWorker.Snapshot(owningWorkerEntityId));
            return template;
        }

        private static EntityTemplate GetNonPlayer()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Metadata.Snapshot("NonPlayer"));
            template.AddComponent(new AuthorityDelegation.Snapshot(new Dictionary<uint, long>()));
            template.AddComponent(new OwningWorker.Snapshot(new EntityId()));
            return template;
        }
    }
}
#endif
