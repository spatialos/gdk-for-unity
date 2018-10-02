using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class SceneItem
    {
        public bool Included;
        public bool Exists;
        public readonly SceneAsset SceneAsset;

        public SceneItem(SceneAsset sceneAsset, bool included, SceneAsset[] scenesInAssetDatabase)
        {
            SceneAsset = sceneAsset;
            Included = included;
            Exists = scenesInAssetDatabase.Contains(sceneAsset);
        }
    }
}
