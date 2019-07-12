using System;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;

namespace CodeGeneration.Tests.FileHandling
{
    public class MockFile : IFile
    {
        public DateTime LastWriteTime { get; }
        public string CompletePath { get; }
        public string DirectoryPath { get; }

        public string FileName { get; }

        public bool WasDeleted { get; private set; }

        private readonly bool shouldExist;

        public MockFile(string filePath, DateTime lastWriteTime, bool shouldExist)
        {
            LastWriteTime = lastWriteTime;
            CompletePath = filePath;
            DirectoryPath = string.Join("/", filePath.Split("/").SkipLast(1));
            FileName = filePath.Split("/").Last();

            this.shouldExist = shouldExist;
        }

        public bool Exists()
        {
            return shouldExist;
        }

        public void Delete()
        {
            WasDeleted = true;
        }
    }
}
