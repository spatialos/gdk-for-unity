using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Tools
{
    internal static class GenerateCode
    {
        private const string ImprobableJsonDir = "build/ImprobableJson";
        private const string SchemaPackageDir = ".schema";
        private const string CodegenDir = ".codegen";

        private static readonly string CodegenTemplatePath = Path.Combine(Common.GetThisPackagePath(), ".CodeGenTemplate");
        private static readonly string CodegenExeDirectory = Path.Combine(Application.dataPath, "..", "build", "codegen");
        private static readonly string CodegenExe = Path.Combine(CodegenExeDirectory, "CodeGen", "CodeGen.csproj");

        private static readonly string SchemaCompilerPath = Path.Combine(
            Common.GetPackagePath("io.improbable.worker.sdk"),
            ".schema_compiler/schema_compiler");

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
        /// Example: Generated\Templates\UnityCommandManagerGenerator.tt(11,9): warning CS0219: The variable 'profilingEnd' is assigned but its value is never used [D:\gdk-for-unity\workers\unity\Packages\io.improbable.gdk.tools\.CodeGenerator\GdkCodeGenerator\GdkCodeGenerator.csproj]
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

            SetupProject();
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

        [MenuItem("SpatialOS/Generate code", isValidateFunction: false, MenuPriorities.GenerateCodePriority)]
        private static void GenerateMenu()
        {
            Debug.Log("Generating code...");
            EditorApplication.delayCall += Generate;
        }

        [MenuItem("SpatialOS/Generate code (force)", isValidateFunction: false,
            MenuPriorities.GenerateCodeForcePriority)]
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
                Directory.Delete(toolsConfig.CodegenOutputDir, recursive: true);
            }

            SetupProject();
            Generate();
        }

        private static void SetupProject()
        {
            Profiler.BeginSample("Install dotnet template");
            InstallDotnetTemplate();
            Profiler.EndSample();

            Profiler.BeginSample("Create dotnet template");
            CreateTemplate();
            Profiler.EndSample();
        }

        private static void Generate()
        {
            try
            {
                if (!Common.CheckDependencies())
                {
                    return;
                }

                if (!File.Exists(CodegenExe))
                {
                    SetupProject();
                }

                EditorApplication.LockReloadAssemblies();

                Profiler.BeginSample("Add modules");
                UpdateModules();
                Profiler.EndSample();

                Profiler.BeginSample("Code generation");

                var schemaCompilerPath = SchemaCompilerPath;

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

                var workerJsonPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

                using (new ShowProgressBarScope("Generating code..."))
                {
                    var errorMessage = new StringBuilder();
                    var exitCode = RedirectedProcess.Command(Common.DotNetBinary)
                        .WithArgs(ConstructArgs(CodegenExe, schemaCompilerPath, workerJsonPath))
                        .RedirectOutputOptions(OutputRedirectBehaviour.None)
                        .AddErrorProcessing((line) => errorMessage.Append($"\n{line}"))
                        .AddOutputProcessing(ProcessStdOut)
                        .Run();

                    if (exitCode.ExitCode != 0)
                    {
                        if (!Application.isBatchMode)
                        {
                            Debug.LogError($"Error(s) compiling schema files!{errorMessage}");
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
                Profiler.EndSample();
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

            // Add user defined schema directories
            baseArgs.AddRange(toolsConfig.SchemaSourceDirs
                .Where(Directory.Exists)
                .Select(directory => $"--schema-path=\"{Path.GetFullPath(directory)}\""));

            // Add package schema directories
            baseArgs.AddRange(FindDirInPackages(SchemaPackageDir)
                .Select(directory => $"--schema-path=\"{directory}\""));

            // Schema Descriptor
            baseArgs.Add($"--descriptor-dir=\"{toolsConfig.DescriptorOutputDir}\"");

            baseArgs.AddRange(
                toolsConfig.SerializationOverrides.Select(@override => $"--serialization-override=\"{@override}\""));

            return baseArgs.ToArray();
        }

        private static void InstallDotnetTemplate()
        {
            var result = RedirectedProcess.Command(Common.DotNetBinary)
                .WithArgs("new", "-i", "./")
                .InDirectory(CodegenTemplatePath)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            if (result.ExitCode != 0)
            {
                throw new Exception("Failed to run.");
            }
        }

        private static void CreateTemplate()
        {
            if (Directory.Exists(CodegenExeDirectory))
            {
                Directory.Delete(CodegenExeDirectory, recursive: true);
            }

            Directory.CreateDirectory(CodegenExeDirectory);

            var result = RedirectedProcess.Command(Common.DotNetBinary)
                .WithArgs("new", "gdk-for-unity-codegen")
                .InDirectory(CodegenExeDirectory)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            if (result.ExitCode != 0)
            {
                throw new Exception("Failed to run.");
            }
        }

        /*
            This method edits the csproj XML to link in the constituent parts of the each codegen module.
            It expects any codegen module to be structured as follows:

            <package>
                .codegen/
                    Source/
                        SourceFile1.cs
                        SourceFile2.cs
                    Templates/
                        MyTemplate.tt
                    Partials/
                        Improbable.Vector3f

            Each of the Source, Templates, and Partials folder are optional.
        */
        private static void UpdateModules()
        {
            var csprojXml = XDocument.Load(CodegenExe);
            var projectNode = csprojXml.Element("Project");
            var codegenDirs = FindDirInPackages(CodegenDir).ToList();

            // Traverse the XML and find all existing ItemGroup nodes with GdkPackageSource items.
            // We will reuse these nodes or remove them if we no longer have a matching package.
            var gdkItemGroups = projectNode
                .Elements("ItemGroup")
                .Where(ele => ele.Element("GdkPackageSource") != null)
                .ToDictionary(ele => ele.Element("GdkPackageSource").Attribute("Remove").Value, ele => ele);

            foreach (var dir in codegenDirs)
            {
                if (!gdkItemGroups.TryGetValue(dir, out var itemGroup))
                {
                    itemGroup = new XElement("ItemGroup");
                    projectNode.Add(itemGroup);
                }

                itemGroup.RemoveAll();

                // Add an identifier so we can match this item group against a codegen module on subsequent runs.
                var idEle = new XElement("GdkPackageSource");
                idEle.SetAttributeValue("Remove", dir);
                itemGroup.Add(idEle);

                var sourceDir = Path.Combine(dir, "Source");
                if (Directory.Exists(sourceDir))
                {
                    // Ensure that we compile any source code provided by the codegen module.
                    var ele = new XElement("Compile");
                    ele.SetAttributeValue("Include", Path.Combine(sourceDir, "**"));
                    itemGroup.Add(ele);
                }

                var templateDir = Path.Combine(dir, "Templates");
                if (Directory.Exists(templateDir))
                {
                    // Ensure that we generate and compile in any T4 templates provided by the codegen module.
                    var ele = new XElement("T4Files");
                    ele.SetAttributeValue("Include", Path.Combine(templateDir, "**"));
                    itemGroup.Add(ele);
                }

                var partialDir = Path.Combine(dir, "Partials");
                if (Directory.Exists(partialDir))
                {
                    // Don't compile the partial.
                    var noneEle = new XElement("None");
                    noneEle.SetAttributeValue("Remove", Path.Combine(partialDir, "**"));
                    itemGroup.Add(noneEle);

                    // Add the partial as an embedded resource.
                    var resEle = new XElement("EmbeddedResource");
                    resEle.SetAttributeValue("Include", Path.Combine(partialDir, "**"));
                    itemGroup.Add(resEle);

                    // Ensure that we can see the Partials in the project view.
                    var folderEle = new XElement("Folder");
                    folderEle.SetAttributeValue("Include", partialDir);
                    itemGroup.Add(folderEle);
                }

                gdkItemGroups.Remove(dir);
            }

            // If we have any items left in gdkItemGroups, we should remove them from the csproj.
            foreach (var pair in gdkItemGroups)
            {
                pair.Value.Remove();
            }

            csprojXml.Save(CodegenExe);
        }

        private static IEnumerable<string> FindDirInPackages(string directory)
        {
            // Get all packages we depend on
            var request = Client.List(offlineMode: true);
            while (!request.IsCompleted)
            {
                // Wait for the request to complete
            }

            var packagePaths = request.Result
                .Select(package => Path.Combine(package.resolvedPath, directory))
                .Where(Directory.Exists);

            var cachedPackagePaths = Directory.GetDirectories("Library/PackageCache")
                .Select(path => Path.GetFullPath(Path.Combine(path, directory)))
                .Where(Directory.Exists);

            return packagePaths.Union(cachedPackagePaths).Distinct();
        }
    }
}
