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
    public class MutableView
    {
        public const long WorkerEntityId = -1337;

        public Dictionary<int, ComponentTranslation> TranslationUnits;
        public Action<Entity, long> AddAllCommandRequestSenders;

        public Connection Connection;
        public World World;

        public EntityManager EntityManager { get; }

        public Entity WorkerEntity { get; }

        private readonly Action<Entity, ComponentType, object> setComponentObjectAction;
        private readonly GameObjectManager gameObjectManager;

        // Reflection magic to get the internal method "SetComponentObject" on the specific EntityManager instance. This is required to add Components to Entities at runtime
        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        private readonly Dictionary<long, Entity> entityMapping;

        public MutableView(World world)
        {
            World = world;
            EntityManager = world.GetOrCreateManager<EntityManager>();
            entityMapping = new Dictionary<long, Entity>();
            gameObjectManager = new GameObjectManager();

            setComponentObjectAction = (Action<Entity, ComponentType, object>) Delegate.CreateDelegate(
                typeof(Action<Entity, ComponentType, object>), EntityManager, setComponentObjectMethodInfo);

            FindTranslationUnits();

            // Create the worker entity
            WorkerEntity = EntityManager.CreateEntity(typeof(WorkerEntityTag));
            AddAllCommandRequestSenders(WorkerEntity, WorkerEntityId);
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

        private void FindTranslationUnits()
        {
            TranslationUnits = new Dictionary<int, ComponentTranslation>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var translationTypes = assembly.GetTypes().Where(
                    type => typeof(ComponentTranslation).IsAssignableFrom(type) && !type.IsAbstract).ToList();

                foreach (var translationType in translationTypes)
                {
                    var translator = (ComponentTranslation) Activator.CreateInstance(translationType, this);
                    TranslationUnits.Add(translator.TargetComponentType.TypeIndex, translator);

                    AddAllCommandRequestSenders += translator.AddCommandRequestSender;
                }
            }
        }

        private T GetOrCreateComponent<T>(Entity entity) where T : Component, new()
        {
            return HasComponent<T>(entity) ? GetComponentObject<T>(entity) : new T();
        }

        private T GetOrCreateComponent<T>(Entity entity, ComponentPool<T> pool) where T : Component
        {
            return HasComponent<T>(entity) ? GetComponentObject<T>(entity) : pool.GetComponent();
        }

        private void AddReceivedMessageToComponent<TMessagesReceived, TMessage>(Entity entity, TMessage message)
            where TMessagesReceived : MessagesReceived<TMessage>, new()
        {
            TMessagesReceived messageBuffer = GetOrCreateComponent<TMessagesReceived>(entity);
            messageBuffer.Buffer.Add(message);
            SetComponentObject(entity, messageBuffer);
        }

        private void AddReceivedMessageToComponent<TMessagesReceived, TMessage>(Entity entity, TMessage message,
            ComponentPool<TMessagesReceived> pool)
            where TMessagesReceived : MessagesReceived<TMessage>, new()
        {
            TMessagesReceived messageBuffer = GetOrCreateComponent(entity, pool);
            messageBuffer.Buffer.Add(message);
            SetComponentObject(entity, messageBuffer);
        }

        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponentData
        {
            EntityManager.AddComponentData(entity, component);
        }

        public void AddComponent<T>(long entityId, T component) where T : struct, IComponentData
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.NoCorrespondingEntityForEntityId, typeof(T).Name, entityId);
                return;
            }

            AddComponent(entity, component);
        }

        public void AddSharedComponent<T>(Entity entity, T component) where T : struct, ISharedComponentData
        {
            EntityManager.AddSharedComponentData(entity, component);
        }

        public void AddSharedComponent<T>(long entityId, T component) where T : struct, ISharedComponentData
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.NoCorrespondingEntityForEntityId, typeof(T).Name, entityId);
            }

            AddSharedComponent(entity, component);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            EntityManager.RemoveComponent<T>(entity);
        }

        public void RemoveComponent(Entity entity, ComponentType componentType)
        {
            EntityManager.RemoveComponent(entity, componentType);
        }

        public void RemoveComponent<T>(long entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.NoCorrespondingEntityForEntityId, typeof(T).Name, entityId);
            }

            RemoveComponent<T>(entity);
        }

        public T GetComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return EntityManager.GetComponentData<T>(entity);
        }

        public T GetSharedComponent<T>(Entity entity) where T : struct, ISharedComponentData
        {
            return EntityManager.GetSharedComponentData<T>(entity);
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

        public void SetComponentObject(Entity entity, ComponentType componentType, object component)
        {
            if (!HasComponent(entity, componentType))
            {
                EntityManager.AddComponent(entity, componentType);
            }

            setComponentObjectAction(entity, componentType, component);
        }

        public void SetComponentObject<T>(long entityId, T component) where T : Component
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.NoCorrespondingEntityForEntityId, typeof(T).Name, entityId);
                return;
            }

            if (!HasComponent<T>(entity))
            {
                EntityManager.AddComponent(entity, typeof(T));
            }

            setComponentObjectAction(entity, typeof(T), component);
        }

        public void UpdateComponent<T>(Entity entity, T component, ComponentPool<ComponentsUpdated<T>> pool)
            where T : struct, IComponentData
        {
            EntityManager.SetComponentData(entity, component);
            AddReceivedMessageToComponent<ComponentsUpdated<T>, T>(entity, component, pool);
        }

        public void UpdateSharedComponent<T>(Entity entity, T component) where T : struct, ISharedComponentData
        {
            EntityManager.SetSharedComponentData(entity, component);
        }

        public void UpdateComponentObject<T>(Entity entity, T component, ComponentPool<ComponentsUpdated<T>> pool)
            where T : Component
        {
            setComponentObjectAction(entity, typeof(T), component);
            AddReceivedMessageToComponent(entity, component, pool);
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

        public void AddCommandResponse<T>(Entity entity, T commandResponse) where T : struct, IIncomingCommandResponse
        {
            AddReceivedMessageToComponent<CommandResponses<T>, T>(entity, commandResponse);
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

        public void CreateEntity(long entityId)
        {
            if (entityMapping.ContainsKey(entityId))
            {
                Debug.LogErrorFormat(Errors.AddEntityButEntityIdAlreadyExists, entityId);
                return;
            }

            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });
            EntityManager.AddComponentData(entity, new NewlyCreatedEntity());

            AddAllCommandRequestSenders(entity, entityId);
            entityMapping.Add(entityId, entity);
        }

        public void AddGameObjectEntity(Entity entity, GameObject gameObject)
        {
            gameObjectManager.AddGameObjectEntity(entity, gameObject);
        }

        public void RemoveEntity(long entityId)
        {
            Entity entity;
            if (!entityMapping.TryGetValue(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.DeleteNonExistantEntity, entityId);
                return;
            }

            EntityManager.DestroyEntity(entityMapping[entityId]);
            gameObjectManager.TryRemoveGameObjectEntity(entity);
            entityMapping.Remove(entityId);
        }

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return entityMapping.TryGetValue(entityId, out entity);
        }

        public void HandleAuthorityChange<T>(Entity entity, Authority authority,
            ComponentPool<AuthoritiesChanged<T>> pool)
        {
            switch (authority)
            {
                case Authority.Authoritative:
                    if (!HasComponent<NotAuthoritative<T>>(entity))
                    {
                        Debug.LogErrorFormat(Errors.IncorrectAuthorityTransition, Authority.Authoritative,
                            Authority.NotAuthoritative);
                        return;
                    }

                    RemoveComponent<NotAuthoritative<T>>(entity);
                    AddComponent(entity, new Authoritative<T>());
                    break;
                case Authority.AuthorityLossImminent:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        Debug.LogErrorFormat(Errors.IncorrectAuthorityTransition, Authority.AuthorityLossImminent,
                            Authority.Authoritative);
                        return;
                    }

                    AddComponent(entity, new AuthorityLossImminent<T>());
                    break;
                case Authority.NotAuthoritative:
                    if (!HasComponent<Authoritative<T>>(entity))
                    {
                        Debug.LogErrorFormat(Errors.IncorrectAuthorityTransition, Authority.NotAuthoritative,
                            Authority.Authoritative);
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

            AuthoritiesChanged<T> bufferedComponents = GetOrCreateComponent(entity, pool);
            bufferedComponents.Buffer.Add(authority);
            SetComponentObject(entity, bufferedComponents);
        }

        public void HandleAuthorityChange<T>(long entityId, Authority authority,
            ComponentPool<AuthoritiesChanged<T>> pool)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                Debug.LogErrorFormat(Errors.NoCorrespondingEntityForEntityId, typeof(T).Name, entityId);
                return;
            }

            HandleAuthorityChange<T>(entity, authority, pool);
        }

        private static class Errors
        {
            public static string NoCorrespondingEntityForEntityId =
                "Received {0} for EntityId {1}, but there is no entity associated with that EntityId.";

            public const string IncorrectAuthorityTransition =
                "Unexpected authority transition to state: {0}. Expected previous state: {1}.";

            public const string AddEntityButEntityIdAlreadyExists =
                "Tried to add an entity with EntityId {0}, but there is already an entity associated with that EntityId.";

            public const string DeleteNonExistantEntity =
                "Tried to delete an entity with EntityId {0}, but there is no entity associated with that EntityId";
        }
    }
}
