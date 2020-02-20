using System;
using System.Linq;
using System.Text;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ComponentDiffStorageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            var eventDetailsList = componentDetails.EventDetails;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "Improbable.Gdk.Core"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.DiffComponentStorage class.");

                        var classDefinition = new StringBuilder("public class DiffComponentStorage : DiffComponentStorage<Update>");

                        foreach (var ev in eventDetailsList)
                        {
                            classDefinition.Append($"{Environment.NewLine}    , IDiffEventStorage<{ev.PascalCaseName}.Event>");
                        }

                        partial.Type(classDefinition.ToString(), storage =>
                        {
                            foreach (var ev in eventDetailsList)
                            {
                                var eventType = $"{ev.PascalCaseName}.Event";
                                storage.Line($@"
private MessageList<ComponentEventReceived<{eventType}>> {ev.CamelCaseName}EventStorage =
    new MessageList<ComponentEventReceived<{eventType}>>();

private readonly EventComparer<{eventType}> {ev.CamelCaseName}Comparer =
    new EventComparer<{eventType}>();
");
                            }

                            storage.Method("public override Type[] GetEventTypes()", m =>
                            {
                                m.Initializer("return new Type[]", () =>
                                {
                                    return eventDetailsList.Select(ev => $"typeof({ev.PascalCaseName}.Event)");
                                });
                            });

                            if (eventDetailsList.Count > 0)
                            {
                                storage.Method("public override void Clear()", m =>
                                {
                                    m.Line("base.Clear();");
                                    m.Line(eventDetailsList
                                        .Select(ev => $"{ev.CamelCaseName}EventStorage.Clear();").ToList());
                                });
                            }

                            storage.Method("protected override void ClearEventStorage(long entityId)", m =>
                            {
                                m.Line(eventDetailsList.Select(ev => $"{ev.CamelCaseName}EventStorage.RemoveAll(change => change.EntityId.Id == entityId);").ToList());
                            });

                            foreach (var ev in eventDetailsList)
                            {
                                var eventType = $"{ev.PascalCaseName}.Event";

                                storage.Method($"MessagesSpan<ComponentEventReceived<{eventType}>> IDiffEventStorage<{eventType}>.GetEvents(EntityId entityId)", () => new[]
                                {
                                    $"var (firstIndex, count) = {ev.CamelCaseName}EventStorage.GetEntityRange(entityId);",
                                    $"return {ev.CamelCaseName}EventStorage.Slice(firstIndex, count);"
                                });

                                storage.Method($"MessagesSpan<ComponentEventReceived<{eventType}>> IDiffEventStorage<{eventType}>.GetEvents()", () => new[]
                                {
                                    $"return {ev.CamelCaseName}EventStorage.Slice();"
                                });

                                storage.Method($"void IDiffEventStorage<{eventType}>.AddEvent(ComponentEventReceived<{eventType}> ev)", () => new[]
                                {
                                    $"{ev.CamelCaseName}EventStorage.InsertSorted(ev, {ev.CamelCaseName}Comparer);"
                                });
                            }
                        });
                    });
                });
            });
        }
    }
}
