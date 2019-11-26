using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator.Core
{
    public class CoreCodegenJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private readonly List<GenerationTarget<UnityTypeDetails>> typesToGenerate;

        private readonly List<GenerationTarget<UnityEnumDetails>> enumsToGenerate;

        private const string FileExtension = ".cs";

        public CoreCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store)
            : base(outputDir, fileSystem, store, LogManager.GetCurrentClassLogger())
        {
            logger.Info("Initialising CoreCodegenJob");

            InputFiles = store.SchemaFiles.ToList();
            OutputFiles = new List<string>();

            logger.Info("Gathering nested types");
            var allNestedTypes = store.Types
                .SelectMany(kv => store.GetNestedTypes(kv.Key))
                .ToHashSet();

            logger.Info("Gathering types details");
            typesToGenerate = store.Types
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityTypeDetails>(kv.Value, kv.Value.Package))
                .ToList();

            logger.Info("Gathering enum details");
            enumsToGenerate = store.Enums
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityEnumDetails>(kv.Value, kv.Value.Package))
                .ToList();

            logger.Info("Gathering component details");
            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Package))
                .ToList();

            logger.Info("Defining job output files for types");
            foreach (var typeTarget in typesToGenerate)
            {
                logger.Trace($"Defining output file for type {typeTarget.Content.QualifiedName}");

                var fileName = Path.ChangeExtension(typeTarget.Content.CapitalisedName, FileExtension);
                OutputFiles.Add(Path.Combine(typeTarget.OutputPath, fileName));
            }

            logger.Info("Defining job output files for components");
            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;

                logger.Trace($"Defining job output files for component {componentTarget.Content.QualifiedName}");

                OutputFiles.Add(Path.Combine(relativeOutputPath, Path.ChangeExtension(componentTarget.Content.ComponentName, FileExtension)));

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    logger.Trace("Defining job output files for commands");

                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandPayloads", FileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandComponents", FileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandDiffDeserializer", FileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandDiffStorage", FileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}CommandMetaDataStorage", FileExtension)));
                }

                if (componentTarget.Content.EventDetails.Count > 0)
                {
                    logger.Trace("Defining job output file for events");

                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}Events", FileExtension)));
                }

                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}UpdateSender", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}EcsViewManager", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ComponentDiffStorage", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ComponentDiffDeserializer", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}Providers", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ViewStorage", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath, Path.ChangeExtension($"{componentName}Metaclass", FileExtension)));
            }

            logger.Info("Defining job output files for enums");
            foreach (var enumTarget in enumsToGenerate)
            {
                logger.Trace($"Defining job output file for enum {enumTarget.Content.QualifiedName}");

                var fileName = Path.ChangeExtension(enumTarget.Content.TypeName, FileExtension);
                OutputFiles.Add(Path.Combine(enumTarget.OutputPath, fileName));
            }

            logger.Info("Finished initialising CoreCodegenJob");
        }

        protected override void RunImpl()
        {
            logger.Info("Creating generators");
            var typeGenerator = new UnityTypeGenerator();
            var enumGenerator = new UnityEnumGenerator();
            var eventGenerator = new UnityEventGenerator();
            var commandPayloadGenerator = new UnityCommandPayloadGenerator();
            var blittableComponentGenerator = new UnityComponentDataGenerator();
            var componentSenderGenerator = new UnityComponentSenderGenerator();
            var ecsViewManagerGenerator = new UnityEcsViewManagerGenerator();
            var referenceTypeProviderGenerator = new UnityReferenceTypeProviderGenerator();
            var componentDiffStorageGenerator = new ComponentDiffStorageGenerator();
            var componentDiffDeserializerGenerator = new ComponentDiffDeserializerGenerator();
            var commandDiffDeserializerGenerator = new CommandDiffDeserializerGenerator();
            var commandDiffStorageGenerator = new CommandDiffStorageGenerator();
            var viewStorageGenerator = new ViewStorageGenerator();
            var commandMetaDataStorageGenerator = new CommandMetaDataStorageGenerator();
            var metaclassGenerator = new MetaclassGenerator();

            logger.Info("Starting code generation for enums");
            foreach (var enumTarget in enumsToGenerate)
            {
                logger.Info($"Generating {enumTarget.Content.QualifiedName}");

                var fileName = Path.ChangeExtension(enumTarget.Content.TypeName, FileExtension);
                var enumCode = enumGenerator.Generate(enumTarget.Content, enumTarget.Package);
                Content.Add(Path.Combine(enumTarget.OutputPath, fileName), enumCode);
            }
            logger.Info("Finished code generation for enums");

            logger.Info("Starting code generation for types");
            foreach (var typeTarget in typesToGenerate)
            {
                logger.Info($"Generating {typeTarget.Content.QualifiedName}");

                var fileName = Path.ChangeExtension(typeTarget.Content.CapitalisedName, FileExtension);
                var typeCode = typeGenerator.Generate(typeTarget.Content, typeTarget.Package);
                Content.Add(Path.Combine(typeTarget.OutputPath, fileName), typeCode);
            }
            logger.Info("Finished code generation for enums");

            logger.Info("Starting code generation for components");
            foreach (var componentTarget in componentsToGenerate)
            {
                logger.Info($"Generating code for {componentTarget.Content.QualifiedName}");

                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;
                var package = componentTarget.Package;

                var componentFileName = Path.ChangeExtension(componentName, FileExtension);
                var componentCode = blittableComponentGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentFileName), componentCode);

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    logger.Info($"Generating code for commands");

                    logger.Trace($"Generating {componentName}CommandPayloads");
                    var commandPayloadsFileName =
                        Path.ChangeExtension($"{componentName}CommandPayloads", FileExtension);
                    var commandPayloadCode =
                        commandPayloadGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandPayloadsFileName), commandPayloadCode);

                    logger.Trace($"Generating {componentName}CommandDiffDeserializer");
                    var commandDiffDeserializerFileName =
                        Path.ChangeExtension($"{componentName}CommandDiffDeserializer", FileExtension);
                    var commandDiffDeserializerCode =
                        commandDiffDeserializerGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandDiffDeserializerFileName),
                        commandDiffDeserializerCode);

                    logger.Trace($"Generating {componentName}CommandDiffStorage");
                    var commandDiffStorageFileName =
                        Path.ChangeExtension($"{componentName}CommandDiffStorage", FileExtension);
                    var commandDiffStorageCode =
                        commandDiffStorageGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandDiffStorageFileName),
                        commandDiffStorageCode);

                    logger.Trace($"Generating {componentName}CommandMetaDataStorage");
                    var commandMetaDataStorageFileName =
                        Path.ChangeExtension($"{componentName}CommandMetaDataStorage", FileExtension);
                    var commandMetaDataStorageCode =
                        commandMetaDataStorageGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandMetaDataStorageFileName),
                        commandMetaDataStorageCode);
                }

                if (componentTarget.Content.EventDetails.Count > 0)
                {
                    logger.Info("Generating code for events");

                    logger.Trace($"Generating {componentName}Events");
                    var eventsFileName = Path.ChangeExtension($"{componentName}Events", FileExtension);
                    var eventsCode = eventGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, eventsFileName), eventsCode);
                }

                logger.Trace($"Generating {componentName}UpdateSender");
                var updateSenderFileName = Path.ChangeExtension($"{componentName}UpdateSender", FileExtension);
                var updateSenderCode = componentSenderGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, updateSenderFileName), updateSenderCode);

                logger.Trace($"Generating {componentName}EcsViewManager");
                var ecsViewManagerFileName = Path.ChangeExtension($"{componentName}EcsViewManager", FileExtension);
                var ecsViewManagerCode = ecsViewManagerGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, ecsViewManagerFileName), ecsViewManagerCode);

                logger.Trace($"Generating {componentName}ComponentDiffStorage");
                var componentDiffStorageFileName = Path.ChangeExtension($"{componentName}ComponentDiffStorage", FileExtension);
                var componentDiffStorageCode = componentDiffStorageGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentDiffStorageFileName), componentDiffStorageCode);

                logger.Trace($"Generating {componentName}ComponentDiffDeserializer");
                var componentDiffDeserializerFileName = Path.ChangeExtension($"{componentName}ComponentDiffDeserializer", FileExtension);
                var componentDiffDeserializerCode = componentDiffDeserializerGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentDiffDeserializerFileName), componentDiffDeserializerCode);

                if (componentTarget.Content.FieldDetails.Any(field => !field.IsBlittable))
                {
                    logger.Info("Generating code for non-blittable fields");

                    logger.Trace($"Generating {componentName}Providers");
                    var referenceProviderFileName = Path.ChangeExtension($"{componentName}Providers", FileExtension);
                    var referenceProviderTranslationCode =
                        referenceTypeProviderGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, referenceProviderFileName),
                        referenceProviderTranslationCode);
                }

                logger.Trace($"Generating {componentName}ViewStorage");
                var viewStorageFileName = Path.ChangeExtension($"{componentName}ViewStorage", FileExtension);
                var viewStorageCode = viewStorageGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, viewStorageFileName), viewStorageCode);

                logger.Trace($"Generating {componentName}Metaclass");
                var metaclassFileName = Path.ChangeExtension($"{componentName}Metaclass", FileExtension);
                var metaclassCode = metaclassGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, metaclassFileName), metaclassCode);
            }
            logger.Info("Finished code generation for enums");
        }
    }
}
