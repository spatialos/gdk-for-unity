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

            // Filter out the data types like PositionData or TransformData. We don't want to generate these.
            typesToGenerate = schemaFile.TypeDefinitions.Where(type =>
                schemaFile.ComponentDefinitions.Select(component => component.DataDefinition.typeDefinition.Name)
                    .All(componentDataName => componentDataName != type.Name)).ToList();

            foreach (var unityTypeDefinition in typesToGenerate)
            {
                var fileName = Path.ChangeExtension(unityTypeDefinition.Name, fileExtension);
                OutputFiles.Add(Path.Combine(relativeOutputPath, fileName));
            }

            componentsToGenerate = schemaFile.ComponentDefinitions;
            foreach (var unityComponentDefinition in componentsToGenerate)
            {
                var fileName = Path.ChangeExtension(unityComponentDefinition.Name, fileExtension);
                OutputFiles.Add(Path.Combine(relativeOutputPath, fileName));

                fileName = Path.ChangeExtension(unityComponentDefinition.Name + "Translation", fileExtension);
                OutputFiles.Add(Path.Combine(relativeOutputPath, fileName));
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
            var blittableComponentGenerator = new UnityComponentDataGenerator();
            var nonBlittableComponentGenerator = new UnityComponentGenerator();
            var blittableComponentConversionGenerator = new UnityComponentDataConversionGenerator();
            var nonBlittableComponentConversionGenerator = new UnityComponentConversionGenerator();

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
                var componentCode = component.IsBlittable
                    ? blittableComponentGenerator.Generate(component, package)
                    : nonBlittableComponentGenerator.Generate(component, package);
                Content.Add(Path.Combine(relativeOutputPath, componentFileName), componentCode);

                if (component.CommandDefinitions.Count > 0)
                {
                    var commandPayloadsFileName =
                        Path.ChangeExtension($"{component.Name}CommandPayloads", fileExtension);
                    var commandPayloadCode =
                        commandPayloadGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, commandPayloadsFileName), commandPayloadCode);
                }

                if (component.EventDefinitions.Count > 0)
                {
                    var eventsFileName = Path.ChangeExtension($"{component.Name}Events", fileExtension);
                    var eventsCode = eventGenerator.Generate(component, package);
                    Content.Add(Path.Combine(relativeOutputPath, eventsFileName), eventsCode);
                }

                var conversionFileName = Path.ChangeExtension($"{component.Name}Translation", fileExtension);
                var componentTranslationCode = component.IsBlittable
                    ? blittableComponentConversionGenerator.Generate(component, package, enumSet)
                    : nonBlittableComponentConversionGenerator.Generate(component, package, enumSet);
                Content.Add(Path.Combine(relativeOutputPath, conversionFileName), componentTranslationCode);
            }
        }
    }
}
