using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class JobRunner
    {
        private readonly IFileSystem fileSystem;
        private readonly Logger logger;

        public JobRunner(IFileSystem fileSystem)
        {
            logger = LogManager.GetCurrentClassLogger();

            this.fileSystem = fileSystem;
        }

        public void Run(params CodegenJob[] jobs)
        {
            logger.Info("Finding dirty jobs.");
            var dirtyJobs = PrepareOutputFolders(jobs);

            var dirtyJobList = string.Join("\n - ", dirtyJobs.Select(job => job.GetType()));
            logger.Info($"Found {dirtyJobs.Count} dirty jobs:\n - {dirtyJobList}");

            logger.Info("Running jobs.");
            foreach (var dirtyJob in dirtyJobs)
            {
                dirtyJob.Run();
            }
        }

        private List<CodegenJob> PrepareOutputFolders(CodegenJob[] jobs)
        {
            var outputDirectories = jobs.Select(job => job.OutputDirectory).Distinct();

            foreach (var outputDirectory in outputDirectories)
            {
                var relatedJobs = jobs.Where(job => job.OutputDirectory == outputDirectory);

                if (!IsOutputDirectoryDirty(relatedJobs, outputDirectory))
                {
                    continue;
                }

                logger.Trace($"Deleting dirty directory {outputDirectory}.");
                fileSystem.DeleteDirectory(outputDirectory);

                foreach (var job in relatedJobs)
                {
                    logger.Trace($"Marking {job.GetType()} as dirty.");
                    job.MarkAsDirty();
                }
            }

            return jobs.Where(job => job.IsDirty()).ToList();
        }

        private bool IsOutputDirectoryDirty(IEnumerable<CodegenJob> jobs, string outputDir)
        {
            var outputFolderFiles = fileSystem.GetFilesInDirectory(outputDir)
                .Select(file => Path.GetFullPath(file.CompletePath)).ToList();

            var expectedFiles = jobs.SelectMany(job => job.ExpectedOutputFiles);

            return outputFolderFiles.Intersect(expectedFiles).Count() != outputFolderFiles.Count;
        }
    }
}
