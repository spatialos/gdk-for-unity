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
    public partial class ExhaustiveMapKey
    {
        public class EcsViewManager : IEcsViewManager
        {
            private WorkerSystem workerSystem;
            private EntityManager entityManager;

            private readonly ComponentType[] initialComponents = new ComponentType[]
            {
                ComponentType.ReadWrite<global::Improbable.TestSchema.ExhaustiveMapKey.Component>(),
                ComponentType.ReadOnly<global::Improbable.TestSchema.ExhaustiveMapKey.HasAuthority>(),
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
                var query = entityManager.CreateEntityQuery(typeof(global::Improbable.TestSchema.ExhaustiveMapKey.Component));
                var componentDataArray = query.ToComponentDataArray<global::Improbable.TestSchema.ExhaustiveMapKey.Component>(Allocator.Temp);

                foreach (var component in componentDataArray)
                {
                    component.field1Handle.Dispose();

                    component.field2Handle.Dispose();

                    component.field3Handle.Dispose();

                    component.field4Handle.Dispose();

                    component.field5Handle.Dispose();

                    component.field6Handle.Dispose();

                    component.field7Handle.Dispose();

                    component.field8Handle.Dispose();

                    component.field9Handle.Dispose();

                    component.field10Handle.Dispose();

                    component.field11Handle.Dispose();

                    component.field12Handle.Dispose();

                    component.field13Handle.Dispose();

                    component.field14Handle.Dispose();

                    component.field15Handle.Dispose();

                    component.field16Handle.Dispose();

                    component.field17Handle.Dispose();

                    component.field18Handle.Dispose();
                }

                componentDataArray.Dispose();
            }

            private void AddComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                var component = new global::Improbable.TestSchema.ExhaustiveMapKey.Component();

                component.field1Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<bool, string>>.Create();

                component.field2Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<float, string>>.Create();

                component.field3Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<byte[], string>>.Create();

                component.field4Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                component.field5Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                component.field6Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<double, string>>.Create();

                component.field7Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, string>>.Create();

                component.field8Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.Create();

                component.field9Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.Create();

                component.field10Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                component.field11Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                component.field12Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.Create();

                component.field13Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.Create();

                component.field14Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                component.field15Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                component.field16Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>>.Create();

                component.field17Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>>.Create();

                component.field18Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>>.Create();

                component.MarkDataClean();
                entityManager.AddComponentData(entity, component);
            }

            private void RemoveComponent(EntityId entityId)
            {
                var entity = workerSystem.GetEntity(entityId);
                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveMapKey.HasAuthority>(entity);

                var data = entityManager.GetComponentData<global::Improbable.TestSchema.ExhaustiveMapKey.Component>(entity);

                data.field1Handle.Dispose();

                data.field2Handle.Dispose();

                data.field3Handle.Dispose();

                data.field4Handle.Dispose();

                data.field5Handle.Dispose();

                data.field6Handle.Dispose();

                data.field7Handle.Dispose();

                data.field8Handle.Dispose();

                data.field9Handle.Dispose();

                data.field10Handle.Dispose();

                data.field11Handle.Dispose();

                data.field12Handle.Dispose();

                data.field13Handle.Dispose();

                data.field14Handle.Dispose();

                data.field15Handle.Dispose();

                data.field16Handle.Dispose();

                data.field17Handle.Dispose();

                data.field18Handle.Dispose();

                entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveMapKey.Component>(entity);
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
                        entityManager.RemoveComponent<global::Improbable.TestSchema.ExhaustiveMapKey.HasAuthority>(entity);
                        break;
                    }
                    case Authority.Authoritative:
                    {
                        var entity = workerSystem.GetEntity(entityId);
                        entityManager.AddComponent<global::Improbable.TestSchema.ExhaustiveMapKey.HasAuthority>(entity);
                        break;
                    }
                    case Authority.AuthorityLossImminent:
                        break;
                }
            }
        }
    }
}
