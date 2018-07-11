using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public Entity WorkerEntity { get; }
        public ILogDispatcher LogDispatcher { get; }

        public readonly Dictionary<int, ComponentTranslation> TranslationUnits =
            new Dictionary<int, ComponentTranslation>();

        private Action<Entity, long> addAllCommandRequestSenders;

        private readonly EntityManager entityManager;

        private readonly Action<Entity, ComponentType, object> setComponentObjectAction;
        private readonly GameObjectManager gameObjectManager;

        // Reflection magic to get the internal method "SetComponentObject" on the specific EntityManager instance. This is required to add Components to Entities at runtime
        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        private readonly Dictionary<long, Entity> entityMapping;

        public const string UnexpectedAuthorityChangeError = "Unexpected state transition during authority change.";

        public MutableView(World world, ILogDispatcher logDispatcher)
        {
            entityManager = world.GetOrCreateManager<EntityManager>();
            entityMapping = new Dictionary<long, Entity>();
            gameObjectManager = new GameObjectManager();
            LogDispatcher = logDispatcher;

            setComponentObjectAction = (Action<Entity, ComponentType, object>) Delegate.CreateDelegate(
                typeof(Action<Entity, ComponentType, object>), entityManager, setComponentObjectMethodInfo);

            FindTranslationUnits();

            // Create the worker entity
            WorkerEntity = entityManager.CreateEntity(typeof(WorkerEntityTag));
            addAllCommandRequestSenders(WorkerEntity, WorkerEntityId);
        }

        public void Dispose()
        {
            entityManager.DestroyEntity(WorkerEntity);

            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
        }

        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponentData
        {
            entityManager.AddComponentData(entity, component);
        }

        public void AddComponent<T>(long entityId, T component) where T : struct, IComponentData
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Entity not found when attempting to add a component to an entity.")
                    .WithField(IdType.EntityId.ToString(), entityId)
                    .WithField("OpName", typeof(T).Name));
                return;
            }

            AddComponent(entity, component);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            entityManager.RemoveComponent<T>(entity);
        }

        public void RemoveComponent(Entity entity, ComponentType componentType)
        {
            entityManager.RemoveComponent(entity, componentType);
        }

        public void RemoveComponent<T>(long entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Entity not found when attemting to remove a component from an entity.")
                    .WithField(IdType.EntityId.ToString(), entityId)
                    .WithField("OpName", typeof(T).Name));
            }

            RemoveComponent<T>(entity);
        }

        public T GetComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return entityManager.GetComponentData<T>(entity);
        }

        public T GetComponentObject<T>(Entity entity) where T : Component
        {
            return entityManager.GetComponentObject<T>(entity);
        }

        public void SetComponentObject<T>(Entity entity, T component) where T : Component
        {
            if (!HasComponent<T>(entity))
            {
                entityManager.AddComponent(entity, typeof(T));
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
                entityManager.SetComponentData(entity, componentData);
            }
        }

        public void SetComponentObject(Entity entity, ComponentType componentType, object component)
        {
            if (!HasComponent(entity, componentType))
            {
                entityManager.AddComponent(entity, componentType);
            }

            setComponentObjectAction(entity, componentType, component);
        }

        public void SetComponentObject<T>(long entityId, T component) where T : Component
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Entity not found when attempting to set component object.")
                    .WithField(IdType.EntityId.ToString(), entityId)
                    .WithField("OpName", typeof(T).Name));
                return;
            }

            if (!HasComponent<T>(entity))
            {
                entityManager.AddComponent(entity, typeof(T));
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
            return entityManager.HasComponent(entity, componentType);
        }

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return entityMapping.TryGetValue(entityId, out entity);
        }

        public void AddGameObjectEntity(Entity entity, GameObject gameObject)
        {
            gameObjectManager.AddGameObjectEntity(entity, gameObject);
        }

        public void HandleAuthorityChange<T>(long entityId, Authority authority,
            ComponentPool<AuthoritiesChanged<T>> pool)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during authority change.")
                    .WithField(IdType.EntityId.ToString(), entityId)
                    .WithField("OpName", typeof(T).Name));
                return;
            }

            switch (authority)
            {
                case Authority.Authoritative:
                    if (!HasComponent<NotAuthoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnexpectedAuthorityChangeError)
                            .WithField("Type", typeof(Authority))
                            .WithField("NewState", Authority.Authoritative)
                            .WithField("OldState", Authority.NotAuthoritative));
                        return;
                    }

                    RemoveComponent<NotAuthoritative<T>>(entity);
                    AddComponent(entity, new Authoritative<T>());
                    break;
                case Authority.AuthorityLossImminent:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnexpectedAuthorityChangeError)
                            .WithField("Type", typeof(Authority))
                            .WithField("NewState", Authority.AuthorityLossImminent)
                            .WithField("OldState", Authority.Authoritative));
                        return;
                    }

                    AddComponent(entity, new AuthorityLossImminent<T>());
                    break;
                case Authority.NotAuthoritative:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnexpectedAuthorityChangeError)
                            .WithField("Type", typeof(Authority))
                            .WithField("NewState", Authority.NotAuthoritative)
                            .WithField("OldState", Authority.Authoritative));
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
            entityManager.AddComponent(WorkerEntity, typeof(IsConnected));
            entityManager.AddComponent(WorkerEntity, typeof(OnConnected));
        }

        internal void Disconnect(string reason)
        {
            entityManager.RemoveComponent<IsConnected>(WorkerEntity);
            entityManager.AddSharedComponentData(WorkerEntity, new OnDisconnected { ReasonForDisconnect = reason });
        }

        internal void CreateEntity(long entityId)
        {
            if (entityMapping.ContainsKey(entityId))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Tried to add an entity but there is already an entity associated with that EntityId.")
                    .WithField(IdType.EntityId.ToString(), entityId));
                return;
            }

            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });
            entityManager.AddComponentData(entity, new NewlyAddedSpatialOSEntity());

            addAllCommandRequestSenders(entity, entityId);
            entityMapping.Add(entityId, entity);
        }

        internal void RemoveEntity(long entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Tried to delete an entity but there is already an entity associated with that EntityId.")
                    .WithField(IdType.EntityId.ToString(), entityId));
                return;
            }

            entityManager.DestroyEntity(entityMapping[entityId]);
            gameObjectManager.TryRemoveGameObjectEntity(entity);
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
    }
}
