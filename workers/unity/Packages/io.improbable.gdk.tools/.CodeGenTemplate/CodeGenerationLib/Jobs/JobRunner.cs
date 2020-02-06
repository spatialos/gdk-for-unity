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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public JobRunner(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Run(params CodegenJob[] jobs)
        {
            Logger.Info("Finding dirty jobs.");
            var dirtyJobs = PrepareOutputFolders(jobs);

            var dirtyJobList = string.Join("\n - ", dirtyJobs.Select(job => job.GetType()));
            Logger.Info($"Found {dirtyJobs.Count} dirty jobs:\n - {dirtyJobList}");

            Logger.Info("Running jobs.");
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
                var relatedJobs = jobs.Where(job => job.OutputDirectory == outputDirectory).ToList();

                if (IsOutputDirectoryDirty(relatedJobs, outputDirectory))
                {
                    Logger.Trace($"Deleting dirty directory {outputDirectory}.");
                    fileSystem.DeleteDirectory(outputDirectory);

                    foreach (var job in relatedJobs)
                    {
                        Logger.Trace($"Marking {job.GetType()} as dirty.");
                        job.MarkAsDirty();
                    }
                }
            }

            return jobs.Where(job => job.IsDirty()).ToList();
        }

        private bool IsOutputDirectoryDirty(IEnumerable<CodegenJob> jobs, string outputDir)
        {
            var outputFolderFiles = fileSystem.GetFilesInDirectory(outputDir)
                .Select(file => Path.GetFullPath(file.CompletePath)).ToList();

            var expectedFiles = jobs.SelectMany(job => job.OutputFiles)
                .Select(path => Path.GetFullPath(Path.Combine(outputDir, path))).ToList();

            return outputFolderFiles.Intersect(expectedFiles).Count() != outputFolderFiles.Count;
        }
    }
}
