using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CustomEditor(typeof(SpatialOSBuildConfiguration))]
    public class SpatialOSBuildConfigurationEditor : Editor
    {
        private string workerTypeName = "NewWorkerType";

        private static readonly GUIContent AddWorkerTypeButtonContents = new GUIContent("+", "Add worker type");
        private static readonly GUIContent RemoveWorkerTypeButtonContents = new GUIContent("-", "Remove worker type");
        private static readonly GUIContent AddSceneButtonContents = new GUIContent("+", "Add scene");
        private static readonly GUIContent RemoveSceneButtonContents = new GUIContent("-", "Remove scene");

        public int CurrentObjectPickerWindowId = -1;

        private class DragAndDropInfo
        {
            public int SourceItemIndex = -1;
            public Rect AllItemsRect = Rect.zero;
            public float ItemHeight;
        }

        private static readonly HashSet<string> ExpandedWorkers = new HashSet<string>();
        private static readonly HashSet<string> ExpandedBuildOptions = new HashSet<string>();
        private static readonly HashSet<string> ExpandedBuildPlatforms = new HashSet<string>();

        public override void OnInspectorGUI()
        {
            var workerConfiguration = (SpatialOSBuildConfiguration) target;

            using (new EditorGUILayout.HorizontalScope())
            {
                workerTypeName = EditorGUILayout.TextField(workerTypeName);

                GUILayout.FlexibleSpace();

                var canAddWorker = string.IsNullOrEmpty(workerTypeName) ||
                    workerConfiguration.WorkerBuildConfigurations.Any(c =>
                        c.WorkerType.Equals(workerTypeName, StringComparison.InvariantCultureIgnoreCase));

                using (new EditorGUI.DisabledScope(canAddWorker))
                {
                    if (GUILayout.Button(AddWorkerTypeButtonContents, EditorStyles.miniButton))
                    {
                        EditorUtility.SetDirty(target);
                        Undo.RecordObject(target, $"Add '{workerTypeName}'");

                        var config = new WorkerBuildConfiguration
                        {
                            WorkerType = workerTypeName,
                            LocalBuildConfig = new BuildEnvironmentConfig
                            {
                                BuildOptions = BuildOptions.Development,
                            },
                            CloudBuildConfig = new BuildEnvironmentConfig
                            {
                            }
                        };
                        workerConfiguration.WorkerBuildConfigurations.Add(config);
                    }
                }
            }

            DrawHorizontalLine();

            var configs = workerConfiguration.WorkerBuildConfigurations;
            foreach (var workerConfig in configs)
            {
                if (!DrawWorkerConfiguration(workerConfig))
                {
                    Undo.RecordObject(target, $"Remove '{workerConfig.WorkerType}'");

                    configs.Remove(workerConfig);
                    break;
                }

                DrawHorizontalLine();
            }
        }

        private static void DrawHorizontalLine()
        {
            var rect = EditorGUILayout.GetControlRect(false, 2, EditorStyles.foldout);
            using (new Handles.DrawingScope(new Color(0.3f, 0.3f, 0.3f, 1)))
            {
                Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            }

            GUILayout.Space(rect.height);
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var workerType = configurationForWorker.WorkerType;
            var expanded = ExpandedWorkers.Contains(configurationForWorker.WorkerType);
            bool currentExpanded;

            var localProblems = WorkerBuildConfiguration
                .GetConfigurationProblems(BuildEnvironment.Local, configurationForWorker);

            var cloudProblems = WorkerBuildConfiguration
                .GetConfigurationProblems(BuildEnvironment.Cloud, configurationForWorker);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUIContent content;
                if (localProblems.Count > 0 || cloudProblems.Count > 0)
                {
                    content = EditorGUIUtility.IconContent("console.erroricon.sml", "|Problems found");
                    content.text = workerType;
                }
                else
                {
                    content = new GUIContent(workerType);
                }

                currentExpanded =
                    EditorGUILayout.Foldout(expanded, content);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(RemoveWorkerTypeButtonContents, EditorStyles.miniButton))
                {
                    ExpandedWorkers.Remove(configurationForWorker.WorkerType);
                    return false;
                }
            }

            if (expanded && !currentExpanded)
            {
                ExpandedWorkers.Remove(configurationForWorker.WorkerType);
            }
            else if (!expanded && currentExpanded)
            {
                ExpandedWorkers.Add(configurationForWorker.WorkerType);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                if (currentExpanded)
                {
                    DrawScenesInspectorForWorker(configurationForWorker);
                    EditorGUILayout.Space();
                    DrawEnvironmentInspectorForWorker(configurationForWorker, localProblems, cloudProblems);
                }
            }

            return true;
        }

        private static Rect RectUnion(Rect a, Rect b)
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

        private static Rect RectExpand(Rect a, Vector2 amount)
        {
            return new Rect(a.xMin - amount.x, a.yMin - amount.y, a.width + amount.x * 2, a.height + amount.y * 2);
        }

        private void DrawScenesInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            DragAndDropInfo dragState;
            var currentEventType = Event.current.type;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Scenes to include (in order)");
                var workerControlId = GUIUtility.GetControlID(FocusType.Passive);
                dragState = (DragAndDropInfo) GUIUtility.GetStateObject(typeof(DragAndDropInfo), workerControlId);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(AddSceneButtonContents, EditorStyles.miniButton))
                {
                    CurrentObjectPickerWindowId = GUIUtility.GetControlID(FocusType.Passive);
                    EditorGUIUtility.ShowObjectPicker<SceneAsset>(null, false, "t:Scene",
                        CurrentObjectPickerWindowId);
                }

                HandleObjectSelectorUpdated(configurationForWorker);
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (configurationForWorker.ScenesForWorker.Length == 0)
                {
                    DrawEmptySceneBox(configurationForWorker, currentEventType);
                }
                else
                {
                    var indexToRemove = -1;
                    var targetItemIndex = -1;

                    if (currentEventType == EventType.Repaint)
                    {
                        dragState.AllItemsRect = Rect.zero;
                        dragState.ItemHeight = 0;
                    }

                    for (var i = 0; i < configurationForWorker.ScenesForWorker.Length; i++)
                    {
                        var item = configurationForWorker.ScenesForWorker[i];

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            indexToRemove = DrawSceneItem(i, dragState, item, currentEventType, indexToRemove);

                            var hitRect = new Rect(dragState.AllItemsRect.xMin,
                                dragState.AllItemsRect.yMin + (i * (dragState.ItemHeight +
                                    EditorGUIUtility.standardVerticalSpacing)) -
                                EditorGUIUtility.standardVerticalSpacing / 2.0f, dragState.AllItemsRect.width,
                                dragState.ItemHeight + EditorGUIUtility.standardVerticalSpacing / 2.0f);

                            if (hitRect.Contains(Event.current.mousePosition))
                            {
                                if (i != dragState.SourceItemIndex)
                                {
                                    targetItemIndex = (Event.current.mousePosition.y >
                                        hitRect.yMin + hitRect.height / 2)
                                        ? i + 1
                                        : i;
                                }

                                TrackDragDrop(configurationForWorker, currentEventType, item, dragState, i);
                            }
                        }
                    }

                    List<SceneAsset> list = null;

                    if (indexToRemove != -1)
                    {
                        list = configurationForWorker.ScenesForWorker.ToList();
                        list.RemoveAt(indexToRemove);
                    }
                    else if (targetItemIndex >= 0)
                    {
                        list = configurationForWorker.ScenesForWorker.ToList();

                        switch (currentEventType)
                        {
                            case EventType.DragPerform:

                                // The drag event is coming from outside of this list, for example:
                                // The asset browser or another worker's scene list.
                                // If the incoming drag contains a duplicate of the item, it's already been rejected in the hit detection code,
                                // so there's no need to validate it again here.
                                if (dragState.SourceItemIndex == -1)
                                {
                                    if (targetItemIndex >= list.Count)
                                    {
                                        list.AddRange(DragAndDrop.objectReferences.OfType<SceneAsset>());
                                    }
                                    else
                                    {
                                        list.InsertRange(targetItemIndex,
                                            DragAndDrop.objectReferences.OfType<SceneAsset>());
                                    }
                                }
                                else
                                {
                                    var movingItem = list[dragState.SourceItemIndex];

                                    if (targetItemIndex >= list.Count)
                                    {
                                        list.RemoveAt(dragState.SourceItemIndex);
                                        list.Add(movingItem);
                                    }
                                    else
                                    {
                                        list.RemoveAt(dragState.SourceItemIndex);
                                        if (targetItemIndex >= dragState.SourceItemIndex)
                                        {
                                            list.Insert(targetItemIndex - 1, movingItem);
                                        }
                                        else
                                        {
                                            list.Insert(targetItemIndex, movingItem);
                                        }
                                    }
                                }

                                break;
                            case EventType.DragExited:
                                dragState.SourceItemIndex = -1;
                                Repaint();
                                break;
                            case EventType.Repaint:
                                if (DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
                                {
                                    var newRect = new Rect(dragState.AllItemsRect.xMin,
                                        dragState.AllItemsRect.yMin + targetItemIndex *
                                        dragState.AllItemsRect.height / list.Count,
                                        dragState.AllItemsRect.width, 2);
                                    EditorGUI.DrawRect(newRect, new Color(0.4f, 0.4f, 0.4f, 1));
                                }
                                else if (DragAndDrop.visualMode == DragAndDropVisualMode.Rejected)
                                {
                                    var newRect = new Rect(dragState.AllItemsRect);
                                    EditorGUI.DrawRect(newRect, new Color(0.8f, 0.0f, 0.0f, 0.25f));
                                }

                                break;
                        }
                    }

                    if (check.changed || list != null)
                    {
                        Undo.RecordObject(target, "Configure scenes for worker");

                        configurationForWorker.ScenesForWorker = list.ToArray();
                    }
                }
            }
        }

        private void TrackDragDrop(WorkerBuildConfiguration configurationForWorker, EventType currentEventType,
            SceneAsset item,
            DragAndDropInfo dragState, int i)
        {
            switch (currentEventType)
            {
                case EventType.MouseDrag:
                    DragAndDrop.PrepareStartDrag();

                    DragAndDrop.objectReferences = new[] { item };
                    DragAndDrop.paths = new[] { AssetDatabase.GetAssetPath(item) };

                    DragAndDrop.StartDrag(item.name);

                    dragState.SourceItemIndex = i;
                    Event.current.Use();
                    Repaint();
                    break;
                case EventType.DragPerform:
                    Event.current.Use();
                    DragAndDrop.AcceptDrag();
                    Repaint();
                    break;
                case EventType.MouseUp: // Fall through.
                case EventType.DragExited:
                    dragState.SourceItemIndex = -1;
                    Repaint();
                    break;
                case EventType.DragUpdated:
                    var sceneAssets = DragAndDrop.objectReferences.OfType<SceneAsset>()
                        .ToList();
                    if (sceneAssets.Any())
                    {
                        if (dragState.SourceItemIndex == -1 &&
                            new HashSet<SceneAsset>(sceneAssets).Overlaps(configurationForWorker
                                .ScenesForWorker))
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        }
                        else
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        }

                        Event.current.Use();
                        Repaint();
                    }

                    break;
            }
        }

        private static int DrawSceneItem(int i, DragAndDropInfo dragState, SceneAsset item, EventType currentEventType,
            int indexToRemove)
        {
            using (new ScopedGUIColor(
                i == dragState.SourceItemIndex
                    ? new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.25f)
                    : GUI.color))
            {
                var rowRect = Rect.zero;
                var content = EditorGUIUtility.ObjectContent(item, typeof(SceneAsset));

                // Reserve space for the handle, draw it later.
                var grabberRect = GUILayoutUtility.GetRect(new GUIContent(content.image),
                    EditorStyles.textField,
                    GUILayout.MinWidth(16), GUILayout.MinHeight(16));

                DrawGrabber(grabberRect);

                using (new EditorGUIUtility.IconSizeScope(new Vector2(24, 24)))
                {
                    GUILayout.Label(content);
                }

                if (currentEventType == EventType.Repaint)
                {
                    rowRect = RectUnion(grabberRect, GUILayoutUtility.GetLastRect());
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(RemoveSceneButtonContents, EditorStyles.miniButton))
                {
                    indexToRemove = i;
                }

                if (currentEventType == EventType.Repaint)
                {
                    rowRect = RectUnion(rowRect, GUILayoutUtility.GetLastRect());
                    dragState.AllItemsRect = RectUnion(dragState.AllItemsRect, rowRect);
                    dragState.ItemHeight = rowRect.height;
                }
            }

            return indexToRemove;
        }

        private void DrawEmptySceneBox(WorkerBuildConfiguration configurationForWorker, EventType currentEventType)
        {
            // Allow dropping to form a new list.
            EditorGUILayout.HelpBox("Drop scenes here", MessageType.Info);
            var rect = GUILayoutUtility.GetLastRect();
            if (rect.Contains(Event.current.mousePosition))
            {
                switch (currentEventType)
                {
                    case EventType.DragPerform:
                        Undo.RecordObject(target, "Configure scenes for worker");

                        configurationForWorker.ScenesForWorker = DragAndDrop.objectReferences
                            .OfType<SceneAsset>().ToArray();

                        DragAndDrop.AcceptDrag();
                        Event.current.Use();
                        Repaint();

                        break;
                    case EventType.DragUpdated:
                        if (DragAndDrop.objectReferences.OfType<SceneAsset>().Any())
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            Event.current.Use();
                            Repaint();
                        }

                        break;
                    case EventType.Repaint:
                        if (DragAndDrop.objectReferences.OfType<SceneAsset>().Any())
                        {
                            EditorGUI.DrawRect(rect, new Color(0, 0.8f, 0, 0.25f));
                        }

                        break;
                }
            }
        }

        private static void DrawGrabber(Rect grabberRect)
        {
            using (new Handles.DrawingScope(new Color(0.4f, 0.4f, 0.4f, 1)))
            {
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.25f)),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.25f)));
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height / 2),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2));
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.75f)),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.75f)));
            }

            using (new Handles.DrawingScope(new Color(0.3f, 0.3f, 0.3f, 1)))
            {
                Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.25f) + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.25f) + 1));
                Handles.DrawLine(new Vector2(grabberRect.xMin,
                        grabberRect.yMin + grabberRect.height / 2 + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2 + 1));
                Handles.DrawLine(new Vector2(grabberRect.xMin,
                        grabberRect.yMin + (grabberRect.height * 0.75f) + 1),
                    new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.75f) + 1));
            }
        }

        private void HandleObjectSelectorUpdated(WorkerBuildConfiguration configurationForWorker)
        {
            if (Event.current.commandName == "ObjectSelectorClosed" &&
                EditorGUIUtility.GetObjectPickerControlID() == CurrentObjectPickerWindowId)
            {
                CurrentObjectPickerWindowId = -1;
                var scene = (SceneAsset) EditorGUIUtility.GetObjectPickerObject();

                if (scene == null)
                {
                    return;
                }

                if (configurationForWorker.ScenesForWorker.All(a => a.name != scene.name))
                {
                    var list = configurationForWorker.ScenesForWorker.ToList();
                    list.Add(scene);
                    configurationForWorker.ScenesForWorker = list.ToArray();
                }
            }
        }

        private void DrawEnvironmentInspectorForWorker(WorkerBuildConfiguration configurationForWorker,
            List<WorkerBuildConfiguration.Problem> localProblems, List<WorkerBuildConfiguration.Problem> cloudProblems)
        {
            DrawEnvironmentInspector(BuildEnvironment.Local, configurationForWorker, localProblems);

            EditorGUILayout.Space();

            DrawEnvironmentInspector(BuildEnvironment.Cloud, configurationForWorker, cloudProblems);
        }

        private void DrawConfigurationProblems(List<WorkerBuildConfiguration.Problem> problems)
        {
            foreach (var problem in problems)
            {
                EditorGUILayout.HelpBox(problem.Message, problem.Type);
            }
        }

        private static TEnum EnumFlagsToggleField<TEnum>(TEnum source, Func<TEnum, bool> disableFunc,
            Action<TEnum> drawFunc)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum type");
            }

            var enumNonZeroValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Where(options => options.ToInt32(NumberFormatInfo.CurrentInfo) != 0)
                .ToArray();

            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    var sourceBitValue = source.ToInt32(NumberFormatInfo.CurrentInfo);
                    foreach (var enumValue in enumNonZeroValues)
                    {
                        var targetBitValue = enumValue.ToInt32(NumberFormatInfo.CurrentInfo);
                        var hasFlag = (sourceBitValue & targetBitValue) != 0;
                        var label = new GUIContent(enumValue.ToString(CultureInfo.InvariantCulture));

                        using (new EditorGUI.DisabledGroupScope(disableFunc(enumValue)))
                        {
                            var newFlag =
                                EditorGUILayout.ToggleLeft(label, hasFlag);
                            if (hasFlag != newFlag)
                            {
                                source = (TEnum) (object) (sourceBitValue ^ targetBitValue);
                            }

                            drawFunc(enumValue);
                        }
                    }
                }
            }

            return source;
        }

        private void DrawEnvironmentInspector(BuildEnvironment environment,
            WorkerBuildConfiguration configurationForWorker, List<WorkerBuildConfiguration.Problem> problems)
        {
            var environmentName = environment.ToString();
            var foldoutName = $"{configurationForWorker.WorkerType} {environmentName}";

            var environmentConfiguration =
                configurationForWorker.GetEnvironmentConfig(environment);

            var buildPlatformsString = SelectedPlatformsToString(environmentConfiguration.BuildPlatforms);

            GUIContent content;
            if (problems.Count > 0)
            {
                content = EditorGUIUtility.IconContent("console.erroricon.sml", "|Problems found");
                content.text = $"{environmentName} Build Options (Problems found)";
            }
            else
            {
                content = new GUIContent($"{environmentName} Build Options");
            }

            if (EditorGUILayout.Foldout(ExpandedBuildOptions.Contains(foldoutName), content))
            {
                ExpandedBuildOptions.Add(foldoutName);
            }
            else
            {
                ExpandedBuildOptions.Remove(foldoutName);
            }

            if (EditorGUILayout.Foldout(ExpandedBuildPlatforms.Contains(foldoutName),
                $"{environmentName} Build Platforms: <b>{buildPlatformsString}</b>",
                new GUIStyle(EditorStyles.foldout) { richText = true }))
            {
                ConfigureBuildPlatforms(environmentConfiguration);
                ExpandedBuildPlatforms.Add($"{foldoutName}");
            }
            else
            {
                ExpandedBuildPlatforms.Remove($"{foldoutName}");
            }

            DrawConfigurationProblems(problems);
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
                .Where(enumValue => value.HasFlag(enumValue))
                .Select(BuildPlatformToString).ToArray());
        }

        private void ConfigureBuildPlatforms(BuildEnvironmentConfig environmentConfiguration)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var newBuildOptions = environmentConfiguration.BuildOptions;
                var prevDevelopmentFlagValue = newBuildOptions.HasFlag(BuildOptions.Development);
                var newDevelopmentFlagValue = EditorGUILayout.ToggleLeft("Development Build", prevDevelopmentFlagValue);
                if (prevDevelopmentFlagValue != newDevelopmentFlagValue)
                {
                    newBuildOptions ^= BuildOptions.Development;
                }

                DrawHorizontalLine();

                var newBuildPlatforms = EnumFlagsToggleField(environmentConfiguration.BuildPlatforms, platform =>
                    {
                        // Manage mutually-exclusive platforms.
                        var current = WorkerBuilder.GetCurrentBuildPlatform();

                        var platforms = environmentConfiguration.BuildPlatforms;
                        if (platforms.HasFlag(SpatialBuildPlatforms.Current))
                        {
                            platforms |= current;
                        }

                        if (platform == SpatialBuildPlatforms.Current &&
                            current == SpatialBuildPlatforms.Windows64 &&
                            platforms.HasFlag(SpatialBuildPlatforms.Windows32))
                        {
                            return true;
                        }

                        return platforms.HasFlag(SpatialBuildPlatforms.Windows32) &&
                            platform == SpatialBuildPlatforms.Windows64 ||
                            platforms.HasFlag(SpatialBuildPlatforms.Windows64) &&
                            platform == SpatialBuildPlatforms.Windows32;
                    },
                    platform =>
                    {
                        if (platform == SpatialBuildPlatforms.Linux)
                        {
                            using (new EditorGUI.DisabledScope(
                                !environmentConfiguration.BuildPlatforms.HasFlag(SpatialBuildPlatforms.Linux)))
                            using (new EditorGUI.IndentLevelScope())
                            {
                                var prevValue = environmentConfiguration.BuildOptions.HasFlag(BuildOptions.EnableHeadlessMode);
                                var newValue = EditorGUILayout.ToggleLeft("Headless Mode", prevValue);
                                if (prevValue != newValue)
                                {
                                    newBuildOptions ^= BuildOptions.EnableHeadlessMode;
                                    if (newValue)
                                    {
                                        newBuildOptions &= ~BuildOptions.Development;
                                    }
                                }
                            }
                        }
                    });

                // Headless mode only has meaning for Linux players.
                if (!newBuildPlatforms.HasFlag(SpatialBuildPlatforms.Linux) &&
                    newBuildOptions.HasFlag(BuildOptions.EnableHeadlessMode))
                {
                    newBuildOptions &= ~BuildOptions.EnableHeadlessMode;
                }

                if (check.changed)
                {
                    Undo.RecordObject(target, "Configure build for worker");
                    environmentConfiguration.BuildOptions = newBuildOptions;
                    environmentConfiguration.BuildPlatforms = newBuildPlatforms;
                }
            }
        }
    }
}
