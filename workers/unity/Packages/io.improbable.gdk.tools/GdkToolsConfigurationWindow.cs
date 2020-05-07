using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        internal const string VerboseLoggingLabel = "Verbose logging";
        internal const string CodegenLogOutputDirLabel = "Log output directory";
        internal const string CodegenOutputDirLabel = "C# output directory";
        internal const string DescriptorOutputDirLabel = "Descriptor directory";
        internal const string DevAuthTokenDirLabel = "Token directory";

        private const string SchemaSourceDirsLabel = "Schema sources";
        private const string DevAuthTokenSectionLabel = "Dev Auth Token Settings";
        private const string DevAuthTokenLifetimeLabel = "Token lifetime (days)";
        private const string CodeGeneratorLabel = "Code generator";
        private const string CustomSnapshotPathLabel = "Selected snapshot path";
        private const string GeneralSectionLabel = "General";
        private const string ResetConfigurationButtonText = "Reset all to default";

        private static GUIContent AddSchemaDirButton;
        private static GUIContent RemoveSchemaDirButton;

        private GdkToolsConfiguration toolsConfig;
        private List<string> configErrors = new List<string>();
        private Vector2 scrollPosition;

        // Minimum time required from last config change before saving to file
        private readonly TimeSpan FileSavingInterval = TimeSpan.FromSeconds(1);
        private DateTime lastSaveTime = DateTime.Now;
        private bool hasUnsavedData;

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

            Application.quitting += OnExit;
        }

        private void OnDestroy()
        {
            OnExit();

            Application.quitting -= OnExit;
        }

        private void OnExit()
        {
            if (hasUnsavedData)
            {
                toolsConfig.Save();
            }
        }

        public void OnGUI()
        {
            if (AddSchemaDirButton == null)
            {
                AddSchemaDirButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"))
                {
                    tooltip = "Add schema directory"
                };

                RemoveSchemaDirButton = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
                {
                    tooltip = "Remove schema directory"
                };
            }

            using (new EditorGUILayout.VerticalScope())
            using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosition))
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                DrawGeneralSection();

                DrawCodeGenerationOptions();

                DrawCustomSnapshotDir();

                if (check.changed)
                {
                    configErrors = toolsConfig.Validate();

                    if (configErrors.Count == 0)
                    {
                        hasUnsavedData = true;
                    }
                }

                GUILayout.Space(10f);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(ResetConfigurationButtonText, EditorStyles.miniButtonMid, GUILayout.Width(150)))
                    {
                        if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to reset to defaults?",
                            "Yes", "No"))
                        {
                            GUI.FocusControl(null);
                            toolsConfig.ResetToDefault();
                        }
                    }

                    GUILayout.FlexibleSpace();
                }

                scrollPosition = scroll.scrollPosition;
            }

            foreach (var error in configErrors)
            {
                EditorGUILayout.HelpBox(error, MessageType.Error);
            }
        }

        private void DrawGeneralSection()
        {
            var previousWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 180;

            GUILayout.Label(GeneralSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.EnvironmentPlatform = EditorGUILayout.TextField("Environment", toolsConfig.EnvironmentPlatform);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.RuntimeVersionOverride =
                    EditorGUILayout.TextField("Runtime Version Override", toolsConfig.RuntimeVersionOverride);

                GUILayout.Label($"Current Runtime version: {toolsConfig.RuntimeVersion}", EditorStyles.helpBox);
            }

            EditorGUIUtility.labelWidth = previousWidth;
        }

        private void DrawCodeGenerationOptions()
        {
            var previousWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 180;

            GUILayout.Label(CodeGeneratorLabel, EditorStyles.boldLabel);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

            using (new EditorGUIUtility.IconSizeScope(new Vector2(12, 12)))
            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.VerboseLogging = EditorGUILayout.Toggle(VerboseLoggingLabel, toolsConfig.VerboseLogging);

                toolsConfig.CodegenLogOutputDir =
                    EditorGUILayout.TextField(CodegenLogOutputDirLabel, toolsConfig.CodegenLogOutputDir);

                toolsConfig.CodegenOutputDir =
                    EditorGUILayout.TextField(CodegenOutputDirLabel, toolsConfig.CodegenOutputDir);

                toolsConfig.DescriptorOutputDir =
                    EditorGUILayout.TextField(DescriptorOutputDirLabel, toolsConfig.DescriptorOutputDir);

                EditorGUILayout.LabelField($"{SchemaSourceDirsLabel}", EditorStyles.boldLabel);
                using (new EditorGUI.IndentLevelScope())
                {
                    for (var i = 0; i < toolsConfig.SchemaSourceDirs.Count; i++)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            toolsConfig.SchemaSourceDirs[i] =
                                EditorGUILayout.TextField($"Schema path [{i}]", toolsConfig.SchemaSourceDirs[i]);

                            if (GUILayout.Button(RemoveSchemaDirButton, EditorStyles.miniButton,
                                GUILayout.ExpandWidth(false)))
                            {
                                toolsConfig.SchemaSourceDirs.RemoveAt(i);
                            }
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button(AddSchemaDirButton, EditorStyles.miniButton))
                        {
                            toolsConfig.SchemaSourceDirs.Add(string.Empty);
                        }
                    }
                }
            }

            GUILayout.Label(DevAuthTokenSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.DevAuthTokenLifetimeDays =
                    EditorGUILayout.IntSlider(DevAuthTokenLifetimeLabel, toolsConfig.DevAuthTokenLifetimeDays, 1, 90);

                toolsConfig.SaveDevAuthTokenToFile = EditorGUILayout.Toggle("Save token to file", toolsConfig.SaveDevAuthTokenToFile);
                using (new EditorGUI.DisabledScope(!toolsConfig.SaveDevAuthTokenToFile))
                {
                    toolsConfig.DevAuthTokenDir = EditorGUILayout.TextField(DevAuthTokenDirLabel, toolsConfig.DevAuthTokenDir);
                    GUILayout.Label($"Token filepath: {Path.GetFullPath(toolsConfig.DevAuthTokenFilepath)}", EditorStyles.helpBox);
                }
            }

            EditorGUIUtility.labelWidth = previousWidth;
        }

        private void DrawCustomSnapshotDir()
        {
            GUILayout.Label(CustomSnapshotPathLabel, EditorStyles.boldLabel);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField(CustomSnapshotPathLabel, toolsConfig.CustomSnapshotPath);
                }

                if (GUILayout.Button("Open", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                {
                    var path = EditorUtility.OpenFilePanel("Select snapshot", toolsConfig.CustomSnapshotPath, "snapshot");

                    if (!string.IsNullOrEmpty(path))
                    {
                        toolsConfig.CustomSnapshotPath = path;
                    }
                }
            }
        }

        private void Update()
        {
            TrySaveChanges();
        }

        private void TrySaveChanges()
        {
            var timeSinceLastSave = DateTime.Now - lastSaveTime;
            if (!hasUnsavedData || timeSinceLastSave < FileSavingInterval || configErrors.Any())
            {
                return;
            }

            toolsConfig.Save();
            lastSaveTime = DateTime.Now;
            hasUnsavedData = false;
        }
    }
}
