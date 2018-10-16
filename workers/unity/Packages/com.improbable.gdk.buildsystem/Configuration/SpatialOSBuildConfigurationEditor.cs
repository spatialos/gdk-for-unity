using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CustomEditor(typeof(SpatialOSBuildConfiguration))]
    public class SpatialOSBuildConfigurationEditor : Editor
    {
        private const int ScreenWidthForHorizontalLayout = 450;

        private bool scenesChanged;
        private SceneAsset[] scenesInAssetDatabase;
        private string workerTypeName = "WorkerType";

        private static readonly GUIContent AddWorkerTypeButtonContents = new GUIContent("+", "Add worker type");
        private static readonly GUIContent RemoveWorkerTypeButtonContents = new GUIContent("-", "Remove worker type");
        private static readonly GUIContent MoveUpButtonContents = new GUIContent("^", "Move item up");
        private static readonly GUIContent MoveDownButtonContents = new GUIContent("v", "Move item down");

        public void OnEnable()
        {
            scenesInAssetDatabase = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>).ToArray();
        }

        public override void OnInspectorGUI()
        {
            var workerConfiguration = (SpatialOSBuildConfiguration) target;

            EditorGUILayout.BeginHorizontal();
            workerTypeName = EditorGUILayout.TextField(workerTypeName);
            var controlRect = EditorGUILayout.GetControlRect(false);
            var buttonRect = new Rect(controlRect);
            buttonRect.width = buttonRect.height + 5;

            if (GUI.Button(buttonRect, AddWorkerTypeButtonContents))
            {
                if (workerConfiguration.WorkerBuildConfigurations.All(x => x.WorkerType != workerTypeName))
                {
                    var config = new WorkerBuildConfiguration
                    {
                        WorkerType = workerTypeName,
                        LocalBuildConfig = new BuildEnvironmentConfig
                        {
                            BuildOptions = BuildOptions.Development,
                        },
                        CloudBuildConfig = new BuildEnvironmentConfig
                        {
                            BuildOptions = BuildOptions.EnableHeadlessMode,
                        }
                    };
                    workerConfiguration.WorkerBuildConfigurations.Add(config);
                }
            }

            EditorGUILayout.EndHorizontal();

            scenesChanged = false;

            var configs = workerConfiguration.WorkerBuildConfigurations;
            foreach (var workerConfig in configs)
            {
                if (!DrawWorkerConfiguration(workerConfig))
                {
                    configs.Remove(workerConfig);
                    break;
                }
            }

            if (scenesChanged)
            {
                scenesChanged = false;
                EditorApplication.delayCall += workerConfiguration.UpdateEditorScenesForBuild;
            }
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var workerType = configurationForWorker.WorkerType;

            EditorGUILayout.BeginHorizontal();
            configurationForWorker.ShowFoldout =
                EditorGUILayout.Foldout(configurationForWorker.ShowFoldout, workerType);

            var controlRect = EditorGUILayout.GetControlRect(false);
            var buttonRect = new Rect(controlRect);
            buttonRect.width = buttonRect.height + 5;
            if (GUI.Button(buttonRect, RemoveWorkerTypeButtonContents))
            {
                return false;
            }

            EditorGUILayout.EndHorizontal();

            if (configurationForWorker.ShowFoldout)
            {
                using (IndentLevelScope(1))
                {
                    DrawScenesInspectorForWorker(configurationForWorker);
                    DrawEnvironmentInspectorForWorker(configurationForWorker);
                }
            }

            return true;
        }

        private void DrawScenesInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            EditorGUILayout.LabelField("Scenes", EditorStyles.boldLabel);

            using (IndentLevelScope(1))
            {
                var scenesToShowInList = configurationForWorker
                    .ScenesForWorker
                    .Select((sceneAsset, index) =>
                        new SceneItem(sceneAsset, true, scenesInAssetDatabase))
                    .ToList();

                EditorGUI.BeginChangeCheck();

                var sceneItems = scenesInAssetDatabase
                    .Where(sceneAsset => !configurationForWorker.ScenesForWorker.Contains(sceneAsset))
                    .Select(sceneAsset => new SceneItem(sceneAsset, false, scenesInAssetDatabase))
                    .ToList();

                var horizontalLayout = Screen.width > ScreenWidthForHorizontalLayout;

                using (horizontalLayout ? new EditorGUILayout.HorizontalScope() : null)
                {
                    using (horizontalLayout ? new EditorGUILayout.VerticalScope() : null)
                    {
                        EditorGUILayout.LabelField("Scenes to include (in order)");
                        DrawSceneList(scenesToShowInList, true, true);
                    }

                    using (horizontalLayout ? new EditorGUILayout.VerticalScope() : null)
                    {
                        using (horizontalLayout ? IndentLevelScope(-EditorGUI.indentLevel) : null)
                        {
                            EditorGUILayout.LabelField("Exclude");
                            DrawSceneList(sceneItems, false, false);
                        }
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure scenes for worker");

                    configurationForWorker.ScenesForWorker = scenesToShowInList.Concat(sceneItems)
                        .Where(item => item.Included)
                        .Select(item => item.SceneAsset)
                        .ToArray();

                    scenesChanged = true;
                }
            }
        }

        private void DrawEnvironmentInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            EditorGUILayout.LabelField("Environments", EditorStyles.boldLabel);

            DrawEnvironmentInspector(BuildEnvironment.Local, configurationForWorker);
            DrawEnvironmentInspector(BuildEnvironment.Cloud, configurationForWorker);
        }

        private static TEnum EnumFlagsToggleField<TEnum>(TEnum source)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum type");
            }

            var enumNonZeroValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Where(options => options.ToInt32(NumberFormatInfo.CurrentInfo) != 0)
                .ToArray();

            using (IndentLevelScope(1))
            {
                var indentedHelpBox =
                    new GUIStyle(EditorStyles.helpBox) { margin = { left = EditorGUI.indentLevel * 16 } };

                using (new EditorGUILayout.VerticalScope(indentedHelpBox))
                using (IndentLevelScope(-EditorGUI.indentLevel))
                {
                    var sourceBitValue = source.ToInt32(NumberFormatInfo.CurrentInfo);
                    foreach (var enumValue in enumNonZeroValues)
                    {
                        var targetBitValue = enumValue.ToInt32(NumberFormatInfo.CurrentInfo);
                        var hasFlag = (sourceBitValue & targetBitValue) != 0;
                        var newFlag = EditorGUILayout.ToggleLeft(enumValue.ToString(CultureInfo.InvariantCulture), hasFlag);
                        if (hasFlag != newFlag)
                        {
                            source = (TEnum) (object) (sourceBitValue ^ targetBitValue);
                        }
                    }
                }
            }

            return source;
        }

        private void DrawEnvironmentInspector(BuildEnvironment environment,
            WorkerBuildConfiguration configurationForWorker)
        {
            using (IndentLevelScope(1))
            {
                EditorGUILayout.LabelField(environment.ToString());

                var environmentConfiguration =
                    configurationForWorker.GetEnvironmentConfig(environment);

                ConfigureBuildPlatforms(environmentConfiguration);
                ConfigureBuildOptions(environmentConfiguration);
            }
        }

        private void ConfigureBuildOptions(BuildEnvironmentConfig environmentConfiguration)
        {
            using (IndentLevelScope(1))
            {
                EditorGUI.BeginChangeCheck();
                var showBuildOptions = EditorGUILayout.Foldout(environmentConfiguration.ShowBuildOptions, "Build Options");
                var newBuildOptions = environmentConfiguration.BuildOptions;
                if (showBuildOptions)
                {
                    using (IndentLevelScope(1))
                    {
                        var indentedHelpBox =
                            new GUIStyle(EditorStyles.helpBox) { margin = { left = EditorGUI.indentLevel * 16 } };

                        using (new EditorGUILayout.VerticalScope(indentedHelpBox))
                        using (IndentLevelScope(-EditorGUI.indentLevel))
                        {
                            var prevValue = (newBuildOptions & BuildOptions.Development) != 0;
                            var newValue = EditorGUILayout.ToggleLeft("Development Build", prevValue);
                            if (prevValue != newValue)
                            {
                                newBuildOptions ^= BuildOptions.Development;
                            }

                            prevValue = (newBuildOptions & BuildOptions.EnableHeadlessMode) != 0;
                            newValue = EditorGUILayout.ToggleLeft("Headless Mode", prevValue);
                            if (prevValue != newValue)
                            {
                                newBuildOptions ^= BuildOptions.EnableHeadlessMode;
                            }
                        }
                    }
                }


                if ((newBuildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                    (newBuildOptions & BuildOptions.Development) != 0)
                {
                    EditorGUILayout.HelpBox(
                        "You cannot have both EnableHeadlessMode and Development build enabled.\n" +
                        "This will crash the Unity Editor during the build.",
                        MessageType.Error);
                }

                if ((newBuildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                    (environmentConfiguration.BuildPlatforms & ~SpatialBuildPlatforms.Linux) != 0)
                {
                    EditorGUILayout.HelpBox(
                        "EnableHeadlessMode is only available for Linux builds.",
                        MessageType.Warning);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure build options for worker");

                    environmentConfiguration.ShowBuildOptions = showBuildOptions;
                    environmentConfiguration.BuildOptions = newBuildOptions;
                }
            }
        }

        private static string BuildPlatformToString(SpatialBuildPlatforms value)
        {
            if (value == SpatialBuildPlatforms.Current)
            {
                return $"Current ({WorkerBuilder.GetCurrentBuildPlatform()})";
            }

            return value.ToString();
        }

        private static string SelectedPlatformsToString(SpatialBuildPlatforms value)
        {
            var enumValues = Enum.GetValues(typeof(SpatialBuildPlatforms)).Cast<SpatialBuildPlatforms>().ToArray();
            if (value == 0)
            {
                return "None";
            }

            return string.Join(", ", enumValues
                    .Where(enumValue => (value & enumValue) != 0)
                    .Select(BuildPlatformToString).ToArray());
        }

        private void ConfigureBuildPlatforms(BuildEnvironmentConfig environmentConfiguration)
        {
            using (IndentLevelScope(1))
            {
                EditorGUI.BeginChangeCheck();

                var buildPlatformsString = SelectedPlatformsToString(environmentConfiguration.BuildPlatforms);
                var newBuildPlatforms = environmentConfiguration.BuildPlatforms;
                var showBuildPlatforms = EditorGUILayout.Foldout(environmentConfiguration.ShowBuildPlatforms,
                    "Build Platforms: " + buildPlatformsString);
                if (showBuildPlatforms)
                {
                    newBuildPlatforms = EnumFlagsToggleField(environmentConfiguration.BuildPlatforms);
                }

                var currentAdjustedPlatforms = newBuildPlatforms;

                if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Current) != 0)
                {
                    currentAdjustedPlatforms |= WorkerBuilder.GetCurrentBuildPlatform();
                }

                if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Windows32) != 0 &&
                    (currentAdjustedPlatforms & SpatialBuildPlatforms.Windows64) != 0)
                {
                    EditorGUILayout.HelpBox(WorkerBuilder.IncompatibleWindowsPlatformsErrorMessage,
                        MessageType.Error);
                }

                var buildTargetsMissingBuildSupport = BuildSupportChecker.GetBuildTargetsMissingBuildSupport(WorkerBuilder.GetUnityBuildTargets(newBuildPlatforms));
                if (buildTargetsMissingBuildSupport.Length > 0)
                {
                    EditorGUILayout.HelpBox($"Missing build support for {string.Join(", ", buildTargetsMissingBuildSupport)}", MessageType.Error);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure build platforms for worker");

                    environmentConfiguration.ShowBuildPlatforms = showBuildPlatforms;
                    environmentConfiguration.BuildPlatforms = newBuildPlatforms;
                }
            }
        }

        private void Drawer(Rect position, SceneItem item)
        {
            var oldColor = GUI.color;
            if (!item.Exists)
            {
                GUI.color = Color.red;
            }

            var positionWidth = position.width;
            var labelWidth = GUI.skin.toggle.CalcSize(GUIContent.none).x + 5;

            position.width = labelWidth;
            item.Included = EditorGUI.Toggle(position, item.Included);
            position.x += labelWidth;
            position.width = positionWidth - labelWidth;

            EditorGUI.ObjectField(position, item.SceneAsset, typeof(SceneAsset), false);
            GUI.color = oldColor;
        }

        private void DrawSceneList(List<SceneItem> list, bool enableReordering, bool showIndices)
        {
            if (list.Count == 0)
            {
                EditorGUILayout.HelpBox("No items in list", MessageType.Info);
                return;
            }

            var indentLevel = EditorGUI.indentLevel;
            using (IndentLevelScope(-EditorGUI.indentLevel))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var controlRect = EditorGUILayout.GetControlRect(false);

                    using (IndentLevelScope(indentLevel))
                    {
                        controlRect = EditorGUI.IndentedRect(controlRect);
                    }

                    if (showIndices)
                    {
                        var indexContent = new GUIContent(i.ToString());
                        var indexRect = new Rect(controlRect) { width = GUI.skin.label.CalcSize(indexContent).x };

                        GUI.Label(indexRect, indexContent);

                        controlRect.x += indexRect.width + 5;
                        controlRect.width -= indexRect.width + 5;
                    }

                    var drawerRect = new Rect(controlRect);

                    if (enableReordering)
                    {
                        drawerRect.width -= 40;
                    }

                    Drawer(drawerRect, item);

                    if (enableReordering)
                    {
                        var buttonRect = new Rect(controlRect);
                        buttonRect.x = buttonRect.xMax - 40;
                        buttonRect.width = 20;

                        using (new EditorGUI.DisabledScope(i == 0))
                        {
                            if (GUI.Button(buttonRect, MoveUpButtonContents))
                            {
                                SwapInList(list, i, i - 1);
                            }
                        }

                        buttonRect.x += 20;

                        using (new EditorGUI.DisabledScope(i == list.Count - 1))
                        {
                            if (GUI.Button(buttonRect, MoveDownButtonContents))
                            {
                                SwapInList(list, i, i + 1);
                            }
                        }
                    }
                }
            }
        }

        private static void SwapInList<T>(IList<T> list, int indexA, int indexB)
        {
            var temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        private static IDisposable IndentLevelScope(int increment)
        {
            return new EditorGUI.IndentLevelScope(increment);
        }
    }
}
