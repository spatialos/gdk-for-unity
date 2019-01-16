using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public abstract class CodegenJob
    {
        public List<string> InputFiles { get; protected set; }
        public List<string> OutputFiles { get; protected set; }
        public string OutputDirectory { get; private set; }

        protected Dictionary<string, string> Content { get; set; }

        private IFileSystem fileSystem;

        public CodegenJob(string outputDirectory, IFileSystem fileSystem)
        {
            OutputDirectory = outputDirectory;
            this.fileSystem = fileSystem;

            Content = new Dictionary<string, string>();
        }

        public void Clean()
        {
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
            RunImpl();

            foreach (var entry in Content)
            {
                var fileInfo = fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry.Key));

                if (!fileSystem.DirectoryExists(fileInfo.DirectoryPath))
                {
                    fileSystem.CreateDirectory(fileInfo.DirectoryPath);
                }

                fileSystem.WriteToFile(fileInfo.CompletePath, entry.Value);
            }
        }

        public bool IsDirty()
        {
            if (isDirtyOverride)
            {
                return true;
            }

            var schemaFiles = InputFiles.Select(entry => fileSystem.GetFileInfo(entry)).ToList();
            var existingFiles = OutputFiles.Select(entry => fileSystem.GetFileInfo(Path.Combine(OutputDirectory, entry))).ToList();

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
    }
}
