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
    public partial class ExhaustiveBlittableSingular
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.ExhaustiveBlittableSingular>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSExhaustiveBlittableSingular);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSExhaustiveBlittableSingular), typeof(Authoritative<SpatialOSExhaustiveBlittableSingular>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(ComponentsUpdated<SpatialOSExhaustiveBlittableSingular>), typeof(AuthoritiesChanged<SpatialOSExhaustiveBlittableSingular>),
            };


            private static readonly ComponentPool<ComponentsUpdated<SpatialOSExhaustiveBlittableSingular>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSExhaustiveBlittableSingular>>(
                    () => new ComponentsUpdated<SpatialOSExhaustiveBlittableSingular>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveBlittableSingular>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveBlittableSingular>>(
                    () => new AuthoritiesChanged<SpatialOSExhaustiveBlittableSingular>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.ExhaustiveBlittableSingular>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.ExhaustiveBlittableSingular>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.ExhaustiveBlittableSingular>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.ExhaustiveBlittableSingular>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.ExhaustiveBlittableSingular> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var data = op.Data.Get().Value;

                var spatialOSExhaustiveBlittableSingular = new SpatialOSExhaustiveBlittableSingular();
                spatialOSExhaustiveBlittableSingular.Field1 = data.field1;
                spatialOSExhaustiveBlittableSingular.Field2 = data.field2;
                spatialOSExhaustiveBlittableSingular.Field4 = data.field4;
                spatialOSExhaustiveBlittableSingular.Field5 = data.field5;
                spatialOSExhaustiveBlittableSingular.Field6 = data.field6;
                spatialOSExhaustiveBlittableSingular.Field8 = data.field8;
                spatialOSExhaustiveBlittableSingular.Field9 = data.field9;
                spatialOSExhaustiveBlittableSingular.Field10 = data.field10;
                spatialOSExhaustiveBlittableSingular.Field11 = data.field11;
                spatialOSExhaustiveBlittableSingular.Field12 = data.field12;
                spatialOSExhaustiveBlittableSingular.Field13 = data.field13;
                spatialOSExhaustiveBlittableSingular.Field14 = data.field14;
                spatialOSExhaustiveBlittableSingular.Field15 = data.field15;
                spatialOSExhaustiveBlittableSingular.Field16 = data.field16.Id;
                spatialOSExhaustiveBlittableSingular.Field17 = global::Generated.Improbable.TestSchema.SomeType.ToNative(data.field17);
                spatialOSExhaustiveBlittableSingular.DirtyBit = false;

                view.AddComponent(entity, spatialOSExhaustiveBlittableSingular);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSExhaustiveBlittableSingular>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.ExhaustiveBlittableSingular> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponent<SpatialOSExhaustiveBlittableSingular>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSExhaustiveBlittableSingular>>(entity))
                {
                    if (update.field1.HasValue)
                    {
                        componentData.Field1 = update.field1.Value;
                    }
                    if (update.field2.HasValue)
                    {
                        componentData.Field2 = update.field2.Value;
                    }
                    if (update.field4.HasValue)
                    {
                        componentData.Field4 = update.field4.Value;
                    }
                    if (update.field5.HasValue)
                    {
                        componentData.Field5 = update.field5.Value;
                    }
                    if (update.field6.HasValue)
                    {
                        componentData.Field6 = update.field6.Value;
                    }
                    if (update.field8.HasValue)
                    {
                        componentData.Field8 = update.field8.Value;
                    }
                    if (update.field9.HasValue)
                    {
                        componentData.Field9 = update.field9.Value;
                    }
                    if (update.field10.HasValue)
                    {
                        componentData.Field10 = update.field10.Value;
                    }
                    if (update.field11.HasValue)
                    {
                        componentData.Field11 = update.field11.Value;
                    }
                    if (update.field12.HasValue)
                    {
                        componentData.Field12 = update.field12.Value;
                    }
                    if (update.field13.HasValue)
                    {
                        componentData.Field13 = update.field13.Value;
                    }
                    if (update.field14.HasValue)
                    {
                        componentData.Field14 = update.field14.Value;
                    }
                    if (update.field15.HasValue)
                    {
                        componentData.Field15 = update.field15.Value;
                    }
                    if (update.field16.HasValue)
                    {
                        componentData.Field16 = update.field16.Value.Id;
                    }
                    if (update.field17.HasValue)
                    {
                        componentData.Field17 = global::Generated.Improbable.TestSchema.SomeType.ToNative(update.field17.Value);
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

                view.RemoveComponent<SpatialOSExhaustiveBlittableSingular>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSExhaustiveBlittableSingular>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.TestSchema.ExhaustiveBlittableSingular.Update();
                        update.SetField1(componentData.Field1);
                        update.SetField2(componentData.Field2);
                        update.SetField4(componentData.Field4);
                        update.SetField5(componentData.Field5);
                        update.SetField6(componentData.Field6);
                        update.SetField8(componentData.Field8);
                        update.SetField9(componentData.Field9);
                        update.SetField10(componentData.Field10);
                        update.SetField11(componentData.Field11);
                        update.SetField12(componentData.Field12);
                        update.SetField13(componentData.Field13);
                        update.SetField14(componentData.Field14);
                        update.SetField15(componentData.Field15);
                        update.SetField16(new global::Improbable.EntityId(componentData.Field16));
                        update.SetField17(global::Generated.Improbable.TestSchema.SomeType.ToSpatial(componentData.Field17));
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.ExhaustiveBlittableSingular.Update update)
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

            public static ExhaustiveBlittableSingular.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ExhaustiveBlittableSingular.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
