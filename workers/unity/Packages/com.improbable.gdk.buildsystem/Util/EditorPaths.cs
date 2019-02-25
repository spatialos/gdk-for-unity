using System.IO;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem
{
    /// <summary>
    ///     Contains common directories related to building assets and players.
    /// </summary>
    /// <remarks>
    ///     All directories should be in Unity path format e.g. "Foo/Bar".
    /// </remarks>
    public static class EditorPaths
    {
        public static readonly string BuildScratchDirectory =
            Path.GetFullPath(Path.Combine(Application.dataPath, "..", "build", "worker"));

        public static readonly string AssetDatabaseDirectory =
            Path.Combine(Application.dataPath, "..", "..", "..", "build", "assembly");
    }
}
