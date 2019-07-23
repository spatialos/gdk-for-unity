// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Unity.Entities;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveEntity
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
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.CleanDataInWorld(world);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.CleanDataInWorld(world);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.CleanDataInWorld(world);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.CleanDataInWorld(world);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.CleanDataInWorld(world);
            }

            private void AddComponent(EntityId entityId)
            {
                workerSystem.TryGetEntity(entityId, out var entity);

                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Component();

                component.field1Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.Allocate(world);
                component.field2Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.Allocate(world);
                component.field3Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.Allocate(world);
                component.field4Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.Allocate(world);
                component.field5Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.Allocate(world);
                component.MarkDataClean();
                entityManager.AddSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                workerSystem.TryGetEntity(entityId, out var entity);
                entityManager.RemoveComponent<ComponentAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.TestSchema.ExhaustiveEntity.Component>(entity);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.Free(data.field1Handle);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.Free(data.field2Handle);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.Free(data.field3Handle);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.Free(data.field4Handle);
                global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.Free(data.field5Handle);

                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveEntity.Component>(entity);
            }

            private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)
            {
                workerSystem.TryGetEntity(update.EntityId, out var entity);
                if (!dataFromEntity.Exists(entity))
                {
                    return;
                }

                var data = dataFromEntity[entity];

                if (update.Update.Field1.HasValue)
                {
                    data.Field1 = update.Update.Field1.Value;
                }

                if (update.Update.Field2.HasValue)
                {
                    data.Field2 = update.Update.Field2.Value;
                }

                if (update.Update.Field3.HasValue)
                {
                    data.Field3 = update.Update.Field3.Value;
                }

                if (update.Update.Field4.HasValue)
                {
                    data.Field4 = update.Update.Field4.Value;
                }

                if (update.Update.Field5.HasValue)
                {
                    data.Field5 = update.Update.Field5.Value;
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
                        workerSystem.TryGetEntity(entityId, out var entity);
                        entityManager.SetSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        workerSystem.TryGetEntity(entityId, out var entity);
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
