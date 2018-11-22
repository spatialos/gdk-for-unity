using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     Defines a custom inspector window that allows you to configure the GDK Tools.
    /// </summary>
    public class GdkToolsConfigurationWindow : EditorWindow
    {
        internal const string SchemaStdLibDirLabel = "Standard library";
        internal const string CodegenOutputDirLabel = "Code generator output";
        internal const string SchemaSourceDirsLabel = "Schema sources";

        private const string CodeGeneratorLabel = "Code generator";

        private static readonly GUIContent AddSchemaDirButtonText = new GUIContent("+", "Add new schema directory");
        private static readonly GUIContent RemoveSchemaDirButtonText = new GUIContent("-", "Remove schema directory");

        private string ResetConfigurationButtonText = "Reset to default";
        private string SaveConfigurationButtonText = "Save";

        private GdkToolsConfiguration toolsConfig;
        private List<string> configErrors = new List<string>();

        private Vector2 scrollPosition;

        [MenuItem("SpatialOS/GDK tools configuration", false, MenuPriorities.GdkToolsConfiguration)]
        public static void ShowWindow()
        {
            GetWindow<GdkToolsConfigurationWindow>().Show();
        }

        private void OnEnable()
        {
            if (toolsConfig != null)
            {
                return;
            }

            titleContent = new GUIContent("GDK Tools");
            toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();

            Undo.undoRedoPerformed += () => { configErrors = toolsConfig.Validate(); };
        }

        public void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosition))
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    var canDelete = configErrors.Count > 0;
                    using (new EditorGUI.DisabledScope(canDelete))
                    {
                        if (GUILayout.Button(SaveConfigurationButtonText, EditorStyles.toolbarButton))
                        {
                            toolsConfig.Save();
                        }
                    }

                    if (GUILayout.Button(ResetConfigurationButtonText, EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to reset to defaults?", "Yes", "No"))
                        {
                            toolsConfig.ResetToDefault();
                        }
                    }
                }

                DrawCodeGenerationOptions();

                if (check.changed)
                {
                    configErrors = toolsConfig.Validate();
                }

                scrollPosition = scroll.scrollPosition;
            }

            foreach (var error in configErrors)
            {
                EditorGUILayout.HelpBox(error, MessageType.Error);
            }
        }

        private void DrawCodeGenerationOptions()
        {
            GUILayout.Label(CodeGeneratorLabel, EditorStyles.boldLabel);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.CodegenOutputDir =
                    EditorGUILayout.TextField(CodegenOutputDirLabel, toolsConfig.CodegenOutputDir);

                GUILayout.Label(SchemaSourceDirsLabel, EditorStyles.boldLabel);
                toolsConfig.SchemaStdLibDir =
                    EditorGUILayout.TextField(SchemaStdLibDirLabel, toolsConfig.SchemaStdLibDir);

                for (var i = 0; i < toolsConfig.SchemaSourceDirs.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        toolsConfig.SchemaSourceDirs[i] =
                            EditorGUILayout.TextField($"Schema dir [{i}]", toolsConfig.SchemaSourceDirs[i]);

                        using (new EditorGUI.DisabledScope(toolsConfig.SchemaSourceDirs.Count == 1))
                        {
                            if (GUILayout.Button(RemoveSchemaDirButtonText, EditorStyles.toolbarButton,
                                GUILayout.ExpandWidth(false), GUILayout.Width(18)))
                            {
                                toolsConfig.SchemaSourceDirs.RemoveAt(i);
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(AddSchemaDirButtonText, EditorStyles.toolbarButton,
                        GUILayout.ExpandWidth(false), GUILayout.Width(18)))
                    {
                        toolsConfig.SchemaSourceDirs.Add(string.Empty);
                    }
                }
            }
        }
    }
}
