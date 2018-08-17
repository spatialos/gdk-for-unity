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
            var tempPath = Path.GetFullPath($"./CoreSdk/{coreSdkVersion}");
            var projectPath = Path.GetFullPath("../../");

            var Packages = new List<Package>
            {
                new Package
                {
                    Type = "worker_sdk",
                    Name = "c-dynamic-x86_64-msvc_mt-win32",
                    TargetPath = $"{nativeDependenciesPath}/Windows/x86_64",
                    CleanPaths = new List<string> {"include", "worker.lib"}
                },
                new Package
                {
                    Type = "worker_sdk",
                    Name = "c-dynamic-x86_64-gcc_libstdcpp-linux",
                    TargetPath = $"{nativeDependenciesPath}/Linux/x86_64",
                    CleanPaths = new List<string> {"include"}
                },
                new Package
                {
                    Type = "worker_sdk",
                    Name = "c-dynamic-x86_64-clang_libcpp-macos",
                    TargetPath = $"{nativeDependenciesPath}/OSX",
                    CleanPaths = new List<string> {"include"}
                },
                new Package
                {
                    Type = "worker_sdk",
                    Name = "csharp_core",
                    TargetPath = $"{managedDependenciesPath}"
                },
                new Package
                {
                    Type = "schema",
                    Name = "standard_library",
                    TargetPath = $"{projectPath}/build/dependencies/schema/standard_library"
                },
                new Package
                {
                    Type = "tools",
                    Name = "schema_compiler-x86_64-win32",
                    TargetPath = $"{tempPath}/schema_compiler",
                    Platform = OSPlatform.Windows
                },
                new Package
                {
                    Type = "tools",
                    Name = "schema_compiler-x86_64-macos",
                    TargetPath = $"{tempPath}/schema_compiler",
                    Platform = OSPlatform.OSX
                }
            };

            try
            {
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

                foreach (var package in Packages.Where(p =>
                    !p.Platform.HasValue || RuntimeInformation.IsOSPlatform(p.Platform.Value)))
                {
                    var tempFilePath = Path.Combine(tempPath, $"{package.Name}.zip");
                    var newVersion = !File.Exists(tempFilePath);
                    if (newVersion)
                    {
                        Console.Out.WriteLine($"Downloading {package.Name} to {tempFilePath}...");

                        Common.RunRedirected("spatial",
                            new[] {"package", "retrieve", package.Type, package.Name, coreSdkVersion, tempFilePath});
                    }

                    if (!newVersion && Directory.Exists(package.TargetPath) &&
                        Directory.GetFileSystemEntries(package.TargetPath).Any())
                        continue;

                    Common.EnsureDirectoryEmpty(package.TargetPath);
                    ZipFile.ExtractToDirectory(tempFilePath, package.TargetPath);

                    // Clean out undesirable files (.h, .lib, etc.)
                    if (package.CleanPaths != null)
                        foreach (var path in package.CleanPaths)
                        {
                            var cleanPath = Path.Combine(package.TargetPath, path);
                            if (File.Exists(cleanPath))
                                File.Delete(cleanPath);
                            else if (Directory.Exists(cleanPath)) Directory.Delete(cleanPath, true);
                        }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e);
            }
        }

        private class Package
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string TargetPath { get; set; }
            public OSPlatform? Platform { get; set; }
            public List<string> CleanPaths { get; set; }
        }
    }
}
