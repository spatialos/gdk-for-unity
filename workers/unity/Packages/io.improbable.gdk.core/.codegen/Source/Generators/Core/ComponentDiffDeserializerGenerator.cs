using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ComponentDiffDeserializerGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        partial.Type(GenerateComponentDeserializer(componentDetails, qualifiedNamespace));
                        partial.Type(GenerateComponentSerializer(componentDetails, qualifiedNamespace));
                    });
                });
            }).Format();
        }

        private static TypeBlock GenerateComponentDeserializer(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            var componentNamespace = $"global::{qualifiedNamespace}.{componentDetails.Name}";
            var eventDetailsList = componentDetails.EventDetails;

            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.Name}.DiffComponentDeserializer class.");

            return Scope.Type("public class DiffComponentDeserializer : IComponentDiffDeserializer", deserializer =>
            {
                deserializer.Line(@"
public uint GetComponentId()
{
    return ComponentId;
}
");

                deserializer.Method("public void AddUpdateToDiff(ComponentUpdateOp op, ViewDiff diff, uint updateId)",
                    m =>
                    {
                        m.If("op.Update.SchemaData.Value.GetFields().GetUniqueFieldIdCount() + op.Update.SchemaData.Value.GetClearedFieldCount() > 0",
                            () => new[]
                            {
                                $"var update = {componentNamespace}.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);",
                                "diff.AddComponentUpdate(update, op.EntityId, op.Update.ComponentId, updateId);"
                            });

                        if (eventDetailsList.Count > 0)
                        {
                            m.Line("var eventsObject = op.Update.SchemaData.Value.GetEvents();");

                            foreach (var ev in eventDetailsList)
                            {
                                m.CustomScope(() => new[]
                                {
                                    $@"
var eventCount = eventsObject.GetObjectCount({ev.EventIndex});
if (eventCount > 0)
{{
    for (uint i = 0; i < eventCount; i++)
    {{
        var payload = {ev.FqnPayloadType}.Serialization.Deserialize(eventsObject.IndexObject({ev.EventIndex}, i));
        var e = new {ev.EventName}.Event(payload);
        diff.AddEvent(e, op.EntityId, op.Update.ComponentId, updateId);
    }}
}}
"
                                });
                            }
                        }
                    });

                deserializer.Method("public void AddComponentToDiff(AddComponentOp op, ViewDiff diff)", () => new[]
                {
                    "var data = Serialization.DeserializeUpdate(op.Data.SchemaData.Value);",
                    "diff.AddComponent(data, op.EntityId, op.Data.ComponentId);"
                });
            });
        }

        private static TypeBlock GenerateComponentSerializer(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            var eventDetailsList = componentDetails.EventDetails;

            return Scope.Type("public class ComponentSerializer : IComponentSerializer", serializer =>
            {
                serializer.Line(@"
public uint GetComponentId()
{
    return ComponentId;
}
");

                serializer.Method("public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages)",
                    m =>
                    {
                        m.Line(@"
var storage = messages.GetComponentDiffStorage(ComponentId);

var updates = ((IDiffUpdateStorage<Update>) storage).GetUpdates();

for (int i = 0; i < updates.Count; ++i)
{
    ref readonly var update = ref updates[i];
    var schemaUpdate = SchemaComponentUpdate.Create();
    var componentUpdate = new ComponentUpdate(ComponentId, schemaUpdate);
    Serialization.SerializeUpdate(update.Update, schemaUpdate);
    serializedMessages.AddComponentUpdate(componentUpdate, update.EntityId.Id);
}
");

                        foreach (var ev in eventDetailsList)
                        {
                            m.CustomScope(() => new[]
                            {
                                $@"
var events = ((IDiffEventStorage<{ev.EventName}.Event>) storage).GetEvents();

for (int i = 0; i < events.Count; ++i)
{{
    ref readonly var ev = ref events[i];
    var schemaUpdate = SchemaComponentUpdate.Create();
    var componentUpdate = new ComponentUpdate(ComponentId, schemaUpdate);
    var obj = schemaUpdate.GetEvents().AddObject({ev.EventIndex});
    {ev.FqnPayloadType}.Serialization.Serialize(ev.Event.Payload, obj);
    serializedMessages.AddComponentUpdate(componentUpdate, ev.EntityId.Id);
}}
"
                            });
                        }
                    });
            });
        }
    }
}
