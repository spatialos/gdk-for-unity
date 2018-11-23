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
        private delegate void PostprocessPackage(string directoryPath);

        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.Error.WriteLine("Usage: <path_to_spatial> <coresdk_version> <path_to_schema_std_lib_directory>");
                Environment.Exit(1);
            }

            var spatialPath = args[0];
            var coreSdkVersion = args[1];
            var schemaStdLibDir = Path.GetFullPath(args[2]);
            var nativeDependenciesPath = Path.GetFullPath("./Assets/Plugins/Improbable/Core");
            var managedDependenciesPath = Path.GetFullPath("./Assets/Plugins/Improbable/Sdk");
            var tempPath = Path.GetFullPath($"./build/CoreSdk/{coreSdkVersion}");

            var packages = new List<Package>
            {
                new Package(tempPath, "worker_sdk", "core-dynamic-x86_64-win32", $"{nativeDependenciesPath}/Windows/x86_64", new List<string> {"include", "CoreSdkDll.lib"}),
                new Package(tempPath, "worker_sdk", "core-dynamic-x86_64-linux", $"{nativeDependenciesPath}/Linux/x86_64", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "core-bundle-x86_64-macos", $"{nativeDependenciesPath}/OSX", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "core-static-fullylinked-arm-ios", $"{nativeDependenciesPath}/iOS/arm", new List<string> {"include", "libCoreSdkStatic.a.pic", "CoreSdkStatic.lib"}, postprocessCallback:PostProcess_iOS_Arm),
                new Package(tempPath, "worker_sdk", "core-static-fullylinked-x86_64-ios", $"{nativeDependenciesPath}/iOS/x86_64", new List<string> {"include", "libCoreSdkStatic.a.pic", "CoreSdkStatic.lib"}, postprocessCallback:PostProcess_iOS_x86_64),
                new Package(tempPath, "worker_sdk", "core-dynamic-arm64-android", $"{nativeDependenciesPath}/Android/arm64", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "core-dynamic-armeabi_v7a-android", $"{nativeDependenciesPath}/Android/armv7", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "core-dynamic-x86-android-android", $"{nativeDependenciesPath}/Android/x86", new List<string> {"include"}),
                new Package(tempPath, "worker_sdk", "csharp-c-interop", $"{managedDependenciesPath}/Common"),
                new Package(tempPath, "worker_sdk", "csharp-c-interop-static", $"{managedDependenciesPath}/iOS"),
                new Package(tempPath, "schema", "standard_library", schemaStdLibDir),
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

                DownloadPackages(packages, spatialPath, coreSdkVersion);

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

                package.PostprocessCallback?.Invoke(package.TargetPath);
            }
        }

        private static void DownloadPackages(IEnumerable<Package> packages, string spatialPath, string coreSdkVersion)
        {
            foreach (var package in packages)
            {
                Console.Out.WriteLine($"Downloading {package.Name} to {package.SourceFile}...");

                File.Delete(package.SourceFile);

                Common.RunRedirected(spatialPath, "--json_output", "package", "retrieve", package.Type, package.Name, coreSdkVersion, $"\"{package.SourceFile}\"");

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

        private static void PostProcess_iOS_Arm(string directoryPath)
        {
            var originalPath = Path.Combine(directoryPath, "libCoreSdkStatic.a");
            var destinationPath = Path.Combine(directoryPath, "libCoreSdkStatic_arm.a");
            File.Move(originalPath, destinationPath);
        }

        private static void PostProcess_iOS_x86_64(string directoryPath)
        {
            var originalPath = Path.Combine(directoryPath, "libCoreSdkStatic.a");
            var destinationPath = Path.Combine(directoryPath, "libCoreSdkStatic_x86_64.a");
            File.Move(originalPath, destinationPath);
        }

        private static void WriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private class Package
        {
            public Package(string tempPath, string type, string name, string targetPath, List<string> cleanPaths = null, OSPlatform? platform = null, PostprocessPackage postprocessCallback = null)
            {
                Type = type;
                Name = name;
                TargetPath = targetPath;
                CleanPaths = cleanPaths ?? new List<string>();
                SourceFile = Path.Combine(tempPath, $"{Name}.zip");
                InstallOnThisPlatform = !platform.HasValue || RuntimeInformation.IsOSPlatform(platform.Value);
                PostprocessCallback = postprocessCallback;
            }

            public string Type { get; }
            public string Name { get; }
            public string SourceFile { get; }
            public string TargetPath { get; }
            public List<string> CleanPaths { get; }
            public bool InstallOnThisPlatform { get; }
            public PostprocessPackage PostprocessCallback { get; }
        }
    }
}
