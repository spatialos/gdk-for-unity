using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator.Core
{
    public class CoreCodegenJob : CodegenJob
    {
        public CoreCodegenJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore store)
            : base(options, fileSystem, store)
        {
            const string jobName = nameof(CoreCodegenJob);
            Logger.Trace($"Initialising {jobName}.");

            AddInputFiles(store.SchemaFiles);

            // Types
            Logger.Trace("Gathering nested types.");
            var allNestedTypes = store.Types
                .SelectMany(kv => store.GetNestedTypes(kv.Key))
                .ToHashSet();

            Logger.Trace("Gathering types details.");
            var typesToGenerate = store.Types
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList();

            Logger.Trace("Adding job targets for types.");
            AddGenerators(typesToGenerate, t => ($"{t.Name}.cs", UnityTypeGenerator.Generate));

            Logger.Trace($"Added job targets for {typesToGenerate.Count} types.");

            // Enums
            Logger.Trace("Gathering enum details.");
            var enumsToGenerate = store.Enums
                .Where(kv => !allNestedTypes.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList();

            Logger.Trace("Adding job targets for enums.");
            AddGenerators(enumsToGenerate,
                e => ($"{e.Name}.cs", UnityEnumGenerator.Generate));

            Logger.Trace($"Added job targets for {enumsToGenerate.Count} enums.");

            // Components
            Logger.Trace("Gathering component details.");
            var componentsToGenerate = store.Components.Values.ToList();

            Logger.Trace("Adding job targets for components.");
            AddGenerators(componentsToGenerate,
                c => ($"{c.Name}.cs", UnityComponentDataGenerator.Generate),
                c => ($"{c.Name}UpdateSender.cs", UnityComponentSenderGenerator.Generate),
                c => ($"{c.Name}EcsViewManager.cs", UnityEcsViewManagerGenerator.Generate),
                c => ($"{c.Name}ComponentDiffStorage.cs", ComponentDiffStorageGenerator.Generate),
                c => ($"{c.Name}ComponentDiffDeserializer.cs", ComponentDiffDeserializerGenerator.Generate),
                c => ($"{c.Name}Metaclass.cs", MetaclassGenerator.Generate));

            Logger.Trace("Adding job targets for commands.");
            AddGenerators(componentsToGenerate.Where(c => c.CommandDetails.Count > 0),
                c => ($"{c.Name}CommandPayloads.cs", UnityCommandPayloadGenerator.Generate),
                c => ($"{c.Name}CommandDiffDeserializer.cs", CommandDiffDeserializerGenerator.Generate),
                c => ($"{c.Name}CommandDiffStorage.cs", CommandDiffStorageGenerator.Generate),
                c => ($"{c.Name}CommandMetaDataStorage.cs", CommandMetaDataStorageGenerator.Generate));

            Logger.Trace("Adding job targets for events.");
            AddGenerators(componentsToGenerate.Where(c => c.EventDetails.Count > 0),
                c => ($"{c.Name}Events.cs", UnityEventGenerator.Generate));

            Logger.Trace($"Added job targets for {componentsToGenerate.Count} components.");

            Logger.Trace($"Finished initialising {jobName}.");
        }
    }
}
