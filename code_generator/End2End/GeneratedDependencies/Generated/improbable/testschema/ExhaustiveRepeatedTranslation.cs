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
    public partial class ExhaustiveRepeated
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.ExhaustiveRepeated>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSExhaustiveRepeated);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSExhaustiveRepeated), typeof(Authoritative<SpatialOSExhaustiveRepeated>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSExhaustiveRepeated>),
                typeof(ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>), 
            };


            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveRepeated>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveRepeated>>(
                    () => new AuthoritiesChanged<SpatialOSExhaustiveRepeated>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>>(
                    () => new ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.ExhaustiveRepeated>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.ExhaustiveRepeated>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.ExhaustiveRepeated>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.ExhaustiveRepeated>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.ExhaustiveRepeated> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }
                var data = op.Data.Get().Value;

                var spatialOSExhaustiveRepeated = new SpatialOSExhaustiveRepeated();
                spatialOSExhaustiveRepeated.Field2 = data.field2;
                spatialOSExhaustiveRepeated.Field4 = data.field4;
                spatialOSExhaustiveRepeated.Field5 = data.field5;
                spatialOSExhaustiveRepeated.Field6 = data.field6;
                spatialOSExhaustiveRepeated.Field7 = data.field7;
                spatialOSExhaustiveRepeated.Field8 = data.field8;
                spatialOSExhaustiveRepeated.Field9 = data.field9;
                spatialOSExhaustiveRepeated.Field10 = data.field10;
                spatialOSExhaustiveRepeated.Field11 = data.field11;
                spatialOSExhaustiveRepeated.Field12 = data.field12;
                spatialOSExhaustiveRepeated.Field13 = data.field13;
                spatialOSExhaustiveRepeated.Field14 = data.field14;
                spatialOSExhaustiveRepeated.Field15 = data.field15;
                spatialOSExhaustiveRepeated.Field16 = data.field16.Select(internalObject => internalObject.Id).ToList();
                spatialOSExhaustiveRepeated.Field17 = data.field17.Select(internalObject => global::Generated.Improbable.TestSchema.SomeType.ToNative(internalObject)).ToList();
                spatialOSExhaustiveRepeated.DirtyBit = false;

                view.SetComponentObject(entity, spatialOSExhaustiveRepeated);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSExhaustiveRepeated>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.ExhaustiveRepeated> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponentObject<SpatialOSExhaustiveRepeated>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSExhaustiveRepeated>>(entity))
                {
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
                    if (update.field7.HasValue)
                    {
                        componentData.Field7 = update.field7.Value;
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
                        componentData.Field16 = update.field16.Value.Select(internalObject => internalObject.Id).ToList();
                    }
                    if (update.field17.HasValue)
                    {
                        componentData.Field17 = update.field17.Value.Select(internalObject => global::Generated.Improbable.TestSchema.SomeType.ToNative(internalObject)).ToList();
                    }
                }

                componentData.DirtyBit = false;

                view.SetComponentObject(entity, componentData);

                var componentFieldsUpdated = false;
                var gdkUpdate = new SpatialOSExhaustiveRepeated.Update();
                if (update.field2.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field2 = new Option<global::System.Collections.Generic.List<float>>(update.field2.Value);
                }
                if (update.field4.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field4 = new Option<global::System.Collections.Generic.List<int>>(update.field4.Value);
                }
                if (update.field5.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field5 = new Option<global::System.Collections.Generic.List<long>>(update.field5.Value);
                }
                if (update.field6.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field6 = new Option<global::System.Collections.Generic.List<double>>(update.field6.Value);
                }
                if (update.field7.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field7 = new Option<global::System.Collections.Generic.List<string>>(update.field7.Value);
                }
                if (update.field8.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field8 = new Option<global::System.Collections.Generic.List<uint>>(update.field8.Value);
                }
                if (update.field9.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field9 = new Option<global::System.Collections.Generic.List<ulong>>(update.field9.Value);
                }
                if (update.field10.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field10 = new Option<global::System.Collections.Generic.List<int>>(update.field10.Value);
                }
                if (update.field11.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field11 = new Option<global::System.Collections.Generic.List<long>>(update.field11.Value);
                }
                if (update.field12.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field12 = new Option<global::System.Collections.Generic.List<uint>>(update.field12.Value);
                }
                if (update.field13.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field13 = new Option<global::System.Collections.Generic.List<ulong>>(update.field13.Value);
                }
                if (update.field14.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field14 = new Option<global::System.Collections.Generic.List<int>>(update.field14.Value);
                }
                if (update.field15.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field15 = new Option<global::System.Collections.Generic.List<long>>(update.field15.Value);
                }
                if (update.field16.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field16 = new Option<global::System.Collections.Generic.List<long>>(update.field16.Value.Select(internalObject => internalObject.Id).ToList());
                }
                if (update.field17.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field17 = new Option<global::System.Collections.Generic.List<global::Generated.Improbable.TestSchema.SomeType>>(update.field17.Value.Select(internalObject => global::Generated.Improbable.TestSchema.SomeType.ToNative(internalObject)).ToList());
                }

                if (componentFieldsUpdated)
                {
                    view.AddComponentsUpdated(entity, gdkUpdate, UpdatesPool);
                }
            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                view.RemoveComponent<SpatialOSExhaustiveRepeated>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentArray<SpatialOSExhaustiveRepeated>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.TestSchema.ExhaustiveRepeated.Update();
                        update.SetField2(new global::Improbable.Collections.List<float>(componentData.Field2));
                        update.SetField4(new global::Improbable.Collections.List<int>(componentData.Field4));
                        update.SetField5(new global::Improbable.Collections.List<long>(componentData.Field5));
                        update.SetField6(new global::Improbable.Collections.List<double>(componentData.Field6));
                        update.SetField7(new global::Improbable.Collections.List<string>(componentData.Field7));
                        update.SetField8(new global::Improbable.Collections.List<uint>(componentData.Field8));
                        update.SetField9(new global::Improbable.Collections.List<ulong>(componentData.Field9));
                        update.SetField10(new global::Improbable.Collections.List<int>(componentData.Field10));
                        update.SetField11(new global::Improbable.Collections.List<long>(componentData.Field11));
                        update.SetField12(new global::Improbable.Collections.List<uint>(componentData.Field12));
                        update.SetField13(new global::Improbable.Collections.List<ulong>(componentData.Field13));
                        update.SetField14(new global::Improbable.Collections.List<int>(componentData.Field14));
                        update.SetField15(new global::Improbable.Collections.List<long>(componentData.Field15));
                        update.SetField16(new global::Improbable.Collections.List<global::Improbable.EntityId>(componentData.Field16.Select(nativeInternalObject => new global::Improbable.EntityId(nativeInternalObject))));
                        update.SetField17(new global::Improbable.Collections.List<global::Improbable.TestSchema.SomeType>(componentData.Field17.Select(nativeInternalObject => global::Generated.Improbable.TestSchema.SomeType.ToSpatial(nativeInternalObject))));
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        view.SetComponentObject(entityId, componentData);

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.ExhaustiveRepeated.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 0);
                RemoveComponents(ref entityCommandBuffer, UpdatesPool, groupIndex: 1);
            }

            public override void SendCommands(Connection connection)
            {
            }

            public static ExhaustiveRepeated.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ExhaustiveRepeated.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
