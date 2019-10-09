using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.FileHandling
{
    public class FileTree : IFileTree
    {
        private Dictionary<string, HashSet<string>> lookup;

        public FileTree(List<string> schemaDirs)
        {
            lookup = schemaDirs
                .AsParallel()
                .ToDictionary(
                    schemaDir => schemaDir,
                    schemaDir => Directory.GetFiles(schemaDir, "*.schema", SearchOption.AllDirectories)
                        .Select(path => path.Substring(schemaDir.Length + 1).Replace('\\', '/'))
                        .ToHashSet());
        }

        public string GetFullPathForRelativeSchema(string canonicalPath)
        {
            var root = lookup.First(entry => entry.Value.Contains(canonicalPath)).Key;
            return Path.Combine(root, canonicalPath);
        }
    }
}
