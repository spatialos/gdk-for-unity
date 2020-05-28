using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator.GameObjectCreation
{
    public class GameObjectCodegenJob : CodegenJob
    {
        public GameObjectCodegenJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore store)
            : base(options, fileSystem, store)
        {
            const string jobName = nameof(GameObjectCodegenJob);
            Logger.Trace($"Initialising {jobName}.");

            AddInputFiles(store.SchemaFiles);

            var componentsToGenerate = store.Components.Values.ToList();
            AddGenerators(componentsToGenerate, c
                => ($"{c.Name}ComponentReaderWriter.cs", UnityComponentReaderWriterGenerator.Generate));
            Logger.Trace($"Added job targets for {componentsToGenerate.Count} components.");

            var componentsWithCommands = componentsToGenerate.Where(c => c.CommandDetails.Count > 0).ToList();
            AddGenerators(componentsWithCommands, c
                => ($"{c.Name}CommandSenderReceiver.cs", UnityCommandSenderReceiverGenerator.Generate));
            Logger.Trace($"Added job targets for {componentsWithCommands.Count} components with commands.");

            Logger.Trace($"Finished initialising {jobName}.");
        }
    }
}
