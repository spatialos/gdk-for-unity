using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ViewStorageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.Name}.{componentDetails.Name}ViewStorage class.");

                        partial.Type($"public class {componentDetails.Name}ViewStorage : IViewStorage, IViewComponentStorage<Snapshot>, IViewComponentUpdater<Update>",
                            vs =>
                            {
                                vs.Line(@"
private readonly Dictionary<long, Authority> authorityStates = new Dictionary<long, Authority>();
private readonly Dictionary<long, Snapshot> componentData = new Dictionary<long, Snapshot>();

public Type GetSnapshotType()
{
    return typeof(Snapshot);
}

public Type GetUpdateType()
{
    return typeof(Update);
}

public uint GetComponentId()
{
    return ComponentId;
}

public bool HasComponent(long entityId)
{
    return componentData.ContainsKey(entityId);
}

public Snapshot GetComponent(long entityId)
{
    if (!componentData.TryGetValue(entityId, out var component))
    {
        throw new ArgumentException($""Entity with Entity ID {entityId} does not have component {typeof(Snapshot)} in the view."");
    }

    return component;
}

public Authority GetAuthority(long entityId)
{
    if (!authorityStates.TryGetValue(entityId, out var authority))
    {
        throw new ArgumentException($""Entity with Entity ID {entityId} does not have component {typeof(Snapshot)} in the view."");
    }

    return authority;
}

public void ApplyDiff(ViewDiff viewDiff)
{
    var storage = viewDiff.GetComponentDiffStorage(ComponentId);

    foreach (var entity in storage.GetComponentsAdded())
    {
        authorityStates[entity.Id] = Authority.NotAuthoritative;
        componentData[entity.Id] = new Snapshot();
    }

    foreach (var entity in storage.GetComponentsRemoved())
    {
        authorityStates.Remove(entity.Id);
        componentData.Remove(entity.Id);
    }

    var updates = ((IDiffUpdateStorage<Update>) storage).GetUpdates();
    for (var i = 0; i < updates.Count; i++)
    {
        ref readonly var update = ref updates[i];
        ApplyUpdate(update.EntityId.Id, in update.Update);
    }

    var authorityChanges = ((IDiffAuthorityStorage) storage).GetAuthorityChanges();
    for (var i = 0; i < authorityChanges.Count; i++)
    {
        var authorityChange = authorityChanges[i];
        authorityStates[authorityChange.EntityId.Id] = authorityChange.Authority;
    }
}
");

                                vs.Method("public void ApplyUpdate(long entityId, in Update update)", m =>
                                {
                                    m.Line(@"
if (!componentData.TryGetValue(entityId, out var data))
{
    return;
}
");
                                    foreach (var field in componentDetails.FieldDetails)
                                    {
                                        var fieldName = field.PascalCaseName;
                                        m.Line($@"
if (update.{fieldName}.HasValue)
{{
    data.{fieldName} = update.{fieldName}.Value;
}}
");
                                    }

                                    m.Line("componentData[entityId] = data;");
                                });
                            });
                    });
                });
            }).Format();
        }
    }
}
