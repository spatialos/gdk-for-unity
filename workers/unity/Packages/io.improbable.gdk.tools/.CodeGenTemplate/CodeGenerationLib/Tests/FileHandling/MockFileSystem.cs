using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;

namespace CodeGeneration.Tests.FileHandling
{
    public class MockFileSystem : IFileSystem
    {
        private readonly FileTreeNode rootNode = new FileTreeNode();

        public List<IFile> GetFilesInDirectory(string path, string searchPattern = "*.*", bool recursive = true)
        {
            if (searchPattern != "*.*")
            {
                throw new NotImplementedException("Search patterns other than '*.*' are not supported.");
            }

            var pathParts = path.Split("/");
            var currentNode = rootNode;

            foreach (var part in pathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    throw new ArgumentException($"No directory at: {path} exists.");
                }
            }

            var files = new List<MockFile>();
            if (recursive)
            {
                var nodesToCheck = new Queue<FileTreeNode>();
                nodesToCheck.Enqueue(currentNode);

                while (nodesToCheck.Count > 0)
                {
                    var node = nodesToCheck.Dequeue();
                    files.AddRange(node.Files.Values.ToList());

                    foreach (var dir in node.Directories)
                    {
                        nodesToCheck.Enqueue(dir.Value);
                    }
                }
            }
            else
            {
                files = currentNode.Files.Values.ToList();
            }

            return files.Cast<IFile>().ToList();
        }

        public void WriteToFile(string path, string content)
        {
            // Intentionally not implementing this. No actual data backing the files.
            throw new NotImplementedException();
        }

        public string ReadFromFile(string path)
        {
            // Intentionally not implementing this. No actual data backing the files.
            throw new NotImplementedException();
        }

        public IFile GetFileInfo(string path)
        {
            var pathParts = path.Split("/").SkipLast(1);
            var currentNode = rootNode;

            foreach (var part in pathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    throw new ArgumentException($"No file found at: {path}");
                }
            }

            if (!currentNode.Files.TryGetValue(path.Split("/").Last(), out var file))
            {
                file = new MockFile(path, DateTime.Today, false);
            }

            return file;
        }

        public bool DirectoryExists(string path)
        {
            var pathParts = path.Split("/");
            var currentNode = rootNode;

            foreach (var part in pathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void CreateDirectory(string path)
        {
            var pathParts = path.Split("/");
            var currentNode = rootNode;

            foreach (var part in pathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    currentNode.AddDirectory(part);
                    currentNode = currentNode.Directories[part];
                }
            }
        }

        public void DeleteDirectory(string path)
        {
            var parentPathParts = path.Split("/").SkipLast(1);
            var currentNode = rootNode;

            foreach (var part in parentPathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    throw new ArgumentException($"No directory at: {path} exists.");
                }
            }

            // currentNode is now the parent directory of the target directory.
            currentNode.Directories.Remove(path.Split("/").Last());
        }

        public void AddFile(string path, DateTime lastWriteTime)
        {
            var parentPathParts = path.Split("/").SkipLast(1);
            var currentNode = rootNode;

            foreach (var part in parentPathParts)
            {
                if (currentNode.Directories.TryGetValue(part, out var directory))
                {
                    currentNode = directory;
                }
                else
                {
                    directory = new FileTreeNode();
                    currentNode.Directories[part] = directory;
                    currentNode = directory;
                }
            }

            var file = new MockFile(path, lastWriteTime, true);
            currentNode.AddFile(file);
        }

        private class FileTreeNode
        {
            public Dictionary<string, FileTreeNode> Directories = new Dictionary<string, FileTreeNode>();
            public Dictionary<string, MockFile> Files = new Dictionary<string, MockFile>();

            public void AddDirectory(string dirName)
            {
                Directories.Add(dirName, new FileTreeNode());
            }

            public void AddFile(MockFile file)
            {
                Files.Add(file.FileName, file);
            }
        }
    }
}
