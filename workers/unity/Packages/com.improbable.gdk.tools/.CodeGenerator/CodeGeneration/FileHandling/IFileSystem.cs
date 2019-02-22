using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.FileHandling
{
    public interface IFileSystem
    {
        List<IFile> GetFilesInDirectory(string path, string searchPattern = "*.*", bool recursive = true);
        void WriteToFile(string path, string content);
        string ReadFromFile(string path);

        IFile GetFileInfo(string path);

        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        void DeleteDirectory(string path);
    }
}
