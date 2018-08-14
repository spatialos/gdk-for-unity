using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.Components;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public class MutableView : IDisposable
    {
        public const long WorkerEntityId = -1337;
        private const string LoggerName = "MutableView";
        public Entity WorkerEntity { get; }
        public ILogDispatcher LogDispatcher { get; }

        public List<Action<Entity>> AddAllCommandComponents = new List<Action<Entity>>();

        public readonly EntityManager EntityManager;

        private readonly Action<Entity, ComponentType, object> setComponentObjectAction;

        // Reflection magic to get the internal method "SetComponentObject" on the specific EntityManager instance. This is required to add Components to Entities at runtime
        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        private readonly Dictionary<EntityId, Entity> entityMapping;

        public MutableView(World world, ILogDispatcher logDispatcher)
        {
            EntityManager = world.GetOrCreateManager<EntityManager>();
            entityMapping = new Dictionary<EntityId, Entity>();
            LogDispatcher = logDispatcher;

            setComponentObjectAction = (Action<Entity, ComponentType, object>) Delegate.CreateDelegate(
                typeof(Action<Entity, ComponentType, object>), EntityManager, setComponentObjectMethodInfo);

            // Create the worker entity
            WorkerEntity = EntityManager.CreateEntity(typeof(WorkerEntityTag));
            AddAllCommandComponents.ForEach(action => action(WorkerEntity));
        }

        public void Dispose()
        {
            EntityManager.DestroyEntity(WorkerEntity);
        }

        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponentData
        {
            EntityManager.AddComponentData(entity, component);
        }

        public void AddComponent<T>(EntityId entityId, T component) where T : struct, IComponentData
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.EntityNotFoundOnAdd)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId)
                    .WithField(Component, typeof(T).Name));
                return;
            }

            AddComponent(entity, component);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            EntityManager.RemoveComponent<T>(entity);
        }

        public void RemoveComponent(Entity entity, ComponentType componentType)
        {
            EntityManager.RemoveComponent(entity, componentType);
        }

        public void RemoveComponent<T>(EntityId entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.EntityNotFoundOnRemove)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId)
                    .WithField(Component, typeof(T).Name));
            }

            RemoveComponent<T>(entity);
        }

        public T GetComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return EntityManager.GetComponentData<T>(entity);
        }

        public T GetComponentObject<T>(Entity entity) where T : Component
        {
            return EntityManager.GetComponentObject<T>(entity);
        }

        public void SetComponentObject<T>(Entity entity, T component) where T : Component
        {
            if (!HasComponent<T>(entity))
            {
                EntityManager.AddComponent(entity, typeof(T));
            }

            setComponentObjectAction(entity, typeof(T), component);
        }

        public void SetComponentData<T>(Entity entity, T componentData) where T : struct, IComponentData
        {
            if (!HasComponent<T>(entity))
            {
                AddComponent(entity, componentData);
            }
            else
            {
                EntityManager.SetComponentData(entity, componentData);
            }
        }

        public void SetComponentObject(Entity entity, ComponentType componentType, object component)
        {
            if (!HasComponent(entity, componentType))
            {
                EntityManager.AddComponent(entity, componentType);
            }

            setComponentObjectAction(entity, componentType, component);
        }

        public void SetComponentObject<T>(EntityId entityId, T component) where T : Component
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.EntityNotFoundOnSetComponentObject)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId)
                    .WithField(Component, typeof(T).Name));
                return;
            }

            if (!HasComponent<T>(entity))
            {
                EntityManager.AddComponent(entity, typeof(T));
            }

            setComponentObjectAction(entity, typeof(T), component);
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

        public bool HasComponent<T>(Entity entity)
        {
            return HasComponent(entity, typeof(T));
        }

        public bool HasComponent(Entity entity, ComponentType componentType)
        {
            return EntityManager.HasComponent(entity, componentType);
        }

        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return entityMapping.TryGetValue(entityId, out entity);
        }

        public void HandleAuthorityChange<T>(EntityId entityId, Authority authority,
            ComponentPool<AuthoritiesChanged<T>> pool)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
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
                    if (!HasComponent<NotAuthoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.Authoritative)
                            .WithField(OldState, Authority.NotAuthoritative));
                        return;
                    }

                    RemoveComponent<NotAuthoritative<T>>(entity);
                    AddComponent(entity, new Authoritative<T>());
                    break;
                case Authority.AuthorityLossImminent:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.AuthorityLossImminent)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    AddComponent(entity, new AuthorityLossImminent<T>());
                    break;
                case Authority.NotAuthoritative:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.NotAuthoritative)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    if (HasComponent<AuthorityLossImminent<T>>(entity))
                    {
                        RemoveComponent<AuthorityLossImminent<T>>(entity);
                    }

                    RemoveComponent<Authoritative<T>>(entity);
                    AddComponent(entity, new NotAuthoritative<T>());
                    break;
            }

            var bufferedComponents = GetOrCreateComponent(entity, pool);
            bufferedComponents.Buffer.Add(authority);
            SetComponentObject(entity, bufferedComponents);
        }

        internal void Connect()
        {
            EntityManager.AddComponent(WorkerEntity, typeof(IsConnected));
            EntityManager.AddComponent(WorkerEntity, typeof(OnConnected));
        }

        internal void Disconnect(string reason)
        {
            EntityManager.RemoveComponent<IsConnected>(WorkerEntity);
            EntityManager.AddSharedComponentData(WorkerEntity, new OnDisconnected { ReasonForDisconnect = reason });
        }

        internal void CreateEntity(EntityId entityId)
        {
            if (entityMapping.ContainsKey(entityId))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.DuplicateAdditionOfEntity)
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

            AddAllCommandComponents.ForEach(action => action(entity));
            entityMapping.Add(entityId, entity);
        }

        internal void RemoveEntity(EntityId entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.NoEntityFoundDuringDeletion)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            EntityManager.DestroyEntity(entityMapping[entityId]);
            entityMapping.Remove(entityId);
        }

        private T GetOrCreateComponent<T>(Entity entity, ComponentPool<T> pool) where T : Component
        {
            return HasComponent<T>(entity) ? GetComponentObject<T>(entity) : pool.GetComponent();
        }

        private void AddReceivedMessageToComponent<TMessagesReceived, TMessage>(Entity entity, TMessage message,
            ComponentPool<TMessagesReceived> pool)
            where TMessagesReceived : MessagesReceived<TMessage>, new()
        {
            var messageBuffer = GetOrCreateComponent(entity, pool);
            messageBuffer.Buffer.Add(message);
            SetComponentObject(entity, messageBuffer);
        }

        private static class Errors
        {
            public const string UnexpectedAuthorityChangeError =
                "Unexpected state transition during authority change.";

            public const string EntityNotFoundOnAdd =
                "Entity not found when attempting to add a component.";

            public const string EntityNotFoundOnRemove =
                "Entity not found when attemting to remove a component.";

            public const string EntityNotFoundOnSetComponentObject =
                "Entity not found when attempting to set component object.";

            public const string EntityNotFoundOnAuthotityChange = "Entity not found during authority change.";

            public const string DuplicateAdditionOfEntity =
                "Tried to add an entity but there is already an entity associated with that EntityId.";

            public const string NoEntityFoundDuringDeletion =
                "Tried to delete an entity but there is no entity associated with that EntityId.";
        }

        public const string Type = "Type";
        public const string OldState = "OldState";
        public const string NewState = "NewState";
        public const string Component = "Component";
    }
}
