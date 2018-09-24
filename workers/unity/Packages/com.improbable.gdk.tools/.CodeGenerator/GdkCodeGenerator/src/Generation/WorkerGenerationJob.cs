using System.Collections.Generic;
using System.IO;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Jobs;

namespace Improbable.Gdk.CodeGenerator
{
    public class WorkerGenerationJob : CodegenJob
    {
        private readonly string relativeOutputPath = "improbable";
        private readonly string package = "Improbable";
        private readonly List<string> workerTypesToGenerate;


        private const string fileExtension = ".cs";

        public WorkerGenerationJob(string outputDir, IFileSystem fileSystem, List<string> workerTypes) : base(
            outputDir, fileSystem)
        {
            InputFiles = new List<string>();
            OutputFiles = new List<string>();
            
            workerTypesToGenerate = workerTypes;
            
            var workerFileName = Path.ChangeExtension("WorkerMenu", fileExtension);
            OutputFiles.Add(Path.Combine(relativeOutputPath, workerFileName));
        }

        protected override void RunImpl()
        {
            var unityWorkerMenuGenerator = new UnityWorkerMenuGenerator();

            var workerFileName = Path.ChangeExtension("WorkerMenu", fileExtension);
            var workerCode = unityWorkerMenuGenerator.Generate(workerTypesToGenerate, package);
            Content.Add(Path.Combine(relativeOutputPath, workerFileName), workerCode);
        }
    }
}
