using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem.Configuration
{
    [CustomEditor(typeof(SpatialOSBuildConfiguration))]
    public class SpatialOSBuildConfigurationEditor : UnityEditor.Editor
    {
        private const int ScreenWidthForHorizontalLayout = 450;

        private SceneAsset[] scenesInAssetDatabase;

        public void OnEnable()
        {
            scenesInAssetDatabase = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>).ToArray();
        }

        private bool scenesChanged;
        private string workerTypeName = "WorkerType";

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
                var workerType = new WorkerPlatform(workerTypeName);
                var exists = workerConfiguration.WorkerBuildConfigurations.Any(x => x.WorkerPlatform == workerType);
                if (!exists)
                {
                    var config = new WorkerBuildConfiguration
                    {
                        WorkerPlatform = new WorkerPlatform(workerTypeName),
                        ScenesForWorker = new SceneAsset[] { },
                        LocalBuildConfig = new BuildEnvironmentConfig()
                        {
                            BuildPlatforms = SpatialBuildPlatforms.Current,
                            BuildOptions = BuildOptions.Development
                        },
                        CloudBuildConfig = new BuildEnvironmentConfig()
                        {
                            BuildPlatforms = SpatialBuildPlatforms.Current
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
                workerConfiguration.UpdateEditorScenesForBuild();
            }
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var platformName = configurationForWorker.WorkerPlatform.ToString();

            EditorGUILayout.BeginHorizontal();
            configurationForWorker.ShowFoldout =
                EditorGUILayout.Foldout(configurationForWorker.ShowFoldout, platformName);

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

        [Flags]
        private enum ReorderableListFlags
        {
            None = 0,
            ShowIndices = 1 << 0,
            EnableReordering = 1 << 1,
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

                        DrawIndentedList(scenesToShowInList,
                            ReorderableListFlags.ShowIndices | ReorderableListFlags.EnableReordering,
                            SceneItem.Drawer);
                    }

                    using (horizontalLayout ? new EditorGUILayout.VerticalScope() : null)
                    {
                        using (horizontalLayout ? IndentLevelScope(-EditorGUI.indentLevel) : null)
                        {
                            EditorGUILayout.LabelField("Exclude");

                            DrawIndentedList(sceneItems,
                                ReorderableListFlags.None,
                                SceneItem.Drawer);
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

        private static TEnum EnumFlagsToggleField<TEnum>(TEnum source) where TEnum : struct, IConvertible
        {
            return EnumFlagsToggleField(source, SimpleToString);
        }

        private static TEnum EnumFlagsToggleField<TEnum>(TEnum source, Func<TEnum, string> nameFunction)
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

                        var newFlag = EditorGUILayout.ToggleLeft(nameFunction(enumValue), hasFlag);

                        if (hasFlag != newFlag)
                        {
                            // the flag has changed, doing an XOR will flip that value
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
                var buildOptionsString = EnumFlagToString(environmentConfiguration.BuildOptions);

                EditorGUI.BeginChangeCheck();

                var showBuildOptions = EditorGUILayout.Foldout(environmentConfiguration.ShowBuildOptions,
                    "Build Options: " + buildOptionsString);

                var newBuildOptions = environmentConfiguration.BuildOptions;

                if (showBuildOptions)
                {
                    newBuildOptions = EnumFlagsToggleField(environmentConfiguration.BuildOptions);
                }

                if ((newBuildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                    (newBuildOptions & BuildOptions.Development) != 0)
                {
                    EditorGUILayout.HelpBox(
                        "\nYou cannot have both EnableHeadlessMode and Development in BuildOptions.\n\n" +
                        "This will crash Unity Editor while building.\n",
                        MessageType.Error);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    // build options have changed

                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure build options for worker");

                    environmentConfiguration.ShowBuildOptions = showBuildOptions;
                    environmentConfiguration.BuildOptions = newBuildOptions;
                }
            }
        }

        private static string EnumFlagToString<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            return EnumFlagToString(value, SimpleToString);
        }

        private static string SimpleToString<TValue>(TValue activeValue) where TValue : IConvertible
        {
            return activeValue.ToString(CultureInfo.InvariantCulture);
        }

        private static string EnumFlagToString<TEnum>(TEnum value, Func<TEnum, string> nameFunction)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum type");
            }

            var enumNonZeroValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Where(options => options.ToInt32(NumberFormatInfo.CurrentInfo) != 0)
                .ToArray();

            var sourceBitValue = value.ToInt32(NumberFormatInfo.CurrentInfo);

            if (sourceBitValue == 0)
            {
                return "None";
            }

            return string.Join(", ",
                enumNonZeroValues
                    .Where(enumValue =>
                        (sourceBitValue & enumValue.ToInt32(NumberFormatInfo.CurrentInfo)) != 0)
                    .Select(nameFunction).ToArray());
        }

        private void ConfigureBuildPlatforms(BuildEnvironmentConfig environmentConfiguration)
        {
            using (IndentLevelScope(1))
            {
                var buildPlatformsString = EnumFlagToString(environmentConfiguration.BuildPlatforms,
                    BuildPlatformToString);

                EditorGUI.BeginChangeCheck();

                var showBuildPlatforms = EditorGUILayout.Foldout(environmentConfiguration.ShowBuildPlatforms,
                    "Build Platforms: " + buildPlatformsString);

                var newBuildPlatforms = environmentConfiguration.BuildPlatforms;

                if (showBuildPlatforms)
                {
                    newBuildPlatforms = EnumFlagsToggleField(environmentConfiguration.BuildPlatforms,
                        BuildPlatformToString);
                }

                var currentAdjustedPlatforms = newBuildPlatforms;

                if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Current) != 0)
                {
                    currentAdjustedPlatforms &= ~SpatialBuildPlatforms.Current;
                    currentAdjustedPlatforms |= WorkerBuilder.GetCurrentBuildPlatform();
                }

                if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Windows32) != 0 &&
                    (currentAdjustedPlatforms & SpatialBuildPlatforms.Windows64) != 0)
                {
                    EditorGUILayout.HelpBox(
                        "\n" + WorkerBuilder.IncompatibleWindowsPlatformsErrorMessage + "\n",
                        MessageType.Error);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    // build platforms have changed
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure build platforms for worker");

                    environmentConfiguration.ShowBuildPlatforms = showBuildPlatforms;
                    environmentConfiguration.BuildPlatforms = newBuildPlatforms;
                }
            }
        }

        private static string BuildPlatformToString(SpatialBuildPlatforms value)
        {
            if (value == SpatialBuildPlatforms.Current)
            {
                return string.Format("Current ({0})", WorkerBuilder.GetCurrentBuildPlatform());
            }

            return value.ToString();
        }

        private static readonly GUIContent AddWorkerTypeButtonContents = new GUIContent("+", "AddWorkerType");
        private static readonly GUIContent RemoveWorkerTypeButtonContents = new GUIContent("-", "RemoveWorkerType");
        private static readonly GUIContent MoveUpButtonContents = new GUIContent("^", "Move item up");
        private static readonly GUIContent MoveDownButtonContents = new GUIContent("v", "Move item down");

        private static void DrawIndentedList<T>(IList<T> list,
            ReorderableListFlags flags, Func<Rect, T, T> drawer)
        {
            if (list.Count == 0)
            {
                EditorGUILayout.HelpBox("No items in list", MessageType.Info);
                return;
            }

            var enableReordering = (flags & ReorderableListFlags.EnableReordering) != 0;
            var showIndices = (flags & ReorderableListFlags.ShowIndices) != 0;
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
                        var indexRect = new Rect(controlRect);
                        indexRect.width = GUI.skin.label.CalcSize(indexContent).x;

                        GUI.Label(indexRect, indexContent);

                        controlRect.x += indexRect.width + 5;
                        controlRect.width -= indexRect.width + 5;
                    }

                    var drawerRect = new Rect(controlRect);

                    if (enableReordering)
                    {
                        drawerRect.width -= 40;
                    }

                    drawer(drawerRect, item);

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
