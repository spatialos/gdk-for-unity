using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Core.Editor
{
    public static class CommonUIElements
    {
        public static void DrawHorizontalLine(int height, Color color)
        {
            var rect = EditorGUILayout.GetControlRect(false, height, EditorStyles.foldout);
            rect.height = height;
            using (new Handles.DrawingScope(color))
            {
                Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            }

            GUILayout.Space(rect.height);
        }
    }
}
