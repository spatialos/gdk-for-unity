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

        private static readonly HashSet<string> ExpandedFoldouts = new HashSet<string>();

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
                        Undo.RecordObject(target, $"Remove '{workerTypeName}'");

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
            }

            var rect = EditorGUILayout.GetControlRect(false, 1, EditorStyles.foldout);
            var oldColor = Handles.color;
            Handles.color = new Color(0.3f, 0.3f, 0.3f, 1);

            Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            Handles.color = oldColor;

            GUILayout.Space(rect.height);

            var configs = workerConfiguration.WorkerBuildConfigurations;
            foreach (var workerConfig in configs)
            {
                if (!DrawWorkerConfiguration(workerConfig))
                {
                    EditorUtility.SetDirty(target);
                    Undo.RecordObject(target, $"Remove '{workerConfig.WorkerType}'");

                    configs.Remove(workerConfig);
                    break;
                }
            }
        }

        private bool DrawWorkerConfiguration(WorkerBuildConfiguration configurationForWorker)
        {
            var workerType = configurationForWorker.WorkerType;
            var expanded = ExpandedFoldouts.Contains(configurationForWorker.WorkerType);
            bool currentExpanded;

            using (new EditorGUILayout.HorizontalScope())
            {
                currentExpanded =
                    EditorGUILayout.Foldout(expanded, workerType);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(RemoveWorkerTypeButtonContents, EditorStyles.miniButton))
                {
                    ExpandedFoldouts.Remove(configurationForWorker.WorkerType);
                    return false;
                }
            }

            if (expanded && !currentExpanded)
            {
                ExpandedFoldouts.Remove(configurationForWorker.WorkerType);
            }
            else if (!expanded && currentExpanded)
            {
                ExpandedFoldouts.Add(configurationForWorker.WorkerType);
            }

            using (IndentLevelScope(1))
            {
                if (currentExpanded)
                {
                    DrawScenesInspectorForWorker(configurationForWorker);
                    DrawEnvironmentInspectorForWorker(configurationForWorker);
                }
                else
                {
                    // Draw an overview of issues.
                    if (GetConfigurationProblems(BuildEnvironment.Local, configurationForWorker).Count > 0 ||
                        GetConfigurationProblems(BuildEnvironment.Cloud, configurationForWorker).Count > 0)
                    {
                        EditorGUILayout.HelpBox("Problems found", MessageType.Warning);
                    }
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

        private class ScopedGUIColor : IDisposable
        {
            public ScopedGUIColor(Color color)
            {
                oldColor = GUI.color;
                GUI.color = color;
            }

            public void Dispose()
            {
                GUI.color = oldColor;
            }

            private readonly Color oldColor;
        }

        private void DrawScenesInspectorForWorker(WorkerBuildConfiguration configurationForWorker)
        {
            EditorGUILayout.LabelField("Scenes", EditorStyles.boldLabel);
            var workerControlId = GUIUtility.GetControlID(FocusType.Passive);
            var dragState = (DragAndDropInfo) GUIUtility.GetStateObject(typeof(DragAndDropInfo), workerControlId);
            var currentEventType = Event.current.type;

            using (IndentLevelScope(1))
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Scenes to include (in order)");
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(AddSceneButtonContents, EditorStyles.miniButton))
                        {
                            CurrentObjectPickerWindowId = GUIUtility.GetControlID(FocusType.Passive);
                            EditorGUIUtility.ShowObjectPicker<SceneAsset>(null, false, "t:Scene",
                                CurrentObjectPickerWindowId);
                        }

                        HandleObjectSelectorUpdated(configurationForWorker);
                    }

                    if (configurationForWorker.ScenesForWorker.Length == 0)
                    {
                        // Allow dropping to form a new list.
                        EditorGUILayout.HelpBox("No scenes", MessageType.Info);
                        var rect = GUILayoutUtility.GetLastRect();
                        if (rect.Contains(Event.current.mousePosition))
                        {
                            switch (currentEventType)
                            {
                                case EventType.DragPerform:
                                    EditorUtility.SetDirty(target);
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
                                using (new ScopedGUIColor(
                                    i == dragState.SourceItemIndex
                                        ? new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.25f)
                                        : GUI.color))
                                {
                                    var rowRect = Rect.zero;
                                    var content = EditorGUIUtility.ObjectContent(item, typeof(SceneAsset));

                                    // Reserve space for the handle, draw it later.
                                    var grabberRect = GUILayoutUtility.GetRect(new GUIContent(content.image), EditorStyles.textField,
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

                                var hitRect = new Rect(dragState.AllItemsRect.xMin,
                                    dragState.AllItemsRect.yMin + (i * (dragState.ItemHeight +
                                        EditorGUIUtility.standardVerticalSpacing)) -
                                    EditorGUIUtility.standardVerticalSpacing / 2.0f, dragState.AllItemsRect.width,
                                    dragState.ItemHeight + EditorGUIUtility.standardVerticalSpacing / 2.0f);

                                if (hitRect.Contains(Event.current.mousePosition))
                                {
                                    targetItemIndex = (Event.current.mousePosition.y >
                                        hitRect.yMin + hitRect.height / 2)
                                        ? i + 1
                                        : i;

                                    if (i == dragState.SourceItemIndex)
                                    {
                                        targetItemIndex = -1;
                                    }

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
                                            Event.current.Use();
                                            dragState.SourceItemIndex = -1;
                                            Repaint();
                                            break;
                                        case EventType.DragUpdated:
                                            var sceneAssets = DragAndDrop.objectReferences.OfType<SceneAsset>()
                                                .ToList();
                                            if (sceneAssets.Any())
                                            {
                                                if (dragState.SourceItemIndex == -1 && new HashSet<SceneAsset>(sceneAssets).Overlaps(configurationForWorker.ScenesForWorker))
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
                                case EventType.DragExited:  // Fall through.
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
                            EditorUtility.SetDirty(target);
                            Undo.RecordObject(target, "Configure scenes for worker");

                            configurationForWorker.ScenesForWorker = list.ToArray();
                        }
                    }
                }
            }
        }

        private static void DrawGrabber(Rect grabberRect)
        {
            EditorGUI.DrawRect(grabberRect, new Color(1,0,0,0.25f));
            var oldColor = Handles.color;
            Handles.color = new Color(0.4f, 0.4f, 0.4f, 1);
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.25f)),
                new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.25f)));
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height / 2),
                new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2));
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.75f)),
                new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.75f)));

            Handles.color = new Color(0.3f, 0.3f, 0.3f, 1);
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.25f) + 1),
                new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.25f) + 1));
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + grabberRect.height / 2 + 1),
                new Vector2(grabberRect.xMax, grabberRect.yMin + grabberRect.height / 2 + 1));
            Handles.DrawLine(new Vector2(grabberRect.xMin, grabberRect.yMin + (grabberRect.height * 0.75f) + 1),
                new Vector2(grabberRect.xMax, grabberRect.yMin + (grabberRect.height * 0.75f) + 1));
            Handles.color = oldColor;
        }

        private void HandleObjectSelectorUpdated(WorkerBuildConfiguration configurationForWorker)
        {
            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == CurrentObjectPickerWindowId)
            {
                CurrentObjectPickerWindowId = -1;
                var scene = (SceneAsset) EditorGUIUtility.GetObjectPickerObject();

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
            EditorGUILayout.LabelField("Environments", EditorStyles.boldLabel);

            DrawEnvironmentInspector(BuildEnvironment.Local, configurationForWorker);
            DrawConfigurationProblems(GetConfigurationProblems(BuildEnvironment.Local, configurationForWorker));

            DrawEnvironmentInspector(BuildEnvironment.Cloud, configurationForWorker);
            DrawConfigurationProblems(GetConfigurationProblems(BuildEnvironment.Cloud, configurationForWorker));
        }

        private void DrawConfigurationProblems(List<Problem> problems)
        {
            using (IndentLevelScope(1))
            {
                foreach (var problem in problems)
                {
                    EditorGUILayout.HelpBox(problem.Message, problem.Type);
                }
            }
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
                        var newFlag =
                            EditorGUILayout.ToggleLeft(enumValue.ToString(CultureInfo.InvariantCulture), hasFlag);
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

        private List<Problem> GetConfigurationProblems(BuildEnvironment environment,
            WorkerBuildConfiguration configurationForWorker)
        {
            var problems = new List<Problem>();
            var environmentConfiguration =
                configurationForWorker.GetEnvironmentConfig(environment);

            var buildOptions = environmentConfiguration.BuildOptions;

            if ((buildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                (buildOptions & BuildOptions.Development) != 0)
            {
                problems.Add(new Problem
                {
                    Message = "You cannot have both EnableHeadlessMode and Development build enabled.\n" +
                        "This will crash the Unity Editor during the build.",
                    Type = MessageType.Error
                });
            }

            if ((buildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                (environmentConfiguration.BuildPlatforms & ~SpatialBuildPlatforms.Linux) != 0)
            {
                problems.Add(new Problem
                {
                    Message =
                        "EnableHeadlessMode is only available for Linux builds.",
                    Type = MessageType.Warning
                });
            }

            var currentAdjustedPlatforms = environmentConfiguration.BuildPlatforms;

            if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Current) != 0)
            {
                currentAdjustedPlatforms |= WorkerBuilder.GetCurrentBuildPlatform();
            }

            if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Windows32) != 0 &&
                (currentAdjustedPlatforms & SpatialBuildPlatforms.Windows64) != 0)
            {
                problems.Add(new Problem
                    {
                        Message = WorkerBuilder.IncompatibleWindowsPlatformsErrorMessage,
                        Type = MessageType.Error
                    }
                );
            }

            var buildTargetsMissingBuildSupport =
                BuildSupportChecker.GetBuildTargetsMissingBuildSupport(
                    WorkerBuilder.GetUnityBuildTargets(environmentConfiguration.BuildPlatforms));

            if (buildTargetsMissingBuildSupport.Length > 0)
            {
                problems.Add(new Problem
                    {
                        Message =
                            $"Missing build support for {string.Join(", ", buildTargetsMissingBuildSupport)}",
                        Type = MessageType.Error
                    }
                );
            }

            return problems;
        }

        private void ConfigureBuildOptions(BuildEnvironmentConfig environmentConfiguration)
        {
            using (IndentLevelScope(1))
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    var showBuildOptions =
                        EditorGUILayout.Foldout(environmentConfiguration.ShowBuildOptions, "Build Options");
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

                    if (check.changed)
                    {
                        EditorUtility.SetDirty(target);
                        Undo.RecordObject(target, "Configure build options for worker");

                        environmentConfiguration.ShowBuildOptions = showBuildOptions;
                        environmentConfiguration.BuildOptions = newBuildOptions;
                    }
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
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    var buildPlatformsString = SelectedPlatformsToString(environmentConfiguration.BuildPlatforms);
                    var newBuildPlatforms = environmentConfiguration.BuildPlatforms;
                    var showBuildPlatforms = EditorGUILayout.Foldout(environmentConfiguration.ShowBuildPlatforms,
                        "Build Platforms: " + buildPlatformsString);
                    if (showBuildPlatforms)
                    {
                        newBuildPlatforms = EnumFlagsToggleField(environmentConfiguration.BuildPlatforms);
                    }

                    if (check.changed)
                    {
                        EditorUtility.SetDirty(target);
                        Undo.RecordObject(target, "Configure build platforms for worker");

                        environmentConfiguration.ShowBuildPlatforms = showBuildPlatforms;
                        environmentConfiguration.BuildPlatforms = newBuildPlatforms;
                    }
                }
            }
        }

        private static IDisposable IndentLevelScope(int increment)
        {
            return new EditorGUI.IndentLevelScope(increment);
        }

        private class Problem
        {
            public string Message;

            public MessageType Type;
        }
    }
}
