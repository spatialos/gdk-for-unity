using System.IO;
using UnityEditor;
using UnityEngine;

namespace Playground.Editor.SnapshotGenerator
{
    internal class SnapshotEditorWindow : EditorWindow
    {
        private SnapshotGenerator.Arguments arguments;

        [MenuItem("SpatialOS/Generate snapshot", false, 200)]
        public static void GenerateMenuItem()
        {
            GetWindow<SnapshotEditorWindow>().Show();
        }

        public void Awake()
        {
            minSize = new Vector2(200, 120);
            titleContent = new GUIContent("Generate snapshot");

            SetDefaults();
        }

        private void SetDefaults()
        {
            arguments = new SnapshotGenerator.Arguments
            {
                NumberEntities = 16,
                OutputPath = Path.GetFullPath(
                    Path.Combine(
                        Application.dataPath,
                        "..",
                        "..",
                        "..",
                        "snapshots",
                        "default.snapshot"))
            };
        }

        public void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (GUILayout.Button("Defaults"))
                {
                    SetDefaults();
                    Repaint();
                }

                arguments.NumberEntities = EditorGUILayout.IntField("Number of entities", arguments.NumberEntities);
                arguments.OutputPath = EditorGUILayout.TextField("Snapshot path", arguments.OutputPath);

                var shouldDisable = string.IsNullOrEmpty(arguments.OutputPath);
                using (new EditorGUI.DisabledScope(shouldDisable))
                {
                    if (GUILayout.Button("Generate snapshot"))
                    {
                        SnapshotGenerator.Generate(arguments);
                    }
                }
            }
        }
    }
}
