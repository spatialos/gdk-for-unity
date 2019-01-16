using System;

namespace Improbable.Gdk.CodeGeneration.FileHandling
{
    public interface IFile
    {
        DateTime LastWriteTime { get; }
        string CompletePath { get; }
        string DirectoryPath { get; }

        bool Exists();
        void Delete();
    }
}
