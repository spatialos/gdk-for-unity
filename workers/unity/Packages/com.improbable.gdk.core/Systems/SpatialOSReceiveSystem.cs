using Improbable.Worker;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private Dispatcher dispatcher;

        private bool inCriticalSection;
        private string LoggerName = nameof(SpatialOSReceiveSystem);

        public struct Data
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<WorkerConfig> WorkerConfigs;
        }

        [Inject] private Data data;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            dispatcher = new Dispatcher();
            SetupDispatcherHandlers();
            worker = data.WorkerConfigs[0].Worker;
            foreach (var translationUnit in worker.TranslationUnits.Values)
            {
                translationUnit.RegisterWithDispatcher(dispatcher);
            }
        }

        private Worker worker;

        protected override void OnUpdate()
        {
            worker = data.WorkerConfigs[0].Worker;
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
            var entityId = op.EntityId.Id;
            if (worker.EntityMapping.ContainsKey(entityId))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to add an entity but there is already an entity associated with that EntityId.")
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

            worker.AddAllCommandRequestSenders(entity, entityId);
            worker.EntityMapping.Add(entityId, entity);
        }

        private void OnRemoveEntity(RemoveEntityOp op)
        {
            var entityId = op.EntityId.Id;
            if (!worker.TryGetEntity(entityId, out var entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to delete an entity but there is no entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            EntityManager.DestroyEntity(entity);
            worker.EntityMapping.Remove(entityId);
        }

        private void OnDisconnect(DisconnectOp op)
        {
            if (!worker.TryGetEntity(Worker.WorkerEntityId, out var entity))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to delete an entity but there is no entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, Worker.WorkerEntityId));
                return;
            }
            
            EntityManager.AddSharedComponentData(entity, new OnDisconnected { ReasonForDisconnect = op.Reason });
        }

        private void SetupDispatcherHandlers()
        {
            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);
            dispatcher.OnDisconnect(OnDisconnect);
            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });
        }
    }
}
