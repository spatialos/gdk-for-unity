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
    public partial class ComponentWithNoFieldsWithEvents
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents>
        {
            private const string LoggerName = "ComponentWithNoFieldsWithEvents.Translation";
        
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSComponentWithNoFieldsWithEvents);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSComponentWithNoFieldsWithEvents), typeof(Authoritative<SpatialOSComponentWithNoFieldsWithEvents>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>),
                typeof(ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>),
                typeof(ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>),
                typeof(EventsReceived<EvtEvent>),
            };

            internal readonly Dictionary<long, List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>> EntityIdToEvtEvents = new Dictionary<long, List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>>();

            private static readonly ComponentPool<EventsReceived<EvtEvent>> EvtEventPool =
                new ComponentPool<EventsReceived<EvtEvent>>(
                    () => new EventsReceived<EvtEvent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>>(
                    () => new AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>(),
                    (component) => component.Buffer.Clear());


            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents>(OnAuthorityChange);

            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents> op)
            {
                if (!View.TryGetEntity(op.EntityId.Id, out var entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnAddComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithEvents"));
                    return;
                }

                var spatialOSComponentWithNoFieldsWithEvents = new SpatialOSComponentWithNoFieldsWithEvents();
                spatialOSComponentWithNoFieldsWithEvents.DirtyBit = false;

                View.AddComponent(entity, spatialOSComponentWithNoFieldsWithEvents);
                View.AddComponent(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>());

                if (View.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    View.RemoveComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                }
                else if (!View.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    View.AddComponent(entity, new ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentAdded but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithEvents"));
                }
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents> op)
            {
                if (!View.TryGetEntity(op.EntityId.Id, out var entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnComponentUpdate.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithEvents"));
                    return;
                }

                var componentData = View.GetComponent<SpatialOSComponentWithNoFieldsWithEvents>(entity);
                var update = op.Update.Get();


                var evtEvents = update.evt;
                foreach (var spatialEvent in evtEvents)
                {
                    var nativeEvent = new EvtEvent
                    {
                        Payload = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToNative(spatialEvent)
                    };

                    View.AddEventReceived(entity, nativeEvent, EvtEventPool);
                }
                componentData.DirtyBit = false;

                View.SetComponentData(entity, componentData);

            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!View.TryGetEntity(op.EntityId.Id, out var entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnRemoveComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithEvents"));
                    return;
                }

                View.RemoveComponent<SpatialOSComponentWithNoFieldsWithEvents>(entity);

                if (View.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    View.RemoveComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                }
                else if (!View.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    View.AddComponent(entity, new ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentRemoved but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithEvents"));
                }
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                if (op.Authority == Authority.Authoritative)
                {
                    EntityIdToEvtEvents[entityId] = new List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>();
                    View.AddComponent(entityId, new EventSender<SpatialOSComponentWithNoFieldsWithEvents>(entityId, TranslationHandle));
                }
                else if (op.Authority == Authority.NotAuthoritative)
                {
                    EntityIdToEvtEvents.Remove(entityId);
                    View.RemoveComponent<EventSender<SpatialOSComponentWithNoFieldsWithEvents>>(entityId);
                }
                View.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithEvents>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;
                    var evtEvents = EntityIdToEvtEvents[entityId];
                    hasPendingEvents |= evtEvents.Count() > 0;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update();
                        foreach (var nativeEvent in evtEvents)
                        {
                            var spatialEvent = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToSpatial(nativeEvent);
                            update.evt.Add(spatialEvent);
                        }
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                        evtEvents.Clear();
                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 0);
                RemoveComponents<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(ref entityCommandBuffer, groupIndex: 1);
                RemoveComponents<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(ref entityCommandBuffer, groupIndex: 2);
                
                RemoveComponents(ref entityCommandBuffer, EvtEventPool, groupIndex: 3);
                
            }

            public override void SendCommands(Connection connection)
            {
            }

            public static ComponentWithNoFieldsWithEvents.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ComponentWithNoFieldsWithEvents.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }


    public static class SpatialOSComponentWithNoFieldsWithEventsEventHandlers
    {
        public static void SendEvtEvent(this EventSender<SpatialOSComponentWithNoFieldsWithEvents> eventSender,
            global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty eventData)
        {
            var translation = ComponentWithNoFieldsWithEvents.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToEvtEvents[eventSender.EntityId].Add(eventData);
        }

        public static List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> GetEvtEvents(this EventSender<SpatialOSComponentWithNoFieldsWithEvents> eventSender)
        {
            var translation = ComponentWithNoFieldsWithEvents.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            return translation.EntityIdToEvtEvents[eventSender.EntityId];
        }

        public static void ClearEvtEvents(this EventSender<SpatialOSComponentWithNoFieldsWithEvents> eventSender)
        {
            var translation = ComponentWithNoFieldsWithEvents.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToEvtEvents[eventSender.EntityId].Clear();
        }

    }
}
