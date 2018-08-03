using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
using Improbable.Worker;
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

        public readonly Dictionary<int, ComponentTranslation> TranslationUnits =
            new Dictionary<int, ComponentTranslation>();

        private Action<Entity, long> addAllCommandRequestSenders;

        public readonly EntityManager EntityManager;

        private readonly Dictionary<long, Entity> entityMapping;

        public MutableView(World world, ILogDispatcher logDispatcher)
        {
            EntityManager = world.GetOrCreateManager<EntityManager>();
            entityMapping = new Dictionary<long, Entity>();
            LogDispatcher = logDispatcher;


            FindTranslationUnits();

            // Create the worker entity
            WorkerEntity = EntityManager.CreateEntity(typeof(WorkerEntityTag));
            addAllCommandRequestSenders(WorkerEntity, WorkerEntityId);
        }

        public void Dispose()
        {
            EntityManager.DestroyEntity(WorkerEntity);

            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
        }

        public void SetComponentObject<T>(Entity entity, T component) where T : Component
        {
            if (!EntityManager.HasComponent<T>(entity))
            {
                EntityManager.AddComponent(entity, typeof(T));
            }

            EntityManager.SetComponentObject(entity, component);
        }

        public void SetComponentData<T>(Entity entity, T componentData) where T : struct, IComponentData
        {
            if (!EntityManager.HasComponent<T>(entity))
            {
                EntityManager.AddComponentData(entity, componentData);
            }
            else
            {
                EntityManager.SetComponentData(entity, componentData);
            }
        }

        public void SetComponentObject(Entity entity, ComponentType componentType, object component)
        {
            if (!EntityManager.HasComponent(entity, componentType))
            {
                EntityManager.AddComponent(entity, componentType);
            }

            EntityManager.SetComponentObject(entity, componentType, component);
        }

        public void SetComponentObject<T>(long entityId, T component) where T : Component
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

            if (!EntityManager.HasComponent<T>(entity))
            {
                EntityManager.AddComponent(entity, typeof(T));
            }

            EntityManager.SetComponentObject(entity, typeof(T), component);
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

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return entityMapping.TryGetValue(entityId, out entity);
        }

        public void HandleAuthorityChange<T>(long entityId, Authority authority,
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
                    if (!EntityManager.HasComponent<NotAuthoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.Authoritative)
                            .WithField(OldState, Authority.NotAuthoritative));
                        return;
                    }

                    EntityManager.RemoveComponent<NotAuthoritative<T>>(entity);
                    EntityManager.AddComponentData(entity, new Authoritative<T>());
                    break;
                case Authority.AuthorityLossImminent:
                    if (!EntityManager.HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.AuthorityLossImminent)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    EntityManager.AddComponentData(entity, new AuthorityLossImminent<T>());
                    break;
                case Authority.NotAuthoritative:
                    if (!EntityManager.HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(Errors.UnexpectedAuthorityChangeError)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(Type, typeof(Authority))
                            .WithField(NewState, Authority.NotAuthoritative)
                            .WithField(OldState, Authority.Authoritative));
                        return;
                    }

                    if (EntityManager.HasComponent<AuthorityLossImminent<T>>(entity))
                    {
                        EntityManager.RemoveComponent<AuthorityLossImminent<T>>(entity);
                    }

                    EntityManager.RemoveComponent<Authoritative<T>>(entity);
                    EntityManager.AddComponentData(entity, new NotAuthoritative<T>());
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

        internal void CreateEntity(long entityId)
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

            addAllCommandRequestSenders(entity, entityId);
            entityMapping.Add(entityId, entity);
        }

        internal void RemoveEntity(long entityId)
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

        private void FindTranslationUnits()
        {
            var translationTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentTranslation).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var translationType in translationTypes)
            {
                var translator = (ComponentTranslation) Activator.CreateInstance(translationType, this);
                TranslationUnits.Add(translator.TargetComponentType.TypeIndex, translator);

                addAllCommandRequestSenders += translator.AddCommandRequestSender;
            }
        }

        private T GetOrCreateComponent<T>(Entity entity, ComponentPool<T> pool) where T : Component
        {
            return EntityManager.HasComponent<T>(entity) ? EntityManager.GetComponentObject<T>(entity) : pool.GetComponent();
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
