using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;
using UnityEngine;

public static class BuildSupportChecker
{
    private static Dictionary<BuildTarget, bool> supportedBuildTargets;

    public static void CheckBuildSupportTargets()
    {
        supportedBuildTargets = WorkerBuildData.AllBuildTargets.ToDictionary(k => k, BuildSupportChecker.CheckForBuildSupport);
    }


    public static bool CanBuildTarget(BuildTarget target)
    {
        if (supportedBuildTargets == null)
        {
            CheckBuildSupportTargets();
        }

        if (supportedBuildTargets.TryGetValue(target, out var supported))
        {
            return supported;
        }

        return false;
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
