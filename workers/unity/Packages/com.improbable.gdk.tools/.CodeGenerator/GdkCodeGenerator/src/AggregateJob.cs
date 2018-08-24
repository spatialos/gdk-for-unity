using System.Collections.Generic;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Jobs;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     A job that wrapps the SingleGenerationJob to enable use with the JobRunner.
    ///     This should be refactored away.
    /// </summary>
    public class AggregateJob : CodegenJob
    {
        private readonly List<ICodegenJob> jobs = new List<ICodegenJob>();

        public AggregateJob(IFileSystem fileSystem, CodeGeneratorOptions options,
            UnitySchemaProcessor schemaProcessor,
            HashSet<string> globalEnumSet) : base(options.NativeOutputDirectory, fileSystem)
        {
            InputFiles = new List<string>();
            OutputFiles = new List<string>();

            foreach (var processedSchema in schemaProcessor.ProcessedSchemaFiles)
            {
                var job = new SingleGenerationJob(options.NativeOutputDirectory, processedSchema, fileSystem,
                    globalEnumSet);
                jobs.Add(job);

                foreach (var file in job.InputFiles)
                {
                    InputFiles.Add(file);
                }

                foreach (var file in job.OutputFiles)
                {
                    OutputFiles.Add(file);
                }
            }
        }

        protected override void RunImpl()
        {
            foreach (var job in jobs)
            {
                job.Run();
            }
        }
    }
}
