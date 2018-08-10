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
using Improbable.Gdk.Tests.ComponentsWithNoFields;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFields
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields>
        {
            private const string LoggerName = "ComponentWithNoFields.Translation";
        
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSComponentWithNoFields);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSComponentWithNoFields), typeof(Authoritative<SpatialOSComponentWithNoFields>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSComponentWithNoFields>),
                typeof(ComponentAdded<SpatialOSComponentWithNoFields>),
                typeof(ComponentRemoved<SpatialOSComponentWithNoFields>),
            };


            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFields>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFields>>(
                    () => new AuthoritiesChanged<SpatialOSComponentWithNoFields>(),
                    (component) => component.Buffer.Clear());


            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnAddComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFields"));
                    return;
                }

                var spatialOSComponentWithNoFields = new SpatialOSComponentWithNoFields();
                spatialOSComponentWithNoFields.DirtyBit = false;

                view.AddComponent(entity, spatialOSComponentWithNoFields);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSComponentWithNoFields>());

                if (view.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFields>>(entity))
                {
                    view.RemoveComponent<ComponentRemoved<SpatialOSComponentWithNoFields>>(entity);
                }
                else if (!view.HasComponent<ComponentAdded<SpatialOSComponentWithNoFields>>(entity))
                {
                    view.AddComponent(entity, new ComponentAdded<SpatialOSComponentWithNoFields>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentAdded but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFields"));
                }
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnComponentUpdate.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFields"));
                    return;
                }

                var componentData = view.GetComponent<SpatialOSComponentWithNoFields>(entity);


                componentData.DirtyBit = false;

                view.SetComponentData(entity, componentData);

            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnRemoveComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFields"));
                    return;
                }

                view.RemoveComponent<SpatialOSComponentWithNoFields>(entity);

                if (view.HasComponent<ComponentAdded<SpatialOSComponentWithNoFields>>(entity))
                {
                    view.RemoveComponent<ComponentAdded<SpatialOSComponentWithNoFields>>(entity);
                }
                else if (!view.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFields>>(entity))
                {
                    view.AddComponent(entity, new ComponentRemoved<SpatialOSComponentWithNoFields>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentRemoved but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFields"));
                }
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSComponentWithNoFields>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update();
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 0);
                RemoveComponents<ComponentAdded<SpatialOSComponentWithNoFields>>(ref entityCommandBuffer, groupIndex: 1);
                RemoveComponents<ComponentRemoved<SpatialOSComponentWithNoFields>>(ref entityCommandBuffer, groupIndex: 2);
                
                
            }

            public override void SendCommands(Connection connection)
            {
            }

            public static ComponentWithNoFields.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ComponentWithNoFields.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


}
