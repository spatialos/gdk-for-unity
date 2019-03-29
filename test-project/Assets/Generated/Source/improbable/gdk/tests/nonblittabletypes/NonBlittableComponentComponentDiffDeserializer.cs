// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class DiffComponentDeserializer : IComponentDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public void AddUpdateToDiff(ComponentUpdateOp op, ViewDiff diff, uint updateId)
            {
                var update = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);
                diff.AddComponentUpdate(update, op.EntityId, op.Update.ComponentId, updateId);
                var eventsObject = op.Update.SchemaData.Value.GetEvents();

                {
                    var eventCount = eventsObject.GetObjectCount(1);
                    if (eventCount > 0)
                    {
                        for (uint i = 0; i < eventCount; i++)
                        {
                            var payload = global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload.Serialization.Deserialize(eventsObject.IndexObject(1, i));
                            var e = new FirstEvent.Event(payload);
                            diff.AddEvent(e, op.EntityId, op.Update.ComponentId, updateId);
                        }
                    }
                }

                {
                    var eventCount = eventsObject.GetObjectCount(2);
                    if (eventCount > 0)
                    {
                        for (uint i = 0; i < eventCount; i++)
                        {
                            var payload = global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload.Serialization.Deserialize(eventsObject.IndexObject(2, i));
                            var e = new SecondEvent.Event(payload);
                            diff.AddEvent(e, op.EntityId, op.Update.ComponentId, updateId);
                        }
                    }
                }
            }

            public void AddComponentToDiff(AddComponentOp op, ViewDiff diff)
            {
                var data = Serialization.DeserializeUpdate(op.Data.SchemaData.Value);
                diff.AddComponent(data, op.EntityId, op.Data.ComponentId);
            }
        }

        public class ComponentSerializer : IComponentSerializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages)
            {
                var storage = messages.GetComponentDiffStorage(ComponentId);

                var updates = ((IDiffUpdateStorage<Update>) storage).GetUpdates();

                for (int i = 0; i < updates.Count; ++i)
                {
                    ref readonly var update = ref updates[i];
                    var schemaUpdate = new SchemaComponentUpdate(ComponentId);
                    // TODO: UTY-1858 - remove this workaround.
                    schemaUpdate.GetEvents();
                    var componentUpdate = new ComponentUpdate(schemaUpdate);
                    Serialization.SerializeUpdate(update.Update, schemaUpdate);
                    serializedMessages.AddComponentUpdate(componentUpdate, update.EntityId.Id);
                }


                {
                    var events = ((IDiffEventStorage<FirstEvent.Event>) storage).GetEvents();

                    for (int i = 0; i < events.Count; ++i)
                    {
                        ref readonly var ev = ref events[i];
                        var schemaUpdate = new SchemaComponentUpdate(ComponentId);
                        // TODO: UTY-1858 - remove this workaround.
                        schemaUpdate.GetFields();
                        var componentUpdate = new ComponentUpdate(schemaUpdate);
                        var obj = schemaUpdate.GetEvents().AddObject(1);
                        global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload.Serialization.Serialize(ev.Event.Payload, obj);
                        serializedMessages.AddComponentUpdate(componentUpdate, ev.EntityId.Id);

                    }
                }

                {
                    var events = ((IDiffEventStorage<SecondEvent.Event>) storage).GetEvents();

                    for (int i = 0; i < events.Count; ++i)
                    {
                        ref readonly var ev = ref events[i];
                        var schemaUpdate = new SchemaComponentUpdate(ComponentId);
                        // TODO: UTY-1858 - remove this workaround.
                        schemaUpdate.GetFields();
                        var componentUpdate = new ComponentUpdate(schemaUpdate);
                        var obj = schemaUpdate.GetEvents().AddObject(2);
                        global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload.Serialization.Serialize(ev.Event.Payload, obj);
                        serializedMessages.AddComponentUpdate(componentUpdate, ev.EntityId.Id);

                    }
                }
            }
        }
    }
}
