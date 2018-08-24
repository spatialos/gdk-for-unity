using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    [InitializeOnLoad]
    internal static class GenerateCode
    {
        private const string AssetsGeneratedSource = "Assets/Generated/Source";
        private const string FromGdkPackagesDir = "from_gdk_packages";
        private const string SchemaRoot = "../../schema";


        static GenerateCode()
        {
            Generate();
        }

        [MenuItem("Improbable/Generate code")]
        private static void Generate()
        {
            try
            {
                EditorApplication.LockReloadAssemblies();

                DownloadCoreSdk.Download();

                CopySchema(SchemaRoot);

                var projectPath = Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(),
                    ".CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj"));
                var schemaCompilerPath = Path.GetFullPath(Path.Combine(Application.dataPath,
                    $"../build/CoreSdk/{Common.CoreSdkVersion}/schema_compiler/schema_compiler"));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    schemaCompilerPath = Path.ChangeExtension(schemaCompilerPath, ".exe");
                }

                var exitCode = Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--", $"--schema-path=\"{SchemaRoot}\"",
                    "--schema-path=../../build/dependencies/schema/standard_library",
                    "--json-dir=build/ImprobableJson",
                    $"--native-output-dir={AssetsGeneratedSource}",
                    $"--schema-compiler-path=\"{schemaCompilerPath}\"");

                if (exitCode != 0)
                {
                    Debug.LogError("Failed to generate code.");
                }

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }
        }

        private static void CopySchema(string schemaRoot)
        {
            try
            {
                CleanDestination(schemaRoot);

                var packages = Common.GetManifestDependencies().Where(kv => kv.Value.StartsWith("file:"))
                    .ToDictionary(kv => kv.Key, RemoveFilePrefix)
                    .ToDictionary(kv => kv.Key, kv => Path.Combine("Packages", kv.Value));
                var schemaSources = packages.Where(SchemaPathExists)
                    .ToDictionary(kv => kv.Key, GetSchemaPath);

                foreach (var source in schemaSources)
                {
                    CopySchemaFiles(schemaRoot, source);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        private static void CopySchemaFiles(string schemaRoot, KeyValuePair<string, string> source)
        {
            var files = Directory.GetFiles(source.Value, "*.schema", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativeFilePath = file.Replace(source.Value, string.Empty).TrimStart(Path.DirectorySeparatorChar);
                var to = Path.Combine(schemaRoot, FromGdkPackagesDir, source.Key, relativeFilePath);

                var directoryName = Path.GetDirectoryName(to);
                if (!string.IsNullOrEmpty(directoryName))
                {
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                }

                File.Copy(file, to);
            }
        }

        private static bool SchemaPathExists(KeyValuePair<string, string> arg)
        {
            return Directory.Exists(GetSchemaPath(arg));
        }

        private static string GetSchemaPath(KeyValuePair<string, string> arg)
        {
            return Path.GetFullPath(Path.Combine(arg.Value, "Schema"));
        }

        private static string RemoveFilePrefix(KeyValuePair<string, string> kv)
        {
            return kv.Value.Replace("file:", string.Empty);
        }


        private static void CleanDestination(string schemaDirectory)
        {
            var destination = Path.Combine(schemaDirectory, FromGdkPackagesDir);
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
        }
    }
}
