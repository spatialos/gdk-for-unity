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

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class FirstCommandReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<FirstCommand.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<FirstCommand.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.FirstCommandRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<FirstCommand.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<FirstCommand.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<FirstCommand.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.FirstCommandResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<FirstCommand.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.FirstCommandRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.FirstCommandResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class FirstCommandCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.FirstCommand();
                commandSender.CommandListHandle = global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.FirstCommand();
                commandResponder.CommandListHandle =
                    global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.FirstCommand>(entity);
                    entityManager.RemoveComponent<CommandResponders.FirstCommand>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.FirstCommandSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.FirstCommandResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.FirstCommandSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.FirstCommandResponderProvider.CleanDataInWorld(world);
            }
        }

        public class SecondCommandReactiveCommandComponentManager : IReactiveCommandComponentManager
        {
            public void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var receivedRequests = commandSystem.GetRequests<SecondCommand.ReceivedRequest>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedRequests.Count; ++i)
                {
                    if (!workerSystem.TryGetEntity(receivedRequests[i].EntityId, out var entity))
                    {
                        continue;
                    }

                    List<SecondCommand.ReceivedRequest> requests;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand>(entity))
                    {
                        requests = entityManager.GetComponentData<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand>(entity).Requests;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.SecondCommandRequestsProvider.Allocate(world)
                        };
                        data.Requests = new List<SecondCommand.ReceivedRequest>();
                        requests = data.Requests;
                        entityManager.AddComponentData(entity, data);
                    }

                    requests.Add(receivedRequests[i]);
                }


                var receivedResponses = commandSystem.GetResponses<SecondCommand.ReceivedResponse>();
                // todo Not efficient if it keeps jumping all over entities but don't care right now
                for (int i = 0; i < receivedResponses.Count; ++i)
                {
                    ref readonly var response = ref receivedResponses[i];

                    if (response.SendingEntity == Unity.Entities.Entity.Null || !entityManager.Exists(response.SendingEntity))
                    {
                        continue;
                    }

                    List<SecondCommand.ReceivedResponse> responses;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand
                        {
                            CommandListHandle = ReferenceTypeProviders.SecondCommandResponsesProvider.Allocate(world)
                        };
                        data.Responses = new List<SecondCommand.ReceivedResponse>();
                        responses = data.Responses;
                        entityManager.AddComponentData(response.SendingEntity, data);
                    }

                    responses.Add(response);
                }
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.SecondCommandRequestsProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.SecondCommandResponsesProvider.CleanDataInWorld(world);
            }
        }

        public class SecondCommandCommandSenderComponentManager : ICommandSenderComponentManager
        {
            private Dictionary<EntityId, (uint Sender, uint Responder)> entityIdToAllocatedHandles =
                new Dictionary<EntityId, (uint Sender, uint Responder)>();

            public void AddComponents(Entity entity, EntityManager entityManager, World world)
            {
                // todo error message if not the worker entity or spatial entity
                EntityId entityId = entityManager.HasComponent<SpatialEntityId>(entity)
                    ? entityManager.GetComponentData<SpatialEntityId>(entity).EntityId
                    : new EntityId(0);

                var commandSender = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.SecondCommand();
                commandSender.CommandListHandle = global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.SecondCommand();
                commandResponder.CommandListHandle =
                    global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingSystem<WorkerSystem>();

                workerSystem.TryGetEntity(entityId, out var entity);

                if (entity != Entity.Null)
                {
                    entityManager.RemoveComponent<CommandSenders.SecondCommand>(entity);
                    entityManager.RemoveComponent<CommandResponders.SecondCommand>(entity);
                }

                if (!entityIdToAllocatedHandles.TryGetValue(entityId, out var handles))
                {
                    throw new ArgumentException("Command components not added to entity");
                }

                entityIdToAllocatedHandles.Remove(entityId);

                ReferenceTypeProviders.SecondCommandSenderProvider.Free(handles.Sender);
                ReferenceTypeProviders.SecondCommandResponderProvider.Free(handles.Responder);
            }

            public void Clean(World world)
            {
                ReferenceTypeProviders.SecondCommandSenderProvider.CleanDataInWorld(world);
                ReferenceTypeProviders.SecondCommandResponderProvider.CleanDataInWorld(world);
            }
        }

    }
}
#endif
