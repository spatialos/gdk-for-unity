// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Unity.Entities;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Unity.Collections;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveEntity
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.ReadWrite<global::Improbable.TestSchema.ExhaustiveEntity.Component>(),
                ComponentType.ReadOnly<global::Improbable.TestSchema.ExhaustiveEntity.HasAuthority>(),
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
                var query = entityManager.CreateEntityQuery(typeof(global::Improbable.TestSchema.ExhaustiveEntity.Component));
                var componentDataArray = query.ToComponentDataArray<global::Improbable.TestSchema.ExhaustiveEntity.Component>(Allocator.Temp);

                foreach (var component in componentDataArray)
                {
                    component.field1Handle.Dispose();

                    component.field2Handle.Dispose();

                    component.field3Handle.Dispose();

                    component.field4Handle.Dispose();

                    component.field5Handle.Dispose();
                }

                componentDataArray.Dispose();
            }

            private void AddComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Component();

                component.field1Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot>.Create();

                component.field2Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot?>.Create();

                component.field3Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>.Create();

                component.field4Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>.Create();

                component.field5Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>.Create();

                component.MarkDataClean();
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveEntity.HasAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.TestSchema.ExhaustiveEntity.Component>(entity);

                data.field1Handle.Dispose();

                data.field2Handle.Dispose();

                data.field3Handle.Dispose();

                data.field4Handle.Dispose();

                data.field5Handle.Dispose();

                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveEntity.Component>(entity);
            }

            private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)
            {
                var entity = workerSystem.GetEntity(update.EntityId);
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
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveEntity.HasAuthority>(entity);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.AddComponent<global::Improbable.TestSchema.ExhaustiveEntity.HasAuthority>(entity);
                        break;
                    }
                    case Authority.AuthorityLossImminent:
                        break;
                }
            }
        }
    }
}
