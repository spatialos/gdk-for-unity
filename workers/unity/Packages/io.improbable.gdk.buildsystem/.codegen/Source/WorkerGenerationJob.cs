using System;
using System.IO;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public class WorkerGenerationJob : CodegenJob
    {
        private const string WorkerFileName = "WorkerMenu.cs";
        private const string WorkerListFileName = "WorkerMenu.txt";
        private const string BuildSystemFileName = "Improbable.Gdk.Generated.BuildSystem.asmdef";
        private readonly string relativeOutputPath = Path.Combine("improbable", "buildsystem");
        private readonly string relativeEditorPath = Path.Combine("improbable", "buildsystem", "Editor");

        public WorkerGenerationJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore detailsStore)
            : base(options.AsEditor(), fileSystem, detailsStore)
        {
            const string jobName = nameof(WorkerGenerationJob);
            Logger.Trace($"Initialising {jobName}.");

            Logger.Trace($"Adding job target {WorkerFileName}.");
            AddJobTarget(Path.Combine(relativeEditorPath, WorkerFileName),
                () => UnityWorkerMenuGenerator.Generate(detailsStore.WorkerTypes));

            Logger.Trace($"Adding job target for {BuildSystemFileName}.");
            AddJobTarget(Path.Combine(relativeEditorPath, BuildSystemFileName),
                () => BuildSystemAssemblyGenerator.Generate());

            Logger.Trace($"Adding job target for {WorkerListFileName}.");
            AddJobTarget(Path.Combine(relativeOutputPath, WorkerListFileName),
                () => string.Join(Environment.NewLine, detailsStore.WorkerTypes));

            Logger.Trace($"Finished initialising {jobName}.");
        }
    }
}
