using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public static class BuildPostProcessXCode
    {
        private static Type pbxType;

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }

            Debug.Log("PostProcessBuild");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().FirstOrDefault(t => t.FullName == "UnityEditor.iOS.Xcode.PBXProject");
                if (type != null)
                {
                    pbxType = type;
                    break;
                }
            }

            //pbxType = Type.GetType("UnityEditor.iOS.Xcode.PBXProject");
            if (pbxType == null)
            {
                Debug.LogWarning("iOS Support not installed.");
                return;
            }

            var xcodeObject = Activator.CreateInstance(pbxType);

            // Instantiate PBXProject and read from xcode project file.
            var projPath = InvokeStaticMethod<string>("GetPBXProjectPath", path);
            var xcodeText = File.ReadAllText(projPath);
            InvokeMethod(xcodeObject, "ReadFromString", xcodeText);

            // Get Target GUIDs
            var unityTargetName = InvokeStaticMethod<string>("GetUnityTargetName");
            var unityTestTargetName = InvokeStaticMethod<string>("GetUnityTestTargetName");
            var targetGUID = InvokeMethod<string>(xcodeObject, "TargetGuidByName", unityTargetName);
            var targetTestingGUID = InvokeMethod<string>(xcodeObject, "TargetGuidByName", unityTestTargetName);

            // Enumerate configGUIDs that need library path patching
            var configNames = InvokeMethod<IEnumerable<string>>(xcodeObject, "BuildConfigNames");
            var configGUIDs = configNames
                .Select(configName => InvokeMethod<string>(xcodeObject, "BuildConfigByName", targetGUID, configName))
                .Concat(configNames.Select(configName =>
                    InvokeMethod<string>(xcodeObject, "BuildConfigByName", targetTestingGUID, configName)));

            // Update library paths for each config
            foreach (var configGUID in configGUIDs)
            {
                InvokeMethod<string>(xcodeObject, "UpdateBuildPropertyForConfig", configGUID, "LIBRARY_SEARCH_PATHS",
                    null, new[]
                    {
                        "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/arm",
                        "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/x86_64"
                    });

                // Add platform specific paths
                InvokeMethod<string>(xcodeObject, "UpdateBuildPropertyForConfig", configGUID,
                    "LIBRARY_SEARCH_PATHS[sdk=iphoneos*]", new[]
                    {
                        "$(inherited)",
                        "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/arm"
                    }, null);

                InvokeMethod<string>(xcodeObject, "UpdateBuildPropertyForConfig", configGUID,
                    "LIBRARY_SEARCH_PATHS[sdk=iphonesimulator*]",
                    new[]
                    {
                        "$(inherited)",
                        "$(SRCROOT)/Libraries/io.improbable.worker.sdk.mobile/Plugins/Improbable/Core/iOS/x86_64"
                    }, null);
            }

            // Save changes to xcode project file
            InvokeMethod<string>(xcodeObject, "WriteToFile", projPath);
        }

        private static T InvokeStaticMethod<T>(string methodName, params object[] parameters)
        {
            return (T) pbxType.InvokeMember(methodName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, parameters);
        }

        private static void InvokeStaticMethod(string methodName, params object[] parameters)
        {
            pbxType.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                null, null, parameters);
        }

        private static T InvokeMethod<T>(object target, string methodName, params object[] parameters)
        {
            return (T) pbxType.InvokeMember(methodName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, target, parameters);
        }

        private static void InvokeMethod(object target, string methodName, params object[] parameters)
        {
            pbxType.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null, target, parameters);
        }
    }
}
