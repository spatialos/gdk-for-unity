using System;
using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Components
{
    public abstract class ComponentTranslation : IDisposable
    {
        protected struct RequestContext
        {
            public long EntityId;
            public IOutgoingCommandRequest Request;

            public RequestContext(long entityId, IOutgoingCommandRequest request)
            {
                EntityId = entityId;
                Request = request;
            }
        }

        public abstract ComponentType TargetComponentType { get; }
        protected readonly ILogDispatcher LogDispatcher;
        protected readonly WorkerBase worker;
        protected readonly string LoggerName = "Translation";

        protected ComponentTranslation(WorkerBase worker)
        {
            this.worker = worker;
            LogDispatcher = worker.LogDispatcher;
            translationHandle = GetNextHandle();
            HandleToTranslation.Add(translationHandle, this);
        }

        public void Dispose()
        {
            ComponentTranslation translationForHandle;
            if (HandleToTranslation.TryGetValue(translationHandle, out translationForHandle) &&
                translationForHandle == this)
            {
                HandleToTranslation.Remove(translationHandle);
            }
        }

        public static readonly Dictionary<uint, ComponentTranslation> HandleToTranslation =
            new Dictionary<uint, ComponentTranslation>();

        private static uint GetNextHandle() => (uint) HandleToTranslation.Count;

        protected uint translationHandle { get; }

        public abstract void RegisterWithDispatcher(Dispatcher dispatcher);

        public abstract ComponentType[] ReplicationComponentTypes { get; }
        public ComponentGroup ReplicationComponentGroup { get; set; }
        public abstract void ExecuteReplication(Connection connection);

        public abstract ComponentType[] CleanUpComponentTypes { get; }
        public List<ComponentGroup> CleanUpComponentGroups { get; set; }
        public abstract void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer);

        public abstract void AddCommandRequestSender(Entity entity, long entityId);
        public abstract void SendCommands(Connection connection);

        protected void RemoveComponents<T>(ref EntityCommandBuffer entityCommandBuffer, int groupIndex)
        {
            var entityArray = CleanUpComponentGroups[groupIndex].GetEntityArray();

            for (var i = 0; i < entityArray.Length; i++)
            {
                var entity = entityArray[i];
                if (worker.EntityManager.HasComponent<T>(entity))
                {
                    entityCommandBuffer.RemoveComponent<T>(entity);
                }
            }
        }

        protected void RemoveComponents<T>(ref EntityCommandBuffer entityCommandBuffer, ComponentPool<T> pool,
            int groupIndex)
            where T : Component
        {
            var entityArray = CleanUpComponentGroups[groupIndex].GetEntityArray();

            for (var i = 0; i < entityArray.Length; i++)
            {
                var entity = entityArray[i];
                if (worker.EntityManager.HasComponent<T>(entity))
                {
                    pool.PutComponent(worker.EntityManager.GetComponentObject<T>(entity));
                    entityCommandBuffer.RemoveComponent<T>(entity);
                }
            }
        }

        public void HandleAuthorityChange<T>(long entityId, Authority authority, ComponentPool<AuthoritiesChanged<T>> pool)
        {
            Entity entity;
            if (!worker.TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.EntityNotFoundOnAuthotityChange)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId)
                    .WithField(Component, typeof(T).Name));
                return;
            }

            switch (authority)
            {
                case Authority.Authoritative:
                    if (!worker.EntityManager.HasComponent<NotAuthoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.Authoritative)
                            .WithField(OldState, Authority.NotAuthoritative));
                        return;
                    }

                    worker.EntityManager.RemoveComponent<NotAuthoritative<T>>(entity);
                    worker.EntityManager.AddComponentData(entity, new Authoritative<T>());
                    break;
                case Authority.AuthorityLossImminent:
                    if (!worker.EntityManager.HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.AuthorityLossImminent)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    worker.EntityManager.AddComponentData(entity, new AuthorityLossImminent<T>());
                    break;
                case Authority.NotAuthoritative:
                    if (!worker.EntityManager.HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.NotAuthoritative)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    if (worker.EntityManager.HasComponent<AuthorityLossImminent<T>>(entity))
                    {
                        worker.EntityManager.RemoveComponent<AuthorityLossImminent<T>>(entity);
                    }

                    worker.EntityManager.RemoveComponent<Authoritative<T>>(entity);
                    worker.EntityManager.AddComponentData(entity, new NotAuthoritative<T>());
                    break;
            }

            var bufferedComponents = worker.EntityManager.HasComponent<AuthoritiesChanged<T>>(entity)
                ? worker.EntityManager.GetComponentObject<AuthoritiesChanged<T>>(entity)
                : pool.GetComponent();
            
            bufferedComponents.Buffer.Add(authority);
            worker.EntityManager.SetComponentObject(entity, bufferedComponents);
        }

        public void AddComponentsUpdated<T>(Entity entity, T update, ComponentPool<ComponentsUpdated<T>> pool)
            where T : struct, ISpatialComponentUpdate
        {
            AddReceivedMessageToComponent(entity, update, pool);
        }

        public void AddEventReceived<T>(Entity entity, T eventData, ComponentPool<EventsReceived<T>> pool)
            where T : struct, ISpatialEvent
        {
            AddReceivedMessageToComponent(entity, eventData, pool);
        }

        public void AddCommandRequest<T>(Entity entity, T commandRequest, ComponentPool<CommandRequests<T>> pool)
            where T : struct, IIncomingCommandRequest
        {
            AddReceivedMessageToComponent(entity, commandRequest, pool);
        }

        public void AddCommandResponse<T>(Entity entity, T commandResponse, ComponentPool<CommandResponses<T>> pool)
            where T : struct, IIncomingCommandResponse
        {
            AddReceivedMessageToComponent(entity, commandResponse, pool);
        }

        private void AddReceivedMessageToComponent<TMessagesReceived, TMessage>(Entity entity, TMessage message,
            ComponentPool<TMessagesReceived> pool)
            where TMessagesReceived : MessagesReceived<TMessage>, new()
        {
            var messageBuffer = worker.EntityManager.HasComponent<TMessagesReceived>(entity) ? worker.EntityManager.GetComponentObject<TMessagesReceived>(entity) : pool.GetComponent();
            messageBuffer.Buffer.Add(message);
            worker.EntityManager.SetComponentObject(entity, messageBuffer);
        }

        public const string Type = "Type";
        public const string OldState = "OldState";
        public const string NewState = "NewState";
        public const string Component = "Component";

        private static class Errors
        {
            public const string UnexpectedAuthorityChangeError =
                "Unexpected state transition during authority change.";

            public const string EntityNotFoundOnAuthotityChange = "Entity not found during authority change.";
        }
    }

    public interface IDispatcherCallbacks<T> where T : IComponentMetaclass
    {
        void OnAddComponent(AddComponentOp<T> op);
        void OnComponentUpdate(ComponentUpdateOp<T> op);
        void OnRemoveComponent(RemoveComponentOp op);
        void OnAuthorityChange(AuthorityChangeOp op);
    }
}
