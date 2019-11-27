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

            InputFiles = store.SchemaFiles.ToList();
            OutputFiles = new List<string>();

            logger.Info("Gathering component details");
            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Package))
                .ToList();

            logger.Info("Defining job output files for components");
            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;

                logger.Trace($"Defining job output files for component {componentTarget.Content.QualifiedName}");

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    logger.Trace("Defining job output file for command sender and receiver");
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandSenderReceiver", FileExtension)));
                }

                logger.Trace("Defining job output file for component reader and writer");
                OutputFiles.Add(Path.Combine(relativeOutputPath,
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
                    Content.Add(Path.Combine(relativeOutputPath, commandSenderReceiverFileName), commandSenderReceiverCode);
                }

                logger.Info("Generating component readers and writers");

                logger.Trace($"Generating {componentName}ComponentReaderWriter");
                var componentReaderWriterFileName =
                    Path.ChangeExtension($"{componentName}ComponentReaderWriter", FileExtension);
                var componentReaderWriterCode =
                    componentReaderWriterGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentReaderWriterFileName), componentReaderWriterCode);
            }
            logger.Info("Finished code generation for components");
        }
    }
}
