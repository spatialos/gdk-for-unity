using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator.GameObjectCreation
{
    public class GameObjectGenerationJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private const string FileExtension = ".cs";

        public GameObjectGenerationJob(string outputDir, IFileSystem fileSystem, DetailsStore store) : base(
            outputDir, fileSystem, store)
        {
            InputFiles = store.SchemaFiles.ToList();
            OutputFiles = new List<string>();

            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Package))
                .ToList();

            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension)));
                }

                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension)));
            }
        }

        protected override void RunImpl()
        {
            var componentReaderWriterGenerator = new UnityComponentReaderWriterGenerator();
            var commandSenderReceiverGenerator = new UnityCommandSenderReceiverGenerator();

            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;
                var package = componentTarget.Package;

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    var commandSenderReceiverFileName =
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension);
                    var commandSenderReceiverCode =
                        commandSenderReceiverGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandSenderReceiverFileName), commandSenderReceiverCode);
                }

                var componentReaderWriterFileName =
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension);
                var componentReaderWriterCode =
                    componentReaderWriterGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentReaderWriterFileName), componentReaderWriterCode);
            }
        }

        private struct GenerationTarget<T>
        {
            public readonly T Content;
            public readonly string Package;
            public readonly string OutputPath;

            public GenerationTarget(T content, string package)
            {
                Content = content;
                Package = Formatting.CapitaliseQualifiedNameParts(package);
                OutputPath = Formatting.GetNamespacePath(package);
            }
        }
    }
}
