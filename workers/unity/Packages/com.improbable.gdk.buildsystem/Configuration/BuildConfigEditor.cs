using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : Editor
    {
        private string workerTypeName = "NewWorkerType";

        private class DragAndDropInfo
        {
            public int SourceItemIndex = -1;
            public Rect AllItemsRect = Rect.zero;
            public float ItemHeight;
        }

        private BuildConfigEditorStyle style;

        public override void OnInspectorGUI()
        {
            if (style == null)
            {
                style = new BuildConfigEditorStyle();
            }

            var workerConfiguration = (BuildConfig) target;

            using (new EditorGUILayout.HorizontalScope())
            {
                workerTypeName = EditorGUILayout.TextField(workerTypeName);

                GUILayout.FlexibleSpace();

                var workerNameInvalid = string.IsNullOrEmpty(workerTypeName) ||
                    workerConfiguration.WorkerBuildConfigurations.Any(c =>
                        c.WorkerType.Equals(workerTypeName, StringComparison.InvariantCultureIgnoreCase));

                using (new EditorGUI.DisabledScope(workerNameInvalid))
                {
                    if (GUILayout.Button(style.AddWorkerTypeButtonContents, EditorStyles.miniButton))
                    {
                        EditorUtility.SetDirty(target);
                        Undo.RecordObject(target, $"Add '{workerTypeName}'");

                        var config = new WorkerBuildConfiguration
                        {
                            WorkerType = workerTypeName,
                            LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets,
                                WorkerBuildData.GetCurrentBuildTargetConfig()),
                            CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets,
                                WorkerBuildData.GetLinuxBuildTargetConfig())
                        };
                        workerConfiguration.WorkerBuildConfigurations.Add(config);
                    }
                }
            }

            BuildConfigEditorStyle.DrawHorizontalLine();

            var configs = workerConfiguration.WorkerBuildConfigurations;

            if (configs.Count == 0)
            {
                EditorGUILayout.HelpBox("No workers configured. Add a worker by clicking the + button above.", MessageType.Info);
            }

            foreach (var workerConfig in configs)
            {
                if (!DrawWorkerConfiguration(workerConfig))
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, $"Remove '{workerConfig.WorkerType}'");

                    configs.Remove(workerConfig);
                    break;
                }

                BuildConfigEditorStyle.DrawHorizontalLine();
            }
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var workerType = configurationForWorker.WorkerType;
            var expanded = style.ExpandedWorkers.Contains(configurationForWorker.WorkerType);
            bool currentExpanded;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUIContent content;
                if (configurationForWorker.CloudBuildConfig.BuildTargets.Any(t =>
                        !WorkerBuildData.BuildTargetsThatCanBeBuilt[t.Target] && t.Enabled) ||
                    configurationForWorker.LocalBuildConfig.BuildTargets.Any(t =>
                        !WorkerBuildData.BuildTargetsThatCanBeBuilt[t.Target] && t.Enabled)
                )
                {
                    content = EditorGUIUtility.IconContent(style.BuiltInErrorIcon);
                    content.text = workerType;
                }
                else if (!style.WorkerContent.TryGetValue(workerType, out content))
                {
                    style.WorkerContent[workerType] = content = new GUIContent(workerType);
                }

                currentExpanded =
                    EditorGUILayout.Foldout(expanded, content, true);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(style.RemoveWorkerTypeButtonContents, EditorStyles.miniButton))
                {
                    style.ExpandedWorkers.Remove(configurationForWorker.WorkerType);
                    return false;
                }
            }

            if (expanded && !currentExpanded)
            {
                style.ExpandedWorkers.Remove(configurationForWorker.WorkerType);
            }
            else if (!expanded && currentExpanded)
            {
                style.ExpandedWorkers.Add(configurationForWorker.WorkerType);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                if (currentExpanded)
                {
                    DrawScenesInspectorForWorker(configurationForWorker);
                    EditorGUILayout.Space();
                    DrawEnvironmentInspectorForWorker(configurationForWorker);
                }
            }

            return true;
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
                var indexToRemove = -1;
                var targetItemIndex = -1;

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

                if (list != null)
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure scenes for worker");

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

        private int DrawSceneItem(int i, DragAndDropInfo dragState, SceneAsset item, EventType currentEventType,
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
                if (GUILayout.Button(style.RemoveSceneButtonContents, EditorStyles.miniButton))
                {
                    indexToRemove = i;
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
                        EditorUtility.SetDirty(target);
                        Undo.RecordObject(target, "Configure scenes for worker");

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
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Configure scenes for worker");

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
            var foldoutName = $"{configurationForWorker.WorkerType} {environmentName}";

            var environmentConfiguration =
                configurationForWorker.GetEnvironmentConfig(environment);

            GUIContent content;
            if (environmentConfiguration.BuildTargets.Any(t =>
                !WorkerBuildData.BuildTargetsThatCanBeBuilt[t.Target] && t.Enabled))
            {
                content = EditorGUIUtility.IconContent(style.BuiltInErrorIcon);
                content.text = $"{environmentName} Build Options";
            }
            else
            {
                content = new GUIContent($"{environmentName} Build Options");
            }

            if (EditorGUILayout.Foldout(style.ExpandedBuildOptions.Contains(foldoutName), content, true))
            {
                DrawBuildTargets(environmentConfiguration, foldoutName);
                style.ExpandedBuildOptions.Add(foldoutName);
            }
            else
            {
                style.ExpandedBuildOptions.Remove(foldoutName);
            }
        }


        private void DrawBuildTargets(BuildEnvironmentConfig env, string identifier)
        {
            // Init cached UI state.
            if (!style.SelectedBuildTarget.TryGetValue(identifier, out var selectedBuildTarget))
            {
                style.SelectedBuildTarget[identifier] = selectedBuildTarget = new BuildConfigEditorStyle.BuildTargetState
                {
                    Choices = env.BuildTargets.Select(c =>
                        !WorkerBuildData.BuildTargetsThatCanBeBuilt[c.Target] && c.Enabled
                            ? style.BuildErrorIcons[c.Target]
                            : style.BuildTargetIcons[c.Target]).ToArray()
                };

                // Select the first enabled target, if any.
                selectedBuildTarget.Index = env.BuildTargets.FindIndex(c => c.Enabled);
                if (selectedBuildTarget.Index == -1)
                {
                    selectedBuildTarget.Index = 0;
                }
            }

            // Draw available build targets.
            var button = new GUIStyle(EditorStyles.toolbarButton)
            {
                alignment = TextAnchor.LowerCenter, imagePosition = ImagePosition.ImageAbove, fixedHeight = 24.0f * 2,
                padding = new RectOffset(2, 2, 2, 4)
            };
            selectedBuildTarget.Index =
                GUILayout.Toolbar(selectedBuildTarget.Index, selectedBuildTarget.Choices, button);

            // Draw selection of included build targets.
            using (new GUILayout.HorizontalScope())
            {
                for (var i = 0; i < selectedBuildTarget.Choices.Length; i++)
                {
                    var config = env.BuildTargets[i];

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        // Surround by Flexible space to center the toggle button under the toolbar buttons above.
                        GUILayout.FlexibleSpace();
                        var enabled = GUILayout.Toggle(config.Enabled, string.Empty);
                        GUILayout.FlexibleSpace();

                        if (check.changed)
                        {
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(target, "Worker build options");

                            env.BuildTargets[i] = config.SetEnabled(enabled);

                            selectedBuildTarget.Choices[i] =
                                !WorkerBuildData.BuildTargetsThatCanBeBuilt[config.Target] && enabled
                                    ? style.BuildErrorIcons[config.Target]
                                    : style.BuildTargetIcons[config.Target];

                            // Windows x84 and x64 are mutually-exclusive, disable the opposing build target.
                            if (enabled)
                            {
                                var index = -1;
                                if (config.Target == BuildTarget.StandaloneWindows)
                                {
                                    index = env.BuildTargets.FindIndex(c =>
                                        c.Target == BuildTarget.StandaloneWindows64);
                                }
                                else if (config.Target == BuildTarget.StandaloneWindows64)
                                {
                                    index = env.BuildTargets.FindIndex(c => c.Target == BuildTarget.StandaloneWindows);
                                }

                                if (index >= 0)
                                {
                                    env.BuildTargets[index] = env.BuildTargets[index].SetEnabled(false);
                                }
                            }

                            selectedBuildTarget.Index = i;
                        }
                    }
                }
            }

            // Draw selected build target.
            var buildTarget = env.BuildTargets[selectedBuildTarget.Index];
            var canBuildTarget = WorkerBuildData.BuildTargetsThatCanBeBuilt[buildTarget.Target];

            var options = buildTarget.Options;
            using (var check = new EditorGUI.ChangeCheckScope())

            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
            {
                GUILayout.Label(buildTarget.Label, EditorStyles.boldLabel);

                using (new EditorGUI.DisabledScope(!buildTarget.Enabled))
                {
                    if (canBuildTarget)
                    {
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
                    }

                    options = ConfigureCompression(options);
                }

                if (!canBuildTarget)
                {
                    EditorGUILayout.HelpBox(
                        $"Your Unity Editor is missing build support for {buildTarget.Target.ToString()}.\n" +
                        "Please add the missing build support options to your Unity Editor",
                        buildTarget.Enabled ? MessageType.Error : MessageType.Warning);
                }

                if (check.changed)
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, "Worker build options");

                    env.BuildTargets[selectedBuildTarget.Index] =
                        new BuildTargetConfig(buildTarget.Target, options, buildTarget.Enabled);
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

            choice = EditorGUILayout.Popup("Compression method", choice, style.CompressionOptions);

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
            var options = buildTarget.Options;
            if (EditorGUILayout.Toggle("Headless", options.HasFlag(BuildOptions.EnableHeadlessMode)))
            {
                options |= BuildOptions.EnableHeadlessMode;
            }
            else
            {
                options &= ~BuildOptions.EnableHeadlessMode;
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
            if (EditorGUILayout.Toggle("Development Build", options.HasFlag(BuildOptions.Development)))
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
    }
}
