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
                    "Improbable.Gdk.Core"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.EcsViewManager class.");

                        partial.Type("public class EcsViewManager : IEcsViewManager", evm =>
                        {
                            evm.Line(@"
private WorkerSystem workerSystem;
private EntityManager entityManager;
private World world;

private readonly ComponentType[] initialComponents = new ComponentType[]
{
    ComponentType.ReadWrite<Component>(),
    ComponentType.ReadWrite<ComponentAuthority>(),
};

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
    var dataFromEntity = workerSystem.GetComponentDataFromEntity<Component>();
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
    this.world = world;
    entityManager = world.EntityManager;

    workerSystem = world.GetExistingSystem<WorkerSystem>();

    if (workerSystem == null)
    {
        throw new ArgumentException(""World instance is not running a valid SpatialOS worker"");
    }
}
");
                            evm.Method("public void Clean(World world)", m =>
                            {
                                foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                                {
                                    m.Line($"{componentNamespace}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.CleanDataInWorld(world);");
                                }
                            });

                            evm.Method("private void AddComponent(EntityId entityId)", m =>
                            {
                                m.Line(new[]
                                {
                                    "workerSystem.TryGetEntity(entityId, out var entity);",
                                    $"var component = new {componentNamespace}.Component();"
                                });

                                foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                                {
                                    m.Line($"component.{fieldDetails.CamelCaseName}Handle = {componentNamespace}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.Allocate(world);");
                                }

                                m.Line(new[]
                                {
                                    "component.MarkDataClean();",
                                    "entityManager.AddSharedComponentData(entity, ComponentAuthority.NotAuthoritative);",
                                    "entityManager.AddComponentData(entity, component);"
                                });
                            });

                            evm.Method("private void RemoveComponent(EntityId entityId)", m =>
                            {
                                m.Line(new[]
                                {
                                    "workerSystem.TryGetEntity(entityId, out var entity);",
                                    "entityManager.RemoveComponent<ComponentAuthority>(entity);"
                                });

                                if (!componentDetails.IsBlittable)
                                {
                                    m.Line($"var data = entityManager.GetComponentData<{componentNamespace}.Component>(entity);");
                                    foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                                    {
                                        m.Line($"{componentNamespace}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.Free(data.{fieldDetails.CamelCaseName}Handle);");
                                    }
                                }

                                m.Line($"entityManager.RemoveComponent<{componentNamespace}.Component>(entity);");
                            });

                            evm.Method("private void ApplyUpdate(in ComponentUpdateReceived<Update> update, ComponentDataFromEntity<Component> dataFromEntity)", m =>
                            {
                                m.Line(@"
workerSystem.TryGetEntity(update.EntityId, out var entity);
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

                            evm.Line(@"
private void SetAuthority(EntityId entityId, Authority authority)
{
    switch (authority)
    {
        case Authority.NotAuthoritative:
        {
            workerSystem.TryGetEntity(entityId, out var entity);
            entityManager.SetSharedComponentData(entity, ComponentAuthority.NotAuthoritative);
            break;
        }
        case Authority.Authoritative:
        {
            workerSystem.TryGetEntity(entityId, out var entity);
            entityManager.SetSharedComponentData(entity, ComponentAuthority.Authoritative);
            break;
        }
        case Authority.AuthorityLossImminent:
            break;
    }
}
");
                        });
                    });
                });
            });
        }
    }
}
