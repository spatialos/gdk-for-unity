using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

namespace Improbable
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Specify the version of the CoreSdk to download.");
                Environment.Exit(1);
            }

            var coreSdkVersion = args[0];
            var nativeDependenciesPath = Path.GetFullPath("./Assets/Plugins/Improbable/Core");
            var managedDependenciesPath = Path.GetFullPath("./Assets/Plugins/Improbable/Sdk");
            var tempPath = Path.GetFullPath($"./build/CoreSdk/{coreSdkVersion}");
            var spatialProjectPath = Path.GetFullPath("../../");

            var packages = new List<Package>
            {
                new Package(tempPath, "worker_sdk", "c-dynamic-x86_64-msvc_mt-win32", $"{nativeDependenciesPath}/Windows/x86_64", new List<string> {"include", "worker.lib"}),
                new Package(tempPath, "worker_sdk", "c-dynamic-x86_64-gcc_libstdcpp-linux", $"{nativeDependenciesPath}/Linux/x86_64", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "c-bundle-x86_64-clang_libcpp-macos", $"{nativeDependenciesPath}/OSX", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "csharp_core", $"{managedDependenciesPath}/OSX"),
                new Package(tempPath, "schema", "standard_library", $"{spatialProjectPath}/build/dependencies/schema/standard_library"),
                new Package(tempPath, "tools", "schema_compiler-x86_64-win32", $"{tempPath}/schema_compiler", null, OSPlatform.Windows),
                new Package(tempPath, "tools", "schema_compiler-x86_64-macos", $"{tempPath}/schema_compiler", null, OSPlatform.OSX),
                new Package(tempPath, "tools", "schema_compiler-x86_64-linux", $"{tempPath}/schema_compiler", null, OSPlatform.Linux),
            }.Where(p => p.InstallOnThisPlatform).ToList();

            try
            {
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                DownloadPackages(packages, coreSdkVersion);

                ExtractPackages(packages);
            }
            catch (Exception e)
            {
                WriteException(e);
                Environment.ExitCode = 1;
            }
        }

        private static void ExtractPackages(IEnumerable<Package> packages)
        {
            foreach (var package in packages)
            {
                ZipFile.ExtractToDirectory(package.SourceFile, package.TargetPath, true);

                // Clean out undesirable files (.h, .lib, etc.)
                foreach (var path in package.CleanPaths)
                {
                    var cleanPath = Path.Combine(package.TargetPath, path);
                    if (File.Exists(cleanPath))
                    {
                        File.Delete(cleanPath);
                    }
                    else if (Directory.Exists(cleanPath))
                    {
                        Directory.Delete(cleanPath, true);
                    }
                }
            }
        }

        private static void DownloadPackages(IEnumerable<Package> packages, string coreSdkVersion)
        {
            var toDownload = packages.Where(p => !File.Exists(p.SourceFile)).ToArray();
            if (!toDownload.Any())
            {
                Console.Out.WriteLine("All packages are up to date. Skipping download.");
                return;
            }

            foreach (var package in toDownload)
            {
                Console.Out.WriteLine($"Downloading {package.Name} to {package.SourceFile}...");

                Common.RunRedirected("spatial", "package", "retrieve", package.Type, package.Name, coreSdkVersion, package.SourceFile);

                try
                {
                    // Open the new archive and ensure that it's a valid zip file.
                    using (ZipFile.OpenRead(package.SourceFile))
                    {
                    }

                    Common.EnsureDirectoryEmpty(package.TargetPath);
                }
                catch (Exception e)
                {
                    WriteException(new Exception($"{package.Name} is corrupted, removing.", e));
                    File.Delete(package.SourceFile);
                }
            }
        }

        private static void WriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private class Package
        {
            public Package(string tempPath, string type, string name, string targetPath, List<string> cleanPaths = null, OSPlatform? platform = null)
            {
                Type = type;
                Name = name;
                TargetPath = targetPath;
                CleanPaths = cleanPaths ?? new List<string>();
                SourceFile = Path.Combine(tempPath, $"{Name}.zip");
                InstallOnThisPlatform = !platform.HasValue || RuntimeInformation.IsOSPlatform(platform.Value);
            }

            public string Type { get; }
            public string Name { get; }
            public string SourceFile { get; }
            public string TargetPath { get; }
            public List<string> CleanPaths { get; }
            public bool InstallOnThisPlatform { get; }
        }
    }
}
