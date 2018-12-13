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
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                // Do not generate BOM at the start of the file
                var encoding = new UTF8Encoding(false);
                using (var streamWriter = new StreamWriter(fileStream, encoding))
                {
                    streamWriter.Write(content);
                }
            }
        }

        public string ReadFromFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
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
            if (!string.IsNullOrEmpty(path))
            {
                var directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    directoryInfo.Delete(true);
                }
            }
        }
    }
}
