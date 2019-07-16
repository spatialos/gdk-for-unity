using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Improbable.Gdk.Mobile
{
    public static class BuildPostProcessXCode
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            // Only run for iOS
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }

            // Load xcode project
            var projPath = PBXProject.GetPBXProjectPath(path);
            var xcodeProject = new PBXProject();
            xcodeProject.ReadFromString(File.ReadAllText(projPath));

            // Get GUIDs for target and testing targets
            var targetGUID = xcodeProject.TargetGuidByName(PBXProject.GetUnityTargetName());
            var targetTestingGUID = xcodeProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());

            // Generate list of all config guids
            var configGUIDs = xcodeProject.BuildConfigNames()
                .Select(configName => xcodeProject.BuildConfigByName(targetGUID, configName))
                .Concat(
                    xcodeProject.BuildConfigNames().Select(configName =>
                        xcodeProject.BuildConfigByName(targetTestingGUID, configName)));

            foreach (var configGUID in configGUIDs)
            {
                // Remove search paths unity has added for all platforms
                xcodeProject.UpdateBuildPropertyForConfig(configGUID, "LIBRARY_SEARCH_PATHS", null, new[]
                {
                    "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/arm",
                    "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/x86_64"
                });

                // Add platform specific paths
                xcodeProject.UpdateBuildPropertyForConfig(configGUID, "LIBRARY_SEARCH_PATHS[sdk=iphoneos*]", new[]
                {
                    "$(inherited)",
                    "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/arm"
                }, null);

                xcodeProject.UpdateBuildPropertyForConfig(configGUID, "LIBRARY_SEARCH_PATHS[sdk=iphonesimulator*]",
                    new[]
                    {
                        "$(inherited)",
                        "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/x86_64"
                    }, null);
            }

            // Save it to file
            xcodeProject.WriteToFile(projPath);
        }
    }
}
