using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class BuildSupportChecker
{
    private static readonly Dictionary<BuildTarget, string> BuildPlatformSupportModuleDirectoryNames;

    static BuildSupportChecker()
    {
        BuildPlatformSupportModuleDirectoryNames = new Dictionary<BuildTarget, string>();

        const string linuxStandaloneSupport = "LinuxStandaloneSupport";

        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinux] = linuxStandaloneSupport;
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinux64] = linuxStandaloneSupport;
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinuxUniversal] = linuxStandaloneSupport;

#if !UNITY_EDITOR_OSX
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneOSX] = "MacStandaloneSupport";
#endif

#if !UNITY_EDITOR_WIN
        var windowsStandaloneSupport = "WindowsStandaloneSupport";

        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneWindows] = windowsStandaloneSupport;
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneWindows64] = windowsStandaloneSupport;
#endif
    }

    public class BuildSupportCheckResult
    {
        public readonly bool CanBuild;
        public readonly BuildTarget[] TargetsWithoutBuildSupport;

        public BuildSupportCheckResult(bool canBuild, BuildTarget[] targetsWithoutBuildSupport)
        {
            CanBuild = canBuild;
            TargetsWithoutBuildSupport = targetsWithoutBuildSupport;
        }
    }

    public static BuildSupportCheckResult CheckBuildSupport(params BuildTarget[] buildTargets)
    {
        string editorExePath = Environment.GetCommandLineArgs()[0];

        var editorDirectory = Directory.GetParent(editorExePath);

        var installDirectory = editorDirectory.Parent;
        if (installDirectory == null)
        {
            Debug.LogError("Build support check failed: could not find Unity Editor installation directory.");

            // Since we're not certain, let's allow a build attempt.
            return new BuildSupportCheckResult(true, new BuildTarget[0]);
        }

        var playbackEnginesDirectory = Path.Combine(installDirectory.FullName, "Editor", "Data", "PlaybackEngines");

        var buildTargetsWithoutSupport = buildTargets
            .Where(target =>
            {
                if (BuildPlatformSupportModuleDirectoryNames.TryGetValue(target, out var playbackEnginesDirectoryName))
                {
                    return !Directory.Exists(Path.Combine(playbackEnginesDirectory, playbackEnginesDirectoryName));
                }

                // If it's not in the dictionary, then assume it's not missing support.
                // e.g. if we're on a Windows Editor, it should have windows support.
                return false;
            })
            .ToArray();

        return new BuildSupportCheckResult(buildTargetsWithoutSupport.Length == 0, buildTargetsWithoutSupport);
    }
}
