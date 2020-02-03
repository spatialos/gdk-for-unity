using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Android;

namespace Improbable.Gdk.Mobile
{
    // It seems that since 14.4.0, the Worker SDK no longer links in libc++_shared.so and Gradle doesn't
    // properly link it in either.
    // So we just manually copy it across.
    // ¯\_(ツ)_/¯
    internal class CopyLibcpp : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get; }

        private static readonly string NdkLibRoot = Path.Combine(EditorPrefs.GetString("AndroidNdkRootR16b"), "sources",
            "cxx-stl", "llvm-libc++", "libs");

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
            File.Copy(Path.Combine(NdkLibRoot, architecture, "libc++_shared.so"), Path.Combine(architectureFolder.FullName, "libc++_shared.so"));
        }
    }
}
