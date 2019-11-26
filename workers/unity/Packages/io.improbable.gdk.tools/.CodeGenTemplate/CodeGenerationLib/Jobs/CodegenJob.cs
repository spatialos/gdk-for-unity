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
        public List<string> InputFiles = new List<string>();
        public List<string> OutputFiles = new List<string>();
        public readonly string OutputDirectory;

        protected Logger logger { get; }

        protected readonly Dictionary<string, string> Content = new Dictionary<string, string>();

        private IFileSystem fileSystem;
        private readonly DetailsStore detailsStore;

        public CodegenJob(string baseOutputDirectory, IFileSystem fileSystem, DetailsStore detailsStore, Logger logger)
        {
            OutputDirectory = baseOutputDirectory;
            this.fileSystem = fileSystem;
            this.detailsStore = detailsStore;
            this.logger = logger;
        }

        public void Clean()
        {
            logger.Info("Cleaning output directories");
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
                    fileSystem.DeleteDirectory(fileInfo.DirectoryPath);
                }
            }
        }

        public void Run()
        {
            logger.Info("Starting code generation job");
            RunImpl();

            logger.Info("Writing generated code to disk");
            foreach (var entry in Content)
            {
                var fileInfo = fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry.Key));

                if (!fileSystem.DirectoryExists(fileInfo.DirectoryPath))
                {
                    fileSystem.CreateDirectory(fileInfo.DirectoryPath);
                }

                logger.Trace("Fixing line endings");
                // Fix up line endings
                var contents = entry.Value
                    .Replace("\r\n", "\n")
                    .Replace("\n", Environment.NewLine);

                logger.Trace($"Writing {fileInfo.CompletePath}");
                fileSystem.WriteToFile(fileInfo.CompletePath, contents);
            }

            logger.Info("Finished code generation job");
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
