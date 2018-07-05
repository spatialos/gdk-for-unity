using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace ImpNuget
{
    public class Program
    {
        private const string PackageFilename = "packages.config";

        private static readonly string CachePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".imp_nuget");

        public static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine(
                    $"A cross-platform helper that restores Nuget packages by reading '{PackageFilename}', which must be in the current working directory.");
                Console.WriteLine(
                    "Once MSBuild 15-level features are widely available on all supported platforms, this utility can be replaced by 'msbuild /t:Restore'.");
                Environment.Exit(0);
            }

            if (!File.Exists(PackageFilename))
            {
                Console.Error.WriteLine($"{PackageFilename} must exist in the current directory");
                Environment.Exit(1);
            }

            try
            {
                var packagesConfig = LoadPackagesConfig();

                foreach (var package in packagesConfig.Packages)
                {
                    var cacheDir = Path.Combine(CachePath, package.Id, package.Version);
                    Directory.CreateDirectory(cacheDir);

                    var cacheFileName = Path.Combine(cacheDir, $"{package.Id}.zip");

                    // As well as being missing, handle the case of a failed write, resulting in an empty file.
                    var fileInfo = new FileInfo(cacheFileName);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                    {
                        DownloadPackage(package, cacheFileName);
                    }

                    var targetDirectory = Path.Combine("packages", $"{package.Id}.{package.Version}");

                    if (!Directory.Exists(targetDirectory))
                    {
                        ZipFile.ExtractToDirectory(cacheFileName, targetDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.Error.WriteLine(ex.InnerException.ToString());
                }

                Environment.Exit(1);
            }
        }

        private static PackagesConfig LoadPackagesConfig()
        {
            var serializer = new XmlSerializer(typeof(PackagesConfig));
            using (var stream = new FileStream("packages.config", FileMode.Open))
            {
                return (PackagesConfig) serializer.Deserialize(stream);
            }
        }

        private static void DownloadPackage(Package package, string cacheFileName)
        {
            Console.Out.WriteLine($"Downloading {package.Id}@{package.Version}");

            var client = new WebClient();
            var data = client.DownloadData(
                $"https://www.nuget.org/api/v2/package/{package.Id}/{package.Version}");

            using (var memoryStream = new MemoryStream(data))
            using (var archive = new ZipArchive(memoryStream))
            {
                UnescapePaths(archive);

                var directoryName = Path.GetDirectoryName(cacheFileName);
                if (string.IsNullOrEmpty(directoryName))
                {
                    throw new InvalidOperationException($"{cacheFileName} has no parent directory");
                }

                Directory.CreateDirectory(directoryName);

                memoryStream.Position = 0;
                using (var fileStream = new FileStream(cacheFileName, FileMode.OpenOrCreate))
                {
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        ///     .nupkg files are zip files, but the filenames are URI-encoded.
        ///     Rewrite the archive with unescaped filenames.
        /// </summary>
        private static void UnescapePaths(ZipArchive zip)
        {
            foreach (var entry in zip.Entries.ToArray())
            {
                var unescaped = Uri.UnescapeDataString(entry.FullName);
                if (unescaped == entry.FullName)
                {
                    continue;
                }

                // The zip interface doesn't allow renaming/moving.                
                var newEntry = zip.CreateEntry(unescaped);
                using (var source = entry.Open())
                using (var dest = newEntry.Open())
                {
                    source.CopyTo(dest);
                }

                entry.Delete();
            }
        }

        [Serializable]
        [XmlRoot("packages", Namespace = "")]
        public class PackagesConfig
        {
            [XmlElement("package")]
            public Package[] Packages { get; set; }
        }

        [Serializable]
        public class Package
        {
            [XmlAttribute("id")]
            public string Id { get; set; }

            [XmlAttribute("version")]
            public string Version { get; set; }
        }
    }
}
