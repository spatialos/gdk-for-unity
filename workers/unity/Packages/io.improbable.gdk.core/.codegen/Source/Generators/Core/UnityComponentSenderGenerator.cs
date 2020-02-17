using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityComponentSenderGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            var componentNamespace = $"global::{componentDetails.Namespace}.{componentDetails.Name}";

            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.ComponentReplicator internal class.");

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "UnityEngine",
                    "UnityEngine.Profiling",
                    "Unity.Mathematics",
                    "Unity.Entities",
                    "Unity.Collections",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.CodegenAdapters"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        partial.Type("internal class ComponentReplicator : IComponentReplicationHandler", replicator =>
                        {
                            replicator.Line($@"
public uint ComponentId => {componentDetails.ComponentId};

public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
{{
    All = new[]
    {{
        ComponentType.ReadWrite<{componentNamespace}.Component>(),
        ComponentType.ReadWrite<{componentNamespace}.ComponentAuthority>(),
        ComponentType.ReadOnly<SpatialEntityId>()
    }},
}};
");
                            replicator.Method(@"
public void SendUpdates(
    NativeArray<ArchetypeChunk> chunkArray,
    ComponentSystemBase system,
    EntityManager entityManager,
    ComponentUpdateSystem componentUpdateSystem)
", m =>
                            {
                                m.ProfilerStart(componentDetails.Name);

                                m.Line(new[]
                                {
                                    "var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);",
                                    $"var componentType = system.GetArchetypeChunkComponentType<{componentNamespace}.Component>();",
                                    "var authorityType = system.GetArchetypeChunkSharedComponentType<ComponentAuthority>();"
                                });

                                m.Loop("foreach (var chunk in chunkArray)", outerLoop =>
                                {
                                    outerLoop.Line(new[]
                                    {
                                        "var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);",
                                        "var componentArray = chunk.GetNativeArray(componentType);",
                                        "var authorityIndex = chunk.GetSharedComponentIndex(authorityType);",
                                    });

                                    outerLoop.If("!entityManager.GetSharedComponentData<ComponentAuthority>(authorityIndex).HasAuthority", () => new[]
                                    {
                                        "continue;"
                                    });

                                    outerLoop.Loop("for (var i = 0; i < componentArray.Length; i++)", innerLoop =>
                                    {
                                        innerLoop.Line("var data = componentArray[i];");

                                        innerLoop.If("data.IsDataDirty()", componentDirtyThen =>
                                        {
                                            componentDirtyThen.Line("Update update = new Update();");
                                            for (var i = 0; i < componentDetails.FieldDetails.Count; i++)
                                            {
                                                var fieldDetails = componentDetails.FieldDetails[i];
                                                componentDirtyThen.If($"data.IsDataDirty({i})", () => new[]
                                                {
                                                    $"update.{fieldDetails.PascalCaseName} = data.{fieldDetails.PascalCaseName};"
                                                });
                                            }

                                            componentDirtyThen.Line(new[]
                                            {
                                                "componentUpdateSystem.SendUpdate(in update, entityIdArray[i].EntityId);",
                                                "data.MarkDataClean();",
                                                "componentArray[i] = data;"
                                            });
                                        });
                                    });
                                });

                                m.ProfilerEnd();
                            });
                        });
                    });
                });
            });
        }
    }
}
