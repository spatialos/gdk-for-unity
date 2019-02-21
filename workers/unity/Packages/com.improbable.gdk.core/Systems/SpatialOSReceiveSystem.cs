using System;
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
        private readonly OpListDeserializer opDeserializer = new OpListDeserializer();

        private ViewDiff diff = new ViewDiff();

        private WorkerSystem worker;
        private ComponentUpdateSystem updateSystem;
        private CommandSystem commandSystem;
        private EntitySystem entitySystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            updateSystem = World.GetOrCreateManager<ComponentUpdateSystem>();
            commandSystem = World.GetOrCreateManager<CommandSystem>();
            entitySystem = World.GetOrCreateManager<EntitySystem>();

            diff = worker.Diff;
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            try
            {
                diff.Clear();

                bool inCriticalSection = false;
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
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }

        private void AddEntity(EntityId entityId)
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

            worker.EntityIdToEntity.Add(entityId, entity);
            Profiler.EndSample();
        }

        private void RemoveEntity(EntityId entityId)
        {
            if (!worker.TryGetEntity(entityId, out var entity))
            {
                throw new InvalidSpatialEntityStateException(
                    string.Format(Errors.EntityNotFoundForDeleteError, entityId.Id));
            }

            Profiler.BeginSample("OnRemoveEntity");
            EntityManager.DestroyEntity(worker.EntityIdToEntity[entityId]);
            worker.EntityIdToEntity.Remove(entityId);
            Profiler.EndSample();
        }

        private void OnDisconnect(string reason)
        {
            EntityManager.AddSharedComponentData(worker.WorkerEntity,
                new OnDisconnected { ReasonForDisconnect = reason });
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
