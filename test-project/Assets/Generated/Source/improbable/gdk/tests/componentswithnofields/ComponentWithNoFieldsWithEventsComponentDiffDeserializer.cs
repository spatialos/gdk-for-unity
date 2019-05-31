// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public class DiffComponentDeserializer : IComponentDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public void AddUpdateToDiff(ComponentUpdateOp op, ViewDiff diff, uint updateId)
            {
                var update = global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);
                diff.AddComponentUpdate(update, op.EntityId, op.Update.ComponentId, updateId);
                var eventsObject = op.Update.SchemaData.Value.GetEvents();

                {
                    var eventCount = eventsObject.GetObjectCount(1);
                    if (eventCount > 0)
                    {
                        for (uint i = 0; i < eventCount; i++)
                        {
                            var payload = global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(eventsObject.IndexObject(1, i));
                            var e = new Evt.Event(payload);
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
                    var componentUpdate = new ComponentUpdate(schemaUpdate);
                    Serialization.SerializeUpdate(update.Update, schemaUpdate);
                    serializedMessages.AddComponentUpdate(componentUpdate, update.EntityId.Id);
                }


                {
                    var events = ((IDiffEventStorage<Evt.Event>) storage).GetEvents();

                    for (int i = 0; i < events.Count; ++i)
                    {
                        ref readonly var ev = ref events[i];
                        var schemaUpdate = new SchemaComponentUpdate(ComponentId);
                        var componentUpdate = new ComponentUpdate(schemaUpdate);
                        var obj = schemaUpdate.GetEvents().AddObject(1);
                        global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(ev.Event.Payload, obj);
                        serializedMessages.AddComponentUpdate(componentUpdate, ev.EntityId.Id);

                    }
                }
            }
        }
    }
}
