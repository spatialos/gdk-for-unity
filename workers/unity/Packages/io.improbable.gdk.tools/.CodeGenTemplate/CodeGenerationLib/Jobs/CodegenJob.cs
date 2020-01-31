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
        public IReadOnlyList<string> InputFiles => inputFiles;
        public IReadOnlyList<string> OutputFiles => outputFiles;

        private readonly List<string> inputFiles = new List<string>();
        private readonly List<string> outputFiles = new List<string>();
        public readonly string OutputDirectory;

        protected Logger logger { get; }

        private readonly Dictionary<string, string> Content = new Dictionary<string, string>();

        private IFileSystem fileSystem;
        private readonly DetailsStore detailsStore;

        public CodegenJob(string baseOutputDirectory, IFileSystem fileSystem, DetailsStore detailsStore, bool force)
        {
            logger = LogManager.GetLogger(GetType().FullName);

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
            inputFiles.Add(inputFilePath);
            logger.Trace($"Added input file: {inputFilePath}.");
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
            outputFiles.Add(outputFilePath);
            logger.Trace($"Added output file: {outputFilePath}.");
        }

        protected void AddContent(string filePath, string fileContents)
        {
            Content.Add(filePath, fileContents);
            logger.Trace($"Added generated content for {filePath}.");
        }

        public void Clean()
        {
            var numRemovedDirectories = 0;

            foreach (var entry in OutputFiles)
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
                    logger.Info($"Deleting output directory {fileInfo.DirectoryPath}.");
                    fileSystem.DeleteDirectory(fileInfo.DirectoryPath);
                    numRemovedDirectories++;
                }
            }

            logger.Info($"Directories cleaned: {numRemovedDirectories}.");
        }

        public void Run()
        {
            var jobType = GetType();
            logger.Info($"Starting {jobType}.");

            RunImpl();

            logger.Info("Writing generated code to disk.");
            foreach (var entry in Content)
            {
                var fileInfo = fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry.Key));

                if (!fileSystem.DirectoryExists(fileInfo.DirectoryPath))
                {
                    logger.Trace($"Creating output directory {fileInfo.DirectoryPath}.");
                    fileSystem.CreateDirectory(fileInfo.DirectoryPath);
                }

                logger.Trace("Fixing line endings.");
                // Fix up line endings
                var contents = entry.Value
                    .Replace("\r\n", "\n")
                    .Replace("\n", Environment.NewLine);

                fileSystem.WriteToFile(fileInfo.CompletePath, contents);
                logger.Trace($"Written {fileInfo.CompletePath}.");
            }

            logger.Info($"Files written: {Content.Count}.");

            logger.Info($"Finished {jobType}.");
        }

        public bool IsDirty()
        {
            if (isDirtyOverride)
            {
                return true;
            }

            var schemaFiles = InputFiles
                .Select(file => detailsStore.FileTree.GetFullPathForRelativeSchema(file))
                .Select(path => fileSystem.GetFileInfo(path))
                .ToList();

            var existingFiles = OutputFiles
                .Select(entry => fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry)))
                .ToList();

            if (schemaFiles.Count == 0 || existingFiles.Count == 0)
            {
                return true;
            }

            //Ensure that all expected output files exist
            foreach (var file in existingFiles)
            {
                if (!file.Exists())
                {
                    return true;
                }
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
