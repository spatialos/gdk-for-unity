using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CopySchema
{
    class Program
    {
        public class PackageInfo
        {
            public readonly string Name;
            public readonly string Path;

            public PackageInfo(string name, string path)
            {
                Name = name;
                Path = path;
            }

            public override string ToString()
            {
                return $"[{Name}]: {Path}";
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (PackageInfo) obj;
                return other.Name == Name && other.Path == Path;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() * (31 + Path.GetHashCode());
            }
        }

        public const string FromGdkPackagesDir = "from_gdk_packages";

        static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine("A helper that copies schema files contained in Unity packages referenced in a manifest.json");
                Console.WriteLine("Example Usage:");
                Console.WriteLine("   CopySchema.exe Packages/manifest.json ../../schema");
                Environment.Exit(0);
            }

            try
            {
                ParseArguments(args, out var manifestFile, out var packagesRoot, out var schemaRoot);

                dynamic manifest = ParseManifest(manifestFile);

                CleanDestination(Path.Combine(schemaRoot));

                var packages = GetPackageInfos(manifest, packagesRoot);
                
                var schemaFiles = FindSchemaFiles(packages);
                
                var filesToCopy = GetFilesToCopy(schemaFiles, schemaRoot);

                var filesCopied = CopyFiles(filesToCopy);
                Console.WriteLine($"Copied {filesCopied} files");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        public static int CopyFiles(Dictionary<string, string> filesToCopy)
        {
            var count = 0;
            foreach (var file in filesToCopy)
            {
                var from = file.Key;
                var to = file.Value;
                if (!Directory.Exists(Path.GetDirectoryName(to)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(to));
                }

                File.Copy(from, to);
                count++;
            }

            return count;
        }

        public static Dictionary<string, string> GetFilesToCopy(List<KeyValuePair<PackageInfo, string>> schemaFiles,
            string schemaRoot)
        {
            var results = new Dictionary<string, string>();
            foreach (var file in schemaFiles)
            {
                var relativeFilePath = file.Value.Replace(Path.Combine(file.Key.Path, "Schema"), string.Empty).TrimStart(Path.DirectorySeparatorChar);
                var newPath = Path.Combine(schemaRoot, FromGdkPackagesDir, file.Key.Name, relativeFilePath);
                results.Add(file.Value, newPath);
            }

            return results;
        }

        public static List<KeyValuePair<PackageInfo, string>> FindSchemaFiles(List<PackageInfo> packages)
        {
            var results = new List<KeyValuePair<PackageInfo, string>>();
            foreach (var package in packages)
            {
                var searchPath = Path.GetFullPath(Path.Combine(package.Path, "Schema"));
                if (!Directory.Exists(searchPath))
                {
                    continue;
                }

                foreach (var file in Directory.GetFiles(searchPath, "*.schema", SearchOption.AllDirectories))
                {
                    results.Add(new KeyValuePair<PackageInfo, string>(package, Path.GetFullPath(file)));
                }
            }

            return results;
        }

        public static List<PackageInfo> GetPackageInfos(dynamic manifest, string packagesRoot)
        {
            var results = new List<PackageInfo>();
            JObject dependencies = manifest.dependencies;
            foreach (var dep in dependencies.Properties())
            {
                var value = dep.Value.ToString();
                if (value.StartsWith("file:"))
                {
                    var fullPath = Path.GetFullPath(Path.Combine(packagesRoot, value.Replace("file:", string.Empty)));
                    results.Add(new PackageInfo(dep.Name, fullPath));
                }
            }

            return results;
        }

        public static void ParseArguments(string[] args, out string manifestFile, out string packagesRoot, out string schemaRoot)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException($"Expected 2 arguments: MANIFEST DESTINATION");
            }

            manifestFile = args[0];
            schemaRoot = args[1].TrimEnd(Path.DirectorySeparatorChar);

            if (!File.Exists(manifestFile))
            {
                throw new ArgumentException($"Manifest file not found: {manifestFile}");
            }

            packagesRoot = Path.GetDirectoryName(manifestFile);

            if (!Directory.Exists(schemaRoot))
            {
                throw new ArgumentException($"Destination directory does not exist: ${schemaRoot}");
            }

            schemaRoot = Path.GetFullPath(schemaRoot);
        }

        public static object ParseManifest(string manifestFile)
        {
            try
            {
                return JsonConvert.DeserializeObject(File.ReadAllText(manifestFile));
            }
            catch (JsonException e)
            {
                throw new ArgumentException($"Failed to parse manifest file: {e.Message}");
            }
        }

        public static void CleanDestination(string schemaDirectory)
        {
            var destination = Path.Combine(schemaDirectory, FromGdkPackagesDir);
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
        }
    }
}
