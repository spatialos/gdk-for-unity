// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Unity.Entities;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;
            private World world;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.ReadWrite<Component>(),
                ComponentType.ReadWrite<ComponentAuthority>(),
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
                this.world = world;
                entityManager = world.EntityManager;

                workerSystem = world.GetExistingSystem<WorkerSystem>();

                if (workerSystem == null)
                {
                    throw new ArgumentException("World instance is not running a valid SpatialOS worker");
                }
            }

            public void Clean(World world)
            {
                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.AProvider.CleanDataInWorld(world);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.CProvider.CleanDataInWorld(world);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.DProvider.CleanDataInWorld(world);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.EProvider.CleanDataInWorld(world);
            }

            private void AddComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                var component = new global::Improbable.DependentSchema.DependentComponent.Component();

                component.aHandle = global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.AProvider.Allocate(world);

                component.cHandle = global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.CProvider.Allocate(world);

                component.dHandle = global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.DProvider.Allocate(world);

                component.eHandle = global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.EProvider.Allocate(world);

                component.MarkDataClean();
                entityManager.AddSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                entityManager.RemoveComponent<ComponentAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.DependentSchema.DependentComponent.Component>(entity);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.AProvider.Free(data.aHandle);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.CProvider.Free(data.cHandle);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.DProvider.Free(data.dHandle);

                global::Improbable.DependentSchema.DependentComponent.ReferenceTypeProviders.EProvider.Free(data.eHandle);

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
                        entityManager.SetSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.SetSharedComponentData(entity, ComponentAuthority.Authoritative);
                        break;
                    }
                    case Authority.AuthorityLossImminent:
                        break;
                }
            }
        }
    }
}
