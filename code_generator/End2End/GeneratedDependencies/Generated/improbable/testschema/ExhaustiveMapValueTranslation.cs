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
    public partial class ExhaustiveMapValue
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.ExhaustiveMapValue>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSExhaustiveMapValue);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSExhaustiveMapValue), typeof(Authoritative<SpatialOSExhaustiveMapValue>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSExhaustiveMapValue>),
                typeof(ComponentsUpdated<SpatialOSExhaustiveMapValue.Update>), 
            };


            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveMapValue>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveMapValue>>(
                    () => new AuthoritiesChanged<SpatialOSExhaustiveMapValue>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<ComponentsUpdated<SpatialOSExhaustiveMapValue.Update>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSExhaustiveMapValue.Update>>(
                    () => new ComponentsUpdated<SpatialOSExhaustiveMapValue.Update>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.ExhaustiveMapValue>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.ExhaustiveMapValue>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.ExhaustiveMapValue>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.ExhaustiveMapValue>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.ExhaustiveMapValue> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }
                var data = op.Data.Get().Value;

                var spatialOSExhaustiveMapValue = new SpatialOSExhaustiveMapValue();
                spatialOSExhaustiveMapValue.Field2 = data.field2.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field4 = data.field4.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field5 = data.field5.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field6 = data.field6.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field7 = data.field7.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field8 = data.field8.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field9 = data.field9.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field10 = data.field10.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field11 = data.field11.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field12 = data.field12.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field13 = data.field13.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field14 = data.field14.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field15 = data.field15.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSExhaustiveMapValue.Field16 = data.field16.ToDictionary(entry => entry.Key, entry => entry.Value.Id);
                spatialOSExhaustiveMapValue.Field17 = data.field17.ToDictionary(entry => entry.Key, entry => global::Generated.Improbable.TestSchema.SomeType.ToNative(entry.Value));
                spatialOSExhaustiveMapValue.DirtyBit = false;

                view.SetComponentObject(entity, spatialOSExhaustiveMapValue);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSExhaustiveMapValue>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.ExhaustiveMapValue> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponentObject<SpatialOSExhaustiveMapValue>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSExhaustiveMapValue>>(entity))
                {
                    if (update.field2.HasValue)
                    {
                        componentData.Field2 = update.field2.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field4.HasValue)
                    {
                        componentData.Field4 = update.field4.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field5.HasValue)
                    {
                        componentData.Field5 = update.field5.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field6.HasValue)
                    {
                        componentData.Field6 = update.field6.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field7.HasValue)
                    {
                        componentData.Field7 = update.field7.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field8.HasValue)
                    {
                        componentData.Field8 = update.field8.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field9.HasValue)
                    {
                        componentData.Field9 = update.field9.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field10.HasValue)
                    {
                        componentData.Field10 = update.field10.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field11.HasValue)
                    {
                        componentData.Field11 = update.field11.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field12.HasValue)
                    {
                        componentData.Field12 = update.field12.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field13.HasValue)
                    {
                        componentData.Field13 = update.field13.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field14.HasValue)
                    {
                        componentData.Field14 = update.field14.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field15.HasValue)
                    {
                        componentData.Field15 = update.field15.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                    if (update.field16.HasValue)
                    {
                        componentData.Field16 = update.field16.Value.ToDictionary(entry => entry.Key, entry => entry.Value.Id);
                    }
                    if (update.field17.HasValue)
                    {
                        componentData.Field17 = update.field17.Value.ToDictionary(entry => entry.Key, entry => global::Generated.Improbable.TestSchema.SomeType.ToNative(entry.Value));
                    }
                }

                componentData.DirtyBit = false;

                view.SetComponentObject(entity, componentData);

                var componentFieldsUpdated = false;
                var gdkUpdate = new SpatialOSExhaustiveMapValue.Update();
                if (update.field2.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field2 = new Option<global::System.Collections.Generic.Dictionary<string, float>>(update.field2.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field4.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field4 = new Option<global::System.Collections.Generic.Dictionary<string, int>>(update.field4.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field5.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field5 = new Option<global::System.Collections.Generic.Dictionary<string, long>>(update.field5.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field6.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field6 = new Option<global::System.Collections.Generic.Dictionary<string, double>>(update.field6.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field7.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field7 = new Option<global::System.Collections.Generic.Dictionary<string, string>>(update.field7.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field8.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field8 = new Option<global::System.Collections.Generic.Dictionary<string, uint>>(update.field8.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field9.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field9 = new Option<global::System.Collections.Generic.Dictionary<string, ulong>>(update.field9.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field10.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field10 = new Option<global::System.Collections.Generic.Dictionary<string, int>>(update.field10.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field11.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field11 = new Option<global::System.Collections.Generic.Dictionary<string, long>>(update.field11.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field12.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field12 = new Option<global::System.Collections.Generic.Dictionary<string, uint>>(update.field12.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field13.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field13 = new Option<global::System.Collections.Generic.Dictionary<string, ulong>>(update.field13.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field14.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field14 = new Option<global::System.Collections.Generic.Dictionary<string, int>>(update.field14.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field15.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field15 = new Option<global::System.Collections.Generic.Dictionary<string, long>>(update.field15.Value.ToDictionary(entry => entry.Key, entry => entry.Value));
                }
                if (update.field16.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field16 = new Option<global::System.Collections.Generic.Dictionary<string, long>>(update.field16.Value.ToDictionary(entry => entry.Key, entry => entry.Value.Id));
                }
                if (update.field17.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field17 = new Option<global::System.Collections.Generic.Dictionary<string, global::Generated.Improbable.TestSchema.SomeType>>(update.field17.Value.ToDictionary(entry => entry.Key, entry => global::Generated.Improbable.TestSchema.SomeType.ToNative(entry.Value)));
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

                view.RemoveComponent<SpatialOSExhaustiveMapValue>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentArray<SpatialOSExhaustiveMapValue>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.TestSchema.ExhaustiveMapValue.Update();
                        update.SetField2(new global::Improbable.Collections.Map<string,float>(componentData.Field2.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField4(new global::Improbable.Collections.Map<string,int>(componentData.Field4.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField5(new global::Improbable.Collections.Map<string,long>(componentData.Field5.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField6(new global::Improbable.Collections.Map<string,double>(componentData.Field6.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField7(new global::Improbable.Collections.Map<string,string>(componentData.Field7.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField8(new global::Improbable.Collections.Map<string,uint>(componentData.Field8.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField9(new global::Improbable.Collections.Map<string,ulong>(componentData.Field9.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField10(new global::Improbable.Collections.Map<string,int>(componentData.Field10.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField11(new global::Improbable.Collections.Map<string,long>(componentData.Field11.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField12(new global::Improbable.Collections.Map<string,uint>(componentData.Field12.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField13(new global::Improbable.Collections.Map<string,ulong>(componentData.Field13.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField14(new global::Improbable.Collections.Map<string,int>(componentData.Field14.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField15(new global::Improbable.Collections.Map<string,long>(componentData.Field15.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        update.SetField16(new global::Improbable.Collections.Map<string,global::Improbable.EntityId>(componentData.Field16.ToDictionary(entry => entry.Key, entry => new global::Improbable.EntityId(entry.Value))));
                        update.SetField17(new global::Improbable.Collections.Map<string,global::Improbable.TestSchema.SomeType>(componentData.Field17.ToDictionary(entry => entry.Key, entry => global::Generated.Improbable.TestSchema.SomeType.ToSpatial(entry.Value))));
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        view.SetComponentObject(entityId, componentData);

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.ExhaustiveMapValue.Update update)
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

            public static ExhaustiveMapValue.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ExhaustiveMapValue.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
