using Improbable.Worker;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static partial class WorldCommands
    {
        private class WorldCommandContext
        {
            public readonly Entity OwnerEntity;
            public readonly object Context;

            public WorldCommandContext(Entity ownerEntity, object context)
            {
                OwnerEntity = ownerEntity;
                Context = context;
            }
        }

        [InjectableId(InjectableType.WorldCommandRequestSender)]
        [InjectionCondition(InjectionCondition.RequireNothing)]
        public class WorldCommandRequestSender : IInjectable
        {
            private readonly Entity entity;
            private readonly EntityManager entityManager;

            private WorldCommandRequestSender(Entity entity, EntityManager entityManager)
            {
                this.entity = entity;
                this.entityManager = entityManager;
            }

            public void ReserveEntityIds(
                uint numberOfEntityIds,
                object context = null)
            {
                var reserveEntityIdCommandSender =
                    entityManager.GetComponentData<Commands.WorldCommands.ReserveEntityIds.CommandSender>(entity);

                reserveEntityIdCommandSender.RequestsToSend.Add(new Commands.WorldCommands.ReserveEntityIds.Request
                {
                    NumberOfEntityIds = numberOfEntityIds,
                    Context = new WorldCommandContext(entity, context)
                });
            }

            public void CreateEntity(
                Improbable.Worker.Core.Entity entityTemplate,
                EntityId? entityId = null,
                object context = null)
            {
                var createEntityCommandSender =
                    entityManager.GetComponentData<Commands.WorldCommands.CreateEntity.CommandSender>(entity);

                createEntityCommandSender.RequestsToSend.Add(new Commands.WorldCommands.CreateEntity.Request
                {
                    Entity = entityTemplate,
                    EntityId = entityId,
                    Context = new WorldCommandContext(entity, context)
                });
            }

            public void DeleteEntity(
                EntityId entityId,
                object context = null)
            {
                var deleteEntityCommandSender =
                    entityManager.GetComponentData<Commands.WorldCommands.DeleteEntity.CommandSender>(entity);

                deleteEntityCommandSender.RequestsToSend.Add(new Commands.WorldCommands.DeleteEntity.Request
                {
                    EntityId = entityId,
                    Context = new WorldCommandContext(entity, context)
                });
            }

            [InjectableId(InjectableType.WorldCommandRequestSender)]
            // ReSharper disable once UnusedMember.Local
            private class WorldCommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager,
                    ILogDispatcher logDispatcher)
                {
                    return new WorldCommandRequestSender(entity, entityManager);
                }
            }
        }
    }
}
