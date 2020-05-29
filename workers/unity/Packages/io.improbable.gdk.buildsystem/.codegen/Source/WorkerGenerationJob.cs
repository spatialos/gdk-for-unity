using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Newtonsoft.Json.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public class WorkerGenerationJob : CodegenJob
    {
        private const string WorkerTypeFlag = "+workerType";
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

            var workerTypesToGenerate = ExtractWorkerTypes(CodeGeneratorOptions.Instance.WorkerJsonDirectory);

            Logger.Trace($"Adding job target {WorkerFileName}.");
            AddJobTarget(Path.Combine(relativeEditorPath, WorkerFileName),
                () => UnityWorkerMenuGenerator.Generate(workerTypesToGenerate));

            Logger.Trace($"Adding job target for {BuildSystemFileName}.");
            AddJobTarget(Path.Combine(relativeEditorPath, BuildSystemFileName),
                () => BuildSystemAssemblyGenerator.Generate());

            Logger.Trace($"Adding job target for {WorkerListFileName}.");
            AddJobTarget(Path.Combine(relativeOutputPath, WorkerListFileName),
                () => string.Join(Environment.NewLine, workerTypesToGenerate));

            Logger.Trace($"Finished initialising {jobName}.");
        }

        private List<string> ExtractWorkerTypes(string path)
        {
            Logger.Trace($"Extracting worker types from {path}.");

            var workerTypes = new List<string>();

            var fileNames = Directory.EnumerateFiles(path, "*.json").ToList();
            Logger.Trace($"Found {fileNames.Count} worker json files:\n - {string.Join("\n - ", fileNames)}");

            foreach (var fileName in fileNames)
            {
                Logger.Trace($"Extracting worker type from {fileName}.");
                var text = File.ReadAllText(fileName);
                if (!text.Contains(WorkerTypeFlag))
                {
                    Logger.Warn($"{fileName} does not contain the '{WorkerTypeFlag}' flag.");
                    continue;
                }

                var jsonRep = JObject.Parse(text);
                var arguments = jsonRep.SelectToken("external.default.windows.arguments");
                if (arguments == null)
                {
                    Logger.Warn($"Could not navigate to external > default > windows > arguments in {fileName}.");
                    continue;
                }

                for (var i = 0; i < arguments.Count() - 1; i++)
                {
                    if (!WorkerTypeFlag.Equals(arguments[i].ToString()))
                    {
                        continue;
                    }

                    var workerType = arguments[i + 1].ToString();
                    Logger.Trace($"Adding {workerType} to list of worker types.");
                    workerTypes.Add(workerType);
                }
            }

            Logger.Trace($"Found {workerTypes.Count} worker types:\n - {string.Join("\n - ", workerTypes)}");
            return workerTypes;
        }
    }
}
