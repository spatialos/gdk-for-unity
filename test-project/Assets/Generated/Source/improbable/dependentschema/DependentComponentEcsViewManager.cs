// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Unity.Entities;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Unity.Collections;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentComponent.Component>(),
                ComponentType.ReadOnly<global::Improbable.DependentSchema.DependentComponent.HasAuthority>(),
            };

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public ComponentType[] GetInitialComponents()
            {
                return initialComponents;
            }

            public void ApplyDiff(ViewDiff diff)
            {
                var diffStorage = (DiffComponentStorage) diff.GetComponentDiffStorage(ComponentId);

                foreach (var entityId in diffStorage.GetComponentsAdded())
                {
                    AddComponent(entityId);
                }

                var updates = diffStorage.GetUpdates();
                var dataFromEntity = workerSystem.GetComponentDataFromEntity<Component>();
                for (int i = 0; i < updates.Count; ++i)
                {
                    ApplyUpdate(in updates[i], dataFromEntity);
                }

                var authChanges = diffStorage.GetAuthorityChanges();
                for (int i = 0; i < authChanges.Count; ++i)
                {
                    ref readonly var change = ref authChanges[i];
                    SetAuthority(change.EntityId, change.Authority);
                }

                foreach (var entityId in diffStorage.GetComponentsRemoved())
                {
                    RemoveComponent(entityId);
                }
            }

            public void Init(World world)
            {
                entityManager = world.EntityManager;

                workerSystem = world.GetExistingSystem<WorkerSystem>();

                if (workerSystem == null)
                {
                    throw new ArgumentException("World instance is not running a valid SpatialOS worker");
                }
            }

            public void Clean()
            {
                var query = entityManager.CreateEntityQuery(typeof(global::Improbable.DependentSchema.DependentComponent.Component));
                var componentDataArray = query.ToComponentDataArray<global::Improbable.DependentSchema.DependentComponent.Component>(Allocator.TempJob);

                foreach (var component in componentDataArray)
                {
                    component.aHandle.Dispose();

                    component.cHandle.Dispose();

                    component.dHandle.Dispose();

                    component.eHandle.Dispose();
                }

                componentDataArray.Dispose();
            }

            private void AddComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                var component = new global::Improbable.DependentSchema.DependentComponent.Component();

                component.aHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.ExhaustiveRepeatedData>.Create();

                component.cHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.SomeEnum?>.Create();

                component.dHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>.Create();

                component.eHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>.Create();

                component.MarkDataClean();
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                entityManager.RemoveComponent<global::Improbable.DependentSchema.DependentComponent.HasAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.DependentSchema.DependentComponent.Component>(entity);

                data.aHandle.Dispose();

                data.cHandle.Dispose();

                data.dHandle.Dispose();

                data.eHandle.Dispose();

                entityManager.RemoveComponent<global::Improbable.DependentSchema.DependentComponent.Component>(entity);
            }

            private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)
            {
                var entity = workerSystem.GetEntity(update.EntityId);
                if (!dataFromEntity.Exists(entity))
                {
                    return;
                }

                var data = dataFromEntity[entity];

                if (update.Update.A.HasValue)
                {
                    data.A = update.Update.A.Value;
                }

                if (update.Update.B.HasValue)
                {
                    data.B = update.Update.B.Value;
                }

                if (update.Update.C.HasValue)
                {
                    data.C = update.Update.C.Value;
                }

                if (update.Update.D.HasValue)
                {
                    data.D = update.Update.D.Value;
                }

                if (update.Update.E.HasValue)
                {
                    data.E = update.Update.E.Value;
                }

                data.MarkDataClean();
                dataFromEntity[entity] = data;
            }

            private void SetAuthority(EntityId entityId, Authority authority)
            {
                switch (authority)
                {
                    case Authority.NotAuthoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.RemoveComponent<global::Improbable.DependentSchema.DependentComponent.HasAuthority>(entity);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.AddComponent<global::Improbable.DependentSchema.DependentComponent.HasAuthority>(entity);
                        break;
                    }
                    case Authority.AuthorityLossImminent:
                        break;
                }
            }
        }
    }
}
