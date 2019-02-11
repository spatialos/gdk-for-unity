using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     A FileSystem implementation that ignores the existence of ".meta" files when listing directory contents.
    /// </summary>
    class MetaDataCompatibleFileSystem : IFileSystem
    {
        private readonly FileSystem fileSystem = new FileSystem();

        public List<IFile> GetFilesInDirectory(string path, string searchPattern = "*.*", bool recursive = true)
        {
            var files = fileSystem.GetFilesInDirectory(path, searchPattern, recursive);
            return files.Where(f => !f.CompletePath.EndsWith(".meta")).ToList();
        }

        public void WriteToFile(string path, string content)
        {
            fileSystem.WriteToFile(path, content);
        }

        public string ReadFromFile(string path)
        {
            return fileSystem.ReadFromFile(path);
        }

        public IFile GetFileInfo(string path)
        {
            return fileSystem.GetFileInfo(path);
        }

        public bool DirectoryExists(string path)
        {
            return fileSystem.DirectoryExists(path);
        }

        public void CreateDirectory(string path)
        {
            fileSystem.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            fileSystem.DeleteDirectory(path);
        }
    }
}
