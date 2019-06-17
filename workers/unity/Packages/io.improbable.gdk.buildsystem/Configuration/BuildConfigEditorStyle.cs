using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class BuildConfigEditorStyle
    {
        internal readonly GUIContent
            AddWorkerTypeButtonContents = new GUIContent(string.Empty, "Add worker type");

        internal readonly GUIContent RemoveWorkerTypeButtonContents =
            new GUIContent(string.Empty, "Remove worker type");

        internal readonly GUIContent AddSceneButtonContents = new GUIContent(string.Empty, "Add Scene");
        internal readonly GUIContent RemoveSceneButtonContents = new GUIContent(string.Empty, "Remove Scene");

        internal const string BuiltInErrorIcon = "console.erroricon.sml";
        internal const string BuiltInWarningIcon = "console.warnicon.sml";

        internal readonly string[] CompressionOptions = { "Default", "LZ4", "LZ4HC" };

        internal readonly Dictionary<BuildTarget, GUIContent> BuildTargetIcons =
            new Dictionary<BuildTarget, GUIContent>();

        internal readonly Dictionary<BuildTarget, GUIContent> BuildTargetText =
            new Dictionary<BuildTarget, GUIContent>();

        internal readonly Dictionary<BuildTarget, GUIContent> BuildErrorIcons =
            new Dictionary<BuildTarget, GUIContent>();

        internal readonly Dictionary<BuildTarget, GUIContent> BuildWarningIcons =
            new Dictionary<BuildTarget, GUIContent>();

        private void BuildTargetToContent(BuildTarget target, string iconName, string label)
        {
            var icon = new GUIContent(EditorGUIUtility.IconContent(iconName)) { text = label, tooltip = label };
            BuildTargetIcons[target] = icon;

            icon = new GUIContent(EditorGUIUtility.IconContent(BuiltInErrorIcon)) { text = label, tooltip = label };
            BuildErrorIcons[target] = icon;

            icon = new GUIContent(EditorGUIUtility.IconContent(BuiltInWarningIcon)) { text = label, tooltip = label };
            BuildWarningIcons[target] = icon;

            BuildTargetText[target] = new GUIContent(label);
        }

        internal BuildConfigEditorStyle()
        {
            BuildTargetToContent(BuildTarget.Android, "BuildSettings.Android", "Android");
            BuildTargetToContent(BuildTarget.iOS, "BuildSettings.iPhone", "iOS");
            BuildTargetToContent(BuildTarget.StandaloneWindows, "BuildSettings.Standalone", "Win x86");
            BuildTargetToContent(BuildTarget.StandaloneWindows64, "BuildSettings.Standalone", "Win x64");
            BuildTargetToContent(BuildTarget.StandaloneLinux64, "BuildSettings.Standalone", "Linux");
            BuildTargetToContent(BuildTarget.StandaloneOSX, "BuildSettings.Standalone", "MacOS");

            AddSceneButtonContents.image = EditorGUIUtility.IconContent("Toolbar Plus").image;
            RemoveSceneButtonContents.image = EditorGUIUtility.IconContent("Toolbar Minus").image;

            AddWorkerTypeButtonContents.image = EditorGUIUtility.IconContent("Toolbar Plus").image;
            RemoveWorkerTypeButtonContents.image = EditorGUIUtility.IconContent("Toolbar Minus").image;
        }

        internal static void DrawHorizontalLine()
        {
            var rect = EditorGUILayout.GetControlRect(false, 2, EditorStyles.foldout);
            using (new Handles.DrawingScope(new Color(0.3f, 0.3f, 0.3f, 1)))
            {
                Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            }

            GUILayout.Space(rect.height);
        }

        internal static Rect RectUnion(Rect a, Rect b)
        {
            if (a.Equals(Rect.zero))
            {
                return b;
            }

            if (b.Equals(Rect.zero))
            {
                return a;
            }

            var minX = Mathf.Min(a.xMin, b.xMin);
            var minY = Mathf.Min(a.yMin, b.yMin);
            var maxX = Mathf.Max(a.xMax, b.xMax);
            var maxY = Mathf.Max(a.yMax, b.yMax);

            var newRect = new Rect(minX, minY, maxX - minX, maxY - minY);

            return newRect;
        }

        internal static void DrawGrabber(Rect grabberRect)
        {
            using (new Handles.DrawingScope(new Color(0.4f, 0.4f, 0.4f, 1)))
            {
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height * 0.25f),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height * 0.25f));
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height / 2),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2));
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height * 0.75f),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height * 0.75f));
            }

            using (new Handles.DrawingScope(new Color(0.3f, 0.3f, 0.3f, 1)))
            {
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height * 0.25f + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height * 0.25f + 1));
                Handles.DrawLine(new Vector2(grabberRect.xMin,
                        grabberRect.yMin + grabberRect.height / 2 + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2 + 1));
                Handles.DrawLine(new Vector2(grabberRect.xMin,
                        grabberRect.yMin + grabberRect.height * 0.75f + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height * 0.75f + 1));
            }
        }
    }
}
