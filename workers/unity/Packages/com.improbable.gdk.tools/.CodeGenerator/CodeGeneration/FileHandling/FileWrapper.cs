using System;
using System.IO;

namespace Improbable.Gdk.CodeGeneration.FileHandling
{
    public class FileWrapper : IFile
    {
        public FileWrapper(string completePath, string directoryPath, DateTime lastWriteTimeStamp)
        {
            CompletePath = completePath;
            LastWriteTime = lastWriteTimeStamp;
            DirectoryPath = directoryPath;
        }

        public DateTime LastWriteTime { get; private set; }
        public string CompletePath { get; private set; }

        public string DirectoryPath { get; private set; }

        public bool Exists()
        {
            var fileInfo = new FileInfo(CompletePath);
            return fileInfo.Exists;
        }

        public void Delete()
        {
            var fileInfo = new FileInfo(CompletePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
