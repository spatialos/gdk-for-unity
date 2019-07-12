using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.Core.Editor;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CustomEditor(typeof(BuildConfig))]
    internal class BuildConfigEditor : Editor
    {
        internal const string BuildConfigurationMenu = EditorConfig.ParentMenu + "/Build Configuration";

        private class DragAndDropInfo
        {
            public int SourceItemIndex = -1;
            public Rect AllItemsRect = Rect.zero;
            public float ItemHeight;
        }

        private class BuildTargetState
        {
            public int Index;
            public GUIContent[] Choices;
        }

        private class FoldoutState
        {
            public bool Expanded;
            public GUIContent Content;
            public GUIContent Icon;
        }

        private Rect addWorkerButtonRect;
        private DragAndDropInfo sourceDragState;
        private BuildConfigEditorStyle style;
        private int invalidateCachedContent;
        private WorkerChoicePopup workerChooser;
        private UIStateManager stateManager = new UIStateManager();

        private static string[] allWorkers;

        private static readonly Vector2 SmallIconSize = new Vector2(12, 12);

        public void Awake()
        {
            Undo.undoRedoPerformed += () => { invalidateCachedContent++; };
        }

        public override void OnInspectorGUI()
        {
            if (style == null)
            {
                style = new BuildConfigEditorStyle();
            }

            if (allWorkers == null)
            {
                try
                {
                    var guids = AssetDatabase.FindAssets("WorkerMenu");
                    var textFile = guids.Select(AssetDatabase.GUIDToAssetPath)
                        .FirstOrDefault(f => Path.GetExtension(f) == ".txt");
                    if (string.IsNullOrEmpty(textFile))
                    {
                        throw new Exception("Could not find WorkerMenu.txt - you may need to regenerate code.");
                    }

                    allWorkers = File.ReadAllLines(Path.Combine(Application.dataPath, "..", textFile));
                }
                catch (Exception e)
                {
                    allWorkers = new string[0];
                    Debug.LogException(e);
                }
            }

            // Clean up state when drag events end.
            if (sourceDragState != null && Event.current.type == EventType.DragExited)
            {
                sourceDragState.SourceItemIndex = -1;
                sourceDragState = null;
                Repaint();
            }

            var workerConfiguration = (BuildConfig) target;

            BuildConfigEditorStyle.DrawHorizontalLine();

            var configs = workerConfiguration.WorkerBuildConfigurations;
            for (var index = 0; index < configs.Count; index++)
            {
                var workerConfig = configs[index];
                if (!DrawWorkerConfiguration(workerConfig))
                {
                    RecordUndo($"Remove '{workerConfig.WorkerType}'");

                    configs.RemoveAt(index);
                    index--;
                }

                BuildConfigEditorStyle.DrawHorizontalLine();
            }

            using (new EditorGUI.DisabledScope(workerConfiguration.WorkerBuildConfigurations.Count ==
                allWorkers.Length))
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add new worker type"))
                {
                    workerChooser = new WorkerChoicePopup(addWorkerButtonRect, workerConfiguration, allWorkers);
                    PopupWindow.Show(addWorkerButtonRect, workerChooser);
                }

                if (Event.current.type == EventType.Repaint)
                {
                    addWorkerButtonRect = GUILayoutUtility.GetLastRect();

                    // Only add the new worker during the Repaint phase - otherwise you'll see errors due to adding new content at the wrong point of the GUI lifecycle.
                    if (workerChooser != null && workerChooser.Choice != -1)
                    {
                        RecordUndo("Add '{Choices[i]}'");

                        var config = new WorkerBuildConfiguration
                        {
                            WorkerType = workerChooser.Choices[workerChooser.Choice],
                            LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets,
                                WorkerBuildData.GetCurrentBuildTargetConfig()),
                            CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets,
                                WorkerBuildData.GetLinuxBuildTargetConfig())
                        };
                        workerConfiguration.WorkerBuildConfigurations.Add(config);
                        workerChooser = null;
                    }
                }

                GUILayout.FlexibleSpace();
            }

            if (invalidateCachedContent > 0)
            {
                invalidateCachedContent--;
            }
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var workerType = configurationForWorker.WorkerType;

            var foldoutState = stateManager.GetStateObjectOrDefault<FoldoutState>(configurationForWorker.WorkerType.GetHashCode());

            using (new EditorGUILayout.HorizontalScope())
            {
                if (foldoutState.Content == null || invalidateCachedContent > 0)
                {
                    var buildStateIcon = GetBuildConfigurationStateIcon(configurationForWorker);
                    if (buildStateIcon != null)
                    {
                        foldoutState.Icon =
                            new GUIContent(EditorGUIUtility.IconContent(buildStateIcon))
                                { tooltip = "Missing build support for one or more build targets." };
                    }
                    else if (configurationForWorker.CloudBuildConfig.BuildTargets.Any(NeedsAndroidSdk) ||
                        configurationForWorker.LocalBuildConfig.BuildTargets.Any(NeedsAndroidSdk))
                    {
                        foldoutState.Icon =
                            new GUIContent(EditorGUIUtility.IconContent(BuildConfigEditorStyle.BuiltInErrorIcon))
                                { tooltip = "Missing Android SDK installation. Go to Preferences > External Tools to set it up." };
                    }
                    else
                    {
                        foldoutState.Icon = null;
                    }

                    foldoutState.Content = new GUIContent(workerType);
                }

                foldoutState.Expanded = EditorGUILayout.Foldout(foldoutState.Expanded, foldoutState.Content, true);

                GUILayout.FlexibleSpace();
                using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
                {
                    if (foldoutState.Icon != null)
                    {
                        using (new EditorGUIUtility.IconSizeScope(Vector2.zero))
                        {
                            GUILayout.Label(foldoutState.Icon);
                        }
                    }

                    if (GUILayout.Button(style.RemoveWorkerTypeButtonContents, EditorStyles.miniButton))
                    {
                        return false;
                    }
                }
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            using (new EditorGUI.IndentLevelScope())
            {
                if (foldoutState.Expanded)
                {
                    DrawScenesInspectorForWorker(configurationForWorker);
                    EditorGUILayout.Space();
                    DrawEnvironmentInspectorForWorker(configurationForWorker);
                }

                if (check.changed)
                {
                    // Re-evaluate heading.
                    foldoutState.Content = null;
                }
            }

            return true;
        }

        private void DrawScenesInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            DragAndDropInfo dragState;
            var currentEventType = Event.current.type;

            using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Scenes to include (in order)");
                var workerControlId = configurationForWorker.WorkerType.GetHashCode() ^ typeof(DragAndDrop).GetHashCode();
                dragState = stateManager.GetStateObjectOrDefault<DragAndDropInfo>(workerControlId);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(style.AddSceneButtonContents, EditorStyles.miniButton))
                {
                    EditorGUIUtility.ShowObjectPicker<SceneAsset>(null, false, "t:Scene",
                        workerControlId);
                }

                HandleObjectSelectorUpdated(configurationForWorker, workerControlId);
            }

            if (configurationForWorker.ScenesForWorker.Count == 0)
            {
                DrawEmptySceneBox(configurationForWorker, currentEventType);
            }
            else
            {
                int? indexToRemove = null;
                int? targetItemIndex = null;

                if (currentEventType == EventType.Repaint)
                {
                    dragState.AllItemsRect = Rect.zero;
                    dragState.ItemHeight = 0;
                }

                for (var i = 0; i < configurationForWorker.ScenesForWorker.Count; i++)
                {
                    var item = configurationForWorker.ScenesForWorker[i];

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Space(EditorGUI.indentLevel * 16.0f);
                        indexToRemove = DrawSceneItem(i, dragState, item, currentEventType, indexToRemove);

                        var hitRect = new Rect(dragState.AllItemsRect.xMin,
                            dragState.AllItemsRect.yMin + i * (dragState.ItemHeight +
                                EditorGUIUtility.standardVerticalSpacing) -
                            EditorGUIUtility.standardVerticalSpacing / 2.0f, dragState.AllItemsRect.width,
                            dragState.ItemHeight + EditorGUIUtility.standardVerticalSpacing / 2.0f);

                        if (hitRect.Contains(Event.current.mousePosition))
                        {
                            if (i != dragState.SourceItemIndex)
                            {
                                targetItemIndex = Event.current.mousePosition.y >
                                    hitRect.yMin + hitRect.height / 2
                                        ? i + 1
                                        : i;
                            }

                            TrackDragDrop(configurationForWorker, currentEventType, item, dragState, i);
                        }
                    }
                }

                List<SceneAsset> list = null;

                if (indexToRemove.HasValue)
                {
                    list = configurationForWorker.ScenesForWorker.ToList();
                    list.RemoveAt(indexToRemove.Value);
                }
                else if (targetItemIndex.HasValue)
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
                                    list.InsertRange(targetItemIndex.Value,
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
                                        list.Insert(targetItemIndex.Value - 1, movingItem);
                                    }
                                    else
                                    {
                                        list.Insert(targetItemIndex.Value, movingItem);
                                    }
                                }
                            }

                            break;
                        case EventType.Repaint:
                            if (DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
                            {
                                var newRect = new Rect(dragState.AllItemsRect.xMin,
                                    dragState.AllItemsRect.yMin + targetItemIndex.Value *
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

                if (list != null)
                {
                    RecordUndo("Configure scenes for worker");

                    configurationForWorker.ScenesForWorker = list;
                }
            }
        }

        private void TrackDragDrop(WorkerBuildConfiguration configurationForWorker, EventType currentEventType,
            SceneAsset item,
            DragAndDropInfo dragState, int itemIndex)
        {
            switch (currentEventType)
            {
                case EventType.MouseDrag:
                    DragAndDrop.PrepareStartDrag();

                    DragAndDrop.objectReferences = new[] { item };
                    DragAndDrop.paths = new[] { AssetDatabase.GetAssetPath(item) };

                    DragAndDrop.StartDrag(item.name);

                    dragState.SourceItemIndex = itemIndex;
                    Event.current.Use();
                    Repaint();

                    sourceDragState = dragState;

                    break;
                case EventType.DragPerform:
                    Event.current.Use();
                    DragAndDrop.AcceptDrag();
                    Repaint();
                    break;
                case EventType.MouseUp: // Fall through.
                case EventType.DragExited:
                    sourceDragState = null;
                    dragState.SourceItemIndex = -1;
                    Repaint();
                    break;
                case EventType.DragUpdated:
                    var sceneAssets = DragAndDrop.objectReferences.OfType<SceneAsset>()
                        .ToList();
                    if (sceneAssets.Any())
                    {
                        if (dragState.SourceItemIndex == -1 &&
                            sceneAssets.Intersect(configurationForWorker.ScenesForWorker).Any())
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

        private int? DrawSceneItem(int itemIndex, DragAndDropInfo dragState, SceneAsset item,
            EventType currentEventType,
            int? indexToRemove)
        {
            using (new GUIColorScope(
                itemIndex == dragState.SourceItemIndex
                    ? new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.25f)
                    : GUI.color))
            {
                var rowRect = Rect.zero;
                var content = EditorGUIUtility.ObjectContent(item, typeof(SceneAsset));

                // Reserve space for the handle, draw it later.
                var grabberRect = GUILayoutUtility.GetRect(new GUIContent(content.image),
                    EditorStyles.helpBox,
                    GUILayout.MinWidth(16), GUILayout.MinHeight(16));

                grabberRect.min = new Vector2(grabberRect.min.x, grabberRect.min.y + 4);
                grabberRect.max = new Vector2(grabberRect.max.x, grabberRect.max.y + 4);

                BuildConfigEditorStyle.DrawGrabber(grabberRect);

                using (new EditorGUIUtility.IconSizeScope(new Vector2(24, 24)))
                {
                    GUILayout.Label(content);
                }

                if (currentEventType == EventType.Repaint)
                {
                    rowRect = BuildConfigEditorStyle.RectUnion(grabberRect, GUILayoutUtility.GetLastRect());
                }

                GUILayout.FlexibleSpace();
                using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
                {
                    if (GUILayout.Button(style.RemoveSceneButtonContents, EditorStyles.miniButton))
                    {
                        indexToRemove = itemIndex;
                    }
                }

                if (currentEventType == EventType.Repaint)
                {
                    rowRect = BuildConfigEditorStyle.RectUnion(rowRect, GUILayoutUtility.GetLastRect());
                    dragState.AllItemsRect = BuildConfigEditorStyle.RectUnion(dragState.AllItemsRect, rowRect);
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
                        RecordUndo("Configure scenes for worker");

                        configurationForWorker.ScenesForWorker = DragAndDrop.objectReferences
                            .OfType<SceneAsset>().ToList();

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

        private void HandleObjectSelectorUpdated(WorkerBuildConfiguration configurationForWorker, int pickerId)
        {
            if (Event.current.commandName == "ObjectSelectorClosed" &&
                EditorGUIUtility.GetObjectPickerControlID() == pickerId)
            {
                var scene = (SceneAsset) EditorGUIUtility.GetObjectPickerObject();

                if (scene == null)
                {
                    return;
                }

                if (configurationForWorker.ScenesForWorker.All(a => a.name != scene.name))
                {
                    RecordUndo("Configure scenes for worker");

                    configurationForWorker.ScenesForWorker.Add(scene);
                }
            }
        }

        private void DrawEnvironmentInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            DrawEnvironmentInspector(BuildEnvironment.Local, configurationForWorker);

            EditorGUILayout.Space();

            DrawEnvironmentInspector(BuildEnvironment.Cloud, configurationForWorker);
        }


        private void DrawEnvironmentInspector(BuildEnvironment environment,
            WorkerBuildConfiguration configurationForWorker)
        {
            var environmentName = environment.ToString();

            var environmentConfiguration =
                configurationForWorker.GetEnvironmentConfig(environment);

            var hash = configurationForWorker.WorkerType.GetHashCode() ^ environment.GetHashCode() ^ typeof(FoldoutState).GetHashCode();
            var foldoutState = stateManager.GetStateObjectOrDefault<FoldoutState>(hash);

            if (foldoutState.Content == null || invalidateCachedContent > 0)
            {
                var builtTargets = string.Join(",",
                    environmentConfiguration.BuildTargets.Where(t => t.Enabled).Select(t => t.Label));

                foldoutState.Content = new GUIContent($"{environmentName} Build Options ({builtTargets})");

                if (environmentConfiguration.BuildTargets.Any(IsBuildTargetError))
                {
                    foldoutState.Icon =
                        new GUIContent(EditorGUIUtility.IconContent(BuildConfigEditorStyle.BuiltInErrorIcon))
                            { tooltip = "Missing build support for one or more build targets." };
                }
                else if (environmentConfiguration.BuildTargets.Any(IsBuildTargetWarning))
                {
                    foldoutState.Icon =
                        new GUIContent(EditorGUIUtility.IconContent(BuildConfigEditorStyle.BuiltInWarningIcon))
                            { tooltip = "Missing build support for one or more build targets." };
                }
                else
                {
                    foldoutState.Icon = null;
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                foldoutState.Expanded =
                    EditorGUILayout.Foldout(foldoutState.Expanded, foldoutState.Content, true);

                GUILayout.FlexibleSpace();
                if (foldoutState.Icon != null)
                {
                    GUILayout.Label(foldoutState.Icon);
                    GUILayout.Space(28);
                }
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldoutState.Expanded)
                {
                    DrawBuildTargets(environmentConfiguration, hash);
                }

                if (check.changed)
                {
                    // Re-evaluate heading.
                    foldoutState.Content = null;
                }
            }
        }

        private GUIContent GetBuildTargetGuiContents(BuildTargetConfig c)
        {
            if (IsBuildTargetError(c))
            {
                return style.BuildErrorIcons[c.Target];
            }

            if (IsBuildTargetWarning(c))
            {
                return style.BuildWarningIcons[c.Target];
            }

            return style.BuildTargetText[c.Target];
        }

        private void DrawBuildTargets(BuildEnvironmentConfig env, int hash)
        {
            // Init cached UI state.
            var selectedBuildTarget = stateManager.GetStateObjectOrDefault<BuildTargetState>(hash ^ typeof(BuildTargetState).GetHashCode());

            if (selectedBuildTarget.Choices == null || invalidateCachedContent > 0)
            {
                selectedBuildTarget.Choices = env.BuildTargets.Select(GetBuildTargetGuiContents).ToArray();
            }

            // Draw available build targets.
            using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
            {
                selectedBuildTarget.Index =
                    GUILayout.Toolbar(selectedBuildTarget.Index, selectedBuildTarget.Choices);
            }

            // Draw selected build target.
            var buildTarget = env.BuildTargets[selectedBuildTarget.Index];
            var canBuildTarget = WorkerBuildData.BuildTargetsThatCanBeBuilt[buildTarget.Target];

            var options = buildTarget.Options;
            var enabled = buildTarget.Enabled;
            var required = buildTarget.Required;

            using (var check = new EditorGUI.ChangeCheckScope())
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
            {
                BuildConfigEditorStyle.DrawHorizontalLine();
                EditorGUILayout.Space();

                enabled = EditorGUILayout.Toggle("Build", enabled);

                if (!enabled)
                {
                    required = false;
                }

                using (new EditorGUI.DisabledScope(!buildTarget.Enabled))
                {
                    required = EditorGUILayout.Toggle("Required", required);

                    switch (buildTarget.Target)
                    {
                        case BuildTarget.StandaloneOSX:
                            options = ConfigureOSX(buildTarget);
                            break;
                        case BuildTarget.StandaloneWindows:
                            options = ConfigureWindows(buildTarget);
                            break;
                        case BuildTarget.iOS:
                            options = ConfigureIOS(buildTarget);
                            break;
                        case BuildTarget.Android:
                            options = ConfigureAndroid(buildTarget);
                            break;
                        case BuildTarget.StandaloneWindows64:
                            options = ConfigureWindows(buildTarget);
                            break;
                        case BuildTarget.StandaloneLinux64:
                            options = ConfigureLinux(buildTarget);
                            break;
                    }

                    options = ConfigureCompression(options);
                }

                if (!canBuildTarget)
                {
                    EditorGUILayout.HelpBox(
                        $"Your Unity Editor is missing build support for {buildTarget.Target.ToString()}.\n" +
                        "Please add the missing build support options to your Unity Editor",
                        buildTarget.Required ? MessageType.Error : MessageType.Warning);
                }

                if (check.changed)
                {
                    RecordUndo("Worker build options");

                    env.BuildTargets[selectedBuildTarget.Index] =
                        new BuildTargetConfig(buildTarget.Target, options, enabled, required);

                    selectedBuildTarget.Choices = null;
                }
            }
        }

        private BuildOptions ConfigureCompression(BuildOptions options)
        {
            var choice = 0;
            if (options.HasFlag(BuildOptions.CompressWithLz4))
            {
                choice = 1;
            }
            else if (options.HasFlag(BuildOptions.CompressWithLz4HC))
            {
                choice = 2;
            }

            choice = EditorGUILayout.Popup("Compression", choice, style.CompressionOptions);

            switch (choice)
            {
                case 0:
                    options &= ~(BuildOptions.CompressWithLz4 | BuildOptions.CompressWithLz4HC);
                    break;
                case 1:
                    options |= BuildOptions.CompressWithLz4;
                    break;
                case 2:
                    options |= BuildOptions.CompressWithLz4HC;
                    break;
            }

            return options;
        }

        private BuildOptions ConfigureLinux(BuildTargetConfig buildTarget)
        {
            // NB: On Linux, headless and Development mode are mutually exclusive.
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Server build", options.HasFlag(BuildOptions.EnableHeadlessMode)))
            {
                options |= BuildOptions.EnableHeadlessMode;
                options &= ~BuildOptions.Development;
            }
            else
            {
                options &= ~BuildOptions.EnableHeadlessMode;
            }

            if (EditorGUILayout.Toggle("Development", options.HasFlag(BuildOptions.Development)))
            {
                options |= BuildOptions.Development;
                options &= ~BuildOptions.EnableHeadlessMode;
            }
            else
            {
                options &= ~BuildOptions.Development;
            }

            return options;
        }

        private BuildOptions ConfigureAndroid(BuildTargetConfig buildTarget)
        {
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Development", options.HasFlag(BuildOptions.Development)))
            {
                options |= BuildOptions.Development;
            }
            else
            {
                options &= ~BuildOptions.Development;
            }

            return options;
        }

        private BuildOptions ConfigureIOS(BuildTargetConfig buildTarget)
        {
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Development", options.HasFlag(BuildOptions.Development)))
            {
                options |= BuildOptions.Development;
            }
            else
            {
                options &= ~BuildOptions.Development;
            }

            return options;
        }

        private BuildOptions ConfigureOSX(BuildTargetConfig buildTarget)
        {
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Development", options.HasFlag(BuildOptions.Development)))
            {
                options |= BuildOptions.Development;
            }
            else
            {
                options &= ~BuildOptions.Development;
            }

            if (EditorGUILayout.Toggle("Server build", options.HasFlag(BuildOptions.EnableHeadlessMode)))
            {
                options |= BuildOptions.EnableHeadlessMode;
            }
            else
            {
                options &= ~BuildOptions.EnableHeadlessMode;
            }

            return options;
        }

        private BuildOptions ConfigureWindows(BuildTargetConfig buildTarget)
        {
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Development", options.HasFlag(BuildOptions.Development)))
            {
                options |= BuildOptions.Development;
            }
            else
            {
                options &= ~BuildOptions.Development;
            }

            if (EditorGUILayout.Toggle("Server build", options.HasFlag(BuildOptions.EnableHeadlessMode)))
            {
                options |= BuildOptions.EnableHeadlessMode;
            }
            else
            {
                options &= ~BuildOptions.EnableHeadlessMode;
            }

            return options;
        }

        private void RecordUndo(string action)
        {
            EditorUtility.SetDirty(target);
            Undo.RecordObject(target, action);
            invalidateCachedContent++;
        }

        private static bool IsBuildTargetWarning(BuildTargetConfig t)
        {
            return !WorkerBuildData.BuildTargetsThatCanBeBuilt[t.Target] && t.Enabled;
        }

        private static bool IsBuildTargetError(BuildTargetConfig t)
        {
            return !WorkerBuildData.BuildTargetsThatCanBeBuilt[t.Target] && t.Required;
        }

        private static string GetBuildConfigurationStateIcon(WorkerBuildConfiguration configuration)
        {
            var isWarning = configuration.CloudBuildConfig.BuildTargets.Any(IsBuildTargetWarning) ||
                configuration.LocalBuildConfig.BuildTargets.Any(IsBuildTargetWarning);

            var isError = configuration.CloudBuildConfig.BuildTargets.Any(IsBuildTargetError) ||
                configuration.LocalBuildConfig.BuildTargets.Any(IsBuildTargetError);

            if (isError)
            {
                return BuildConfigEditorStyle.BuiltInErrorIcon;
            }

            if (isWarning)
            {
                return BuildConfigEditorStyle.BuiltInWarningIcon;
            }

            return null;
        }


        private static bool NeedsAndroidSdk(BuildTargetConfig t)
        {
            return t.Enabled && t.Target == BuildTarget.Android && string.IsNullOrEmpty(EditorPrefs.GetString("AndroidSdkRoot"));
        }
    }
}
