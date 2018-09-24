using System.IO;
using System.Linq;

namespace Improbable.Gdk.BuildSystem
{
    /// <summary>
    ///     Contains common directories related to building assets and players.
    /// </summary>
    /// <remarks>
    ///     All directories should be in Unity path format e.g. "Foo/Bar".
    /// </remarks>
    public static class PathUtils
    {
        public static readonly string BuildScratchDirectory = Path.GetFullPath(Path.Combine("build", "worker"));
        public static readonly string AssetDatabaseDirectory = Combine("..", "..", "build", "assembly").ToUnityPath();
        
        /// <summary>
        ///     Combines multiple path components via <c>Path.Combine</c>.
        /// </summary>
        internal static string Combine(params string[] paths)
        {
            return paths.Aggregate(Path.Combine);
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
