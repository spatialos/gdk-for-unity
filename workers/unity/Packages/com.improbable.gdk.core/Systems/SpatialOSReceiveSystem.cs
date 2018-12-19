using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;
using Entity = Improbable.Worker.CInterop.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Receives incoming messages from the SpatialOS runtime.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private ComponentUpdateSystem updateSystem;
        private CommandSystem commandSystem;
        private EntitySystem entitySystem;

        private readonly Dictionary<uint, ComponentDispatcherHandler> componentSpecificDispatchers =
            new Dictionary<uint, ComponentDispatcherHandler>();

        public List<Action<Unity.Entities.Entity>> AddAllCommandComponents = new List<Action<Unity.Entities.Entity>>();

        private bool inCriticalSection;

        private readonly OpListDeserializer opDeserializer = new OpListDeserializer();
        private readonly ViewDiff diff = new ViewDiff();

        private const string LoggerName = nameof(SpatialOSReceiveSystem);
        private const string UnknownComponentIdError = "Received an op with an unknown ComponentId";
        private const string EntityNotFound = "No entity found for SpatialOS EntityId specified in op.";
        private const string RequestIdNotFound = "No corresponding request found for response.";

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            updateSystem = World.GetOrCreateManager<ComponentUpdateSystem>();
            commandSystem = World.GetOrCreateManager<CommandSystem>();
            entitySystem = World.GetOrCreateManager<EntitySystem>();
            SetupDispatcherHandlers();
        }

        protected override void OnDestroyManager()
        {
            foreach (var pair in componentSpecificDispatchers)
            {
                pair.Value.Dispose();
            }
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            try
            {
                do
                {
                    using (var opList = worker.Connection.GetOpList(0))
                    {
                        inCriticalSection = opDeserializer.ParseOpListIntoDiff(opList, diff);
                    }
                }
                while (inCriticalSection);

                opDeserializer.Reset();

                if (diff.Disconnected)
                {
                    OnDisconnect(diff.DisconnectMessage);
                    return;
                }

                foreach (var entityId in diff.GetEntitiesAdded())
                {
                    AddEntity(entityId);
                }

                updateSystem.ApplyDiff(diff);
                commandSystem.ApplyDiff(diff);
                entitySystem.ApplyDiff(diff);

                foreach (var entityId in diff.GetEntitiesRemoved())
                {
                    RemoveEntity(entityId);
                }

                diff.Clear();
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }

        internal void OnAddEntity(AddEntityOp op)
        {
        }

        internal void OnRemoveEntity(RemoveEntityOp op)
        {
        }

        internal void OnDisconnect(DisconnectOp op)
        {
        }

        internal void AddEntity(EntityId entityId)
        {
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

        internal void RemoveEntity(EntityId entityId)
        {
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

        internal void OnDisconnect(string reason)
        {
            WorldCommands.DeallocateWorldCommandRequesters(EntityManager, worker.WorkerEntity);
            WorldCommands.RemoveWorldCommandRequesters(EntityManager, worker.WorkerEntity);
            EntityManager.AddSharedComponentData(worker.WorkerEntity,
                new OnDisconnected { ReasonForDisconnect = reason });
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
            Profiler.EndSample();
        }

        internal void AddDispatcherHandler(ComponentDispatcherHandler componentDispatcher)
        {
            componentSpecificDispatchers.Add(componentDispatcher.ComponentId, componentDispatcher);
            AddAllCommandComponents.Add(componentDispatcher.AddCommandComponents);
            componentDispatcher.AddCommandComponents(worker.WorkerEntity);
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
