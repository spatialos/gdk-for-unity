using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class JobRunner
    {
        private IFileSystem fileSystem;

        public JobRunner(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Run(params CodegenJob[] jobs)
        {
            PrepareOutputFolders(jobs);
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
                    fileSystem.DeleteDirectory(outputDirectory);
                    foreach (var job in relatedJobs)
                    {
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
