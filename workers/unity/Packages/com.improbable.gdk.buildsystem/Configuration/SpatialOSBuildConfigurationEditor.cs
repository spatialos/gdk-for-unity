using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    // Still TODO
    // The build options don't extend fully horizontally
    // Windows/Windows64 need to be made mutually exclusive in the UI
    // Lots of optimization (caching GUIStyles, lots of LINQ)
    // Lots of API cleanup (constructing the BuildEnvironmentConfigs, etc. is bad)
    // Test building from the menu
    [CustomEditor(typeof(SpatialOSBuildConfiguration))]
    public class SpatialOSBuildConfigurationEditor : Editor
    {
        private string workerTypeName = "NewWorkerType";

        private static readonly GUIContent AddWorkerTypeButtonContents = new GUIContent("+", "Add worker type");
        private static readonly GUIContent RemoveWorkerTypeButtonContents = new GUIContent("-", "Remove worker type");
        private static readonly GUIContent AddSceneButtonContents = new GUIContent("+", "Add scene");
        private static readonly GUIContent RemoveSceneButtonContents = new GUIContent("-", "Remove scene");

        private class DragAndDropInfo
        {
            public int SourceItemIndex = -1;
            public Rect AllItemsRect = Rect.zero;
            public float ItemHeight;
        }

        private static readonly HashSet<string> ExpandedWorkers = new HashSet<string>();
        private static readonly HashSet<string> ExpandedBuildOptions = new HashSet<string>();
        private static readonly HashSet<string> ExpandedBuildTargets = new HashSet<string>();

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
                            LocalBuildConfig = new BuildEnvironmentConfig(t => BuildOptions.None, t => false),
                            CloudBuildConfig = new BuildEnvironmentConfig(t => BuildOptions.None, t => false)
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

            using (new EditorGUILayout.HorizontalScope())
            {
                GUIContent content;
                if (configurationForWorker.CloudBuildConfig.BuildTargets.Any(t =>
                        !t.BuildSupportInstalled && t.Enabled) ||
                    configurationForWorker.LocalBuildConfig.BuildTargets.Any(t => !t.BuildSupportInstalled && t.Enabled)
                )
                {
                    content = EditorGUIUtility.IconContent("console.erroricon.sml");
                    content.text = workerType;
                }
                else
                {
                    content = new GUIContent(workerType);
                }

                currentExpanded =
                    EditorGUILayout.Foldout(expanded, content, true);

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
                    DrawEnvironmentInspectorForWorker(configurationForWorker);
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
                    EditorGUIUtility.ShowObjectPicker<SceneAsset>(null, false, "t:Scene",
                        workerControlId);
                }

                HandleObjectSelectorUpdated(configurationForWorker, workerControlId);
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
                    var list = configurationForWorker.ScenesForWorker.ToList();
                    list.Add(scene);
                    configurationForWorker.ScenesForWorker = list.ToArray();
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
            if (environmentConfiguration.BuildTargets.Any(t => !t.BuildSupportInstalled && t.Enabled)
            )
            {
                content = EditorGUIUtility.IconContent("console.erroricon.sml");
                content.text = $"{environmentName} Build Options";
            }
            else
            {
                content = new GUIContent($"{environmentName} Build Options");
            }

            if (EditorGUILayout.Foldout(ExpandedBuildOptions.Contains(foldoutName), content, true))
            {
                ConfigureBuildPlatforms(environmentConfiguration, foldoutName);
                ExpandedBuildOptions.Add(foldoutName);
            }
            else
            {
                ExpandedBuildOptions.Remove(foldoutName);
            }
        }

        private void ConfigureBuildPlatforms(BuildEnvironmentConfig environmentConfiguration, string identifier)
        {
            foreach (var buildTarget in environmentConfiguration.BuildTargets)
            {
                var foldoutIdentifier = identifier + buildTarget.Label;
                GUIContent content;

                if(!buildTarget.BuildSupportInstalled && buildTarget.Enabled)
                {
                    content = EditorGUIUtility.IconContent("console.erroricon.sml");
                    content.text = buildTarget.Label;
                }
                else
                {
                    content = new GUIContent(buildTarget.Label);
                }

                using (var check = new EditorGUI.ChangeCheckScope())
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                {
                    // Toggle field force the inclusion of an aligned Label which takes up lots of horizontal space.
                    // Copy the visual aspects of the Toggle and apply them to a Button that uses minimal space.
                    var tstyle = new GUIStyle(EditorStyles.toggle);
                
                    if (buildTarget.Enabled)
                    {
                        tstyle.normal = tstyle.onNormal;
                        tstyle.active = tstyle.onActive;
                        tstyle.focused = tstyle.onFocused;
                        tstyle.hover = tstyle.onHover;
                    }

                    var options = buildTarget.Options;
                    var enabled = buildTarget.Enabled;

                    if (GUILayout.Button(string.Empty, tstyle, GUILayout.ExpandWidth(false)))
                    {
                        enabled = !enabled;

                        if (enabled)
                        {
                            // Open up the settings for the newly-enabled platform.
                            ExpandedBuildTargets.Add(foldoutIdentifier);
                        }
                        else
                        {
                            ExpandedBuildTargets.Remove(foldoutIdentifier);
                        }
                    }                                       

                    var toggleOnLabelClick = new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };

                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (EditorGUILayout.Foldout(ExpandedBuildTargets.Contains(foldoutIdentifier), content,
                            true, buildTarget.Enabled ? toggleOnLabelClick : EditorStyles.foldout))
                        {
                            ExpandedBuildTargets.Add(foldoutIdentifier);

                            using (new EditorGUI.IndentLevelScope())
                            using (new EditorGUI.DisabledScope(buildTarget.Enabled == false))
                            {
                                if (buildTarget.BuildSupportInstalled)
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
                                else
                                {
                                    EditorGUILayout.HelpBox(
                                        $"Your Unity Editor is missing build support for {buildTarget.Target.ToString()}.\n" +
                                        "Please add the missing build support options to your Unity Editor",
                                        MessageType.Error);
                                }

                                options = ConfigureCompression(options);

                                DrawHorizontalLine();
                            }
                        }
                        else
                        {
                            ExpandedBuildTargets.Remove(foldoutIdentifier);
                        }

                        if (check.changed)
                        {
                            Undo.RecordObject(target, "Worker build options");
                            buildTarget.Options = options;
                            buildTarget.Enabled = enabled;
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
            }
        }

        private BuildOptions ConfigureCompression(BuildOptions options)
        {
            EditorGUILayout.Popup("Compression", 0, new[] { "TODO: Default", "TODO: LZ4", "TODO: LZ4HC" });
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

            return options;
        }
    }
}
