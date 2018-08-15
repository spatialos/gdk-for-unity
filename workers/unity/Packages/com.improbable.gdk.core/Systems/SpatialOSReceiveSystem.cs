using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerBase worker;
        private MutableView view;
        private ILogDispatcher logDispatcher;
        private Dispatcher dispatcher;

        private readonly Dictionary<uint, ComponentDispatcherHandler> componentSpecificDispatchers =
            new Dictionary<uint, ComponentDispatcherHandler>();

        private bool inCriticalSection;

        private const string LoggerName = nameof(SpatialOSReceiveSystem);
        private const string UnknownComponentIdError = "Received an op with an unknown ComponentId";
        private const string EntityNotFound = "No entity found for SpatialOS EntityId specified in op.";
        private const string RequestIdNotFound = "No corresponding request found for response.";

        private WorldCommands.CreateEntity.Storage createEntityStorage;
        private WorldCommands.DeleteEntity.Storage deleteEntityStorage;
        private WorldCommands.ReserveEntityIds.Storage reserveEntityIdsStorage;
        private WorldCommands.EntityQuery.Storage entityQueryStorage;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;
            logDispatcher = view.LogDispatcher;

            dispatcher = new Dispatcher();
            SetupDispatcherHandlers();

            var requestTracker = World.GetOrCreateManager<CommandRequestTrackerSystem>();
            createEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.CreateEntity.Storage>();
            deleteEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.DeleteEntity.Storage>();
            reserveEntityIdsStorage = requestTracker.GetCommandStorageForType<WorldCommands.ReserveEntityIds.Storage>();
            entityQueryStorage = requestTracker.GetCommandStorageForType<WorldCommands.EntityQuery.Storage>();
        }

        protected override void OnDestroyManager()
        {
            foreach (var pair in componentSpecificDispatchers)
            {
                pair.Value.Dispose();
            }

            // Remove data for AuthorityChanges
            AuthorityChangesProvider.CleanDataInWorld(World);
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            do
            {
                using (var opList = worker.Connection.GetOpList(0))
                {
                    dispatcher.Process(opList);
                }
            }
            while (inCriticalSection);
        }

        private void OnAddEntity(AddEntityOp op)
        {
            view.CreateEntity(op.EntityId);
        }

        private void OnRemoveEntity(RemoveEntityOp op)
        {
            view.RemoveEntity(op.EntityId);
        }

        private void OnDisconnect(DisconnectOp op)
        {
            view.Disconnect(op.Reason);
        }

        private void OnAddComponent(AddComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Data.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Data.ComponentId));
                return;
            }

            specificDispatcher.OnAddComponent(op);
        }

        private void OnRemoveComponent(RemoveComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.ComponentId));
                return;
            }

            specificDispatcher.OnRemoveComponent(op);
        }

        private void OnComponentUpdate(ComponentUpdateOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Update.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Update.ComponentId));
                return;
            }

            specificDispatcher.OnComponentUpdate(op);
        }

        private void OnAuthorityChange(AuthorityChangeOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.ComponentId));
                return;
            }

            specificDispatcher.OnAuthorityChange(op);
        }

        private void OnCommandRequest(CommandRequestOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Request.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Request.ComponentId));
                return;
            }

            specificDispatcher.OnCommandRequest(op);
        }

        private void OnCommandResponse(CommandResponseOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Response.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Response.ComponentId));
                return;
            }

            specificDispatcher.OnCommandResponse(op);
        }

        private void OnCreateEntityResponse(CreateEntityResponseOp op)
        {
            if (!createEntityStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out CommandRequestStore<WorldCommands.CreateEntity.Request> requestBundle))
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(RequestIdNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("RequestId", op.RequestId.Id)
                    .WithField("Command Type", "CreateEntity"));
                return;
            }

            var entity = requestBundle.Entity;
            createEntityStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("Op", "CreateEntityResponseOp")
                );
                return;
            }

            List<WorldCommands.CreateEntity.ReceivedResponse> responses;
            if (EntityManager.HasComponent<WorldCommands.CreateEntity.CommandResponses>(entity))
            {
                responses = EntityManager.GetComponentData<WorldCommands.CreateEntity.CommandResponses>(entity)
                    .Responses;
            }
            else
            {
                var data = new WorldCommands.CreateEntity.CommandResponses
                {
                    Handle = WorldCommands.CreateEntity.ResponsesProvider.Allocate(World)
                };
                responses = data.Responses = new List<WorldCommands.CreateEntity.ReceivedResponse>();
                EntityManager.AddComponentData(entity, data);
            }

            responses.Add(new WorldCommands.CreateEntity.ReceivedResponse(op, requestBundle.Request, requestBundle.Context));
        }

        private void OnDeleteEntityResponse(DeleteEntityResponseOp op)
        {
            if (!deleteEntityStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out CommandRequestStore<WorldCommands.DeleteEntity.Request> requestBundle))
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(RequestIdNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("RequestId", op.RequestId.Id)
                    .WithField("Command Type", "DeleteEntity"));
                return;
            }

            var entity = requestBundle.Entity;
            deleteEntityStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("Op", "DeleteEntityResponseOp")
                );
                return;
            }

            List<WorldCommands.DeleteEntity.ReceivedResponse> responses;
            if (EntityManager.HasComponent<WorldCommands.DeleteEntity.CommandResponses>(entity))
            {
                responses = EntityManager.GetComponentData<WorldCommands.DeleteEntity.CommandResponses>(entity)
                    .Responses;
            }
            else
            {
                var data = new WorldCommands.DeleteEntity.CommandResponses
                {
                    Handle = WorldCommands.DeleteEntity.ResponsesProvider.Allocate(World)
                };
                responses = data.Responses = new List<WorldCommands.DeleteEntity.ReceivedResponse>();
                EntityManager.AddComponentData(entity, data);
            }

            responses.Add(new WorldCommands.DeleteEntity.ReceivedResponse(op, requestBundle.Request, requestBundle.Context));
        }

        private void OnReserveEntityIdsResponse(ReserveEntityIdsResponseOp op)
        {
            if (!reserveEntityIdsStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out CommandRequestStore<WorldCommands.ReserveEntityIds.Request> requestBundle))
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(RequestIdNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("RequestId", op.RequestId.Id)
                    .WithField("Command Type", "ReserveEntityIds"));
                return;
            }

            var entity = requestBundle.Entity;
            reserveEntityIdsStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("Op", "ReserveEntityIdsResponseOp")
                );
                return;
            }

            List<WorldCommands.ReserveEntityIds.ReceivedResponse> responses;
            if (EntityManager.HasComponent<WorldCommands.ReserveEntityIds.CommandResponses>(entity))
            {
                responses = EntityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandResponses>(entity)
                    .Responses;
            }
            else
            {
                var data = new WorldCommands.ReserveEntityIds.CommandResponses
                {
                    Handle = WorldCommands.ReserveEntityIds.ResponsesProvider.Allocate(World)
                };
                responses = data.Responses = new List<WorldCommands.ReserveEntityIds.ReceivedResponse>();
                EntityManager.AddComponentData(entity, data);
            }

            responses.Add(new WorldCommands.ReserveEntityIds.ReceivedResponse(op, requestBundle.Request, requestBundle.Context));
        }

        private void OnEntityQueryResponse(EntityQueryResponseOp op)
        {
            if (!entityQueryStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out CommandRequestStore<WorldCommands.EntityQuery.Request> requestBundle))
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(RequestIdNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("RequestId", op.RequestId.Id)
                    .WithField("Command Type", "EntityQuery"));
                return;
            }

            var entity = requestBundle.Entity;
            entityQueryStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("Op", "EntityQueryResponseOp")
                );
                return;
            }

            List<WorldCommands.EntityQuery.ReceivedResponse> responses;
            if (EntityManager.HasComponent<WorldCommands.EntityQuery.CommandResponses>(entity))
            {
                responses = EntityManager.GetComponentData<WorldCommands.EntityQuery.CommandResponses>(entity)
                    .Responses;
            }
            else
            {
                var data = new WorldCommands.EntityQuery.CommandResponses
                {
                    Handle = WorldCommands.EntityQuery.ResponsesProvider.Allocate(World)
                };
                responses = data.Responses = new List<WorldCommands.EntityQuery.ReceivedResponse>();
                EntityManager.AddComponentData(entity, data);
            }

            responses.Add(new WorldCommands.EntityQuery.ReceivedResponse(op, requestBundle.Request, requestBundle.Context));
        }

        private void SetupDispatcherHandlers()
        {
            // Find all component specific dispatchers and create an instance.
            var componentDispatcherTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentDispatcherHandler).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var componentDispatcherType in componentDispatcherTypes)
            {
                var componentDispatcher =
                    (ComponentDispatcherHandler) Activator.CreateInstance(componentDispatcherType,
                        new object[] { view, World });
                componentSpecificDispatchers.Add(componentDispatcher.ComponentId, componentDispatcher);
                // TODO: UTY-836 temporary work around until Jess's worker refactor comes in.
                view.AddAllCommandComponents.Add(componentDispatcher.AddCommandComponents);
                componentDispatcher.AddCommandComponents(view.WorkerEntity);
            }

            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);
            dispatcher.OnDisconnect(OnDisconnect);
            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            dispatcher.OnAddComponent(OnAddComponent);
            dispatcher.OnRemoveComponent(OnRemoveComponent);
            dispatcher.OnComponentUpdate(OnComponentUpdate);
            dispatcher.OnAuthorityChange(OnAuthorityChange);

            dispatcher.OnCommandRequest(OnCommandRequest);
            dispatcher.OnCommandResponse(OnCommandResponse);

            dispatcher.OnCreateEntityResponse(OnCreateEntityResponse);
            dispatcher.OnDeleteEntityResponse(OnDeleteEntityResponse);
            dispatcher.OnReserveEntityIdsResponse(OnReserveEntityIdsResponse);
            dispatcher.OnEntityQueryResponse(OnEntityQueryResponse);
        }
    }
}
