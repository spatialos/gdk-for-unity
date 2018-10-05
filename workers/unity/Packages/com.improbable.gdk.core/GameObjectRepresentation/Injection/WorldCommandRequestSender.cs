using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using Unity.Entities;

#region Diagnostic control

// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

#endregion

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static partial class Requirable
        {
            /// <summary>
            ///     A requirable object which enables sending World Commands in Monobehaviours.
            /// </summary>
            [InjectableId(InjectableType.WorldCommandRequestSender, InjectableId.NullComponentId)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class WorldCommandRequestSender : RequirableBase
            {
                private readonly Entity entity;
                private readonly EntityManager entityManager;

                private WorldCommandRequestSender(Entity entity, EntityManager entityManager,
                    ILogDispatcher logDispatcher) : base(logDispatcher)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                }

                /// <summary>
                ///     Sends a ReserveEntityIds command request.
                /// </summary>
                /// <param name="numberOfEntityIds">The number of entity IDs to reserve.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The request ID of the command request.</returns>
                public long ReserveEntityIds(uint numberOfEntityIds, uint? timeoutMillis = null, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var request =
                        WorldCommands.ReserveEntityIds.CreateRequest(numberOfEntityIds, timeoutMillis, context);

                    entityManager.GetComponentData<ReserveEntityIds.CommandSender>(entity)
                        .RequestsToSend.Add(request);

                    return request.RequestId;
                }

                /// <summary>
                ///     Sends a CreateEntity command request.
                /// </summary>
                /// <param name="entityTemplate">
                ///     The EntityTemplate object that defines the SpatialOS components on the to-be-created entity.
                /// </param>
                /// <param name="entityId">
                ///     (Optional) The EntityId that the to-be-created entity should take.
                ///     This should only be provided if received as the result of a ReserveEntityIds command.
                /// </param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The request ID of the command request.</returns>
                public long CreateEntity(EntityTemplate entityTemplate, EntityId? entityId = null,
                    uint? timeoutMillis = null, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var request =
                        WorldCommands.CreateEntity.CreateRequest(entityTemplate, entityId, timeoutMillis, context);

                    entityManager.GetComponentData<CreateEntity.CommandSender>(entity)
                        .RequestsToSend.Add(request);

                    return request.RequestId;
                }

                /// <summary>
                ///     Sends a DeleteEntity command request.
                /// </summary>
                /// <param name="entityId"> The entity ID that is to be deleted.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The request ID of the command request.</returns>
                public long DeleteEntity(EntityId entityId, uint? timeoutMillis = null, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var request = WorldCommands.DeleteEntity.CreateRequest(entityId, timeoutMillis, context);

                    entityManager.GetComponentData<DeleteEntity.CommandSender>(entity)
                        .RequestsToSend.Add(request);

                    return request.RequestId;
                }

                /// <summary>
                ///     Sends an EntityQuery command request.
                /// </summary>
                /// <param name="entityQuery">The EntityQuery object defining the constraints and query type.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The request ID of the command request.</returns>
                public long EntityQuery(Improbable.Worker.Query.EntityQuery entityQuery, uint? timeoutMillis = null,
                    object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var request = WorldCommands.EntityQuery.CreateRequest(entityQuery, timeoutMillis, context);
                    entityManager.GetComponentData<EntityQuery.CommandSender>(entity)
                        .RequestsToSend.Add(request);

                    return request.RequestId;
                }

                [InjectableId(InjectableType.WorldCommandRequestSender, InjectableId.NullComponentId)]
                private class WorldCommandRequestSenderCreator : IInjectableCreator
                {
                    public IInjectable CreateInjectable(Entity entity, EntityManager entityManager,
                        ILogDispatcher logDispatcher)
                    {
                        return new WorldCommandRequestSender(entity, entityManager, logDispatcher);
                    }
                }
            }
        }
    }
}
