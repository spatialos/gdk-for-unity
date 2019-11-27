using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using Improbable.Gdk.Core;
using UnityEditor;
using UnityEditor.Build;

namespace Improbable.Gdk.BuildSystem
{
    public static class CommandlineParser
    {
        public static string[] GetWorkerTypesToBuild(CommandLineArgs args)
        {
            var workerTypesArg = args.GetCommandLineValue("buildWorkerTypes", "UnityClient,UnityGameLogic");
            var wantedWorkerTypes = workerTypesArg.Split(',');
            return wantedWorkerTypes;
        }

        public static List<BuildTarget> GetBuildTargetFilter(CommandLineArgs args)
        {
            var buildTargetFilterArg = string.Empty;
            if (!args.TryGetCommandLineValue("buildTargetFilter", ref buildTargetFilterArg))
            {
                return null;
            }

            return buildTargetFilterArg
                .Split(',')
                .Select(target =>
                {
                    switch (buildTargetFilterArg.ToLower())
                    {
                        case "android":
                            return BuildTarget.Android;
                        case "ios":
                            return BuildTarget.iOS;
                        case "winx86":
                            return BuildTarget.StandaloneWindows;
                        case "win":
                            return BuildTarget.StandaloneWindows64;
                        case "linux":
                            return BuildTarget.StandaloneLinux64;
                        case "macos":
                            return BuildTarget.StandaloneOSX;
                        default:
                            throw new BuildFailedException(
                                "Unknown build target value: " + buildTargetFilterArg);
                    }
                }).ToList();
        }

        public static ScriptingImplementation GetScriptingImplementation(CommandLineArgs args)
        {
            var wantedScriptingBackend = args.GetCommandLineValue("scriptingBackend", "mono");
            switch (wantedScriptingBackend.ToLower())
            {
                case "mono":
                    return ScriptingImplementation.Mono2x;
                case "il2cpp":
                    return ScriptingImplementation.IL2CPP;
                default:
                    throw new BuildFailedException("Unknown scripting backend value: " + wantedScriptingBackend);
            }
        }

        public static BuildEnvironment GetBuildEnvironment(CommandLineArgs args)
        {
            var buildEnvironmentArg = args.GetCommandLineValue("buildEnvironment", "local");

            switch (buildEnvironmentArg.ToLower())
            {
                case "cloud":
                    return BuildEnvironment.Cloud;
                case "local":
                    return BuildEnvironment.Local;
                default:
                    throw new BuildFailedException("Unknown build environment value: " + buildEnvironmentArg);
            }
        }
    }
}
