using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityWorkerMenuGenerator
    {
        public static string Generate(List<string> workerTypes)
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

                            buildWorkerMenu.Method($"public static void BuildLocal{workerType}()", m =>
                            {
                                m.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/{workerType}"", false, EditorConfig.MenuOffset + {i})");
                                m.Line($@"MenuBuildLocal(new[] {{ {workerTypeString} }});");
                            });

                            buildWorkerMenu.Method($"public static void BuildCloud{workerType}()", m =>
                            {
                                m.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/{workerType}"", false, EditorConfig.MenuOffset + {i})");
                                m.Line($@"MenuBuildCloud(new[] {{ {workerTypeString} }});");
                            });
                        }

                        buildWorkerMenu.Method("public static void BuildLocalAll()", m =>
                        {
                            m.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + LocalMenu + ""/All workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})");
                            m.Line("MenuBuildLocal(AllWorkers);");
                        });

                        buildWorkerMenu.Method("public static void BuildCloudAll()", m =>
                        {
                            m.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/"" + CloudMenu + ""/All workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})");
                            m.Line("MenuBuildCloud(AllWorkers);");
                        });

                        buildWorkerMenu.Method("public static void Clean()", m =>
                        {
                            m.Annotate($@"MenuItem(EditorConfig.ParentMenu + ""/Clean all workers"", false, EditorConfig.MenuOffset + {workerTypes.Count})");
                            m.Line("MenuCleanAll();");
                        });

                        buildWorkerMenu.Method("private static void MenuBuildLocal(string[] filteredWorkerTypes)", m =>
                        {
                            m.Line("WorkerBuilder.MenuBuild(BuildEnvironment.Local, filteredWorkerTypes);");
                        });

                        buildWorkerMenu.Method("private static void MenuBuildCloud(string[] filteredWorkerTypes)", m =>
                        {
                            m.Line("WorkerBuilder.MenuBuild(BuildEnvironment.Cloud, filteredWorkerTypes);");
                        });

                        buildWorkerMenu.Method("private static void MenuCleanAll()", m =>
                        {
                            m.Line(new[]
                            {
                                "WorkerBuilder.Clean();",
                                "Debug.Log(\"Clean completed\");"
                            });
                        });
                    });
                });
            }).Format();
        }
    }
}
