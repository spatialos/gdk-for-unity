using Improbable.Gdk.Core.Commands;
using Improbable.Worker;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

#region Diagnostic control

// ReSharper disable UnusedMember.Local

#endregion

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static partial class WorldCommandsRequirables
    {
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

            public WorldCommands.ReserveEntityIds.Request ReserveEntityIds(
                uint numberOfEntityIds,
                object context = null)
            {
                var reserveEntityIdCommandSender =
                    entityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandSender>(entity);

                var request = new WorldCommands.ReserveEntityIds.Request
                {
                    NumberOfEntityIds = numberOfEntityIds,
                    Context = context
                };

                reserveEntityIdCommandSender.RequestsToSend.Add(request);

                return request;
            }

            public WorldCommands.CreateEntity.Request CreateEntity(
                Improbable.Worker.Core.Entity entityTemplate,
                EntityId? entityId = null,
                object context = null)
            {
                var createEntityCommandSender =
                    entityManager.GetComponentData<WorldCommands.CreateEntity.CommandSender>(entity);

                var request = new WorldCommands.CreateEntity.Request
                {
                    Entity = entityTemplate,
                    EntityId = entityId,
                    Context = context
                };

                createEntityCommandSender.RequestsToSend.Add(request);

                return request;
            }

            public WorldCommands.DeleteEntity.Request DeleteEntity(
                EntityId entityId,
                object context = null)
            {
                var deleteEntityCommandSender =
                    entityManager.GetComponentData<WorldCommands.DeleteEntity.CommandSender>(entity);

                var request = new WorldCommands.DeleteEntity.Request
                {
                    EntityId = entityId,
                    Context = context
                };

                deleteEntityCommandSender.RequestsToSend.Add(request);

                return request;
            }

            [InjectableId(InjectableType.WorldCommandRequestSender)]
            private class WorldCommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity,
                    EntityManager entityManager,
                    ILogDispatcher logDispatcher)
                {
                    return new WorldCommandRequestSender(entity, entityManager);
                }
            }
        }
    }
}
