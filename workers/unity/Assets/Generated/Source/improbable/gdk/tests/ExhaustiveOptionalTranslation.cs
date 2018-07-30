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
using Improbable.Gdk.Tests;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveOptional
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.Gdk.Tests.ExhaustiveOptional>
        {
            private const string LoggerName = "ExhaustiveOptional.Translation";
        
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSExhaustiveOptional);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSExhaustiveOptional), typeof(Authoritative<SpatialOSExhaustiveOptional>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSExhaustiveOptional>),
                typeof(ComponentAdded<SpatialOSExhaustiveOptional>),
                typeof(ComponentRemoved<SpatialOSExhaustiveOptional>),
                typeof(ComponentsUpdated<SpatialOSExhaustiveOptional.Update>), 
            };


            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveOptional>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSExhaustiveOptional>>(
                    () => new AuthoritiesChanged<SpatialOSExhaustiveOptional>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<ComponentsUpdated<SpatialOSExhaustiveOptional.Update>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSExhaustiveOptional.Update>>(
                    () => new ComponentsUpdated<SpatialOSExhaustiveOptional.Update>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.Gdk.Tests.ExhaustiveOptional>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.Gdk.Tests.ExhaustiveOptional>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.Gdk.Tests.ExhaustiveOptional>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.Gdk.Tests.ExhaustiveOptional>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.Gdk.Tests.ExhaustiveOptional> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnAddComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSExhaustiveOptional"));
                    return;
                }
                var data = op.Data.Get().Value;

                var spatialOSExhaustiveOptional = new SpatialOSExhaustiveOptional();
                spatialOSExhaustiveOptional.Field2 = data.field2.HasValue ? new global::System.Nullable<float>(data.field2.Value) : new global::System.Nullable<float>();
                spatialOSExhaustiveOptional.Field4 = data.field4.HasValue ? new global::System.Nullable<int>(data.field4.Value) : new global::System.Nullable<int>();
                spatialOSExhaustiveOptional.Field5 = data.field5.HasValue ? new global::System.Nullable<long>(data.field5.Value) : new global::System.Nullable<long>();
                spatialOSExhaustiveOptional.Field6 = data.field6.HasValue ? new global::System.Nullable<double>(data.field6.Value) : new global::System.Nullable<double>();
                spatialOSExhaustiveOptional.Field8 = data.field8.HasValue ? new global::System.Nullable<uint>(data.field8.Value) : new global::System.Nullable<uint>();
                spatialOSExhaustiveOptional.Field9 = data.field9.HasValue ? new global::System.Nullable<ulong>(data.field9.Value) : new global::System.Nullable<ulong>();
                spatialOSExhaustiveOptional.Field10 = data.field10.HasValue ? new global::System.Nullable<int>(data.field10.Value) : new global::System.Nullable<int>();
                spatialOSExhaustiveOptional.Field11 = data.field11.HasValue ? new global::System.Nullable<long>(data.field11.Value) : new global::System.Nullable<long>();
                spatialOSExhaustiveOptional.Field12 = data.field12.HasValue ? new global::System.Nullable<uint>(data.field12.Value) : new global::System.Nullable<uint>();
                spatialOSExhaustiveOptional.Field13 = data.field13.HasValue ? new global::System.Nullable<ulong>(data.field13.Value) : new global::System.Nullable<ulong>();
                spatialOSExhaustiveOptional.Field14 = data.field14.HasValue ? new global::System.Nullable<int>(data.field14.Value) : new global::System.Nullable<int>();
                spatialOSExhaustiveOptional.Field15 = data.field15.HasValue ? new global::System.Nullable<long>(data.field15.Value) : new global::System.Nullable<long>();
                spatialOSExhaustiveOptional.Field16 = data.field16.HasValue ? new global::System.Nullable<long>(data.field16.Value.Id) : new global::System.Nullable<long>();
                spatialOSExhaustiveOptional.Field17 = data.field17.HasValue ? new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>(global::Generated.Improbable.Gdk.Tests.SomeType.ToNative(data.field17.Value)) : new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>();
                spatialOSExhaustiveOptional.DirtyBit = false;

                view.SetComponentObject(entity, spatialOSExhaustiveOptional);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSExhaustiveOptional>());

                if (view.HasComponent<ComponentRemoved<SpatialOSExhaustiveOptional>>(entity))
                {
                    view.RemoveComponent<ComponentRemoved<SpatialOSExhaustiveOptional>>(entity);
                }
                else if (!view.HasComponent<ComponentAdded<SpatialOSExhaustiveOptional>>(entity))
                {
                    view.AddComponent(entity, new ComponentAdded<SpatialOSExhaustiveOptional>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentAdded but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSExhaustiveOptional"));
                }
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.Gdk.Tests.ExhaustiveOptional> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnComponentUpdate.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSExhaustiveOptional"));
                    return;
                }

                var componentData = view.GetComponentObject<SpatialOSExhaustiveOptional>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSExhaustiveOptional>>(entity))
                {
                    if (update.field2.HasValue)
                    {
                        componentData.Field2 = update.field2.Value.HasValue ? new global::System.Nullable<float>(update.field2.Value.Value) : new global::System.Nullable<float>();
                    }
                    if (update.field4.HasValue)
                    {
                        componentData.Field4 = update.field4.Value.HasValue ? new global::System.Nullable<int>(update.field4.Value.Value) : new global::System.Nullable<int>();
                    }
                    if (update.field5.HasValue)
                    {
                        componentData.Field5 = update.field5.Value.HasValue ? new global::System.Nullable<long>(update.field5.Value.Value) : new global::System.Nullable<long>();
                    }
                    if (update.field6.HasValue)
                    {
                        componentData.Field6 = update.field6.Value.HasValue ? new global::System.Nullable<double>(update.field6.Value.Value) : new global::System.Nullable<double>();
                    }
                    if (update.field8.HasValue)
                    {
                        componentData.Field8 = update.field8.Value.HasValue ? new global::System.Nullable<uint>(update.field8.Value.Value) : new global::System.Nullable<uint>();
                    }
                    if (update.field9.HasValue)
                    {
                        componentData.Field9 = update.field9.Value.HasValue ? new global::System.Nullable<ulong>(update.field9.Value.Value) : new global::System.Nullable<ulong>();
                    }
                    if (update.field10.HasValue)
                    {
                        componentData.Field10 = update.field10.Value.HasValue ? new global::System.Nullable<int>(update.field10.Value.Value) : new global::System.Nullable<int>();
                    }
                    if (update.field11.HasValue)
                    {
                        componentData.Field11 = update.field11.Value.HasValue ? new global::System.Nullable<long>(update.field11.Value.Value) : new global::System.Nullable<long>();
                    }
                    if (update.field12.HasValue)
                    {
                        componentData.Field12 = update.field12.Value.HasValue ? new global::System.Nullable<uint>(update.field12.Value.Value) : new global::System.Nullable<uint>();
                    }
                    if (update.field13.HasValue)
                    {
                        componentData.Field13 = update.field13.Value.HasValue ? new global::System.Nullable<ulong>(update.field13.Value.Value) : new global::System.Nullable<ulong>();
                    }
                    if (update.field14.HasValue)
                    {
                        componentData.Field14 = update.field14.Value.HasValue ? new global::System.Nullable<int>(update.field14.Value.Value) : new global::System.Nullable<int>();
                    }
                    if (update.field15.HasValue)
                    {
                        componentData.Field15 = update.field15.Value.HasValue ? new global::System.Nullable<long>(update.field15.Value.Value) : new global::System.Nullable<long>();
                    }
                    if (update.field16.HasValue)
                    {
                        componentData.Field16 = update.field16.Value.HasValue ? new global::System.Nullable<long>(update.field16.Value.Value.Id) : new global::System.Nullable<long>();
                    }
                    if (update.field17.HasValue)
                    {
                        componentData.Field17 = update.field17.Value.HasValue ? new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>(global::Generated.Improbable.Gdk.Tests.SomeType.ToNative(update.field17.Value.Value)) : new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>();
                    }
                }

                componentData.DirtyBit = false;

                view.SetComponentObject(entity, componentData);

                var componentFieldsUpdated = false;
                var gdkUpdate = new SpatialOSExhaustiveOptional.Update();
                if (update.field2.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field2 = new Option<global::System.Nullable<float>>(update.field2.Value.HasValue ? new global::System.Nullable<float>(update.field2.Value.Value) : new global::System.Nullable<float>());
                }
                if (update.field4.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field4 = new Option<global::System.Nullable<int>>(update.field4.Value.HasValue ? new global::System.Nullable<int>(update.field4.Value.Value) : new global::System.Nullable<int>());
                }
                if (update.field5.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field5 = new Option<global::System.Nullable<long>>(update.field5.Value.HasValue ? new global::System.Nullable<long>(update.field5.Value.Value) : new global::System.Nullable<long>());
                }
                if (update.field6.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field6 = new Option<global::System.Nullable<double>>(update.field6.Value.HasValue ? new global::System.Nullable<double>(update.field6.Value.Value) : new global::System.Nullable<double>());
                }
                if (update.field8.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field8 = new Option<global::System.Nullable<uint>>(update.field8.Value.HasValue ? new global::System.Nullable<uint>(update.field8.Value.Value) : new global::System.Nullable<uint>());
                }
                if (update.field9.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field9 = new Option<global::System.Nullable<ulong>>(update.field9.Value.HasValue ? new global::System.Nullable<ulong>(update.field9.Value.Value) : new global::System.Nullable<ulong>());
                }
                if (update.field10.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field10 = new Option<global::System.Nullable<int>>(update.field10.Value.HasValue ? new global::System.Nullable<int>(update.field10.Value.Value) : new global::System.Nullable<int>());
                }
                if (update.field11.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field11 = new Option<global::System.Nullable<long>>(update.field11.Value.HasValue ? new global::System.Nullable<long>(update.field11.Value.Value) : new global::System.Nullable<long>());
                }
                if (update.field12.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field12 = new Option<global::System.Nullable<uint>>(update.field12.Value.HasValue ? new global::System.Nullable<uint>(update.field12.Value.Value) : new global::System.Nullable<uint>());
                }
                if (update.field13.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field13 = new Option<global::System.Nullable<ulong>>(update.field13.Value.HasValue ? new global::System.Nullable<ulong>(update.field13.Value.Value) : new global::System.Nullable<ulong>());
                }
                if (update.field14.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field14 = new Option<global::System.Nullable<int>>(update.field14.Value.HasValue ? new global::System.Nullable<int>(update.field14.Value.Value) : new global::System.Nullable<int>());
                }
                if (update.field15.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field15 = new Option<global::System.Nullable<long>>(update.field15.Value.HasValue ? new global::System.Nullable<long>(update.field15.Value.Value) : new global::System.Nullable<long>());
                }
                if (update.field16.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field16 = new Option<global::System.Nullable<long>>(update.field16.Value.HasValue ? new global::System.Nullable<long>(update.field16.Value.Value.Id) : new global::System.Nullable<long>());
                }
                if (update.field17.HasValue)
                {
                    componentFieldsUpdated = true;
                    gdkUpdate.Field17 = new Option<global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>>(update.field17.Value.HasValue ? new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>(global::Generated.Improbable.Gdk.Tests.SomeType.ToNative(update.field17.Value.Value)) : new global::System.Nullable<global::Generated.Improbable.Gdk.Tests.SomeType>());
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
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnRemoveComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSExhaustiveOptional"));
                    return;
                }

                view.RemoveComponent<SpatialOSExhaustiveOptional>(entity);

                if (view.HasComponent<ComponentAdded<SpatialOSExhaustiveOptional>>(entity))
                {
                    view.RemoveComponent<ComponentAdded<SpatialOSExhaustiveOptional>>(entity);
                }
                else if (!view.HasComponent<ComponentRemoved<SpatialOSExhaustiveOptional>>(entity))
                {
                    view.AddComponent(entity, new ComponentRemoved<SpatialOSExhaustiveOptional>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentRemoved but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSExhaustiveOptional"));
                }
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentArray<SpatialOSExhaustiveOptional>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.Gdk.Tests.ExhaustiveOptional.Update();
                        update.SetField2(componentData.Field2.HasValue ? new global::Improbable.Collections.Option<float>(componentData.Field2.Value) : new global::Improbable.Collections.Option<float>());
                        update.SetField4(componentData.Field4.HasValue ? new global::Improbable.Collections.Option<int>(componentData.Field4.Value) : new global::Improbable.Collections.Option<int>());
                        update.SetField5(componentData.Field5.HasValue ? new global::Improbable.Collections.Option<long>(componentData.Field5.Value) : new global::Improbable.Collections.Option<long>());
                        update.SetField6(componentData.Field6.HasValue ? new global::Improbable.Collections.Option<double>(componentData.Field6.Value) : new global::Improbable.Collections.Option<double>());
                        update.SetField8(componentData.Field8.HasValue ? new global::Improbable.Collections.Option<uint>(componentData.Field8.Value) : new global::Improbable.Collections.Option<uint>());
                        update.SetField9(componentData.Field9.HasValue ? new global::Improbable.Collections.Option<ulong>(componentData.Field9.Value) : new global::Improbable.Collections.Option<ulong>());
                        update.SetField10(componentData.Field10.HasValue ? new global::Improbable.Collections.Option<int>(componentData.Field10.Value) : new global::Improbable.Collections.Option<int>());
                        update.SetField11(componentData.Field11.HasValue ? new global::Improbable.Collections.Option<long>(componentData.Field11.Value) : new global::Improbable.Collections.Option<long>());
                        update.SetField12(componentData.Field12.HasValue ? new global::Improbable.Collections.Option<uint>(componentData.Field12.Value) : new global::Improbable.Collections.Option<uint>());
                        update.SetField13(componentData.Field13.HasValue ? new global::Improbable.Collections.Option<ulong>(componentData.Field13.Value) : new global::Improbable.Collections.Option<ulong>());
                        update.SetField14(componentData.Field14.HasValue ? new global::Improbable.Collections.Option<int>(componentData.Field14.Value) : new global::Improbable.Collections.Option<int>());
                        update.SetField15(componentData.Field15.HasValue ? new global::Improbable.Collections.Option<long>(componentData.Field15.Value) : new global::Improbable.Collections.Option<long>());
                        update.SetField16(componentData.Field16.HasValue ? new global::Improbable.Collections.Option<global::Improbable.EntityId>(new global::Improbable.EntityId(componentData.Field16.Value)) : new global::Improbable.Collections.Option<global::Improbable.EntityId>());
                        update.SetField17(componentData.Field17.HasValue ? new global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType>(global::Generated.Improbable.Gdk.Tests.SomeType.ToSpatial(componentData.Field17.Value)) : new global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType>());
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        view.SetComponentObject(entityId, componentData);

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.Gdk.Tests.ExhaustiveOptional.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 0);
                RemoveComponents<ComponentAdded<SpatialOSExhaustiveOptional>>(ref entityCommandBuffer, groupIndex: 1);
                RemoveComponents<ComponentRemoved<SpatialOSExhaustiveOptional>>(ref entityCommandBuffer, groupIndex: 2);
                RemoveComponents(ref entityCommandBuffer, UpdatesPool, groupIndex: 3);
                
                
            }

            public override void SendCommands(Connection connection)
            {
            }

            public static ExhaustiveOptional.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ExhaustiveOptional.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
