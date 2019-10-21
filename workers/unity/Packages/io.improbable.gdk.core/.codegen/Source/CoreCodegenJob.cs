using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator.Core
{
    public class CoreCodegenJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private readonly List<GenerationTarget<UnityTypeDetails>> typesToGenerate;

        private readonly List<GenerationTarget<UnityEnumDetails>> enumsToGenerate;

        private const string FileExtension = ".cs";

        public CoreCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store) : base(
            outputDir, fileSystem, store)
        {
            InputFiles = store.SchemaFiles.ToList();
            OutputFiles = new List<string>();

            var allNestedTypes = store.Types
                .SelectMany(kv => store.GetNestedTypes(kv.Key))
                .ToHashSet();

            typesToGenerate = store.Types
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityTypeDetails>(kv.Value, kv.Value.Package))
                .ToList();

            enumsToGenerate = store.Enums
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityEnumDetails>(kv.Value, kv.Value.Package))
                .ToList();

            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Value.Package))
                .ToList();

            foreach (var typeTarget in typesToGenerate)
            {
                var fileName = Path.ChangeExtension(typeTarget.Content.CapitalisedName, FileExtension);
                OutputFiles.Add(Path.Combine(typeTarget.OutputPath, fileName));
            }

            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;

                OutputFiles.Add(Path.Combine(relativeOutputPath, Path.ChangeExtension(componentTarget.Content.ComponentName, FileExtension)));

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
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

            foreach (var enumTarget in enumsToGenerate)
            {
                var fileName = Path.ChangeExtension(enumTarget.Content.TypeName, FileExtension);
                OutputFiles.Add(Path.Combine(enumTarget.OutputPath, fileName));
            }
        }

        protected override void RunImpl()
        {
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

            foreach (var enumTarget in enumsToGenerate)
            {
                var fileName = Path.ChangeExtension(enumTarget.Content.TypeName, FileExtension);
                var enumCode = enumGenerator.Generate(enumTarget.Content, enumTarget.Package);
                Content.Add(Path.Combine(enumTarget.OutputPath, fileName), enumCode);
            }

            foreach (var typeTarget in typesToGenerate)
            {
                var fileName = Path.ChangeExtension(typeTarget.Content.CapitalisedName, FileExtension);
                var typeCode = typeGenerator.Generate(typeTarget.Content, typeTarget.Package);
                Content.Add(Path.Combine(typeTarget.OutputPath, fileName), typeCode);
            }

            foreach (var componentTarget in componentsToGenerate)
            {
                var relativeOutputPath = componentTarget.OutputPath;
                var componentName = componentTarget.Content.ComponentName;
                var package = componentTarget.Package;

                var componentFileName = Path.ChangeExtension(componentName, FileExtension);
                var componentCode = blittableComponentGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentFileName), componentCode);

                if (componentTarget.Content.CommandDetails.Count > 0)
                {
                    var commandPayloadsFileName =
                        Path.ChangeExtension($"{componentName}CommandPayloads", FileExtension);
                    var commandPayloadCode =
                        commandPayloadGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandPayloadsFileName), commandPayloadCode);

                    var commandDiffDeserializerFileName =
                        Path.ChangeExtension($"{componentName}CommandDiffDeserializer", FileExtension);
                    var commandDiffDeserializerCode =
                        commandDiffDeserializerGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandDiffDeserializerFileName),
                        commandDiffDeserializerCode);

                    var commandDiffStorageFileName =
                        Path.ChangeExtension($"{componentName}CommandDiffStorage", FileExtension);
                    var commandDiffStorageCode =
                        commandDiffStorageGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandDiffStorageFileName),
                        commandDiffStorageCode);

                    var commandMetaDataStorageFileName =
                        Path.ChangeExtension($"{componentName}CommandMetaDataStorage", FileExtension);
                    var commandMetaDataStorageCode =
                        commandMetaDataStorageGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandMetaDataStorageFileName),
                        commandMetaDataStorageCode);
                }

                if (componentTarget.Content.EventDetails.Count > 0)
                {
                    var eventsFileName = Path.ChangeExtension($"{componentName}Events", FileExtension);
                    var eventsCode = eventGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, eventsFileName), eventsCode);
                }

                var updateSenderFileName = Path.ChangeExtension($"{componentName}UpdateSender", FileExtension);
                var updateSenderCode = componentSenderGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, updateSenderFileName), updateSenderCode);

                var ecsViewManagerFileName = Path.ChangeExtension($"{componentName}EcsViewManager", FileExtension);
                var ecsViewManagerCode = ecsViewManagerGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, ecsViewManagerFileName), ecsViewManagerCode);

                var componentDiffStorageFileName = Path.ChangeExtension($"{componentName}ComponentDiffStorage", FileExtension);
                var componentDiffStorageCode = componentDiffStorageGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentDiffStorageFileName), componentDiffStorageCode);

                var componentDiffDeserializerFileName = Path.ChangeExtension($"{componentName}ComponentDiffDeserializer", FileExtension);
                var componentDiffDeserializerCode = componentDiffDeserializerGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, componentDiffDeserializerFileName), componentDiffDeserializerCode);

                if (componentTarget.Content.FieldDetails.Any(field => !field.IsBlittable))
                {
                    var referenceProviderFileName = Path.ChangeExtension($"{componentName}Providers", FileExtension);
                    var referenceProviderTranslationCode =
                        referenceTypeProviderGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, referenceProviderFileName),
                        referenceProviderTranslationCode);
                }

                var viewStorageFileName = Path.ChangeExtension($"{componentName}ViewStorage", FileExtension);
                var viewStorageCode = viewStorageGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, viewStorageFileName), viewStorageCode);

                var metaclassFileName = Path.ChangeExtension($"{componentName}Metaclass", FileExtension);
                var metaclassCode = metaclassGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, metaclassFileName), metaclassCode);
            }
        }
    }
}
