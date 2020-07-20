using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityWorkerMenuGenerator
    {
        public static CodeWriter Generate(List<string> workerTypes)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "Improbable.Gdk.BuildSystem",
                    "Improbable.Gdk.BuildSystem.Configuration",
                    "Improbable.Gdk.Tools",
                    "UnityEditor",
                    "UnityEngine"
                );

                cgw.Namespace("Improbable", ns =>
                {
                    ns.Type("internal static class BuildWorkerMenu", buildWorkerMenu =>
                    {
                        buildWorkerMenu.Line(new[]
                        {
                            "private const string LocalMenu = \"Build for local\";",
                            "private const string CloudMenu = \"Build for cloud\";"
                        });

                        buildWorkerMenu.Initializer("private static readonly string[] AllWorkers = new string[]", () =>
                        {
                            return workerTypes.Select(workerType => $@"""{workerType}""");
                        });

                        for (var i = 0; i < workerTypes.Count; i++)
                        {
                            var workerType = workerTypes[i];
                            var workerTypeString = $@"""{workerType}""";

                            buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/{workerType}"", false, EditorConfig.MenuOffset + {i})")
                                .Method($"public static void BuildLocal{workerType}()", () => new[]
                                {
                                    $@"MenuBuildLocal(new[] {{ {workerTypeString} }});"
                                });

                            buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/{workerType}"", false, EditorConfig.MenuOffset + {i})")
                                .Method($"public static void BuildCloud{workerType}()", () => new[]
                                {
                                    $@"MenuBuildCloud(new[] {{ {workerTypeString} }});"
                                });

                            buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/{workerType}"", true, EditorConfig.MenuOffset + {i})")
                                .Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/{workerType}"", true, EditorConfig.MenuOffset + {i})")
                                .Method($"public static bool BuildMenuValidator{workerType}()", () => new[]
                                {
                                    "return CanMenuBuild();"
                                });
                        }

                        buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/All workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})")
                            .Method("public static void BuildLocalAll()", () => new[]
                            {
                                "MenuBuildLocal(AllWorkers);"
                            });

                        buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/All workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})")
                            .Method("public static void BuildCloudAll()", () => new[]
                            {
                                "MenuBuildCloud(AllWorkers);"
                            });
                        
                        buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/{workerType}"", true, EditorConfig.MenuOffset + {i})")
                            .Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/{workerType}"", true, EditorConfig.MenuOffset + {i})")
                            .Method($"public static bool BuildAllMenuValidator{workerType}()", () => new[]
                            {
                                "return CanMenuBuild();"
                            });

                        buildWorkerMenu.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/Clean all workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})")
                            .Method("public static void Clean()", () => new[]
                            {
                                "MenuCleanAll();"
                            });

                        buildWorkerMenu.Method("private static void MenuBuildLocal(string[] filteredWorkerTypes)", () => new[]
                        {
                            "WorkerBuilder.MenuBuild(BuildEnvironment.Local, filteredWorkerTypes);"
                        });

                        buildWorkerMenu.Method("private static void MenuBuildCloud(string[] filteredWorkerTypes)", () => new[]
                        {
                            "WorkerBuilder.MenuBuild(BuildEnvironment.Cloud, filteredWorkerTypes);"
                        });

                        buildWorkerMenu.Method("private static void MenuCleanAll()", () => new[]
                        {
                            "WorkerBuilder.Clean();",
                            "Debug.Log(\"Clean completed\");"
                        });

                        buildWorkerMenu.Method("private static bool CanMenuBuild()", () => new[]
                        {
                            "return !BuildSupportChecker.EditorHasCompileErrors()"
                        });
                    });
                });
            });
        }
    }
}
