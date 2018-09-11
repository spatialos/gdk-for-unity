using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class SceneItem
    {
        public readonly SceneAsset SceneAsset;
        public bool Included;
        private readonly bool exists;

        public SceneItem(SceneAsset sceneAsset, bool included, SceneAsset[] inAssetDatabase)
        {
            SceneAsset = sceneAsset;
            Included = included;
            exists = inAssetDatabase.Contains(sceneAsset);
        }

        public static SceneItem Drawer(Rect position, SceneItem item)
        {
            using (item.exists ? null : new GUIColorScope(Color.red))
            {
                var positionWidth = position.width;
                var labelWidth = GUI.skin.toggle.CalcSize(GUIContent.none).x + 5;

                position.width = labelWidth;
                item.Included = EditorGUI.Toggle(position, item.Included);

                position.x += labelWidth;
                position.width = positionWidth - labelWidth;

                EditorGUI.ObjectField(position, item.SceneAsset, typeof(SceneAsset), false);
            }

            return item;
        }
    }
}
