// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if USE_LEGACY_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class BarCommandReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<BarCommand.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<BarCommand.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.DependentSchema.DependentDataComponent.CommandRequests.BarCommand>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.DependentSchema.DependentDataComponent.CommandRequests.BarCommand>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.DependentSchema.DependentDataComponent.CommandRequests.BarCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.BarCommandRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<BarCommand.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<BarCommand.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<BarCommand.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.DependentSchema.DependentDataComponent.CommandResponses.BarCommand>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.DependentSchema.DependentDataComponent.CommandResponses.BarCommand>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.DependentSchema.DependentDataComponent.CommandResponses.BarCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.BarCommandResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<BarCommand.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.BarCommandRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.BarCommandResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class BarCommandCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.DependentSchema.DependentDataComponent.CommandSenders.BarCommand();
                commandSender.CommandListHandle = global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.DependentSchema.DependentDataComponent.CommandResponders.BarCommand();
                commandResponder.CommandListHandle =
                    global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.BarCommand>(entity);
                    entityManager.RemoveComponent<CommandResponders.BarCommand>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.BarCommandSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.BarCommandResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.BarCommandSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.BarCommandResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
