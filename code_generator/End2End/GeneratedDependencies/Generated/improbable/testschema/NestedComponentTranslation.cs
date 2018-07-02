// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Improbable.Worker;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Components;
using Improbable.TestSchema;

namespace Generated.Improbable.TestSchema
{
    public partial class NestedComponent
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.NestedComponent>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSNestedComponent);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSNestedComponent), typeof(Authoritative<SpatialOSNestedComponent>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(ComponentsUpdated<SpatialOSNestedComponent>), typeof(AuthoritiesChanged<SpatialOSNestedComponent>),
            };


            private static readonly ComponentPool<ComponentsUpdated<SpatialOSNestedComponent>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSNestedComponent>>(
                    () => new ComponentsUpdated<SpatialOSNestedComponent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSNestedComponent>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSNestedComponent>>(
                    () => new AuthoritiesChanged<SpatialOSNestedComponent>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.NestedComponent>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.NestedComponent>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.NestedComponent>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.NestedComponent>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.NestedComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var data = op.Data.Get().Value;

                var spatialOSNestedComponent = new SpatialOSNestedComponent();
                spatialOSNestedComponent.NestedType = global::Generated.Improbable.TestSchema.TypeName.ToNative(data.nestedType);
                spatialOSNestedComponent.DirtyBit = false;

                view.AddComponent(entity, spatialOSNestedComponent);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSNestedComponent>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.NestedComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponent<SpatialOSNestedComponent>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSNestedComponent>>(entity))
                {
                    if (update.nestedType.HasValue)
                    {
                        componentData.NestedType = global::Generated.Improbable.TestSchema.TypeName.ToNative(update.nestedType.Value);
                    }
                }

                componentData.DirtyBit = false;
                view.UpdateComponent(entity, componentData, UpdatesPool);
            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                view.RemoveComponent<SpatialOSNestedComponent>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSNestedComponent>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.TestSchema.NestedComponent.Update();
                        update.SetNestedType(global::Generated.Improbable.TestSchema.TypeName.ToSpatial(componentData.NestedType));
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.NestedComponent.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, UpdatesPool, groupIndex: 0);
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 1);
            }

            public override void SendCommands(Connection connection)
            {
            }

            public static NestedComponent.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (NestedComponent.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
