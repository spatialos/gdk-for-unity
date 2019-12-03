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
        private readonly List<string> workerTypesToGenerate;
        private readonly string workerTypeFlag = "+workerType";
        private readonly string workerFileName = "WorkerMenu.cs";
        private readonly string workerListFileName = "WorkerMenu.txt";
        private readonly string buildSystemFileName = "Improbable.Gdk.Generated.BuildSystem.asmdef";
        private readonly string relativeOutputPath = Path.Combine("improbable", "buildsystem");
        private readonly string relativeEditorPath = Path.Combine("improbable", "buildsystem", "Editor");


        public WorkerGenerationJob(string baseOutputDir, IFileSystem fileSystem, DetailsStore detailsStore)
            : base(baseOutputDir, fileSystem, detailsStore)
        {
            var jobName = nameof(WorkerGenerationJob);
            logger.Info($"Initialising {jobName}");

            workerTypesToGenerate = ExtractWorkerTypes(CodeGeneratorOptions.Instance.WorkerJsonDirectory);

            var outputFilePaths = new List<string>
            {
                Path.Combine(relativeEditorPath, workerFileName),
                Path.Combine(relativeEditorPath, workerListFileName),
                Path.Combine(relativeOutputPath, buildSystemFileName)
            };

            foreach (var filePath in outputFilePaths)
            {
                AddOutputFile(filePath);
            }
            logger.Info($"Added {outputFilePaths.Count} job output files");

            logger.Info($"Finished initialising {jobName}");
        }

        protected override void RunImpl()
        {
            logger.Info($"Generating {workerFileName}");
            var unityWorkerMenuGenerator = new UnityWorkerMenuGenerator();
            var workerCode = unityWorkerMenuGenerator.Generate(workerTypesToGenerate);
            AddContent(Path.Combine(relativeEditorPath, workerFileName), workerCode);

            logger.Info($"Generating {buildSystemFileName}");
            var buildSystemAssemblyGenerator = new BuildSystemAssemblyGenerator();
            var assemblyCode = buildSystemAssemblyGenerator.Generate();
            AddContent(Path.Combine(relativeOutputPath, buildSystemFileName), assemblyCode);

            logger.Info($"Generating {workerListFileName}");
            AddContent(Path.Combine(relativeEditorPath, workerListFileName), string.Join(Environment.NewLine, workerTypesToGenerate));
        }

        private List<string> ExtractWorkerTypes(string path)
        {
            logger.Info($"Extracting worker types from {path}");

            var workerTypes = new List<string>();

            var fileNames = Directory.EnumerateFiles(path, "*.json").ToList();
            logger.Trace($"Found {fileNames.Count()} worker json files: {string.Join(", ", fileNames)}");

            foreach (var fileName in fileNames)
            {
                logger.Trace($"Extracting worker type from {fileName}");
                var text = File.ReadAllText(fileName);
                if (!text.Contains(workerTypeFlag))
                {
                    logger.Warn($"{fileName} does not contain the '{workerTypeFlag}' flag");
                    continue;
                }

                var jsonRep = JObject.Parse(text);
                var arguments = jsonRep.SelectToken("external.default.windows.arguments");
                if (arguments == null)
                {
                    logger.Warn($"Could not navigate to external > default > windows > arguments in {fileName}");
                    continue;
                }

                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (workerTypeFlag.Equals(arguments[i].ToString()))
                    {
                        var workerType = arguments[i + 1].ToString();
                        logger.Trace($"Adding {workerType} to list of worker types");
                        workerTypes.Add(workerType);
                    }
                }
            }

            logger.Info($"Found {workerTypes.Count} worker types: {string.Join(", ", workerTypes)}");
            return workerTypes;
        }
    }
}
