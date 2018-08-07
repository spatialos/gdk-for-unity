using System;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : SpatialOSSystem
    {
        private Dispatcher dispatcher;

        private bool inCriticalSection;
        private string LoggerName = nameof(SpatialOSReceiveSystem);

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            dispatcher = new Dispatcher();
            SetupDispatcherHandlers();
        }

        protected override void OnUpdate()
        {
            do
            {
                using (var opList = Worker.Connection.GetOpList(0))
                {
                    dispatcher.Process(opList);
                }
            }
            while (inCriticalSection);
        }

        private void OnAddEntity(AddEntityOp op)
        {
            var entityId = op.EntityId.Id;
            if (Worker.EntityMapping.ContainsKey(entityId))
            {
                Worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to add an entity but there is already an entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });
            EntityManager.AddComponentData(entity, new NewlyAddedSpatialOSEntity());

            Worker.AddAllCommandRequestSenders(entity, entityId);
            Worker.EntityMapping.Add(entityId, entity);
        }

        private void OnRemoveEntity(RemoveEntityOp op)
        {
            var entityId = op.EntityId.Id;
            if (!Worker.TryGetEntity(entityId, out var entity))
            {
                Worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to delete an entity but there is no entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            EntityManager.DestroyEntity(entity);
            Worker.EntityMapping.Remove(entityId);
        }

        private void OnDisconnect(DisconnectOp op)
        {
            if (!Worker.TryGetEntity(WorkerBase.WorkerEntityId, out var entity))
            {
                Worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to delete an entity but there is no entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, WorkerBase.WorkerEntityId));
                return;
            }
            EntityManager.RemoveComponent<IsConnected>(entity);
            EntityManager.AddSharedComponentData(entity, new OnDisconnected { ReasonForDisconnect = op.Reason });
        }

        private void SetupDispatcherHandlers()
        {
            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);
            dispatcher.OnDisconnect(OnDisconnect);
            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            foreach (var translationUnit in Worker.TranslationUnits.Values)
            {
                translationUnit.RegisterWithDispatcher(dispatcher);
            }
        }
    }
}
