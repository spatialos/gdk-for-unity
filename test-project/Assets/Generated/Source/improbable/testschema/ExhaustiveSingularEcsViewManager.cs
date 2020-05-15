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
    public partial class ExhaustiveSingular
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.ReadWrite<global::Improbable.TestSchema.ExhaustiveSingular.Component>(),
                ComponentType.ReadOnly<global::Improbable.TestSchema.ExhaustiveSingular.HasAuthority>(),
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
                var query = entityManager.CreateEntityQuery(typeof(global::Improbable.TestSchema.ExhaustiveSingular.Component));
                var componentDataArray = query.ToComponentDataArray<global::Improbable.TestSchema.ExhaustiveSingular.Component>(Allocator.TempJob);

                foreach (var component in componentDataArray)
                {
                    component.field3Handle.Dispose();

                    component.field7Handle.Dispose();
                }

                componentDataArray.Dispose();
            }

            private void AddComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                var component = new global::Improbable.TestSchema.ExhaustiveSingular.Component();

                component.field3Handle = global::Improbable.Gdk.Core.ReferenceProvider<byte[]>.Create();

                component.field7Handle = global::Improbable.Gdk.Core.ReferenceProvider<string>.Create();

                component.MarkDataClean();
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveSingular.HasAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.TestSchema.ExhaustiveSingular.Component>(entity);

                data.field3Handle.Dispose();

                data.field7Handle.Dispose();

                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveSingular.Component>(entity);
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

                if (update.Update.Field6.HasValue)
                {
                    data.Field6 = update.Update.Field6.Value;
                }

                if (update.Update.Field7.HasValue)
                {
                    data.Field7 = update.Update.Field7.Value;
                }

                if (update.Update.Field8.HasValue)
                {
                    data.Field8 = update.Update.Field8.Value;
                }

                if (update.Update.Field9.HasValue)
                {
                    data.Field9 = update.Update.Field9.Value;
                }

                if (update.Update.Field10.HasValue)
                {
                    data.Field10 = update.Update.Field10.Value;
                }

                if (update.Update.Field11.HasValue)
                {
                    data.Field11 = update.Update.Field11.Value;
                }

                if (update.Update.Field12.HasValue)
                {
                    data.Field12 = update.Update.Field12.Value;
                }

                if (update.Update.Field13.HasValue)
                {
                    data.Field13 = update.Update.Field13.Value;
                }

                if (update.Update.Field14.HasValue)
                {
                    data.Field14 = update.Update.Field14.Value;
                }

                if (update.Update.Field15.HasValue)
                {
                    data.Field15 = update.Update.Field15.Value;
                }

                if (update.Update.Field16.HasValue)
                {
                    data.Field16 = update.Update.Field16.Value;
                }

                if (update.Update.Field17.HasValue)
                {
                    data.Field17 = update.Update.Field17.Value;
                }

                if (update.Update.Field18.HasValue)
                {
                    data.Field18 = update.Update.Field18.Value;
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
                        entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveSingular.HasAuthority>(entity);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.AddComponent<global::Improbable.TestSchema.ExhaustiveSingular.HasAuthority>(entity);
                        break;
                    }
                    case Authority.AuthorityLossImminent:
                        break;
                }
            }
        }
    }
}
