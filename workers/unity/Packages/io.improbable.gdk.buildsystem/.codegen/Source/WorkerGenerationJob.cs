using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
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

        public WorkerGenerationJob(string outputDir, CodeGeneratorOptions options, IFileSystem fileSystem) : base(
            outputDir, fileSystem)
        {
            InputFiles = new List<string>();
            OutputFiles = new List<string>();

            workerTypesToGenerate = ExtractWorkerTypes(options.WorkerJsonDirectory);

            OutputFiles.Add(Path.Combine(relativeEditorPath, workerFileName));
            OutputFiles.Add(Path.Combine(relativeEditorPath, workerListFileName));
            OutputFiles.Add(Path.Combine(relativeOutputPath, buildSystemFileName));
        }

        protected override void RunImpl()
        {
            var unityWorkerMenuGenerator = new UnityWorkerMenuGenerator();
            var workerCode = unityWorkerMenuGenerator.Generate(workerTypesToGenerate);
            Content.Add(Path.Combine(relativeEditorPath, workerFileName), workerCode);

            var buildSystemAssemblyGenerator = new BuildSystemAssemblyGenerator();
            var assemblyCode = buildSystemAssemblyGenerator.Generate();
            Content.Add(Path.Combine(relativeOutputPath, buildSystemFileName), assemblyCode);

            Content.Add(Path.Combine(relativeEditorPath, workerListFileName), string.Join(Environment.NewLine, workerTypesToGenerate));
        }

        private List<string> ExtractWorkerTypes(string path)
        {
            var workerTypes = new List<string>();
            var fileNames = Directory.EnumerateFiles(path, "*.json");
            foreach (var fileName in fileNames)
            {
                string text = File.ReadAllText(fileName);
                if (!text.Contains(workerTypeFlag))
                {
                    Console.WriteLine($"{fileName} does not contain the following flag: {workerTypeFlag}");
                    continue;
                }

                var jsonRep = JObject.Parse(text);
                var arguments = jsonRep.SelectToken("external.default.windows.arguments");
                if (arguments == null)
                {
                    Console.WriteLine($"Could not navigate to external > default > windows > arguments in {fileName}");
                    continue;
                }

                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (workerTypeFlag.Equals(arguments[i].ToString()))
                    {
                        workerTypes.Add(arguments[i + 1].ToString());
                    }
                }
            }

            return workerTypes;
        }
    }
}
