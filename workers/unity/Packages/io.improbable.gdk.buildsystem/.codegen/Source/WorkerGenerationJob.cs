using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Newtonsoft.Json.Linq;
using NLog;

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


        public WorkerGenerationJob(string baseOutputDir, IFileSystem fileSystem, DetailsStore detailsStore) : base(baseOutputDir, fileSystem, detailsStore, LogManager.GetCurrentClassLogger())
        {
            logger.Info("Initialising WorkerGenerationJob");

            logger.Info($"Extracting worker types from {CodeGeneratorOptions.Instance.WorkerJsonDirectory}");
            workerTypesToGenerate = ExtractWorkerTypes(CodeGeneratorOptions.Instance.WorkerJsonDirectory);

            logger.Info("Setting job outputs");
            OutputFiles.Add(Path.Combine(relativeEditorPath, workerFileName));
            OutputFiles.Add(Path.Combine(relativeEditorPath, workerListFileName));
            OutputFiles.Add(Path.Combine(relativeOutputPath, buildSystemFileName));

            logger.Info("Finished initialising WorkerGenerationJob");
        }

        protected override void RunImpl()
        {
            logger.Info($"Generating {workerFileName}");
            var unityWorkerMenuGenerator = new UnityWorkerMenuGenerator();
            var workerCode = unityWorkerMenuGenerator.Generate(workerTypesToGenerate);
            Content.Add(Path.Combine(relativeEditorPath, workerFileName), workerCode);
            logger.Info($"Finished generating {workerFileName}");

            logger.Info($"Generating {buildSystemFileName}");
            var buildSystemAssemblyGenerator = new BuildSystemAssemblyGenerator();
            var assemblyCode = buildSystemAssemblyGenerator.Generate();
            Content.Add(Path.Combine(relativeOutputPath, buildSystemFileName), assemblyCode);
            logger.Info($"Finished generating {buildSystemFileName}");

            logger.Info($"Generating {workerListFileName}");
            Content.Add(Path.Combine(relativeEditorPath, workerListFileName), string.Join(Environment.NewLine, workerTypesToGenerate));
            logger.Info($"Finished generating {workerListFileName}");
        }

        private List<string> ExtractWorkerTypes(string path)
        {
            var workerTypes = new List<string>();

            logger.Trace("Finding all worker json files");
            var fileNames = Directory.EnumerateFiles(path, "*.json");

            foreach (var fileName in fileNames)
            {
                logger.Trace($"Extracting worker type from {fileName}");
                var text = File.ReadAllText(fileName);
                if (!text.Contains(workerTypeFlag))
                {
                    logger.Warn($"{fileName} does not contain the following flag: {workerTypeFlag}");
                    continue;
                }

                logger.Trace("Parsing to JObject");
                var jsonRep = JObject.Parse(text);
                var arguments = jsonRep.SelectToken("external.default.windows.arguments");
                if (arguments == null)
                {
                    logger.Warn($"Could not navigate to external > default > windows > arguments in {fileName}");
                    continue;
                }

                logger.Trace("Finding worker type in list of arguments");
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (workerTypeFlag.Equals(arguments[i].ToString()))
                    {
                        var workerType = arguments[i + 1].ToString();
                        logger.Trace($"Adding {workerType} to list of worker type");
                        workerTypes.Add(workerType);
                    }
                }
            }

            logger.Trace($"Found {workerTypes.Count} worker types");
            return workerTypes;
        }
    }
}
