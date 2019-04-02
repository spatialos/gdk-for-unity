using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal static class GenerateCode
    {
        private const string CsProjectFile = ".CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj";
        private const string FromGdkPackagesDir = "from_gdk_packages";
        private const string ImprobableJsonDir = "build/ImprobableJson";

        private static string SchemaWarningMessage =
            "// ------------------------------------------------------------------------" + Environment.NewLine +
            "// WARNING: DO NOT EDIT." + Environment.NewLine +
            "// Any changes made to this file will be overwritten by the Code Generator." + Environment.NewLine +
            "// ------------------------------------------------------------------------" + Environment.NewLine +
            Environment.NewLine;

        private static readonly string SchemaCompilerRelativePath =
            $"../build/CoreSdk/{Common.CoreSdkVersion}/schema_compiler/schema_compiler";

        private static readonly string StartupCodegenMarkerFile =
            Path.GetFullPath(Path.Combine("Temp", "ImprobableCodegen.marker"));

        /// <summary>
        ///     This regex matches a C# compile error or warning log.
        ///     It captures the following components:
        ///     File that caused the issue
        ///     Line and column in the file
        ///     Log type, warning or error
        ///     CS error code
        ///     Message
        /// </summary>
        /// Example: Generated\Templates\UnityCommandManagerGenerator.tt(11,9): warning CS0219: The variable 'profilingEnd' is assigned but its value is never used [D:\gdk-for-unity\workers\unity\Packages\com.improbable.gdk.tools\.CodeGenerator\GdkCodeGenerator\GdkCodeGenerator.csproj]
        private static readonly Regex dotnetRegex = new Regex(
            @"(?<file>[\w\\\.]+)\((?<line>\d+),(?<col>\d+)\): (?<type>\w+) (?<code>\w+): (?<message>[\s\S]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Ensure that code is generated on editor startup.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (!CanGenerateOnLoad())
            {
                return;
            }

            Generate();
        }

        private static bool CanGenerateOnLoad()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // Don't generate code when entering PlayMode.
                return false;
            }

            return !File.Exists(StartupCodegenMarkerFile);
        }

        [MenuItem("SpatialOS/Generate code", false, MenuPriorities.GenerateCodePriority)]
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

                CopySchema();

                var projectPath = Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(),
                    CsProjectFile));

                var schemaCompilerPath =
                    Path.GetFullPath(Path.Combine(Application.dataPath, SchemaCompilerRelativePath));

                var workerJsonPath =
                    Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        schemaCompilerPath = Path.ChangeExtension(schemaCompilerPath, ".exe");
                        break;
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.OSXEditor:
                        RedirectedProcess.Command("chmod")
                            .WithArgs("+x", $"\"{schemaCompilerPath}\"")
                            .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                            .Run();
                        break;
                    default:
                        throw new PlatformNotSupportedException(
                            $"The {Application.platform} platform does not support code generation.");
                }

                using (new ShowProgressBarScope("Generating code..."))
                {
                    var exitCode = RedirectedProcess.Command(Common.DotNetBinary)
                        .WithArgs(ConstructArgs(projectPath, schemaCompilerPath, workerJsonPath))
                        .RedirectOutputOptions(OutputRedirectBehaviour.None)
                        .AddErrorProcessing(Debug.LogError)
                        .AddOutputProcessing(ProcessStdOut)
                        .Run();

                    if (exitCode != 0)
                    {
                        if (!Application.isBatchMode)
                        {
                            EditorApplication.delayCall += () =>
                            {
                                EditorUtility.DisplayDialog("Generate Code",
                                    "Failed to generate code from schema.\nPlease view the console for errors.",
                                    "Close");
                            };
                        }
                    }
                    else
                    {
                        File.WriteAllText(StartupCodegenMarkerFile, string.Empty);
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

        private static void ProcessStdOut(string output)
        {
            var match = dotnetRegex.Match(output);
            if (match.Success)
            {
                switch (match.Groups["type"].Value)
                {
                    case "warning":
                        Debug.LogWarning(output);
                        break;
                    case "error":
                        Debug.LogError(output);
                        break;
                    default:
                        Debug.Log(output);
                        break;
                }
            }
            else
            {
                Debug.Log(output);
            }
        }

        private static string[] ConstructArgs(string projectPath, string schemaCompilerPath, string workerJsonPath)
        {
            var baseArgs = new List<string>
            {
                "run",
                "-p",
                $"\"{projectPath}\"",
                "--",
                $"--json-dir=\"{ImprobableJsonDir}\"",
                $"--schema-compiler-path=\"{schemaCompilerPath}\"",
                $"--worker-json-dir=\"{workerJsonPath}\""
            };

            var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();

            baseArgs.Add($"--native-output-dir=\"{toolsConfig.CodegenOutputDir}\"");
            baseArgs.Add($"--schema-path=\"{toolsConfig.SchemaStdLibDir}\"");

            foreach (var schemaSourceDir in toolsConfig.SchemaSourceDirs)
            {
                baseArgs.Add($"--schema-path=\"{schemaSourceDir}\"");
            }

            return baseArgs.ToArray();
        }

        [MenuItem("SpatialOS/Generate code (force)", false, MenuPriorities.GenerateCodeForcePriority)]
        private static void ForceGenerateMenu()
        {
            Debug.Log("Generating code (forced rebuild)...");
            EditorApplication.delayCall += ForceGenerate;
        }

        private static void ForceGenerate()
        {
            var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
            if (Directory.Exists(toolsConfig.CodegenOutputDir))
            {
                Directory.Delete(toolsConfig.CodegenOutputDir, true);
            }

            Generate();
        }

        private static void CopySchema()
        {
            try
            {
                var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
                // Safe as we validate there is at least one entry.
                var schemaRoot = toolsConfig.SchemaSourceDirs[0];
                CleanDestination(schemaRoot);

                // Get all packages we depend on
                var request = Client.List(offlineMode: true);
                while (!request.IsCompleted)
                {
                    // Wait for the request to complete
                }

                var schemaSources = request.Result.ToDictionary(package => package.name,
                        package => Path.Combine(package.resolvedPath, "Schema"))
                    .Where(kv => Directory.Exists(kv.Value));

                foreach (var source in schemaSources)
                {
                    CopySchemaFiles(schemaRoot, source.Key, source.Value);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        private static void CopySchemaFiles(string schemaRoot, string packageName, string packageSchemaPath)
        {
            var files = Directory.GetFiles(packageSchemaPath, "*.schema", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativeFilePath =
                    file.Replace(packageSchemaPath, string.Empty).TrimStart(Path.DirectorySeparatorChar);
                var fileCopy = Path.Combine(schemaRoot, FromGdkPackagesDir, packageName, relativeFilePath);

                var directoryName = Path.GetDirectoryName(fileCopy);
                if (!string.IsNullOrEmpty(directoryName))
                {
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                }

                using (var output = File.OpenWrite(fileCopy))
                using (var outputText = new StreamWriter(output))
                {
                    outputText.Write(SchemaWarningMessage);
                    using (var inputText = File.OpenRead(file))
                    {
                        inputText.CopyTo(outputText.BaseStream);
                    }
                }
            }
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
