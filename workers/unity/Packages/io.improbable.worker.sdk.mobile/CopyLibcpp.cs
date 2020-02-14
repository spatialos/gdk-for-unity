using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;

namespace Improbable.Gdk.Mobile
{
    // Since 14.4.0, the Worker SDK no longer links 'libc++_shared.so' and Gradle doesn't seem to
    // properly link it in either.
    // So we just manually copy it across.
    // ¯\_(ツ)_/¯
    internal class CopyLibcpp : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get; }

        private static string NdkLibRoot => Path.Combine(GetNDKDirectory(), "sources", "cxx-stl", "llvm-libc++", "libs");

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            foreach (var arch in GetTargetArchitectures(path))
            {
                CopyLibcppSo(arch);
            }
        }

        private static IEnumerable<DirectoryInfo> GetTargetArchitectures(string gradleRootPath)
        {
            var jniLibsPath = Path.Combine(gradleRootPath, "src", "main", "jniLibs");
            var dirInfo = new DirectoryInfo(jniLibsPath);
            return dirInfo.EnumerateDirectories();
        }

        private static void CopyLibcppSo(DirectoryInfo architectureFolder)
        {
            var architecture = architectureFolder.Name;
            var filePath = Path.Combine(NdkLibRoot, architecture, "libc++_shared.so");
            if (!File.Exists(filePath))
            {
                throw new BuildFailedException("Unable to find libc++_shared.so. Ensure that the Android NDK is installed properly.");
            }

            File.Copy(filePath, Path.Combine(architectureFolder.FullName, "libc++_shared.so"));
        }

        private static bool UseEmbeddedNDK()
        {
            const string NDKPrefKey = "NdkUseEmbedded";
            return !EditorPrefs.HasKey(NDKPrefKey) || EditorPrefs.GetBool(NDKPrefKey);
        }

        private static string GetNDKDirectory()
        {
            if (UseEmbeddedNDK())
            {
                return Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None), "NDK");
            }
            else
            {
                return EditorPrefs.GetString("AndroidNdkRootR16b");
            }
        }
    }
}
