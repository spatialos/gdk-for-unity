using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class IgnoreCodegenJobAttribute : Attribute
    {
    }

    public abstract class CodegenJob
    {
        public IEnumerable<string> ExpectedOutputFiles
            => jobTargets.Select(target => target.FilePath);

        private readonly List<string> expectedInputFiles = new List<string>();
        public readonly string OutputDirectory;

        protected readonly Logger Logger;

        private readonly List<JobTarget> jobTargets = new List<JobTarget>();

        private readonly IFileSystem fileSystem;
        private readonly DetailsStore detailsStore;

        protected CodegenJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore detailsStore)
        {
            Logger = LogManager.GetLogger(GetType().FullName);

            OutputDirectory = options.OutputDir;
            this.fileSystem = fileSystem;
            this.detailsStore = detailsStore;
            isDirtyOverride = options.IsForce;
        }

        protected void AddInputFiles(IEnumerable<string> inputFilePaths)
        {
            foreach (var inputFilePath in inputFilePaths)
            {
                AddInputFile(inputFilePath);
            }
        }

        protected void AddInputFile(string inputFilePath)
        {
            expectedInputFiles.Add(inputFilePath);
            Logger.Trace($"Added input file: {inputFilePath}.");
        }

        protected void AddJobTarget(string filePath, Func<CodeWriter.CodeWriter> generateFunc)
        {
            jobTargets.Add(new JobTarget(Path.Combine(OutputDirectory, filePath), generateFunc));
        }

        protected void AddJobTarget(string filePath, Func<string> generateFunc)
        {
            jobTargets.Add(new JobTarget(Path.Combine(OutputDirectory, filePath), generateFunc));
        }

        protected delegate TGenOutput GenerateDelegate<in TDetails, out TGenOutput>(TDetails details)
            where TDetails : GeneratorInputDetails;

        protected delegate (string relativeFilePath, GenerateDelegate<TDetails, TGenOutput> generateFunc) GeneratorSetupDelegate<TDetails, TGenOutput>(TDetails details)
            where TDetails : GeneratorInputDetails;

        protected void AddGenerators<TDetails>(IEnumerable<TDetails> details, params GeneratorSetupDelegate<TDetails, CodeWriter.CodeWriter>[] generatorSetupDelegates)
            where TDetails : GeneratorInputDetails
        {
            jobTargets.AddRange(details.SelectMany(detail =>
            {
                return generatorSetupDelegates.Select(generatorSetup =>
                {
                    var (filePath, generate) = generatorSetup(detail);
                    return new JobTarget(Path.Combine(OutputDirectory, detail.NamespacePath, filePath), () => generate(detail));
                });
            }));
        }

        protected void AddGenerators<TDetails>(IEnumerable<TDetails> details, params GeneratorSetupDelegate<TDetails, string>[] generatorSetupDelegates)
            where TDetails : GeneratorInputDetails
        {
            jobTargets.AddRange(details.SelectMany(detail =>
            {
                return generatorSetupDelegates.Select(generatorSetup =>
                {
                    var (filePath, generate) = generatorSetup(detail);
                    return new JobTarget(Path.Combine(OutputDirectory, detail.NamespacePath, filePath), () => generate(detail));
                });
            }));
        }

        protected void AddGenerators<TDetails>(string relativeOutputDir, IEnumerable<TDetails> details, params GeneratorSetupDelegate<TDetails, CodeWriter.CodeWriter>[] generatorSetupDelegates)
            where TDetails : GeneratorInputDetails
        {
            jobTargets.AddRange(details.SelectMany(detail =>
            {
                return generatorSetupDelegates.Select(generatorSetup =>
                {
                    var (filePath, generate) = generatorSetup(detail);
                    return new JobTarget(Path.Combine(OutputDirectory, relativeOutputDir, detail.NamespacePath, filePath), () => generate(detail));
                });
            }));
        }

        protected void AddGenerators<TDetails>(string relativeOutputDir, IEnumerable<TDetails> details, params GeneratorSetupDelegate<TDetails, string>[] generatorSetupDelegates)
            where TDetails : GeneratorInputDetails
        {
            jobTargets.AddRange(details.SelectMany(detail =>
            {
                return generatorSetupDelegates.Select(generatorSetup =>
                {
                    var (filePath, generate) = generatorSetup(detail);
                    return new JobTarget(Path.Combine(OutputDirectory, relativeOutputDir, detail.NamespacePath, filePath), () => generate(detail));
                });
            }));
        }

        public void Clean()
        {
            var numRemovedDirectories = 0;

            foreach (var filePath in ExpectedOutputFiles)
            {
                var fileInfo = fileSystem.GetFileInfo(filePath);

                if (fileInfo.Exists())
                {
                    fileInfo.Delete();
                }

                var remainingFilesInFolder = fileSystem.GetFilesInDirectory(fileInfo.DirectoryPath);
                if (remainingFilesInFolder.Count != 0)
                {
                    continue;
                }

                Logger.Trace($"Deleting output directory {fileInfo.DirectoryPath}.");
                fileSystem.DeleteDirectory(fileInfo.DirectoryPath);
                numRemovedDirectories++;
            }

            Logger.Info($"Directories cleaned: {numRemovedDirectories}.");
        }

        public void Run()
        {
            var jobType = GetType();
            Logger.Info($"Starting {jobType}.");

            // Run generators for all targets
            foreach (var jobTarget in jobTargets)
            {
                Logger.Trace($"Generating {jobTarget.FilePath}.");
                jobTarget.Generate();
            }

            // Write generated code to disk
            Logger.Trace("Writing generated code to disk.");
            foreach (var jobTarget in jobTargets)
            {
                var fileInfo = fileSystem.GetFileInfo(jobTarget.FilePath);

                if (!fileSystem.DirectoryExists(fileInfo.DirectoryPath))
                {
                    Logger.Trace($"Creating output directory {fileInfo.DirectoryPath}.");
                    fileSystem.CreateDirectory(fileInfo.DirectoryPath);
                }

                fileSystem.WriteToFile(fileInfo.CompletePath, jobTarget.Format());
                Logger.Trace($"Written {fileInfo.CompletePath}.");
            }

            Logger.Info($"Finished {jobType}. Files written: {jobTargets.Count}.");
        }

        private bool isDirtyOverride;

        public void MarkAsDirty()
        {
            isDirtyOverride = true;
        }

        public bool IsDirty()
        {
            if (isDirtyOverride)
            {
                return true;
            }

            var schemaFiles = expectedInputFiles
                .Select(file => detailsStore.FileTree.GetFullPathForRelativeSchema(file))
                .Select(path => fileSystem.GetFileInfo(path))
                .ToList();

            var existingFiles = ExpectedOutputFiles.Select(fileSystem.GetFileInfo).ToList();

            if (schemaFiles.Count == 0 || existingFiles.Count == 0)
            {
                return true;
            }

            //Ensure that all expected output files exist
            if (existingFiles.Any(file => !file.Exists()))
            {
                return true;
            }

            var sortedSchemaFileInfo = schemaFiles.OrderByDescending(item => item.LastWriteTime).ToList();
            var sortedExistingFiles = existingFiles.OrderByDescending(item => item.LastWriteTime).ToList();

            return sortedSchemaFileInfo.First().LastWriteTime > sortedExistingFiles.First().LastWriteTime;
        }
    }
}
