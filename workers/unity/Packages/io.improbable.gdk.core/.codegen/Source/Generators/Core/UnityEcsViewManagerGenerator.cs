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

            var nonBlittableFields = componentDetails.FieldDetails
                .Where(fd => !fd.IsBlittable)
                .ToList();

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Unity.Collections"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace(
                            $"Generating {componentDetails.Namespace}.{componentDetails.Name}.EcsViewManager class.");

                        partial.Type(
                            $"public class EcsViewManager : EcsViewManager<{componentNamespace}.Component, {componentNamespace}.Update, {componentNamespace}.HasAuthority>",
                            evm =>
                            {
                                evm.Method($"protected override {componentNamespace}.Component CreateEmptyComponent()", mb =>
                                {
                                    mb.Line($"var component = new {componentNamespace}.Component();");

                                    foreach (var fieldDetails in nonBlittableFields)
                                    {
                                        mb.Line(
                                            $"component.{fieldDetails.CamelCaseName}Handle = global::Improbable.Gdk.Core.ReferenceProvider<{fieldDetails.Type}>.Create();");
                                    }

                                    mb.Line(new[] { "component.MarkDataClean();", "return component;" });
                                });

                                evm.Method(
                                    $"protected override void ApplyUpdate(ref {componentNamespace}.Component data, in {componentNamespace}.Update update)",
                                    mb =>
                                    {
                                        foreach (var fieldDetails in componentDetails.FieldDetails)
                                        {
                                            mb.If($"update.{fieldDetails.PascalCaseName}.HasValue", sb =>
                                            {
                                                sb.Line(
                                                    $"data.{fieldDetails.PascalCaseName} = update.{fieldDetails.PascalCaseName}.Value;");
                                            });

                                            mb.Line("data.MarkDataClean();");
                                        }
                                    });

                                if (nonBlittableFields.Count > 0)
                                {
                                    evm.Method("public override void Clean()", m =>
                                    {
                                        m.Line(new[]
                                        {
                                            $"var query = EntityManager.CreateEntityQuery(typeof({componentNamespace}.Component));",
                                            $"var componentDataArray = query.ToComponentDataArray<{componentNamespace}.Component>(Allocator.TempJob);"
                                        });

                                        m.Loop("foreach (var component in componentDataArray)", loop =>
                                        {
                                            foreach (var fieldDetails in nonBlittableFields)
                                            {
                                                loop.Line($"component.{fieldDetails.CamelCaseName}Handle.Dispose();");
                                            }
                                        });

                                        m.Line("componentDataArray.Dispose();");
                                    });

                                    evm.Method($"protected override void DisposeData({componentNamespace}.Component data)",
                                        mb =>
                                        {
                                            foreach (var fieldDetails in nonBlittableFields)
                                            {
                                                mb.Line($"data.{fieldDetails.CamelCaseName}Handle.Dispose();");
                                            }
                                        });
                                }

                                if (componentDetails.FieldDetails.Count > 0)
                                {
                                    evm.Method($"protected override void AddReplicationSystem()",
                                        mb =>
                                        {
                                            mb.Line(new[]
                                            {
                                                "base.AddReplicationSystem();",
                                                $"var replicationSystem = ReplicationGroupSystem.World.GetOrCreateSystem<{componentNamespace}.ReplicationSystem>();",
                                                "ReplicationGroupSystem.AddSystemToUpdateList(replicationSystem);"
                                            });
                                        });
                                }
                            });
                    });
                });
            });
        }
    }
}
