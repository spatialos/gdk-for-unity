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
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop",
                    "Unity.Collections",
                    "Unity.Entities",
                    "Unity.Profiling"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        partial.Annotate("DisableAutoCreation, UpdateInGroup(typeof(SpatialOSReplicationGroup))")
                            .Type("internal class ReplicationSystem : SystemBase", system =>
                            {
                                system.Line($@"
private NativeQueue<SerializedMessagesToSend.UpdateToSend> dirtyComponents;
private SpatialOSSendSystem spatialOsSendSystem;

private ProfilerMarker foreachMarker = new ProfilerMarker(""{componentDetails.Name}SerializationJob"");

protected override void OnCreate()
{{
    spatialOsSendSystem = World.GetExistingSystem<SpatialOSSendSystem>();
}}
");
                                system.Method("protected override void OnUpdate()", m =>
                                {
                                    m.Line(new[]
                                    {
                                        "dirtyComponents = new NativeQueue<SerializedMessagesToSend.UpdateToSend>(Allocator.TempJob);",
                                        "var dirtyComponentsWriter = dirtyComponents.AsParallelWriter();",
                                        "var marker = foreachMarker;",
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
        marker.Begin();
        if (!component.IsDataDirty())
        {
            marker.End();
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
        marker.End();
    })
    .ScheduleParallel(Dependency);

spatialOsSendSystem.AddReplicationJobProducer(Dependency, dirtyComponents);
dirtyComponents = default;
");
                                });
                            });
                    });
                });
            });
        }
    }
}
