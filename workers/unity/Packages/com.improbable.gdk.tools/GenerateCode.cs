using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    [InitializeOnLoad]
    internal static class GenerateCode
    {
        private const string AssetsGeneratedSourceDir = "Assets/Generated/Source";
        private const string CsProjectFile = ".CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj";
        private const string FromGdkPackagesDir = "from_gdk_packages";
        private const string ImprobableJsonDir = "build/ImprobableJson";
        private const string SchemaRootDir = "../../schema";
        private const string SchemaStandardLibraryDir = "../../build/dependencies/schema/standard_library";

        private const int GenerateCodePriority = 38;
        private const int GenerateCodeForcePriority = 39;

        private static readonly string SchemaCompilerRelativePath =
            $"../build/CoreSdk/{Common.CoreSdkVersion}/schema_compiler/schema_compiler";

        /// <summary>
        ///     Ensure that code is generated on editor startup.
        /// </summary>
        static GenerateCode()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // Don't generate code when entering PlayMode.
                return;
            }

            Generate();
        }

        [MenuItem("SpatialOS/Generate code", false, GenerateCodePriority)]
        private static void GenerateMenu()
        {
            Debug.Log("Generating code...");
            EditorApplication.delayCall += Generate;
        }

        private static void Generate()
        {
            try
            {
                EditorApplication.LockReloadAssemblies();

                // Ensure that all dependencies are in place.
                if (DownloadCoreSdk.TryDownload() == DownloadResult.Error)
                {
                    return;
                }

                CopySchema(SchemaRootDir);

                var projectPath = Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(),
                    CsProjectFile));

                var schemaCompilerPath =
                    Path.GetFullPath(Path.Combine(Application.dataPath, SchemaCompilerRelativePath));

                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        schemaCompilerPath = Path.ChangeExtension(schemaCompilerPath, ".exe");
                        break;
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.OSXEditor:
                        // Ensure the schema compiler is executable.
                        var _ = RedirectedProcess.Run("chmod", "+x", schemaCompilerPath);
                        break;
                    default:
                        throw new PlatformNotSupportedException(
                            $"The {Application.platform} platform does not support code generation.");
                }

                using (new ShowProgressBarScope("Generating code..."))
                {
                    var exitCode = RedirectedProcess.Run(Common.DotNetBinary, "run", "-p", $"\"{projectPath}\"", "--",
                        $"--schema-path=\"{SchemaRootDir}\"",
                        $"--schema-path={SchemaStandardLibraryDir}",
                        $"--json-dir={ImprobableJsonDir}",
                        $"--native-output-dir={AssetsGeneratedSourceDir}",
                        $"--schema-compiler-path=\"{schemaCompilerPath}\"");

                    if (exitCode != 0)
                    {
                        Debug.LogError("Failed to generate code.");
                    }
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

        [MenuItem("SpatialOS/Generate code (force)", false, GenerateCodeForcePriority)]
        private static void ForceGenerateMenu()
        {
            Debug.Log("Generating code (forced rebuild)...");
            EditorApplication.delayCall += ForceGenerate;
        }

        private static void ForceGenerate()
        {
            if (Directory.Exists(AssetsGeneratedSourceDir))
            {
                Directory.Delete(AssetsGeneratedSourceDir, true);
            }

            Generate();
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
