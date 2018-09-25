using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Jobs;

namespace Improbable.Gdk.CodeGenerator
{
    public class WorkerGenerationJob : CodegenJob
    {
        private readonly List<string> workerTypesToGenerate;
        private readonly string relativeOutputPath = Path.Combine("improbable", "buildsystem");
        private readonly string relativeEditorPath = Path.Combine("improbable", "buildsystem", "Editor");
        private readonly string workerFileName = Path.ChangeExtension("WorkerMenu", ".cs");
        private readonly string buildSystemFileName = Path.ChangeExtension("Improbable.Gdk.Generated.BuildSystem", ".asmdef");

        public WorkerGenerationJob(string outputDir, CodeGeneratorOptions options, IFileSystem fileSystem) : base(
            outputDir, fileSystem)
        {
            InputFiles = new List<string>();
            OutputFiles = new List<string>();
            
            workerTypesToGenerate = ExtractWorkerTypes(options.WorkerJsonDirectory);

            OutputFiles.Add(Path.Combine(relativeEditorPath, "Editor", workerFileName));
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
        }

        private List<string> ExtractWorkerTypes(string path)
        {
            var fileNames = Directory.EnumerateFiles(path, "spatialos.*.worker.json");
            return fileNames.Select(GetWorkerType).ToList();
        }

        private string GetWorkerType(string fileName)
        {
            return Path.GetFileName(fileName).Split('.')[1];
        }

    }
}
