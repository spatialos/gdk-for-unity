using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class SceneItem
    {
        public bool Included;
        public readonly bool Exists;
        public readonly SceneAsset SceneAsset;

        public SceneItem(SceneAsset sceneAsset, bool included, SceneAsset[] inAssetDatabase)
        {
            SceneAsset = sceneAsset;
            Included = included;
            Exists = inAssetDatabase.Contains(sceneAsset);
        }
    }
}
