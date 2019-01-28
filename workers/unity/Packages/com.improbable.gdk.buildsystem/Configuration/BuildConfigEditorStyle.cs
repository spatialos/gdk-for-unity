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

        internal readonly GUIContent AddSceneButtonContents = new GUIContent(string.Empty, "Add scene");
        internal readonly GUIContent RemoveSceneButtonContents = new GUIContent(string.Empty, "Remove scene");
        internal string BuiltInErrorIcon = "console.erroricon";

        internal readonly string[] CompressionOptions = { "Default", "LZ4", "LZ4HC" };

        internal readonly Dictionary<BuildTarget, GUIContent> BuildTargetIcons =
            new Dictionary<BuildTarget, GUIContent>();

        internal readonly Dictionary<BuildTarget, GUIContent> BuildErrorIcons =
            new Dictionary<BuildTarget, GUIContent>();

        internal readonly HashSet<string> ExpandedWorkers = new HashSet<string>();
        internal readonly HashSet<string> ExpandedBuildOptions = new HashSet<string>();
        internal readonly Dictionary<string, GUIContent> WorkerContent = new Dictionary<string, GUIContent>();

        internal readonly Dictionary<string, BuildTargetState> SelectedBuildTarget =
            new Dictionary<string, BuildTargetState>();

        internal class BuildTargetState
        {
            public int Index;
            public GUIContent[] Choices;
        }

        private void BuildTargetToContent(BuildTarget target, string iconName, string label)
        {
            var icon = new GUIContent(EditorGUIUtility.IconContent(iconName));

            icon.text = label;
            icon.tooltip = label;
            BuildTargetIcons[target] = icon;

            icon = new GUIContent(EditorGUIUtility.IconContent(BuiltInErrorIcon));
            icon.text = label;
            icon.tooltip = label;
            BuildErrorIcons[target] = icon;
        }

        internal BuildConfigEditorStyle()
        {
            BuildTargetToContent(BuildTarget.Android, "BuildSettings.Android.Small", "Android");
            BuildTargetToContent(BuildTarget.iOS, "BuildSettings.iPhone.Small", "iOS");
            BuildTargetToContent(BuildTarget.StandaloneWindows, "BuildSettings.Standalone.Small", "Win x86");
            BuildTargetToContent(BuildTarget.StandaloneWindows64, "BuildSettings.Standalone.Small", "Win x64");
            BuildTargetToContent(BuildTarget.StandaloneLinux64, "BuildSettings.Standalone.Small", "Linux");
            BuildTargetToContent(BuildTarget.StandaloneOSX, "BuildSettings.Standalone.Small", "MacOS");

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
