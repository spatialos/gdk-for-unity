using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Improbable.ExternalPackageFixer
{
    public class ExternalPackageFixer : AssetPostprocessor
    {
        private static readonly Type FoldersType = Assembly.GetAssembly(typeof(AssetDatabase)).GetType("UnityEditor.PackageManager.Folders");

        private static readonly Type PackagesType =
            Assembly.GetAssembly(typeof(AssetDatabase)).GetType("UnityEditor.PackageManager.Packages");

        private static readonly Type PackageInfoType = Assembly.GetAssembly(typeof(AssetDatabase)).GetType("UnityEditor.PackageManager.PackageInfo");


        public static bool OnPreGeneratingCSProjectFiles()
        {
            foreach (var assetPath in AssetDatabase.GetAllAssetPaths())
            {
                ProcessPath(assetPath);
            }

            AssetDatabase.Refresh();
            return false;
        }

        private static void ProcessPath(string assetPath)
        {
            if (IsCustomPackage(assetPath))
            {
                if (IsExternalPackage(assetPath) && !IsReadonly(assetPath))
                {
                    SetPackageReadonly(assetPath);
                }
            }
        }

        public static bool IsCustomPackage(string assetPath)
        {
            return assetPath.StartsWith("Packages/") && assetPath.Split('/').Length == 2 &&
                !assetPath.Contains("com.unity.");
        }

        public static void SetPackageReadonly(string assetPath)
        {
            var packageInfo = GetPackageInfo(assetPath);
            var resolvedPath = GetPackageInfoProperty(packageInfo, "resolvedPath");
            var name = GetPackageInfoProperty(packageInfo, "name");
            UnregisterPackage(name, resolvedPath);
            RegisterPackageReadOnly(name, resolvedPath);
        }

        private static object GetPackageInfo(string assetPath)
        {
            return PackagesType.GetMethod("GetForAssetPath", BindingFlags.Public | BindingFlags.Static)
                .Invoke(null, new object[] { assetPath });
        }

        private static string GetPackageInfoProperty(object packageInfo, string propertyName)
        {
            return (string)PackageInfoType.GetProperty(propertyName).GetValue(packageInfo, new object[] { });
        }

        private static void UnregisterPackage(string name, string resolvedPath)
        {
            FoldersType.GetMethod("UnregisterPackageFolder", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[]
                {
                    name,
                    resolvedPath
                });
        }

        private static void RegisterPackageReadOnly(string name, string resolvedPath)
        {
            FoldersType.GetMethod("RegisterPackageFolder", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(
                    null, new object[]
                    {
                        name,
                        resolvedPath,
                        true
                    });
        }

        private static bool IsReadonly(string assetPath)
        {
            var getInfo =
                typeof(AssetDatabase).GetMethod("GetAssetFolderInfo", BindingFlags.Static | BindingFlags.NonPublic);
            var args = new object[] { assetPath, true, true };
            getInfo.Invoke(null, args);
            return (bool) args[2];
        }

        private static bool IsExternalPackage(string assetPath)
        {
            var packageInfo = GetPackageInfo(assetPath);
            var resolvedPath = GetPackageInfoProperty(packageInfo, "resolvedPath");
            var unityRoot = Directory.GetParent(Application.dataPath).FullName;
            return !resolvedPath.StartsWith(unityRoot);
        }
    }
}
