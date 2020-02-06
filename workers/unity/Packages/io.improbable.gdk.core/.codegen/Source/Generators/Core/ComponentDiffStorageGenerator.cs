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

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            var eventDetailsList = componentDetails.EventDetails;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic",
                    "System",
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.Name}.DiffComponentStorage class.");

                        var classDefinition = new StringBuilder("public class DiffComponentStorage : IDiffUpdateStorage<Update>, IDiffComponentAddedStorage<Update>, IDiffAuthorityStorage");

                        foreach (var ev in eventDetailsList)
                        {
                            classDefinition.Append($"{Environment.NewLine}    , IDiffEventStorage<{ev.Name}.Event>");
                        }

                        partial.Type(classDefinition.ToString(), storage =>
                        {
                            storage.Line(@"
private readonly HashSet<EntityId> entitiesUpdated = new HashSet<EntityId>();

private List<EntityId> componentsAdded = new List<EntityId>();
private List<EntityId> componentsRemoved = new List<EntityId>();

private readonly AuthorityComparer authorityComparer = new AuthorityComparer();
private readonly UpdateComparer<Update> updateComparer = new UpdateComparer<Update>();

// Used to represent a state machine of authority changes. Valid state changes are:
// authority lost -> authority lost temporarily
// authority lost temporarily -> authority lost
// authority gained -> authority gained
// Creating the authority lost temporarily set is the aim as it signifies authority epoch changes
private readonly HashSet<EntityId> authorityLost = new HashSet<EntityId>();
private readonly HashSet<EntityId> authorityGained = new HashSet<EntityId>();
private readonly HashSet<EntityId> authorityLostTemporary = new HashSet<EntityId>();

private MessageList<ComponentUpdateReceived<Update>> updateStorage =
    new MessageList<ComponentUpdateReceived<Update>>();

private MessageList<AuthorityChangeReceived> authorityChanges =
    new MessageList<AuthorityChangeReceived>();
");

                            foreach (var ev in eventDetailsList)
                            {
                                var eventType = $"{ev.Name}.Event";
                                storage.Line($@"
private MessageList<ComponentEventReceived<{eventType}>> {ev.CamelCaseName}EventStorage =
    new MessageList<ComponentEventReceived<{eventType}>>();

private readonly EventComparer<{eventType}> {ev.CamelCaseName}Comparer =
    new EventComparer<{eventType}>();
");
                            }

                            storage.Method("public Type[] GetEventTypes()", m =>
                            {
                                m.Initializer("return new Type[]", () =>
                                {
                                    return eventDetailsList.Select(ev => $"typeof({ev.Name}.Event)");
                                });
                            });

                            storage.Method("public Type GetUpdateType()", m =>
                            {
                                m.Return("typeof(Update)");
                            });

                            storage.Method("public uint GetComponentId()", m =>
                            {
                                m.Return("ComponentId");
                            });

                            storage.Method("public void Clear()", m =>
                            {
                                m.Line(new[]
                                {
                                    "entitiesUpdated.Clear();",
                                    "updateStorage.Clear();",
                                    "authorityChanges.Clear();",
                                    "componentsAdded.Clear();",
                                    "componentsRemoved.Clear();"
                                });

                                m.Line(eventDetailsList.Select(ev => $"{ev.CamelCaseName}EventStorage.Clear();").ToList());
                            });

                            storage.Method("public void RemoveEntityComponent(long entityId)", m =>
                            {
                                m.Line("var id = new EntityId(entityId);");

                                m.Line("// Adding a component always updates it, so this will catch the case where the component was just added");
                                m.If("entitiesUpdated.Remove(id)", then =>
                                {
                                    then.Line(new[]
                                    {
                                        "updateStorage.RemoveAll(update => update.EntityId.Id == entityId);",
                                        "authorityChanges.RemoveAll(change => change.EntityId.Id == entityId);"
                                    });

                                    then.Line(eventDetailsList.Select(ev =>
                                        $"{ev.CamelCaseName}EventStorage.RemoveAll(change => change.EntityId.Id == entityId);").ToList());
                                });

                                m.If("!componentsAdded.Remove(id)", () => new[]
                                {
                                    "componentsRemoved.Add(id);"
                                });
                            });

                            storage.Line(@"
public void AddEntityComponent(long entityId, Update component)
{
    var id = new EntityId(entityId);
    if (!componentsRemoved.Remove(id))
    {
        componentsAdded.Add(id);
    }

    AddUpdate(new ComponentUpdateReceived<Update>(component, id, 0));
}

public void AddUpdate(ComponentUpdateReceived<Update> update)
{
    entitiesUpdated.Add(update.EntityId);
    updateStorage.InsertSorted(update, updateComparer);
}

public void AddAuthorityChange(AuthorityChangeReceived authorityChange)
{
    if (authorityChange.Authority == Authority.NotAuthoritative)
    {
        if (authorityLostTemporary.Remove(authorityChange.EntityId) || !authorityGained.Contains(authorityChange.EntityId))
        {
            authorityLost.Add(authorityChange.EntityId);
        }
    }
    else if (authorityChange.Authority == Authority.Authoritative)
    {
        if (authorityLost.Remove(authorityChange.EntityId))
        {
            authorityLostTemporary.Add(authorityChange.EntityId);
        }
        else
        {
            authorityGained.Add(authorityChange.EntityId);
        }
    }

    authorityChanges.InsertSorted(authorityChange, authorityComparer);
}

public List<EntityId> GetComponentsAdded()
{
    return componentsAdded;
}

public List<EntityId> GetComponentsRemoved()
{
    return componentsRemoved;
}

public MessagesSpan<ComponentUpdateReceived<Update>> GetUpdates()
{
    return updateStorage.Slice();
}

public MessagesSpan<ComponentUpdateReceived<Update>> GetUpdates(EntityId entityId)
{
    var range = updateStorage.GetEntityRange(entityId);
    return updateStorage.Slice(range.FirstIndex, range.Count);
}

public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges()
{
    return authorityChanges.Slice();
}

public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId)
{
    var range = authorityChanges.GetEntityRange(entityId);
    return authorityChanges.Slice(range.FirstIndex, range.Count);
}
");
                            foreach (var ev in eventDetailsList)
                            {
                                var eventType = $"{ev.Name}.Event";

                                storage.Method($"MessagesSpan<ComponentEventReceived<{eventType}>> IDiffEventStorage<{eventType}>.GetEvents(EntityId entityId)", () => new[]
                                {
                                    $"var range = {ev.CamelCaseName}EventStorage.GetEntityRange(entityId);",
                                    $"return {ev.CamelCaseName}EventStorage.Slice(range.FirstIndex, range.Count);"
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
            }).Format();
        }
    }
}
