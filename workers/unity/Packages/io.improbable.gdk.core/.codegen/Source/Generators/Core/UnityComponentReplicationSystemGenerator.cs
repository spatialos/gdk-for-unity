using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityComponentReplicationSystemGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            var componentNamespace = $"global::{componentDetails.Namespace}.{componentDetails.Name}";

            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.ReplicationSystem internal class.");

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable",
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop",
                    "Unity.Collections",
                    "Unity.Entities",
                    "UnityEngine"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        partial.Annotate("UpdateInGroup(typeof(SpatialOSSendGroup)), UpdateBefore(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))")
                            .Type("internal class ReplicationSystem : SystemBase", system =>
                            {
                                system.Line(@"
private NativeQueue<SerializedMessagesToSend.UpdateToSend> dirtyComponents;
private SpatialOSSendSystem spatialOsSendSystem;

protected override void OnCreate()
{
    dirtyComponents = new NativeQueue<SerializedMessagesToSend.UpdateToSend>(Allocator.TempJob);
    spatialOsSendSystem = World.GetExistingSystem<SpatialOSSendSystem>();
}
");
                                system.Method("protected override void OnUpdate()", m =>
                                {
                                    m.Line(new[]
                                    {
                                        "dirtyComponents.Dispose();",
                                        "dirtyComponents = new NativeQueue<SerializedMessagesToSend.UpdateToSend>(Allocator.TempJob);",
                                        "var dirtyComponentsWriter = dirtyComponents.AsParallelWriter();",
                                    });

                                    m.Line($@"
Dependency = Entities.WithName(""{componentDetails.Name}Replication"")");
                                    if (!componentDetails.IsBlittable)
                                    {
                                        m.Line(@"
    .WithoutBurst()");
                                    }

                                    m.Line(@"
    .WithAll<HasAuthority>()
    .WithChangeFilter<Component>()
    .ForEach((ref Component component, in SpatialEntityId entity) =>
    {
        if (!component.IsDataDirty())
        {
            return;
        }

        // Serialize component
        var schemaUpdate = SchemaComponentUpdate.Create();
        Serialization.SerializeUpdate(component, schemaUpdate);

        component.MarkDataClean();

        // Schedule update
        var componentUpdate = new ComponentUpdate(ComponentId, schemaUpdate);
        var update = new SerializedMessagesToSend.UpdateToSend(componentUpdate, entity.EntityId.Id);
        dirtyComponentsWriter.Enqueue(update);
    })
    .ScheduleParallel(Dependency);

spatialOsSendSystem.AddReplicationJobProducer(Dependency, dirtyComponents);
");
                                });
                            });
                    });
                });
            });
        }
    }
}
