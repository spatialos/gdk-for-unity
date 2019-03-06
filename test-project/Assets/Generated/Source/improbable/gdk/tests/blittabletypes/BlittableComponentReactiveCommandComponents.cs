// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
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
                    if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand>(entity))
                    {
                        requests = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand>(entity).Requests;
                    }
                    else
                    {
                        var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand
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
                    if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand
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

                var commandSender = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand();
                commandSender.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand();
                commandResponder.CommandListHandle =
                    Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingManager<WorkerSystem>();

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
                    if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand>(entity))
                    {
                        requests = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand>(entity).Requests;
                    }
                    else
                    {
                        var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand
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
                    if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand>(response.SendingEntity))
                    {
                        responses = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand>(response.SendingEntity).Responses;
                    }
                    else
                    {
                        var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand
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

                var commandSender = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand();
                commandSender.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Allocate(world);
                commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request>();

                entityManager.AddComponentData(entity, commandSender);

                var commandResponder = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand();
                commandResponder.CommandListHandle =
                    Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                entityIdToAllocatedHandles.Add(entityId, (commandSender.CommandListHandle, commandResponder.CommandListHandle));
            }

            public void RemoveComponents(EntityId entityId, EntityManager entityManager, World world)
            {
                var workerSystem = world.GetExistingManager<WorkerSystem>();

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
