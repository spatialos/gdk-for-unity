using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        internal static void AddWorldCommandRequesters(World world, EntityManager manager, Unity.Entities.Entity entity)
        {
            var createEntitySender = new CreateEntity.CommandSender
            {
                Handle = CreateEntity.RequestsProvider.Allocate(world)
            };

            createEntitySender.RequestsToSend = new List<CreateEntity.Request>();
            manager.AddComponentData(entity, createEntitySender);

            var deleteEntitySender = new DeleteEntity.CommandSender
            {
                Handle = DeleteEntity.RequestsProvider.Allocate(world)
            };

            deleteEntitySender.RequestsToSend = new List<DeleteEntity.Request>();
            manager.AddComponentData(entity, deleteEntitySender);

            var reserveEntityIdsSender = new ReserveEntityIds.CommandSender
            {
                Handle = ReserveEntityIds.RequestsProvider.Allocate(world)
            };

            reserveEntityIdsSender.RequestsToSend = new List<ReserveEntityIds.Request>();
            manager.AddComponentData(entity, reserveEntityIdsSender);

            var entityQuerySender = new EntityQuery.CommandSender
            {
                Handle = EntityQuery.RequestsProvider.Allocate(world)
            };

            entityQuerySender.RequestsToSend = new List<EntityQuery.Request>();
            manager.AddComponentData(entity, entityQuerySender);
        }

        internal static void RemoveWorldCommandRequesters(EntityManager manager, Unity.Entities.Entity entity)
        {
            manager.RemoveComponent<CreateEntity.CommandSender>(entity);
            manager.RemoveComponent<DeleteEntity.CommandSender>(entity);
            manager.RemoveComponent<ReserveEntityIds.CommandSender>(entity);
            manager.RemoveComponent<EntityQuery.CommandSender>(entity);
        }

        internal static void DeallocateWorldCommandRequesters(EntityManager manager, Unity.Entities.Entity entity)
        {
            var createEntityData = manager.GetComponentData<CreateEntity.CommandSender>(entity);
            CreateEntity.RequestsProvider.Free(createEntityData.Handle);

            var deleteEntityData = manager.GetComponentData<DeleteEntity.CommandSender>(entity);
            DeleteEntity.RequestsProvider.Free(deleteEntityData.Handle);

            var reserveEntityIdsData = manager.GetComponentData<ReserveEntityIds.CommandSender>(entity);
            ReserveEntityIds.RequestsProvider.Free(reserveEntityIdsData.Handle);

            var entityQueryData = manager.GetComponentData<EntityQuery.CommandSender>(entity);
            EntityQuery.RequestsProvider.Free(entityQueryData.Handle);
        }
    }
}
