// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Unity.Entities;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;
            private World world;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.Create<Component>(),
                ComponentType.Create<ComponentAuthority>(),
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
                entityManager = world.GetOrCreateManager<EntityManager>();

                workerSystem = world.GetExistingManager<WorkerSystem>();

                if (workerSystem == null)
                {
                    throw new ArgumentException("World instance is not running a valid SpatialOS worker");
                }
            }

            public void Clean(World world)
            {
                NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.CleanDataInWorld(world);
                NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.CleanDataInWorld(world);
                NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.CleanDataInWorld(world);
                NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.CleanDataInWorld(world);
            }

            private void AddComponent(EntityId entityId)
            {
                workerSystem.TryGetEntity(entityId, out var entity);

                var component = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component();

                component.stringFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Allocate(world);
                component.optionalFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Allocate(world);
                component.listFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Allocate(world);
                component.mapFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Allocate(world);
                component.MarkDataClean();
                entityManager.AddSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                workerSystem.TryGetEntity(entityId, out var entity);
                entityManager.RemoveComponent<ComponentAuthority>(entity);

                var data = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(entity);
                NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Free(data.stringFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Free(data.optionalFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Free(data.listFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Free(data.mapFieldHandle);

                entityManager.RemoveComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(entity);
            }

            private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)
            {
                workerSystem.TryGetEntity(update.EntityId, out var entity);
                if (!dataFromEntity.Exists(entity))
                {
                    return;
                }

                var data = dataFromEntity[entity];

                if (update.Update.BoolField.HasValue)
                {
                    data.BoolField = update.Update.BoolField.Value;
                }

                if (update.Update.IntField.HasValue)
                {
                    data.IntField = update.Update.IntField.Value;
                }

                if (update.Update.LongField.HasValue)
                {
                    data.LongField = update.Update.LongField.Value;
                }

                if (update.Update.FloatField.HasValue)
                {
                    data.FloatField = update.Update.FloatField.Value;
                }

                if (update.Update.DoubleField.HasValue)
                {
                    data.DoubleField = update.Update.DoubleField.Value;
                }

                if (update.Update.StringField.HasValue)
                {
                    data.StringField = update.Update.StringField.Value;
                }

                if (update.Update.OptionalField.HasValue)
                {
                    data.OptionalField = update.Update.OptionalField.Value;
                }

                if (update.Update.ListField.HasValue)
                {
                    data.ListField = update.Update.ListField.Value;
                }

                if (update.Update.MapField.HasValue)
                {
                    data.MapField = update.Update.MapField.Value;
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
