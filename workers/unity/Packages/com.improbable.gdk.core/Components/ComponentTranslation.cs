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
        protected readonly MutableView View;
        protected readonly ILogDispatcher LogDispatcher;

        protected ComponentTranslation(MutableView view)
        {
            View = view;
            LogDispatcher = view.LogDispatcher;
            TranslationHandle = GetNextHandle();
            HandleToTranslation.Add(TranslationHandle, this);
        }

        public void Dispose()
        {
            if (HandleToTranslation.TryGetValue(TranslationHandle, out var translationForHandle) &&
                translationForHandle == this)
            {
                HandleToTranslation.Remove(TranslationHandle);
            }
        }

        public static readonly Dictionary<uint, ComponentTranslation> HandleToTranslation =
            new Dictionary<uint, ComponentTranslation>();

        private static uint GetNextHandle() => (uint) HandleToTranslation.Count;

        protected uint TranslationHandle { get; }

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
                if (View.HasComponent<T>(entity))
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
                if (View.HasComponent<T>(entity))
                {
                    pool.PutComponent(View.GetComponentObject<T>(entity));
                    entityCommandBuffer.RemoveComponent<T>(entity);
                }
            }
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
