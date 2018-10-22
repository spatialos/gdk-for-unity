using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Receives incoming messages from the SpatialOS runtime.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        public Dispatcher Dispatcher;

        private WorkerSystem worker;

        private readonly Dictionary<uint, ComponentDispatcherHandler> componentSpecificDispatchers =
            new Dictionary<uint, ComponentDispatcherHandler>();

        public List<Action<Unity.Entities.Entity>> AddAllCommandComponents = new List<Action<Unity.Entities.Entity>>();

        private bool inCriticalSection;

        private const string LoggerName = nameof(SpatialOSReceiveSystem);
        private const string UnknownComponentIdError = "Received an op with an unknown ComponentId";
        private const string EntityNotFound = "No entity found for SpatialOS EntityId specified in op.";
        private const string RequestIdNotFound = "No corresponding request found for response.";

        private WorldCommands.CreateEntity.Storage createEntityStorage;
        private WorldCommands.DeleteEntity.Storage deleteEntityStorage;
        private WorldCommands.ReserveEntityIds.Storage reserveEntityIdsStorage;
        private WorldCommands.EntityQuery.Storage entityQueryStorage;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            Dispatcher = new Dispatcher();
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
                    Dispatcher.Process(opList);
                }
            }
            while (inCriticalSection);
        }

        internal void OnAddEntity(AddEntityOp op)
        {
            var entityId = op.EntityId;
            if (worker.EntityIdToEntity.ContainsKey(entityId))
            {
                throw new InvalidSpatialEntityStateException(
                    string.Format(Errors.EntityAlreadyExistsError, entityId.Id));
            }

            Profiler.BeginSample("OnAddEntity");
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });
            EntityManager.AddComponent(entity, ComponentType.Create<NewlyAddedSpatialOSEntity>());

            foreach (var AddCommandCompoent in AddAllCommandComponents)
            {
                AddCommandCompoent(entity);
            }

            WorldCommands.AddWorldCommandRequesters(World, EntityManager, entity);
            worker.EntityIdToEntity.Add(entityId, entity);
            Profiler.EndSample();
        }

        internal void OnRemoveEntity(RemoveEntityOp op)
        {
            var entityId = op.EntityId;
            if (!worker.TryGetEntity(entityId, out var entity))
            {
                throw new InvalidSpatialEntityStateException(
                    string.Format(Errors.EntityNotFoundForDeleteError, entityId.Id));
            }

            Profiler.BeginSample("OnRemoveEntity");
            WorldCommands.DeallocateWorldCommandRequesters(EntityManager, entity);
            EntityManager.DestroyEntity(worker.EntityIdToEntity[entityId]);
            worker.EntityIdToEntity.Remove(entityId);
            Profiler.EndSample();
        }

        internal void OnDisconnect(DisconnectOp op)
        {
            WorldCommands.DeallocateWorldCommandRequesters(EntityManager, worker.WorkerEntity);
            WorldCommands.RemoveWorldCommandRequesters(EntityManager, worker.WorkerEntity);
            EntityManager.AddSharedComponentData(worker.WorkerEntity,
                new OnDisconnected { ReasonForDisconnect = op.Reason });
        }

        internal void OnAddComponent(AddComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Data.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.Data.ComponentId));
            }

            Profiler.BeginSample("OnAddComponent");
            specificDispatcher.OnAddComponent(op);
            Profiler.EndSample();
        }

        internal void OnRemoveComponent(RemoveComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.ComponentId));
            }

            Profiler.BeginSample("OnRemoveComponent");
            specificDispatcher.OnRemoveComponent(op);
            Profiler.EndSample();
        }

        internal void OnComponentUpdate(ComponentUpdateOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Update.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.Update.ComponentId));
            }

            Profiler.BeginSample("OnComponentUpdate");
            specificDispatcher.OnComponentUpdate(op);
            Profiler.EndSample();
        }

        internal void OnAuthorityChange(AuthorityChangeOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.ComponentId));
            }

            Profiler.BeginSample("OnAuthorityChange");
            specificDispatcher.OnAuthorityChange(op);
            Profiler.EndSample();
        }

        internal void OnCommandRequest(CommandRequestOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Request.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.Request.ComponentId));
            }

            Profiler.BeginSample("OnCommandRequest");
            specificDispatcher.OnCommandRequest(op);
            Profiler.EndSample();
        }

        internal void OnCommandResponse(CommandResponseOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Response.ComponentId, out var specificDispatcher))
            {
                throw new UnknownComponentIdException(
                    string.Format(Errors.UnknownComponentIdError, op.GetType(), op.Response.ComponentId));
            }

            Profiler.BeginSample("OnCommandResponse");
            specificDispatcher.OnCommandResponse(op);
            Profiler.EndSample();
        }

        internal void OnCreateEntityResponse(CreateEntityResponseOp op)
        {
            if (!createEntityStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
            {
                throw new UnknownRequestIdException(string.Format(Errors.UnknownRequestIdError, op.GetType(),
                    op.RequestId.Id));
            }

            var entity = requestBundle.Entity;
            createEntityStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
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

            responses.Add(
                new WorldCommands.CreateEntity.ReceivedResponse(op, requestBundle.Request, requestBundle.Context,
                    requestBundle.RequestId));
        }

        internal void OnDeleteEntityResponse(DeleteEntityResponseOp op)
        {
            if (!deleteEntityStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
            {
                throw new UnknownRequestIdException(string.Format(Errors.UnknownRequestIdError, op.GetType(),
                    op.RequestId.Id));
            }

            var entity = requestBundle.Entity;
            deleteEntityStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
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

            responses.Add(
                new WorldCommands.DeleteEntity.ReceivedResponse(op, requestBundle.Request, requestBundle.Context,
                    requestBundle.RequestId));
        }

        internal void OnReserveEntityIdsResponse(ReserveEntityIdsResponseOp op)
        {
            if (!reserveEntityIdsStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
            {
                throw new UnknownRequestIdException(string.Format(Errors.UnknownRequestIdError, op.GetType(),
                    op.RequestId.Id));
            }

            var entity = requestBundle.Entity;
            reserveEntityIdsStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
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

            responses.Add(
                new WorldCommands.ReserveEntityIds.ReceivedResponse(op, requestBundle.Request, requestBundle.Context,
                    requestBundle.RequestId));
        }

        internal void OnEntityQueryResponse(EntityQueryResponseOp op)
        {
            if (!entityQueryStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
            {
                throw new UnknownRequestIdException(string.Format(Errors.UnknownRequestIdError, op.GetType(),
                    op.RequestId.Id));
            }

            var entity = requestBundle.Entity;
            entityQueryStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);

            if (!EntityManager.Exists(entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
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

            responses.Add(
                new WorldCommands.EntityQuery.ReceivedResponse(op, requestBundle.Request, requestBundle.Context,
                    requestBundle.RequestId));
        }

        internal void AddDispatcherHandler(ComponentDispatcherHandler componentDispatcher)
        {
            componentSpecificDispatchers.Add(componentDispatcher.ComponentId, componentDispatcher);
            AddAllCommandComponents.Add(componentDispatcher.AddCommandComponents);
            componentDispatcher.AddCommandComponents(worker.WorkerEntity);
        }

        private void HandleException(Exception e)
        {
            worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                .WithException(e));
        }

        private void SetupDispatcherHandlers()
        {
            // Find all component specific dispatchers and create an instance.
            var componentDispatcherTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentDispatcherHandler).IsAssignableFrom(type) && !type.IsAbstract
                    && type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) ==
                    null);

            WorldCommands.AddWorldCommandRequesters(World, EntityManager, worker.WorkerEntity);
            foreach (var componentDispatcherType in componentDispatcherTypes)
            {
                AddDispatcherHandler((ComponentDispatcherHandler)
                    Activator.CreateInstance(componentDispatcherType, worker, World));
            }

            Dispatcher.OnAddEntity(OnAddEntity);
            Dispatcher.OnRemoveEntity(OnRemoveEntity);
            Dispatcher.OnDisconnect(OnDisconnect);
            Dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            Dispatcher.OnAddComponent(OnAddComponent);
            Dispatcher.OnRemoveComponent(OnRemoveComponent);
            Dispatcher.OnComponentUpdate(OnComponentUpdate);
            Dispatcher.OnAuthorityChange(OnAuthorityChange);

            Dispatcher.OnCommandRequest(OnCommandRequest);
            Dispatcher.OnCommandResponse(OnCommandResponse);

            Dispatcher.OnCreateEntityResponse(OnCreateEntityResponse);
            Dispatcher.OnDeleteEntityResponse(OnDeleteEntityResponse);
            Dispatcher.OnReserveEntityIdsResponse(OnReserveEntityIdsResponse);
            Dispatcher.OnEntityQueryResponse(OnEntityQueryResponse);

            ClientError.ExceptionCallback = HandleException;
        }

        private static class Errors
        {
            public const string EntityAlreadyExistsError =
                "Received an AddEntityOp with Spatial entity ID {0}, but an entity with that EntityId already exists.";

            public const string EntityNotFoundForDeleteError =
                "Received a DeleteEntityOp with Spatial entity ID {0}, but an entity with that EntityId could not be found."
                + "This could be caused by deleting SpatialOS entities locally. "
                + "Use a DeleteEntity command to delete entities instead.";

            public const string UnknownComponentIdError =
                "Received an {0} with component ID {1}, but this component ID is unknown."
                + "This could be caused by adding schema components and not recompiling Unity workers.";

            public const string UnknownRequestIdError =
                "Received an {0} with Request ID {1}, but this Request ID is unknown.";
        }
    }
}
