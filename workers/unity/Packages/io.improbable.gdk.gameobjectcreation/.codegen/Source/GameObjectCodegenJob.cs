using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator.GameObjectCreation
{
    public class GameObjectCodegenJob : CodegenJob
    {
        public GameObjectCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store, bool force)
            : base(outputDir, fileSystem, store, force)
        {
            const string jobName = nameof(GameObjectCodegenJob);
            Logger.Info($"Initialising {jobName}.");

            AddInputFiles(store.SchemaFiles);

            var componentsToGenerate = store.Components.Values.ToList();
            AddGenerators(componentsToGenerate, c
                => ($"{c.Name}ComponentReaderWriter.cs", UnityComponentReaderWriterGenerator.Generate));
            Logger.Info($"Added job targets for {componentsToGenerate.Count} components.");

            var componentsWithCommands = componentsToGenerate.Where(c => c.CommandDetails.Count > 0).ToList();
            AddGenerators(componentsWithCommands, c
                => ($"{c.Name}CommandSenderReceiver.cs", UnityCommandSenderReceiverGenerator.Generate));
            Logger.Info($"Added job targets for {componentsWithCommands.Count} components with commands.");

            Logger.Info($"Finished initialising {jobName}.");
        }

        protected override void RunImpl()
        {
            // base CodegenJob runs jobs
        }
    }
}
