using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Improbable.Gdk.CodeGeneration.FileHandling
{
    public class FileSystem : IFileSystem
    {
        public List<IFile> GetFilesInDirectory(string path, string searchPattern, bool recursive)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                var fileInfoList = directoryInfo.GetFiles(searchPattern,
                    recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                var fileList = fileInfoList.Select(fileInfo =>
                        new FileWrapper(fileInfo.FullName, fileInfo.DirectoryName, fileInfo.LastWriteTime)).ToList()
                    .Cast<IFile>();
                return fileList.ToList();
            }

            return new List<IFile>();
        }

        public void WriteToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public string ReadFromFile(string path)
        {
            return File.ReadAllText(path);
        }

        public IFile GetFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);

            return new FileWrapper(fileInfo.FullName, fileInfo.DirectoryName, fileInfo.LastWriteTime);
        }

        public bool DirectoryExists(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Exists;
        }

        public void CreateDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        public void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Argument 'path' cannot be null or empty.");
            }

            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
