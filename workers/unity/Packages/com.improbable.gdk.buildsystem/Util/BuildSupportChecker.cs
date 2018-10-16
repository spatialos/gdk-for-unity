using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;
using UnityEngine;

public static class BuildSupportChecker
{
    private static readonly Dictionary<BuildTarget, string> BuildPlatformSupportModuleDirectoryNames;

    static BuildSupportChecker()
    {
        BuildPlatformSupportModuleDirectoryNames = new Dictionary<BuildTarget, string>();

        const string linuxStandaloneSupport = "LinuxStandaloneSupport";
        const string windowsStandaloneSupport = "WindowsStandaloneSupport";
        const string macStandaloneSupport = "MacStandaloneSupport";

        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinux] = linuxStandaloneSupport;
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinux64] = linuxStandaloneSupport;
        BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneLinuxUniversal] = linuxStandaloneSupport;

        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneOSX] = macStandaloneSupport;
        }

        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneWindows] = windowsStandaloneSupport;
            BuildPlatformSupportModuleDirectoryNames[BuildTarget.StandaloneWindows64] = windowsStandaloneSupport;
        }
    }

    public static BuildTarget[] GetBuildTargetsMissingBuildSupport(params BuildTarget[] buildTargets)
    {
        var editorDirectory = Directory.GetParent(EditorApplication.applicationPath);

        string playbackEnginesDirectory;

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                playbackEnginesDirectory = Path.Combine(editorDirectory.FullName, "PlaybackEngines");
                break;
            case RuntimePlatform.WindowsEditor:
                playbackEnginesDirectory = Path.Combine(editorDirectory.FullName, "Data", "PlaybackEngines");
                break;
            default:
                return new BuildTarget[0];
        }

        return buildTargets
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
    }

    public static string ConstructMissingSupportMessage(string workerType, BuildEnvironment environment,
        BuildTarget[] buildTargetsMissingBuildSupport)
    {
        return
            $"The worker \"{workerType}\" cannot be built for a {environment} deployment -" +
            $" the Unity Editor is missing build support for {string.Join(", ", buildTargetsMissingBuildSupport)}.\n" +
            "Please add the missing build support options to your Unity Editor.";
    }
}
