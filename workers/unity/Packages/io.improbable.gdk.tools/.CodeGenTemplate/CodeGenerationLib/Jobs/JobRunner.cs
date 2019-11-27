using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class JobRunner
    {
        private IFileSystem fileSystem;
        private Logger logger;

        public JobRunner(IFileSystem fileSystem)
        {
            logger = LogManager.GetCurrentClassLogger();

            this.fileSystem = fileSystem;
        }

        public void Run(params CodegenJob[] jobs)
        {
            logger.Info("Finding dirty jobs");
            PrepareOutputFolders(jobs);

            logger.Info("Running jobs");
            RunJobs(jobs);
        }

        private void PrepareOutputFolders(CodegenJob[] jobs)
        {
            var outputDirectories = jobs.Select(job => job.OutputDirectory).Distinct();

            foreach (var outputDirectory in outputDirectories)
            {
                var relatedJobs = jobs.Where(job => job.OutputDirectory == outputDirectory);

                if (IsOutputDirectoryDirty(relatedJobs, outputDirectory))
                {
                    logger.Trace($"Deleting dirty directory {outputDirectory}");
                    fileSystem.DeleteDirectory(outputDirectory);

                    foreach (var job in relatedJobs)
                    {
                        logger.Info($"Marking {job.GetType()} as dirty");
                        job.MarkAsDirty();
                    }
                }
            }
        }

        private bool IsOutputDirectoryDirty(IEnumerable<CodegenJob> jobs, string outputDir)
        {
            var outputFolderFiles = fileSystem.GetFilesInDirectory(outputDir)
                .Select(file => Path.GetFullPath(file.CompletePath)).ToList();

            var expectedFiles = jobs.SelectMany(job => job.OutputFiles)
                .Select(path => Path.GetFullPath(Path.Combine(outputDir, path))).ToList();

            return outputFolderFiles.Intersect(expectedFiles).Count() != outputFolderFiles.Count;
        }

        private void RunJobs(CodegenJob[] jobs)
        {
            var dirtyJobs = jobs.Where(job => job.IsDirty()).ToList();

            foreach (var job in dirtyJobs)
            {
                job.Run();
            }
        }
    }
}
