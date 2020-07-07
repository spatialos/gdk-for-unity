using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;
using UnityEngine;

public static class BuildSupportChecker
{
    private static HashSet<BuildTarget> supportedBuildTargets;

    private static void CheckBuildSupportTargets()
    {
        supportedBuildTargets = new HashSet<BuildTarget>(WorkerBuildData.AllBuildTargets.Where(CheckForBuildSupport));
    }

    public static bool CanBuildTarget(BuildTarget target)
    {
        if (supportedBuildTargets == null)
        {
            CheckBuildSupportTargets();
        }

        return supportedBuildTargets.Contains(target);
    }

    public static bool CanBuildHeadless(BuildTarget target)
    {
        // Check if the target platform supports headless mode
        // Supported platforms: see https://docs.unity3d.com/ScriptReference/BuildOptions.EnableHeadlessMode.html
        return target == BuildTarget.StandaloneWindows64
            || target == BuildTarget.StandaloneOSX
            || target == BuildTarget.StandaloneLinux64;
    }

    private static bool CheckForBuildSupport(BuildTarget target)
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
                return true;
        }

        if (target == BuildTarget.StandaloneOSX && Application.platform == RuntimePlatform.OSXEditor)
        {
            // OSXEditor will always have StandaloneOSX support
            return true;
        }

        if (WorkerBuildData.BuildTargetSupportDirectoryNames.TryGetValue(target,
            out var playbackEnginesDirectoryName))
        {
            return Directory.Exists(Path.Combine(playbackEnginesDirectory, playbackEnginesDirectoryName));
        }

        return true;
    }
}
