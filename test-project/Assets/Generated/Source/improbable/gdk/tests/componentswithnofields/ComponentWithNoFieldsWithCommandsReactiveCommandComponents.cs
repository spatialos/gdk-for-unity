// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class CmdReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<Cmd.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Cmd.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd
                        {
                            CommandListHandle = ReferenceTypeProviders.CmdRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<Cmd.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<Cmd.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<Cmd.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd
                        {
                            CommandListHandle = ReferenceTypeProviders.CmdResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<Cmd.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.CmdRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.CmdResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class CmdCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd();
                commandSender.CommandListHandle = global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd();
                commandResponder.CommandListHandle =
                    global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingManager<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.Cmd>(entity);
                    entityManager.RemoveComponent<CommandResponders.Cmd>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.CmdSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.CmdResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.CmdSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.CmdResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
