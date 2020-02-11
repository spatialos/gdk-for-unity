using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Improbable.Gdk.CodeGeneration.Utils;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class IgnoreCodegenJobAttribute : Attribute
    {
    }

    public abstract class CodegenJob
    {
        public IEnumerable<string> ExpectedOutputFiles => expectedOutputFiles.Select(filePath => Path.Combine(OutputDirectory, filePath));
        private readonly List<string> expectedOutputFiles = new List<string>();

        private readonly List<string> expectedInputFiles = new List<string>();
        public readonly string OutputDirectory;

        protected readonly Logger Logger;

        private readonly Dictionary<string, string> content = new Dictionary<string, string>();

        private readonly IFileSystem fileSystem;
        private readonly DetailsStore detailsStore;

        protected CodegenJob(string baseOutputDirectory, IFileSystem fileSystem, DetailsStore detailsStore, bool force)
        {
            Logger = LogManager.GetLogger(GetType().FullName);

            OutputDirectory = baseOutputDirectory;
            this.fileSystem = fileSystem;
            this.detailsStore = detailsStore;
            isDirtyOverride = force;
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

        protected void AddOutputFiles(IEnumerable<string> outputFilePaths)
        {
            foreach (var outputFilePath in outputFilePaths)
            {
                AddOutputFile(outputFilePath);
            }
        }

        protected void AddOutputFile(string outputFilePath)
        {
            expectedOutputFiles.Add(outputFilePath);
            Logger.Trace($"Added output file: {outputFilePath}.");
        }

        protected void AddContent(string filePath, string fileContents)
        {
            content.Add(filePath, fileContents);
            Logger.Trace($"Added generated content for {filePath}.");
        }

        public void Clean()
        {
            var numRemovedDirectories = 0;

            foreach (var entry in ExpectedOutputFiles)
            {
                var path = Path.Combine(OutputDirectory, entry);
                var fileInfo = fileSystem.GetFileInfo(path);

                if (fileInfo.Exists())
                {
                    fileInfo.Delete();
                }

                var remainingFilesInFolder = fileSystem.GetFilesInDirectory(fileInfo.DirectoryPath);
                if (remainingFilesInFolder.Count == 0)
                {
                    Logger.Info($"Deleting output directory {fileInfo.DirectoryPath}.");
                    fileSystem.DeleteDirectory(fileInfo.DirectoryPath);
                    numRemovedDirectories++;
                }
            }

            Logger.Info($"Directories cleaned: {numRemovedDirectories}.");
        }

        public void Run()
        {
            var jobType = GetType();
            Logger.Info($"Starting {jobType}.");

            RunImpl();

            Logger.Info("Writing generated code to disk.");
            foreach (var (filePath, fileContents) in content)
            {
                var fileInfo = fileSystem.GetFileInfo(Path.Combine(OutputDirectory, filePath));

                if (!fileSystem.DirectoryExists(fileInfo.DirectoryPath))
                {
                    Logger.Trace($"Creating output directory {fileInfo.DirectoryPath}.");
                    fileSystem.CreateDirectory(fileInfo.DirectoryPath);
                }

                Logger.Trace("Fixing line endings.");
                // Fix up line endings
                var contents = fileContents
                    .Replace("\r\n", "\n")
                    .Replace("\n", Environment.NewLine);

                fileSystem.WriteToFile(fileInfo.CompletePath, contents);
                Logger.Trace($"Written {fileInfo.CompletePath}.");
            }

            Logger.Info($"Files written: {content.Count}.");

            Logger.Info($"Finished {jobType}.");
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

            var existingFiles = ExpectedOutputFiles
                .Select(entry => fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry)))
                .ToList();

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

        public void MarkAsDirty()
        {
            isDirtyOverride = true;
        }

        protected abstract void RunImpl();
        private bool isDirtyOverride;

        protected struct GenerationTarget<T>
        {
            public readonly T Content;
            public readonly string Package;
            public readonly string OutputPath;

            public GenerationTarget(T content, string package)
            {
                Content = content;
                Package = Formatting.CapitaliseQualifiedNameParts(package);
                OutputPath = Formatting.GetNamespacePath(package);
            }
        }
    }
}
