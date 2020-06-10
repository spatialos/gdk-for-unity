using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public class ComponentVisualElementJob : CodegenJob
    {
        private const string DebugAsmdefFileName = "Improbable.Gdk.Generated.Debug.asmdef";
        private readonly string relativeOutputPath = Path.Combine("improbable", "debugextensions");

        public ComponentVisualElementJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore detailsStore) : base(options.AsEditor(), fileSystem, detailsStore)
        {
            const string jobName = nameof(ComponentVisualElementJob);
            Logger.Trace($"Initialising {jobName}.");

            Logger.Trace($"Adding job target for {DebugAsmdefFileName}");
            AddJobTarget(Path.Combine(relativeOutputPath, DebugAsmdefFileName), () => DebugAssemblyGenerator.Generate());

            var componentsToGenerate = detailsStore.Components.Values.ToList();
            var componentGenerator = new ComponentVisualElementGenerator(detailsStore);
            AddGenerators(relativeOutputPath, componentsToGenerate, component => ($"{component.Name}Renderer.cs", componentGenerator.Generate));
            Logger.Trace($"Added job targets for {componentsToGenerate.Count} components");

            // Types
            Logger.Trace("Gathering nested types.");
            var allNestedTypes = detailsStore.Types
                .SelectMany(kv => detailsStore.GetNestedTypes(kv.Key))
                .ToHashSet();

            Logger.Trace("Gathering types details.");
            var typesToGenerate = detailsStore.Types
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList();

            var typeGenerator = new TypeVisualElementGenerator(detailsStore);
            AddGenerators(relativeOutputPath, typesToGenerate, type => ($"{type.Name}Renderer.cs", typeGenerator.Generate));
            Logger.Trace($"Added job targets for {typesToGenerate.Count} types");
        }
    }
}
