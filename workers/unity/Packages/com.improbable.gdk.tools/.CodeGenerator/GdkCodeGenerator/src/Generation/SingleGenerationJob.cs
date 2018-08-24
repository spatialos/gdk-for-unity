using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Jobs;
using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class SingleGenerationJob : CodegenJob
    {
        private readonly string relativeOutputPath;
        private readonly string package;
        private readonly List<UnityTypeDefinition> typesToGenerate;
        private readonly List<UnityComponentDefinition> componentsToGenerate;
        private readonly List<EnumDefinitionRaw> enumsToGenerate;

        private readonly HashSet<string> enumSet = new HashSet<string>();

        private const string fileExtension = ".cs";

        public SingleGenerationJob(string outputDir, UnitySchemaFile schemaFile, IFileSystem fileSystem,
            HashSet<string> enumSet) : base(
            outputDir, fileSystem)
        {
            InputFiles = new List<string> { schemaFile.CompletePath };
            OutputFiles = new List<string>();

            relativeOutputPath = Formatting.GetNamespacePath(schemaFile.Package);
            package = Formatting.CapitaliseQualifiedNameParts(schemaFile.Package);

            typesToGenerate = SelectTypesToGenerate(schemaFile);

            foreach (var unityTypeDefinition in typesToGenerate)
            {
                var fileName = Path.ChangeExtension(unityTypeDefinition.Name, fileExtension);
                OutputFiles.Add(Path.Combine(relativeOutputPath, fileName));
            }

            componentsToGenerate = schemaFile.ComponentDefinitions;
            foreach (var component in componentsToGenerate)
            {
                OutputFiles.Add(Path.Combine(relativeOutputPath, Path.ChangeExtension(component.Name, fileExtension)));

                if (component.CommandDefinitions.Count > 0)
                {
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{component.Name}CommandPayloads", fileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{component.Name}CommandComponents", fileExtension)));
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{component.Name}CommandStorage", fileExtension)));
                }

                if (component.EventDefinitions.Count > 0)
                {
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{component.Name}Events", fileExtension)));
                }

                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{component.Name}Translation", fileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{component.Name}Providers", fileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{component.Name}GameObjectComponentDispatcher", fileExtension)));
                OutputFiles.Add(Path.Combine(relativeOutputPath,
                    Path.ChangeExtension($"{component.Name}ReaderWriter", fileExtension)));

                if (component.CommandDefinitions.Count > 0)
                {
                    OutputFiles.Add(Path.Combine(relativeOutputPath,
                        Path.ChangeExtension($"{component.Name}MonoBehaviourCommandHandlers", fileExtension)));
                }
            }

            enumsToGenerate = new List<EnumDefinitionRaw>();
            enumsToGenerate.AddRange(schemaFile.EnumDefinitions);
            foreach (var unityEnum in enumsToGenerate)
            {
                var fileName = Path.ChangeExtension(unityEnum.name, fileExtension);
                OutputFiles.Add(Path.Combine(relativeOutputPath, fileName));
            }

            this.enumSet = enumSet;
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
            var commandStorageGenerator = new UnityCommandStorageGenerator();
            var gameObjectComponentDispatcherGenerator = new UnityGameObjectComponentDispatcherGenerator();
            var gameObjectCommandHandlersGenerator = new UnityGameObjectCommandHandlersGenerator();
            var readerWriterGenerator = new UnityReaderWriterGenerator();

            foreach (var enumType in enumsToGenerate)
            {
                var fileName = Path.ChangeExtension(enumType.name, fileExtension);
                var enumCode = enumGenerator.Generate(enumType, package);
                Content.Add(Path.Combine(relativeOutputPath, fileName), enumCode);
            }

            foreach (var type in typesToGenerate)
            {
                var fileName = Path.ChangeExtension(type.Name, fileExtension);
                var typeCode = typeGenerator.Generate(type, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, fileName), typeCode);
            }

            foreach (var component in componentsToGenerate)
            {
                var componentFileName = Path.ChangeExtension(component.Name, fileExtension);
                var componentCode = blittableComponentGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, componentFileName), componentCode);

                if (component.CommandDefinitions.Count > 0)
                {
                    var commandPayloadsFileName =
                        Path.ChangeExtension($"{component.Name}CommandPayloads", fileExtension);
                    var commandPayloadCode =
                        commandPayloadGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandPayloadsFileName), commandPayloadCode);

                    var commandComponentsFileName =
                        Path.ChangeExtension($"{component.Name}CommandComponents", fileExtension);
                    var commandComponentsCode =
                        commandComponentsGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandComponentsFileName), commandComponentsCode);

                    var commandStorageFileName =
                        Path.ChangeExtension($"{component.Name}CommandStorage", fileExtension);
                    var commandStorageCode = commandStorageGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandStorageFileName), commandStorageCode);
                }

                if (component.EventDefinitions.Count > 0)
                {
                    var eventsFileName = Path.ChangeExtension($"{component.Name}Events", fileExtension);
                    var eventsCode = eventGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, eventsFileName), eventsCode);
                }

                var conversionFileName = Path.ChangeExtension($"{component.Name}Translation", fileExtension);
                var componentTranslationCode = componentConversionGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, conversionFileName), componentTranslationCode);

                var referenceProviderFileName = Path.ChangeExtension($"{component.Name}Providers", fileExtension);
                var referenceProviderTranslationCode =
                    referenceTypeProviderGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, referenceProviderFileName),
                    referenceProviderTranslationCode);

                var gameObjectComponentDispatcherFileName =
                    Path.ChangeExtension($"{component.Name}GameObjectComponentDispatcher", fileExtension);
                var gameObjectComponentDispatcherCode =
                    gameObjectComponentDispatcherGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, gameObjectComponentDispatcherFileName),
                    gameObjectComponentDispatcherCode);

                var readerWriterFileName =
                    Path.ChangeExtension($"{component.Name}ReaderWriter", fileExtension);
                var readerWriterCode =
                    readerWriterGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, readerWriterFileName), readerWriterCode);

                if (component.CommandDefinitions.Count > 0)
                {
                    var monobehaviourCommandHandlerFileName =
                        Path.ChangeExtension($"{component.Name}MonoBehaviourCommandHandlers", fileExtension);
                    var monobehaviourCommandHandlerCode =
                        gameObjectCommandHandlersGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, monobehaviourCommandHandlerFileName),
                        monobehaviourCommandHandlerCode);
                }
            }
        }

        /// <summary>
        ///     Filters out auto-generated types like PositionData from the JSON AST.
        ///     However, we want to keep types that are used as a "data" field in a component.
        /// </summary>
        private List<UnityTypeDefinition> SelectTypesToGenerate(UnitySchemaFile schemaFile)
        {
            var componentDataTypes =
                schemaFile.ComponentDefinitions.Select(component => component.RawDataDefinition);

            // From inspection of the JSON AST you can observe that a type definition is auto-generated if the following
            // conditions are true:
            //     1. The FQN type names are the same .
            //     2. The source references are the same.
            // Using this information, we can effectively filter out auto-generated types.
            var filteredTypes = schemaFile.TypeDefinitions.Where(type => componentDataTypes.All(componentData =>
                type.QualifiedName != componentData.TypeName ||
                !SourceReferenceEquals(type.SourceReference, componentData.sourceReference)));

            return filteredTypes.ToList();
        }

        private bool SourceReferenceEquals(SourceReferenceRaw sourceRef1, SourceReferenceRaw sourceRef2)
        {
            return sourceRef1.column == sourceRef2.column && sourceRef1.line == sourceRef2.line;
        }
    }
}
