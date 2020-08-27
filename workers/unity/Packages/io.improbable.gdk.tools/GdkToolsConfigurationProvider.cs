using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     Defines a custom section in Unity Project settings for GDK Tools configuration.
    /// </summary>
    public class GdkToolsConfigurationProvider : SettingsProvider
    {
        public const string ProjectSettingsPath = "Project/Spatial OS";

        internal const string VerboseLoggingLabel = "Verbose logging";
        internal const string CodegenLogOutputDirLabel = "Log output directory";
        internal const string CodegenOutputDirLabel = "C# output directory";
        internal const string CodegenEditorOutputDirLabel = "C# Editor output directory";
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

        // Flag to indicate if we have unsaved changes in the settings window
        private bool hasUnsavedData;

        private GdkToolsConfigurationProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateGdkToolsConfigurationProvider()
        {
            var provider = new GdkToolsConfigurationProvider(ProjectSettingsPath, SettingsScope.Project);

            // Extract all members (fields, methods etc) from the GdkToolsConfiguration class
            var gdkToolsConfigProperties = typeof(GdkToolsConfiguration).GetMembers();

            // Filter the results so that fields and properties remain
            // Use the names of the discovered members as the keyword search terms in the Project Settings panel
            provider.keywords = gdkToolsConfigProperties
                .Where(member => member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)
                .Select(property => property.Name).ToList();
            return provider;
        }


        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (toolsConfig != null)
            {
                return;
            }

            toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();

            Undo.undoRedoPerformed += () => { configErrors = toolsConfig.Validate(); };
        }

        public override void OnDeactivate()
        {
            if (!hasUnsavedData || configErrors.Any())
            {
                return;
            }

            toolsConfig.Save();
            AssetDatabase.SaveAssets();
            hasUnsavedData = false;
        }


        public override void OnGUI(string searchContext)
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

                DrawDevAuthTokenOptions();

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
                    if (GUILayout.Button(ResetConfigurationButtonText, EditorStyles.miniButtonMid, GUILayout.Width(150))
                        && EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to reset to defaults?", "Yes", "No"))
                    {
                        GUI.FocusControl(null);
                        toolsConfig.ResetToDefault();
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
                toolsConfig.EnvironmentPlatform = EditorGUILayout.TextField(new GUIContent("Environment",
                        "The environment argument to provide to GDK tooling such as the spatial CLI and Deployment Launcher."),
                    toolsConfig.EnvironmentPlatform);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.RuntimeVersionOverride =
                    EditorGUILayout.TextField(new GUIContent("Runtime Version Override",
                            "Overrides the default runtime version used by the GDK for local and cloud deployments."),
                        toolsConfig.RuntimeVersionOverride);

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
                toolsConfig.VerboseLogging = EditorGUILayout.Toggle(new GUIContent(VerboseLoggingLabel,
                        "Toggles verbose logging of the code generator."),
                    toolsConfig.VerboseLogging);

                toolsConfig.CodegenLogOutputDir =
                    EditorGUILayout.TextField(new GUIContent(CodegenLogOutputDirLabel,
                            "The directory, relative to the Unity project root, where code generator places its log files."),
                        toolsConfig.CodegenLogOutputDir);

                toolsConfig.CodegenOutputDir =
                    EditorGUILayout.TextField(new GUIContent(CodegenOutputDirLabel,
                            "The directory, relative to the Unity project root, where the code generator should place non-Editor generated code."),
                        toolsConfig.CodegenOutputDir);

                toolsConfig.CodegenEditorOutputDir = EditorGUILayout.TextField(new GUIContent(CodegenEditorOutputDirLabel,
                        "The directory, relative to the Unity project root, where the code generator should place Editor-specific generated code."),
                    toolsConfig.CodegenEditorOutputDir);

                toolsConfig.DescriptorOutputDir =
                    EditorGUILayout.TextField(new GUIContent(DescriptorOutputDirLabel,
                            "The directory, relative to the Unity project root, where the code generator should place the generated descriptor."),
                        toolsConfig.DescriptorOutputDir);

                EditorGUILayout.LabelField(new GUIContent($"{SchemaSourceDirsLabel}",
                        "A list of directories, relative to the Unity project root, containing schema that are not already in a package." +
                        " Note that the GDK automatically finds and provides paths to any `.schema` folder found inside a package."),
                    EditorStyles.boldLabel);
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

            EditorGUIUtility.labelWidth = previousWidth;
        }

        private void DrawDevAuthTokenOptions()
        {
            var previousWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 180;

            GUILayout.Label(DevAuthTokenSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                toolsConfig.DevAuthTokenLifetimeDays =
                    EditorGUILayout.IntSlider(new GUIContent(DevAuthTokenLifetimeLabel,
                            "Sets the lifetime for requested development authentication tokens, between 1 and 90 days. " +
                            "Changes made to this setting do not affect already requested tokens."),
                        toolsConfig.DevAuthTokenLifetimeDays, 1, 90);

                toolsConfig.SaveDevAuthTokenToFile = EditorGUILayout.Toggle(new GUIContent("Save token to file",
                        "Sets whether to save the development authentication to a file, instead of in player preferences."),
                    toolsConfig.SaveDevAuthTokenToFile);
                using (new EditorGUI.DisabledScope(!toolsConfig.SaveDevAuthTokenToFile))
                {
                    toolsConfig.DevAuthTokenDir = EditorGUILayout.TextField(new GUIContent(DevAuthTokenDirLabel,
                            "The directory, relative to the project's Assets/ folder, where development authentication tokens will be " +
                            " saved to if saving to file is enabled. The full path is displayed in the window for ease of use."),
                        toolsConfig.DevAuthTokenDir);
                    GUILayout.Label($"Token filepath: {Path.GetFullPath(toolsConfig.DevAuthTokenFilepath)}", EditorStyles.helpBox);
                }
            }

            EditorGUIUtility.labelWidth = previousWidth;
        }

        private void DrawCustomSnapshotDir()
        {
            GUILayout.Label(new GUIContent(CustomSnapshotPathLabel,
                    "The default snapshot the GDK will use for local deployments."),
                EditorStyles.boldLabel);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField(new GUIContent(CustomSnapshotPathLabel,
                            "The default snapshot the GDK will use for local deployments."),
                        toolsConfig.CustomSnapshotPath);
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
    }
}
