using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     Defines a custom inspector window that allows you to configure the GDK Tools.
    /// </summary>
    [CustomEditor(typeof(ScriptableGdkToolsConfiguration))]
    public class GdkToolsConfigurationInspector : Editor
    {
        internal const string SchemaStdLibDirLabel = "Schema Standard Library Directory";
        internal const string CodegenOutputDirLabel = "Code Generator Output Directory";
        internal const string SchemaSourceDirsLabel = "Schema Source Directories";

        private const string CodeGeneratorLabel = "Code Generator Options";
        private const string DownloadCoreSdkLabel = "Core SDK Options";

        private const string AddSchemaDirButtonText = "Add Schema Source Directory";
        private const string RemoveSchemaDirButtonText = "Remove";

        private const string ResetConfigurationButtonText = "Reset GDK Tools Configuration to Default";

        private ScriptableGdkToolsConfiguration toolsConfig;
        private List<string> configErrors = new List<string>();

        private readonly GUIStyle errorLayoutOption = new GUIStyle();

        private void OnEnable()
        {
            if (toolsConfig != null)
            {
                return;
            }

            toolsConfig = ScriptableGdkToolsConfiguration.GetOrCreateInstance();

            errorLayoutOption.normal.textColor = Color.red;

            Undo.undoRedoPerformed += () =>
            {
                configErrors = toolsConfig.Validate();
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            GUILayout.Label(DownloadCoreSdkLabel);
            GUILayout.Space(5);
            GUILayout.Label(SchemaStdLibDirLabel);
            toolsConfig.SchemaStdLibDir = GUILayout.TextField(toolsConfig.SchemaStdLibDir);

            DrawHorizontalBreak();

            GUILayout.Label(CodeGeneratorLabel);
            GUILayout.Space(5);
            GUILayout.Label(CodegenOutputDirLabel);
            toolsConfig.CodegenOutputDir = GUILayout.TextField(toolsConfig.CodegenOutputDir);

            GUILayout.Space(5);
            GUILayout.Label(SchemaSourceDirsLabel);
            for (var i = 0; i < toolsConfig.SchemaSourceDirs.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new ObjectUndoScope(toolsConfig, "Inspector"))
                    {
                        toolsConfig.SchemaSourceDirs[i] = GUILayout.TextField(toolsConfig.SchemaSourceDirs[i]);
                    }

                    if (GUILayout.Button(RemoveSchemaDirButtonText, GUILayout.Width(100)))
                    {
                        using (new ObjectUndoScope(toolsConfig, "Inspector"))
                        {
                            toolsConfig.SchemaSourceDirs.RemoveAt(i);
                        }
                    }
                }
            }

            if (GUILayout.Button(AddSchemaDirButtonText, GUILayout.Width(250)))
            {
                using (new ObjectUndoScope(toolsConfig, "Inspector"))
                {
                    toolsConfig.SchemaSourceDirs.Add(string.Empty);
                }
            }

            DrawHorizontalBreak();

            if (GUILayout.Button(ResetConfigurationButtonText, GUILayout.Width(250)))
            {
                using (new ObjectUndoScope(toolsConfig, "Inspector"))
                {
                    toolsConfig.ResetToDefault();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                configErrors = toolsConfig.Validate();
            }

            if (configErrors.Count <= 0)
            {
                return;
            }

            DrawHorizontalBreak();
            foreach (var error in configErrors)
            {
                EditorGUILayout.HelpBox(error, MessageType.Error);
            }
        }

        private void DrawHorizontalBreak()
        {
            GUILayout.Space(10);
            EditorGUILayout.TextArea(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(10);
        }

        private struct ObjectUndoScope : IDisposable
        {
            private const string NullArgumentError = "ObjectUndoScope received null as an argument for the object.";

            private Object obj;

            public ObjectUndoScope(Object obj, string name)
            {
                this.obj = obj;
                CheckObject();
                Undo.RecordObject(obj, name);
            }

            public void Dispose()
            {
                CheckObject();
                EditorUtility.SetDirty(obj);
                obj = null;
            }

            private void CheckObject()
            {
                if (obj == null)
                {
                    throw new ArgumentException(NullArgumentError);
                }
            }
        }
    }
}


