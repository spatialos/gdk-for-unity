using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator.GameObjectCreation
{
    public class GameObjectCodegenJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private const string FileExtension = ".cs";

        public GameObjectCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store, bool force)
            : base(outputDir, fileSystem, store, force)
        {
            var jobName = nameof(GameObjectCodegenJob);
            logger.Info($"Initialising {jobName}.");

            AddInputFiles(store.SchemaFiles.ToList());

            logger.Info("Gathering component details.");
            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Namespace))
                .ToList();

            logger.Trace("Adding job output files.");
            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.Name;

                logger.Trace($"Adding job output files for component {componentName}.");

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    AddOutputFile(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension)));
                }

                AddOutputFile(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension)));
            }

            logger.Info($"Added job output files for {componentsToGenerate.Count} components.");

            logger.Info($"Finished initialising {jobName}.");
        }

        protected override void RunImpl()
        {
            logger.Info("Starting code generation for components.");
            foreach (var componentTarget in componentsToGenerate)
            {
                logger.Trace($"Generating code for {componentTarget.Content.Name}.");

                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.Name;
                var package = componentTarget.Package;

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    var commandSenderReceiverFileName =
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension);
                    var commandSenderReceiverCode =
                        UnityCommandSenderReceiverGenerator.Generate(componentTarget.Content, package);
                    AddContent(Path.Combine(relativeOutputPath, commandSenderReceiverFileName), commandSenderReceiverCode);
                }

                var componentReaderWriterFileName =
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension);
                var componentReaderWriterCode =
                    UnityComponentReaderWriterGenerator.Generate(componentTarget.Content, package);
                AddContent(Path.Combine(relativeOutputPath, componentReaderWriterFileName), componentReaderWriterCode);
            }

            logger.Info($"Finished code generation for {componentsToGenerate.Count} components.");
        }
    }
}
