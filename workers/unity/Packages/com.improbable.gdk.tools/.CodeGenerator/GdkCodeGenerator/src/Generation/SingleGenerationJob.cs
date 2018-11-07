using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class SingleGenerationJob : CodegenJob
    {
        private readonly List<GenerationTarget<UnityComponentDetails>> componentsToGenerate;

        private readonly List<GenerationTarget<UnityTypeDetails>> typesToGenerate;

        private readonly List<GenerationTarget<UnityEnumDetails>> enumsToGenerate;

        private const string FileExtension = ".cs";

        public SingleGenerationJob(string outputDir, DetailsStore store, IFileSystem fileSystem) : base(
            outputDir, fileSystem)
        {
            InputFiles = store.SchemaFiles.ToList();
            OutputFiles = new List<string>();

            var allNestedTypes = store.Types
                .SelectMany(kv => store.GetNestedTypes(kv.Key))
                .ToHashSet();

            typesToGenerate = store.Types
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityTypeDetails>(kv.Value, kv.Key.PackagePath))
                .ToList();

            enumsToGenerate = store.Enums
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => new GenerationTarget<UnityEnumDetails>(kv.Value, kv.Key.PackagePath))
                .ToList();

            componentsToGenerate = store.Components
                .Select(kv => new GenerationTarget<UnityComponentDetails>(kv.Value, kv.Key.PackagePath))
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
                        Path.ChangeExtension($"{componentName}MonoBehaviourCommandHandlers", FileExtension)));
                }

                if (componentTarget.Content.EventDetails.Count > 0)
                {
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{componentName}Events", FileExtension)));
                }

                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}Translation", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}Providers", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}GameObjectComponentDispatcher", FileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{componentName}ReaderWriter", FileExtension)));
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
            var commandComponentsGenerator = new UnityCommandComponentsGenerator();
            var blittableComponentGenerator = new UnityComponentDataGenerator();
            var componentConversionGenerator = new UnityComponentConversionGenerator();
            var referenceTypeProviderGenerator = new UnityReferenceTypeProviderGenerator();
            var gameObjectComponentDispatcherGenerator = new UnityGameObjectComponentDispatcherGenerator();
            var gameObjectCommandHandlersGenerator = new UnityGameObjectCommandHandlersGenerator();
            var readerWriterGenerator = new UnityReaderWriterGenerator();

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

                    var commandComponentsFileName =
                        Path.ChangeExtension($"{componentName}CommandComponents", FileExtension);
                    var commandComponentsCode =
                        commandComponentsGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandComponentsFileName), commandComponentsCode);

                    var monobehaviourCommandHandlerFileName =
                        Path.ChangeExtension($"{componentName}MonoBehaviourCommandHandlers", FileExtension);
                    var monobehaviourCommandHandlerCode =
                        gameObjectCommandHandlersGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, monobehaviourCommandHandlerFileName),
                        monobehaviourCommandHandlerCode);
                }

                if (componentTarget.Content.EventDetails.Count > 0)
                {
                    var eventsFileName = Path.ChangeExtension($"{componentName}Events", FileExtension);
                    var eventsCode = eventGenerator.Generate(componentTarget.Content, package);
                    Content.Add(Path.Combine(relativeOutputPath, eventsFileName), eventsCode);
                }

                var conversionFileName = Path.ChangeExtension($"{componentName}Translation", FileExtension);
                var componentTranslationCode = componentConversionGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, conversionFileName), componentTranslationCode);

                var referenceProviderFileName = Path.ChangeExtension($"{componentName}Providers", FileExtension);
                var referenceProviderTranslationCode =
                    referenceTypeProviderGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, referenceProviderFileName),
                    referenceProviderTranslationCode);

                var gameObjectComponentDispatcherFileName =
                    Path.ChangeExtension($"{componentName}GameObjectComponentDispatcher", FileExtension);
                var gameObjectComponentDispatcherCode =
                    gameObjectComponentDispatcherGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, gameObjectComponentDispatcherFileName),
                    gameObjectComponentDispatcherCode);

                var readerWriterFileName =
                    Path.ChangeExtension($"{componentName}ReaderWriter", FileExtension);
                var readerWriterCode =
                    readerWriterGenerator.Generate(componentTarget.Content, package);
                Content.Add(Path.Combine(relativeOutputPath, readerWriterFileName), readerWriterCode);
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
