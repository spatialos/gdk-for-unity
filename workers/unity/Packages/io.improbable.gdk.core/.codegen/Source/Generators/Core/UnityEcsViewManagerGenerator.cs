using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEcsViewManagerGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            var componentNamespace = $"global::{componentDetails.Namespace}.{componentDetails.Name}";

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "Unity.Entities",
                    "Improbable.Worker.CInterop",
                    "Improbable.Gdk.Core",
                    "Unity.Collections"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.EcsViewManager class.");

                        partial.Type("public class EcsViewManager : IEcsViewManager", evm =>
                        {
                            evm.Line($@"
private WorkerSystem workerSystem;
private SpatialOSReceiveSystem spatialOSReceiveSystem;
private EntityManager entityManager;

private readonly ComponentType[] initialComponents = new ComponentType[]
{{
    ComponentType.ReadWrite<{componentNamespace}.Component>(),
    ComponentType.ReadOnly<{componentNamespace}.HasAuthority>(),
}};
");

                            evm.Line(@"
public uint GetComponentId()
{
    return ComponentId;
}

public ComponentType[] GetInitialComponents()
{
    return initialComponents;
}

public void ApplyDiff(ViewDiff diff)
{
    var diffStorage = (DiffComponentStorage) diff.GetComponentDiffStorage(ComponentId);

    foreach (var entityId in diffStorage.GetComponentsAdded())
    {
        AddComponent(entityId);
    }

    var updates = diffStorage.GetUpdates();
    var dataFromEntity = spatialOSReceiveSystem.GetComponentDataFromEntity<Component>();
    for (int i = 0; i < updates.Count; ++i)
    {
        ApplyUpdate(in updates[i], dataFromEntity);
    }

    var authChanges = diffStorage.GetAuthorityChanges();
    for (int i = 0; i < authChanges.Count; ++i)
    {
        ref readonly var change = ref authChanges[i];
        SetAuthority(change.EntityId, change.Authority);
    }

    foreach (var entityId in diffStorage.GetComponentsRemoved())
    {
        RemoveComponent(entityId);
    }
}

public void Init(World world)
{
    entityManager = world.EntityManager;

    workerSystem = world.GetExistingSystem<WorkerSystem>();

    if (workerSystem == null)
    {
        throw new ArgumentException(""World instance is not running a valid SpatialOS worker"");
    }

    spatialOSReceiveSystem = world.GetExistingSystem<SpatialOSReceiveSystem>();

    if (spatialOSReceiveSystem == null)
    {
        throw new ArgumentException(""Could not find SpatialOS Receive System in the current world instance"");
    }
}
");
                            evm.Method("public void Clean()", m =>
                            {
                                var nonBittableFields =
                                    componentDetails.FieldDetails.Where(fd => !fd.IsBlittable).ToList();

                                if (nonBittableFields.Count == 0)
                                {
                                    return;
                                }

                                m.Line(new[]
                                {
                                    $"var query = entityManager.CreateEntityQuery(typeof({componentNamespace}.Component));",
                                    $"var componentDataArray = query.ToComponentDataArray<{componentNamespace}.Component>(Allocator.TempJob);"
                                });

                                m.Loop("foreach (var component in componentDataArray)", loop =>
                                {
                                    foreach (var fieldDetails in nonBittableFields)
                                    {
                                        loop.Line($"component.{fieldDetails.CamelCaseName}Handle.Dispose();");
                                    }
                                });

                                m.Line("componentDataArray.Dispose();");
                            });

                            evm.Method("private void AddComponent(EntityId entityId)", m =>
                            {
                                m.Line(new[]
                                {
                                    "var entity = workerSystem.GetEntity(entityId);",
                                    $"var component = new {componentNamespace}.Component();"
                                });

                                foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                                {
                                    m.Line($"component.{fieldDetails.CamelCaseName}Handle = global::Improbable.Gdk.Core.ReferenceProvider<{fieldDetails.Type}>.Create();");
                                }

                                m.Line(new[]
                                {
                                    "component.MarkDataClean();",
                                    "entityManager.AddComponentData(entity, component);"
                                });
                            });

                            evm.Method("private void RemoveComponent(EntityId entityId)", m =>
                            {
                                m.Line(new[]
                                {
                                    "var entity = workerSystem.GetEntity(entityId);",
                                    $"entityManager.RemoveComponent<{componentNamespace}.HasAuthority>(entity);",
                                });

                                if (!componentDetails.IsBlittable)
                                {
                                    m.Line($"var data = entityManager.GetComponentData<{componentNamespace}.Component>(entity);");
                                    foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                                    {
                                        m.Line($"data.{fieldDetails.CamelCaseName}Handle.Dispose();");
                                    }
                                }

                                m.Line($"entityManager.RemoveComponent<{componentNamespace}.Component>(entity);");
                            });

                            evm.Method("private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)", m =>
                            {
                                m.Line(@"
var entity = workerSystem.GetEntity(update.EntityId);
if (!dataFromEntity.Exists(entity))
{
    return;
}

var data = dataFromEntity[entity];
");
                                foreach (var fieldDetails in componentDetails.FieldDetails)
                                {
                                    m.Line($@"
if (update.Update.{fieldDetails.PascalCaseName}.HasValue)
{{
    data.{fieldDetails.PascalCaseName} = update.Update.{fieldDetails.PascalCaseName}.Value;
}}
");
                                }

                                m.Line(new[]
                                {
                                    "data.MarkDataClean();",
                                    "dataFromEntity[entity] = data;"
                                });
                            });

                            evm.Line($@"
private void SetAuthority(EntityId entityId, Authority authority)
{{
    switch (authority)
    {{
        case Authority.NotAuthoritative:
        {{
            var entity = workerSystem.GetEntity(entityId);
            entityManager.RemoveComponent<{componentNamespace}.HasAuthority>(entity);
            break;
        }}
        case Authority.Authoritative:
        {{
            var entity = workerSystem.GetEntity(entityId);
            entityManager.AddComponent<{componentNamespace}.HasAuthority>(entity);
            break;
        }}
        case Authority.AuthorityLossImminent:
            break;
    }}
}}
");
                        });
                    });
                });
            });
        }
    }
}
