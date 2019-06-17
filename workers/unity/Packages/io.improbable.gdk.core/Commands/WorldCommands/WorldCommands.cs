using System;
using System.Collections.Generic;
using Improbable.Gdk.ReactiveComponents;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        internal class CommandSenderComponentManager : ICommandSenderComponentManager
        {
            private struct Handles
            {
                public uint CreateEntity;
                public uint DeleteEntity;
                public uint ReserveEntityId;
                public uint EntityQuery;
            }

            private Dictionary<EntityId, Handles> entityIdToAllocatedHandles = new Dictionary<EntityId, Handles>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var handles = new Handles();

                var createEntitySender = new CreateEntity.CommandSender
                {
                    Handle = CreateEntity.RequestsProvider.Allocate(world)
                };

                createEntitySender.RequestsToSend = new List<CreateEntity.Request>();
                entityManager.AddComponentData(entity, createEntitySender);

                var deleteEntitySender = new DeleteEntity.CommandSender
                {
                    Handle = DeleteEntity.RequestsProvider.Allocate(world)
                };

                deleteEntitySender.RequestsToSend = new List<DeleteEntity.Request>();
                entityManager.AddComponentData(entity, deleteEntitySender);

                var reserveEntityIdsSender = new ReserveEntityIds.CommandSender
                {
                    Handle = ReserveEntityIds.RequestsProvider.Allocate(world)
                };

                reserveEntityIdsSender.RequestsToSend = new List<ReserveEntityIds.Request>();
                entityManager.AddComponentData(entity, reserveEntityIdsSender);

                var entityQuerySender = new EntityQuery.CommandSender
                {
                    Handle = EntityQuery.RequestsProvider.Allocate(world)
                };

                entityQuerySender.RequestsToSend = new List<EntityQuery.Request>();
                entityManager.AddComponentData(entity, entityQuerySender);

                handles.CreateEntity = createEntitySender.Handle;
                handles.DeleteEntity = deleteEntitySender.Handle;
                handles.ReserveEntityId = reserveEntityIdsSender.Handle;
                handles.EntityQuery = entityQuerySender.Handle;

                entityIdToAllocatedHandles.Add(entityId, handles);
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CreateEntity.CommandSender>(entity);
                    entityManager.RemoveComponent<DeleteEntity.CommandSender>(entity);
                    entityManager.RemoveComponent<ReserveEntityIds.CommandSender>(entity);
                    entityManager.RemoveComponent<EntityQuery.CommandSender>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Word command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                CreateEntity.RequestsProvider.Free(handles.CreateEntity);
                DeleteEntity.RequestsProvider.Free(handles.DeleteEntity);
                ReserveEntityIds.RequestsProvider.Free(handles.ReserveEntityId);
                EntityQuery.RequestsProvider.Free(handles.EntityQuery);
            }

            public void Clean(World world)
            {
                CreateEntity.RequestsProvider.CleanDataInWorld(world);
                DeleteEntity.RequestsProvider.CleanDataInWorld(world);
                ReserveEntityIds.RequestsProvider.CleanDataInWorld(world);
                EntityQuery.RequestsProvider.CleanDataInWorld(world);
            }
        }
    }
}
