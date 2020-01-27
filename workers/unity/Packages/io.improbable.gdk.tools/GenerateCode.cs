using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private static readonly string WorkerJsonPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

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
        /// Example: D:\gdk-for-unity\workers\unity\Packages\io.improbable.gdk.core\.codegen\Source\CoreCodegenJob.cs(128,64): error CS1002: ; expected [D:\gdk-for-unity\test-project\build\codegen\CodeGen\CodeGen.csproj]
        private static readonly Regex dotnetRegex = new Regex(
            @"(?<file>[\w\\\.]+)\((?<line>\d+),(?<col>\d+)\): (?<type>\w+) (?<code>\w+): (?<message>[\s\S]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Dictionary<CodegenLogLevel, int> codegenLogCounts;

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
            try
            {
                Profiler.BeginSample("Install dotnet template");
                InstallDotnetTemplate();
                Profiler.EndSample();

                Profiler.BeginSample("Create dotnet template");
                CreateTemplate();
                Profiler.EndSample();

                Profiler.BeginSample("Generate IDE run configurations");
                GenerateCodegenRunConfigs();
                Profiler.EndSample();
            }
            catch (Exception)
            {
                EditorUtility.DisplayDialog("Generate Code",
                    "Code generation failed.\nPlease check the console for more information.",
                    "Close");
                throw;
            }
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

                using (new ShowProgressBarScope("Generating code..."))
                {
                    ResetCodegenLogCounter();

                    var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
                    var loggerOutputPath = GetDefaultLoggerOutputPath(toolsConfig);

                    var exitCode = RedirectedProcess.Command(Common.DotNetBinary)
                        .WithArgs("run", "-p", $"\"{CodegenExe}\"")
                        .RedirectOutputOptions(OutputRedirectBehaviour.None)
                        .AddOutputProcessing(ProcessDotnetOutput)
                        .AddOutputProcessing(ProcessCodegenOutput)
                        .Run();

                    var numWarnings = codegenLogCounts[CodegenLogLevel.Warn];
                    var numErrors = codegenLogCounts[CodegenLogLevel.Error] + codegenLogCounts[CodegenLogLevel.Fatal];

                    if (exitCode.ExitCode != 0 || numErrors > 0)
                    {
                        if (!Application.isBatchMode)
                        {
                            Debug.LogError("Code generation failed! Please check the console for more information.");

                            EditorApplication.delayCall += () =>
                            {
                                if (File.Exists(loggerOutputPath))
                                {
                                    var option = EditorUtility.DisplayDialogComplex("Generate Code",
                                        $"Code generation failed with {numWarnings} warnings and {numErrors} errors!\n\nPlease check the code generation logs for more information: {loggerOutputPath}",
                                        "Open logfile",
                                        "Close",
                                        "");

                                    switch (option)
                                    {
                                        // Open logfile
                                        case 0:
                                            Application.OpenURL(loggerOutputPath);
                                            break;

                                        // Close
                                        case 1:
                                        // Alt
                                        case 2:
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException("Unrecognised option");
                                    }
                                }
                                else
                                {
                                    DisplayGeneralFailure();
                                }
                            };
                        }
                    }
                    else
                    {
                        if (numWarnings > 0)
                        {
                            Debug.LogWarning($"Code generation completed successfully with {numWarnings} warnings. Please check the logs for more information: {loggerOutputPath}");
                        }
                        else
                        {
                            Debug.Log("Code generation complete!");
                        }

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

        private static string GetDefaultLoggerOutputPath(GdkToolsConfiguration toolsConfig = null)
        {
            toolsConfig = toolsConfig ?? GdkToolsConfiguration.GetOrCreateInstance();
            return Path.GetFullPath(Path.Combine(toolsConfig.CodegenLogOutputDir, "codegen-output.log"));
        }

        private static string GetSchemaCompilerPath()
        {
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

            return schemaCompilerPath;
        }

        private static void ResetCodegenLogCounter()
        {
            codegenLogCounts = new Dictionary<CodegenLogLevel, int>
            {
                { CodegenLogLevel.Trace, 0 },
                { CodegenLogLevel.Debug, 0 },
                { CodegenLogLevel.Info, 0 },
                { CodegenLogLevel.Warn, 0 },
                { CodegenLogLevel.Error, 0 },
                { CodegenLogLevel.Fatal, 0 }
            };
        }

        private static void DisplayGeneralFailure()
        {
            EditorUtility.DisplayDialog("Generate Code",
                "Code generation failed.\nPlease check the console for more information.",
                "Close");
        }

        private static void ProcessDotnetOutput(string output)
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
        }

        private static void ProcessCodegenOutput(string output)
        {
            try
            {
                var log = CodegenLog.FromRaw(output);
                codegenLogCounts[log.Level]++;
                Debug.unityLogger.Log(log.GetUnityLogType(), $"{log.Message}\n{log.Logger}");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
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
                    Partials/
                        Improbable.Vector3f

            Each of the Source and Partials folder are optional.
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

        internal static void GenerateCodegenRunConfigs(GdkToolsConfiguration toolsConfig = null)
        {
            toolsConfig = toolsConfig ?? GdkToolsConfiguration.GetOrCreateInstance();

            var schemaCompilerPath = GetSchemaCompilerPath();
            var logfilePath = GetDefaultLoggerOutputPath();

            var codegenArgs = new List<string>
            {
                $"--json-dir=\"{ImprobableJsonDir}\"",
                $"--schema-compiler-path=\"{schemaCompilerPath}\"",
                $"--worker-json-dir=\"{WorkerJsonPath}\"",
                $"--log-file=\"{logfilePath}\""
            };

            if (toolsConfig.VerboseLogging)
            {
                codegenArgs.Add($"--verbose");
            }

            var codegenOutputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", toolsConfig.CodegenOutputDir));
            codegenArgs.Add($"--native-output-dir=\"{codegenOutputPath}\"");

            // Add user defined schema directories, warn if directory does not exist
            foreach (var schemaDir in toolsConfig.SchemaSourceDirs)
            {
                if (!Directory.Exists(schemaDir))
                {
                    Debug.LogWarning($"Schema directory defined in GDK tools configuration does not exist: {schemaDir}");
                    continue;
                }

                codegenArgs.Add($"--schema-path=\"{Path.GetFullPath(schemaDir)}\"");
            }

            // Add package schema directories
            codegenArgs.AddRange(FindDirInPackages(SchemaPackageDir)
                .Select(directory => $"--schema-path=\"{directory}\""));

            // Schema Descriptor
            codegenArgs.Add($"--descriptor-dir=\"{toolsConfig.DescriptorOutputDir}\"");

            codegenArgs.AddRange(
                toolsConfig.SerializationOverrides.Select(@override => $"--serialization-override=\"{@override}\""));

            var codegenArgsString = string.Join(" ", codegenArgs);

            // For dotnet run / visual studio
            try
            {
                var csprojXml = XDocument.Load(CodegenExe);
                var projectNode = csprojXml.Element("Project");
                var propertyGroup = projectNode.Element("PropertyGroup");

                var args = propertyGroup.Element("StartArguments");
                args?.Remove();

                propertyGroup.Add(XElement.Parse($"<StartArguments>{codegenArgsString}</StartArguments>"));
                csprojXml.Save(CodegenExe);
            }
            catch
            {
                throw new Exception("Unable to update csproj with run configuration.");
            }

            // For jetbrains rider
            try
            {
                var runConfigPath = Path.Combine(CodegenExeDirectory, ".idea", ".idea.CodeGen", ".idea",
                    "runConfigurations");
                Directory.CreateDirectory(runConfigPath);
                using (var w = new StreamWriter(Path.Combine(runConfigPath, "CodeGen.xml"), false))
                {
                    w.Write($@"
<component name=""ProjectRunConfigurationManager"">
  <configuration default=""false"" name=""CodeGen"" type=""DotNetProject"" factoryName="".NET Project"">
    <option name=""PROJECT_PATH"" value=""$PROJECT_DIR$/CodeGen/CodeGen.csproj"" />
    <option name=""PROGRAM_PARAMETERS"" value=""{codegenArgsString.Replace("\"", "&quot;")}"" />
  </configuration>
</component>
");
                }
            }
            catch
            {
                throw new Exception("Unable to generate Rider run configuration.");
            }
        }
    }
}
