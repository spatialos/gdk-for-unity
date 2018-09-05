using System.IO;
using System.Linq;

namespace Improbable.Gdk.BuildSystem.Util
{
    public static class PathUtil
    {
        /// <summary>
        ///     Combines multiple path components via <c>Path.Combine</c>.
        /// </summary>
        public static string Combine(params string[] paths)
        {
            return paths.Aggregate(Path.Combine);
        }

        /// <summary>
        ///     Attempts to create a directory if it does not already exist.
        /// </summary>
        public static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        ///     Converts the path in platform-native format to Unity path format "Foo/Bar".
        /// </summary>
        public static string ToUnityPath(this string nativePath)
        {
            return nativePath.Replace(Path.DirectorySeparatorChar, '/');
        }
    }
}
