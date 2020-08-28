using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public struct BuildContextSettings
    {
        public IEnumerable<string> WantedWorkerTypes;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation? ScriptImplementation;
        public ICollection<BuildTarget> BuildTargetFilter;
        public iOSSdkVersion? IOSSdkVersion;

        public BuildContextSettings(IEnumerable<string> wantedWorkerTypes, BuildEnvironment buildEnvironment,
            ScriptingImplementation? scriptImplementation = null, ICollection<BuildTarget> buildTargetFilter = null,
            iOSSdkVersion? iosSdkVersion = null)
        {
            WantedWorkerTypes = wantedWorkerTypes;
            BuildEnvironment = buildEnvironment;
            ScriptImplementation = scriptImplementation;
            BuildTargetFilter = buildTargetFilter;
            IOSSdkVersion = iosSdkVersion;
        }

        public static BuildContextSettings Local(params string[] wantedWorkerTypes)
        {
            return new BuildContextSettings(wantedWorkerTypes, BuildEnvironment.Local);
        }

        public static BuildContextSettings Cloud(params string[] wantedWorkerTypes)
        {
            return new BuildContextSettings(wantedWorkerTypes, BuildEnvironment.Cloud);
        }

        public static BuildContextSettings FromCommandLine(CommandLineArgs args)
        {
            return new BuildContextSettings
            {
                WantedWorkerTypes = GetWorkerTypesToBuild(args),
                BuildEnvironment = GetBuildEnvironment(args),
                ScriptImplementation = GetScriptingImplementation(args),
                BuildTargetFilter = GetBuildTargetFilter(args),
                IOSSdkVersion = GetTargetIOSSdk(args)
            };
        }

        private static string[] GetWorkerTypesToBuild(CommandLineArgs args)
        {
            var workerTypesArg = args.GetCommandLineValue("buildWorkerTypes", "UnityClient,UnityGameLogic");
            var wantedWorkerTypes = workerTypesArg.Split(',');
            return wantedWorkerTypes;
        }

        private static BuildEnvironment GetBuildEnvironment(CommandLineArgs args)
        {
            var buildEnvironmentArg = args.GetCommandLineValue("buildEnvironment", "local").ToLower();

            switch (buildEnvironmentArg)
            {
                case "cloud":
                    return BuildEnvironment.Cloud;
                case "local":
                    return BuildEnvironment.Local;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildEnvironmentArg), buildEnvironmentArg,
                        "Unknown build environment");
            }
        }

        private static ScriptingImplementation GetScriptingImplementation(CommandLineArgs args)
        {
            var wantedScriptingBackend = args.GetCommandLineValue("scriptingBackend", "mono").ToLower();
            switch (wantedScriptingBackend)
            {
                case "mono":
                    return ScriptingImplementation.Mono2x;
                case "il2cpp":
                    return ScriptingImplementation.IL2CPP;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wantedScriptingBackend), wantedScriptingBackend,
                        "Unknown scripting backend");
            }
        }

        private static List<BuildTarget> GetBuildTargetFilter(CommandLineArgs args)
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
                            throw new ArgumentException("winx86 is no longer supported, use win instead.");
                        case "win":
                            return BuildTarget.StandaloneWindows64;
                        case "linux":
                            return BuildTarget.StandaloneLinux64;
                        case "macos":
                            return BuildTarget.StandaloneOSX;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(target), target,
                                "Unknown build target value");
                    }
                }).ToList();
        }

        private static iOSSdkVersion GetTargetIOSSdk(CommandLineArgs args)
        {
            var targetIOSSdkArg = args.GetCommandLineValue("targetiOSSdk", string.Empty).ToLower();
            if (string.IsNullOrEmpty(targetIOSSdkArg))
            {
                return PlayerSettings.iOS.sdkVersion;
            }

            switch (targetIOSSdkArg)
            {
                case "device":
                    return iOSSdkVersion.DeviceSDK;
                case "simulator":
                    return iOSSdkVersion.SimulatorSDK;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetIOSSdkArg), targetIOSSdkArg,
                        "Unknown target iOS SDK");
            }
        }

        public bool Matches(BuildTarget target)
        {
            return BuildTargetFilter?.Contains(target) ?? true;
        }
    }
}
