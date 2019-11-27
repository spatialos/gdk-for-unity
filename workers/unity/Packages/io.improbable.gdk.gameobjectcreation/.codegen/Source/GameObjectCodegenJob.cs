using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator.GameObjectCreation
{
    public class GameObjectCodegenJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private const string FileExtension = ".cs";

        public GameObjectCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store)
            : base(outputDir, fileSystem, store)
        {
            var jobName = nameof(GameObjectCodegenJob);
            logger.Info($"Initialising {jobName}");

            AddInputFiles(store.SchemaFiles.ToList());

            logger.Info("Gathering component details");
            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Package))
                .ToList();

            logger.Info("Adding job output files");
            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;

                logger.Trace($"Adding job output files for component {componentTarget.Content.QualifiedName}");

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    AddOutputFile(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension)));
                }

                AddOutputFile(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension)));
            }

            logger.Info($"Finished initialising {jobName}");
        }

        protected override void RunImpl()
        {
            logger.Info("Creating generators");
            var componentReaderWriterGenerator = new UnityComponentReaderWriterGenerator();
            var commandSenderReceiverGenerator = new UnityCommandSenderReceiverGenerator();

            logger.Info("Starting code generation for components");
            foreach (var componentTarget in componentsToGenerate)
            {
                logger.Info($"Generating code for {componentTarget.Content.QualifiedName}");

                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;
                var package = componentTarget.Package;

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    logger.Info("Generating command senders and command receivers");

                    logger.Trace($"Generating {componentName}CommandSenderReceiver");
                    var commandSenderReceiverFileName =
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension);
                    var commandSenderReceiverCode =
                        commandSenderReceiverGenerator.Generate(componentTarget.Content, package);
                    AddContent(Path.Combine(relativeOutputPath, commandSenderReceiverFileName), commandSenderReceiverCode);
                }

                logger.Info("Generating component readers and writers");

                logger.Trace($"Generating {componentName}ComponentReaderWriter");
                var componentReaderWriterFileName =
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension);
                var componentReaderWriterCode =
                    componentReaderWriterGenerator.Generate(componentTarget.Content, package);
                AddContent(Path.Combine(relativeOutputPath, componentReaderWriterFileName), componentReaderWriterCode);
            }
            logger.Info("Finished code generation for components");
        }
    }
}
