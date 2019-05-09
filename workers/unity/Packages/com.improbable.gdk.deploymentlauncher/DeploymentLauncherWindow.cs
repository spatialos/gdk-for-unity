using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Improbable.Gdk.Core.Editor;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEditor;
using UnityEngine;
using UploadTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.AssemblyConfig>;
using LaunchTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, (string, string, Improbable.Gdk.DeploymentLauncher.BaseDeploymentConfig)>;
using ListTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<System.Collections.Generic.List<Improbable.Gdk.DeploymentLauncher.DeploymentInfo>, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, string>;
using StopTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, Improbable.Gdk.DeploymentLauncher.DeploymentInfo>;
using AuthTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Tools.RedirectedProcessResult, int>;

namespace Improbable.Gdk.DeploymentLauncher
{
    internal class DeploymentLauncherWindow : EditorWindow
    {
        private const string BuiltInErrorIcon = "console.erroricon.sml";
        private const string BuiltInRefreshIcon = "Refresh";
        private const string BuiltInWebIcon = "BuildSettings.Web.Small";

        private static readonly Vector2 SmallIconSize = new Vector2(12, 12);
        private readonly Color horizontalLineColor = new Color(0.3f, 0.3f, 0.3f, 1);
        private Material spinnerMaterial;

        private readonly TaskManager manager = new TaskManager();
        private readonly UIStateManager stateManager = new UIStateManager();

        private string[] allWorkers;

        private DeploymentLauncherConfig launcherConfig;
        private int selectedDeploymentIndex;
        private Vector2 scrollPos;
        private string projectName;

        private List<DeploymentInfo> listedDeployments = new List<DeploymentInfo>();
        private int selectedListedDeploymentIndex;

        [MenuItem("SpatialOS/Deployment Launcher", false, 51)]
        private static void LaunchDeploymentMenu()
        {
            var inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var deploymentWindow = GetWindow<DeploymentLauncherWindow>(inspectorWindowType);
            deploymentWindow.titleContent.text = "Deployment Launcher";
            deploymentWindow.titleContent.tooltip = "A tab for managing your SpatialOS deployments.";
            deploymentWindow.Show();
        }

        private void OnEnable()
        {
            launcherConfig = DeploymentLauncherConfig.GetInstance();
            projectName = GetProjectName();

            if (launcherConfig != null && projectName != null)
            {
                launcherConfig.SetProjectName(projectName);
                EditorUtility.SetDirty(launcherConfig);
                AssetDatabase.SaveAssets();
            }

            spinnerMaterial = new Material(Shader.Find("UI/Default"));

            Application.quitting += OnExit;
        }

        private void OnDestroy()
        {
            OnExit();

            Application.quitting -= OnExit;
        }

        private void OnExit()
        {
            manager.Cancel();
        }

        private void Update()
        {
            manager.Update();

            foreach (var wrappedTask in manager.CompletedTasks.OfType<UploadTask>())
            {
                if (wrappedTask.Task.Result.ExitCode != 0)
                {
                    Debug.LogError($"Upload of {wrappedTask.Context.AssemblyName} failed.");
                }
                else
                {
                    Debug.Log($"Upload of {wrappedTask.Context.AssemblyName} succeeded.");
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<LaunchTask>())
            {
                var (cachedProjectName, assemblyName, config) = wrappedTask.Context;

                var result = wrappedTask.Task.Result;
                if (result.IsOkay)
                {
                    Application.OpenURL($"https://console.improbable.io/projects/{cachedProjectName}/deployments/{config.Name}/overview");
                }
                else
                {
                    var error = result.UnwrapError();

                    if (TryAuthAndRetry(error))
                    {
                        manager.Launch(cachedProjectName, assemblyName, config, TaskManager.QueueMode.RunNext);
                    }
                    else
                    {
                        Debug.LogError($"Launch of {config.Name} failed. Code: {error.Code} Message: {error.Message}");
                    }
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<ListTask>())
            {
                var result = wrappedTask.Task.Result;
                if (result.IsOkay)
                {
                    listedDeployments = result.Unwrap();
                    listedDeployments.Sort((first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));
                    selectedDeploymentIndex = -1;
                }
                else
                {
                    var error = result.UnwrapError();

                    if (TryAuthAndRetry(error))
                    {
                        manager.List(wrappedTask.Context, TaskManager.QueueMode.RunNext);
                    }
                    else
                    {
                        Debug.LogError($"Failed to list deployments in project {wrappedTask.Context}. Code: {error.Code} Message: {error.Message}");
                    }
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<StopTask>())
            {
                var result = wrappedTask.Task.Result;
                var info = wrappedTask.Context;
                if (result.IsOkay)
                {
                    Debug.Log($"Stopped deployment: \"{info.Name}\" successfully.");

                    for (var i = 0; i < listedDeployments.Count; i++)
                    {
                        if (listedDeployments[i].Name == info.Name)
                        {
                            listedDeployments.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    var error = result.UnwrapError();

                    if (TryAuthAndRetry(error))
                    {
                        manager.Stop(wrappedTask.Context, TaskManager.QueueMode.RunNext);
                    }
                    else
                    {
                        Debug.LogError($"Failed to stop deployment: \"{info.Name}\". Code: {error.Code} Message: {error.Message}.");
                    }
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<AuthTask>())
            {
                var result = wrappedTask.Task.Result;

                if (result.ExitCode == 0)
                {
                    Debug.Log("Successfully authenticated with SpatialOS. Retrying previous action.");
                }
                else
                {
                    // Stop the potential infinite loop of retries.
                    manager.Cancel();
                    Debug.LogError("Failed to authenticate with SpatialOS. Please run \"spatial auth login\" manually.");
                }
            }

            manager.ClearResults();
        }

        private void OnGUI()
        {
            if (launcherConfig == null)
            {
                EditorGUILayout.HelpBox($"Could not find a {nameof(DeploymentLauncherConfig)} instance.\nPlease create one via the Assets > Create > SpatialOS menu.", MessageType.Info);
                return;
            }

            if (projectName == null)
            {
                EditorGUILayout.HelpBox("Could not parse your SpatialOS project name. See the Console for more details", MessageType.Error);
                return;
            }

            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(manager.IsActive))
                    {
                        var buttonIcon = new GUIContent(EditorGUIUtility.IconContent(BuiltInRefreshIcon))
                        {
                            tooltip = "Refresh your project name."
                        };

                        if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                        {
                            projectName = GetProjectName();
                            launcherConfig.SetProjectName(projectName);
                            EditorUtility.SetDirty(launcherConfig);
                            AssetDatabase.SaveAssets();
                        }
                    }

                    EditorGUILayout.LabelField("Project Name", projectName);
                }

                DrawHorizontalLine(5);

                launcherConfig.AssemblyConfig = DrawAssemblyConfig(launcherConfig.AssemblyConfig);

                GUILayout.Label("Deployment Configurations", EditorStyles.boldLabel);

                for (var index = 0; index < launcherConfig.DeploymentConfigs.Count; index++)
                {
                    var deplConfig = launcherConfig.DeploymentConfigs[index];
                    var (shouldRemove, updated) = DrawDeploymentConfig(deplConfig);
                    if (shouldRemove)
                    {
                        launcherConfig.DeploymentConfigs.RemoveAt(index);
                        index--;
                    }
                    else
                    {
                        launcherConfig.DeploymentConfigs[index] = updated;
                    }
                }

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Add new deployment configuration"))
                    {
                        var deploymentConfig = new DeploymentConfig
                        {
                            AssemblyName = launcherConfig.AssemblyConfig.AssemblyName,
                            Deployment = new BaseDeploymentConfig
                            {
                                Name = $"deployment_{launcherConfig.DeploymentConfigs.Count}"
                            }
                        };

                        launcherConfig.DeploymentConfigs.Add(deploymentConfig);
                    }
                }

                if (launcherConfig.DeploymentConfigs.Count > 0)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        selectedDeploymentIndex = EditorGUILayout.Popup("Deployment", selectedDeploymentIndex,
                            launcherConfig.DeploymentConfigs.Select(config => config.Deployment.Name).ToArray());

                        var isValid = IsSelectedValid(launcherConfig.DeploymentConfigs, selectedDeploymentIndex);

                        var hasErrors = isValid && launcherConfig.DeploymentConfigs[selectedDeploymentIndex].GetErrors().Any();

                        using (new EditorGUI.DisabledScope(!isValid || hasErrors || manager.IsActive))
                        {
                            if (GUILayout.Button("Launch deployment"))
                            {
                                var deplConfig = launcherConfig.DeploymentConfigs[selectedDeploymentIndex];

                                manager.Launch(deplConfig.ProjectName, deplConfig.AssemblyName, deplConfig.Deployment);

                                foreach (var simPlayerDepl in deplConfig.SimulatedPlayerDeploymentConfigs)
                                {
                                    manager.Launch(deplConfig.ProjectName, deplConfig.AssemblyName, simPlayerDepl);
                                }
                            }
                        }
                    }
                }

                DrawHorizontalLine(5);
                GUILayout.Label("Live Deployments", EditorStyles.boldLabel);
                DrawDeploymentList();

                scrollPos = scrollView.scrollPosition;

                if (check.changed)
                {
                    EditorUtility.SetDirty(launcherConfig);
                    AssetDatabase.SaveAssets();
                }

                if (manager.IsActive)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.HelpBox(GetStatusMessage(), MessageType.Info);

                        if (!(manager.CurrentTask is AuthTask) &&
                            GUILayout.Button("Cancel", GUILayout.Height(38), GUILayout.Width(75)))
                        {
                            CancelCurrentTask();
                        }
                    }

                    var rect = EditorGUILayout.GetControlRect(false, 20);
                    DrawSpinner(Time.realtimeSinceStartup * 10, rect);

                    Repaint();
                }
            }
        }

        private void CancelCurrentTask()
        {
            var sb = new StringBuilder();

            switch (manager.CurrentTask)
            {
                case UploadTask task:
                    sb.Append("Are you sure you want to cancel uploading your assembly?");
                    break;
                case LaunchTask task:
                    sb.Append("Are you sure you want to cancel launching your deployment?");
                    break;
                case ListTask task:
                    sb.Append("Are you sure you want to cancel listing live deployments?");
                    break;
                case StopTask task:
                    sb.Append("Are you sure you want to cancel stopping your deployment?");
                    break;
                default:
                    return;
            }

            sb.AppendLine(" Cancelling a running task can have unintended side-effects.");

            var taskId = manager.CurrentTask.GetId();
            if (EditorUtility.DisplayDialog(
                "Cancel running task",
                sb.ToString(),
                "Cancel",
                "Keep running task"))
            {
                if (manager.CancelCurrentTask(taskId))
                {
                    Debug.Log("Cancelled task.");
                }
                else
                {
                    Debug.LogWarning("Cannot cancel task as it has already reached completion.");
                }
            }
        }

        private AssemblyConfig DrawAssemblyConfig(AssemblyConfig config)
        {
            GUILayout.Label("Assembly Upload", EditorStyles.boldLabel);

            var copy = config.DeepCopy();
            var error = config.GetError();

            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    copy.AssemblyName = EditorGUILayout.TextField("Assembly Name", config.AssemblyName);

                    if (GUILayout.Button("Generate", GUILayout.ExpandWidth(false)))
                    {
                        copy.AssemblyName = $"{projectName}_{DateTime.Now:MMdd_HHmm}";
                        GUI.FocusControl(null);
                    }
                }

                copy.ShouldForceUpload = EditorGUILayout.Toggle("Force Upload", config.ShouldForceUpload);

                GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                using (new EditorGUILayout.HorizontalScope())
                {
                    var shouldBeVertical = EditorGUIUtility.currentViewWidth < 550;
                    /* Response Layout, Intuitive API! */
                    if (shouldBeVertical)
                    {
                        EditorGUILayout.BeginVertical();
                    }
                    else
                    {
                        GUILayout.FlexibleSpace();
                    }

                    using (new EditorGUI.DisabledScope(error != null))
                    {
                        if (GUILayout.Button("Assign assembly name to deployments"))
                        {
                            foreach (var deplConfig in launcherConfig.DeploymentConfigs)
                            {
                                deplConfig.AssemblyName = launcherConfig.AssemblyConfig.AssemblyName;
                            }
                        }

                        using (new EditorGUI.DisabledScope(manager.IsActive))
                        {
                            if (GUILayout.Button("Upload assembly"))
                            {
                                manager.Upload(config);
                            }
                        }
                    }

                    if (shouldBeVertical)
                    {
                        EditorGUILayout.EndVertical();
                    }
                }

                if (error != null)
                {
                    EditorGUILayout.HelpBox(error, MessageType.Error);
                }
            }

            DrawHorizontalLine(3);

            return copy;
        }

        private (bool, DeploymentConfig) DrawDeploymentConfig(DeploymentConfig config)
        {
            var foldoutState = stateManager.GetStateObjectOrDefault<bool>(config.Deployment.Name.GetHashCode());
            var copy = config.DeepCopy();

            var errors = copy.GetErrors();

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    foldoutState = EditorGUILayout.Foldout(foldoutState, new GUIContent(config.Deployment.Name), true);

                    GUILayout.FlexibleSpace();

                    using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
                    {
                        if (errors.Any())
                        {
                            GUILayout.Label(new GUIContent(EditorGUIUtility.IconContent(BuiltInErrorIcon))
                            {
                                tooltip = "One or more errors in deployment configuration."
                            });
                        }

                        var buttonContent = new GUIContent(string.Empty, "Remove deployment configuration");
                        buttonContent.image = EditorGUIUtility.IconContent("Toolbar Minus").image;

                        if (GUILayout.Button(buttonContent, EditorStyles.miniButton))
                        {
                            return (true, null);
                        }
                    }
                }

                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUILayout.VerticalScope())
                {
                    if (foldoutState)
                    {
                        copy.AssemblyName = EditorGUILayout.TextField("Assembly Name", config.AssemblyName);
                        RenderBaseDeploymentConfig(config.Deployment, copy.Deployment);

                        if (copy.Deployment.Name != config.Deployment.Name)
                        {
                            UpdateSimulatedDeploymentNames(copy);
                        }

                        GUILayout.Space(10);

                        EditorGUILayout.LabelField("Simulated Player Deployments");

                        for (var i = 0; i < copy.SimulatedPlayerDeploymentConfigs.Count; i++)
                        {
                            var simConfig = copy.SimulatedPlayerDeploymentConfigs[i];
                            var (shouldRemove, updated) = DrawSimulatedConfig(i, simConfig);

                            GUILayout.Space(5);

                            if (shouldRemove)
                            {
                                copy.SimulatedPlayerDeploymentConfigs.RemoveAt(i);
                                i--;
                                UpdateSimulatedDeploymentNames(copy);
                            }
                            else
                            {
                                copy.SimulatedPlayerDeploymentConfigs[i] = updated;
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (foldoutState)
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Add simulated player deployment"))
                        {
                            var newSimPlayerDepl = new SimulatedPlayerDeploymentConfig();
                            newSimPlayerDepl.TargetDeploymentName = config.Deployment.Name;
                            newSimPlayerDepl.Name = $"{config.Deployment.Name}_sim{config.SimulatedPlayerDeploymentConfigs.Count + 1}";

                            copy.SimulatedPlayerDeploymentConfigs.Add(newSimPlayerDepl);
                        }
                    }
                }

                if (errors.Any())
                {
                    EditorGUILayout.HelpBox($"This deployment configuration has the following errors:\n\n{errors.FormatErrors()}", MessageType.Error);
                }

                if (check.changed)
                {
                    stateManager.SetStateObject(copy.Deployment.Name.GetHashCode(), foldoutState);
                }
            }

            DrawHorizontalLine(5);

            return (false, copy);
        }

        private void RenderBaseDeploymentConfig(BaseDeploymentConfig source, BaseDeploymentConfig dest)
        {
            using (new EditorGUI.DisabledScope(source is SimulatedPlayerDeploymentConfig))
            {
                dest.Name = EditorGUILayout.TextField("Deployment Name", source.Name);
            }

            dest.SnapshotPath = EditorGUILayout.TextField("Snapshot Path", source.SnapshotPath);
            dest.LaunchJson = EditorGUILayout.TextField("Launch Config", source.LaunchJson);
            dest.Region = (DeploymentRegionCode) EditorGUILayout.EnumPopup("Region", source.Region);

            EditorGUILayout.LabelField("Tags");

            using (new EditorGUI.IndentLevelScope())
            {
                for (var i = 0; i < dest.Tags.Count; i++)
                {
                    dest.Tags[i] = EditorGUILayout.TextField($"Tag #{i + 1}", dest.Tags[i]);
                }

                dest.Tags.Add(EditorGUILayout.TextField($"Tag #{dest.Tags.Count + 1}", string.Empty));

                dest.Tags = dest.Tags.Where(tag => !string.IsNullOrEmpty(tag)).ToList();
            }
        }

        private (bool, SimulatedPlayerDeploymentConfig) DrawSimulatedConfig(int index, SimulatedPlayerDeploymentConfig config)
        {
            var copy = config.DeepCopy();
            var foldoutState = stateManager.GetStateObjectOrDefault<bool>(config.Name.GetHashCode());

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    foldoutState = EditorGUILayout.Foldout(foldoutState, new GUIContent($"Simulated Player Deployment {index + 1}"), true);

                    GUILayout.FlexibleSpace();

                    using (new EditorGUIUtility.IconSizeScope(SmallIconSize))
                    {
                        var buttonContent = new GUIContent(string.Empty, "Remove simulated player deployment");
                        buttonContent.image = EditorGUIUtility.IconContent("Toolbar Minus").image;

                        if (GUILayout.Button(buttonContent, EditorStyles.miniButton))
                        {
                            return (true, null);
                        }
                    }
                }

                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUILayout.VerticalScope())
                {
                    if (foldoutState)
                    {
                        RenderBaseDeploymentConfig(config, copy);
                        copy.FlagPrefix = EditorGUILayout.TextField("Flag Prefix", config.FlagPrefix);

                        if (!ValidAllWorkersList() && !RefreshAllWorkersList())
                        {
                            copy.WorkerTypeId = 0;
                            copy.WorkerType = string.Empty;
                        }
                        else
                        {
                            copy.WorkerTypeId = EditorGUILayout.Popup(
                                new GUIContent("Worker Type"),
                                copy.WorkerTypeId,
                                allWorkers);

                            copy.WorkerType = allWorkers[copy.WorkerTypeId];
                        }
                    }
                }

                if (check.changed)
                {
                    stateManager.SetStateObject(copy.Name.GetHashCode(), foldoutState);
                }
            }

            return (false, copy);
        }

        private bool ValidAllWorkersList()
        {
            return allWorkers != null && allWorkers.Length > 0;
        }

        private bool RefreshAllWorkersList()
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
                return true;
            }
            catch (Exception e)
            {
                allWorkers = new string[0];
                Debug.LogException(e);
                return false;
            }
        }

        private void DrawDeploymentList()
        {
            if (listedDeployments.Count == 0)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Could not find any live deployments.");
                        GUILayout.FlexibleSpace();
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Press the \"Refresh\" button to search again.");
                        GUILayout.FlexibleSpace();
                    }
                }
            }
            else
            {
                // Temporarily change the label width field to allow better spacing in the deployment list screen.
                var previousWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 250f;

                for (var index = 0; index < listedDeployments.Count; index++)
                {
                    var deplInfo = listedDeployments[index];

                    var foldoutState = stateManager.GetStateObjectOrDefault<bool>(deplInfo.Id.GetHashCode());
                    using (var check = new EditorGUI.ChangeCheckScope())
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        foldoutState = EditorGUILayout.Foldout(foldoutState, new GUIContent(deplInfo.Name), true);

                        var buttonIcon = new GUIContent(EditorGUIUtility.IconContent(BuiltInWebIcon))
                        {
                            tooltip = "Open this deployment in your browser."
                        };

                        if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                        {
                            Application.OpenURL($"https://console.improbable.io/projects/{projectName}/deployments/{deplInfo.Name}/overview/{deplInfo.Id}");
                        }

                        if (check.changed)
                        {
                            stateManager.SetStateObject(deplInfo.Id.GetHashCode(), foldoutState);
                        }
                    }

                    using (new EditorGUI.IndentLevelScope())
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (foldoutState)
                        {
                            EditorGUILayout.LabelField("Start Time", deplInfo.StartTime.ToString(CultureInfo.CurrentCulture));
                            EditorGUILayout.LabelField("Region", deplInfo.Region);

                            if (deplInfo.Workers.Count > 0)
                            {
                                EditorGUILayout.LabelField("Connected Workers");
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    foreach (var workerPair in deplInfo.Workers)
                                    {
                                        EditorGUILayout.LabelField(workerPair.Key, $"{workerPair.Value}");
                                    }
                                }
                            }

                            if (deplInfo.Tags.Count > 0)
                            {
                                EditorGUILayout.LabelField("Tags");
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    foreach (var tag in deplInfo.Tags)
                                    {
                                        EditorGUILayout.LabelField(tag);
                                    }
                                }
                            }
                        }

                        DrawHorizontalLine(3);
                    }
                }

                EditorGUIUtility.labelWidth = previousWidth;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                using (new EditorGUI.DisabledScope(manager.IsActive))
                {
                    if (GUILayout.Button("Refresh"))
                    {
                        manager.List(projectName);
                    }
                }
            }

            if (listedDeployments.Count > 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    selectedListedDeploymentIndex = EditorGUILayout.Popup("Deployment", selectedListedDeploymentIndex,
                        listedDeployments.Select(config => config.Name).ToArray());

                    using (new EditorGUI.DisabledScope(!IsSelectedValid(listedDeployments, selectedListedDeploymentIndex) || manager.IsActive))
                    {
                        if (GUILayout.Button("Stop deployment"))
                        {
                            manager.Stop(listedDeployments[selectedListedDeploymentIndex]);
                        }
                    }
                }
            }
        }

        private void UpdateSimulatedDeploymentNames(DeploymentConfig config)
        {
            for (var i = 0; i < config.SimulatedPlayerDeploymentConfigs.Count; i++)
            {
                var previousFoldoutState =
                    stateManager.GetStateObjectOrDefault<bool>(config.SimulatedPlayerDeploymentConfigs[i].Name.GetHashCode());

                config.SimulatedPlayerDeploymentConfigs[i].Name = $"{config.Deployment.Name}_sim{i + 1}";
                config.SimulatedPlayerDeploymentConfigs[i].TargetDeploymentName = config.Deployment.Name;

                stateManager.SetStateObject(config.SimulatedPlayerDeploymentConfigs[i].Name.GetHashCode(), previousFoldoutState);
            }
        }

        private void DrawHorizontalLine(int height)
        {
            var rect = EditorGUILayout.GetControlRect(false, height, EditorStyles.foldout);
            using (new Handles.DrawingScope(horizontalLineColor))
            {
                Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            }

            GUILayout.Space(rect.height);
        }

        private string GetProjectName()
        {
            var spatialJsonFile = Path.Combine(Tools.Common.SpatialProjectRootDir, "spatialos.json");

            if (!File.Exists(spatialJsonFile))
            {
                Debug.LogError($"Could not find a spatialos.json file located at: {spatialJsonFile}");
                return null;
            }

            var data = Json.Deserialize(File.ReadAllText(spatialJsonFile));

            if (data == null)
            {
                Debug.LogError($"Could not parse spatialos.json file located at: {spatialJsonFile}");
                return null;
            }

            try
            {
                return (string) data["name"];
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError($"Could not find a \"name\" field in {spatialJsonFile}.\n Raw exception: {e.Message}");
                return null;
            }
        }

        private bool TryAuthAndRetry(Ipc.Error error)
        {
            if (error.Code != Ipc.ErrorCode.Unauthenticated)
            {
                return false;
            }

            Debug.Log("Attempting to authenticate...");
            manager.Auth();

            return true;
        }

        private string GetStatusMessage()
        {
            if (!manager.IsActive)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            switch (manager.CurrentTask)
            {
                case UploadTask task:
                    sb.AppendLine($"Uploading assembly \"{task.Context.AssemblyName}\".");
                    break;
                case LaunchTask task:
                    sb.AppendLine($"Launching deployment \"{task.Context.Item3.Name}\" in project \"{task.Context.Item1}\".");
                    break;
                case ListTask task:
                    sb.AppendLine($"Listing deployments in project \"{task.Context}\"");
                    break;
                case StopTask task:
                    sb.AppendLine($"Stopping deployment \"{task.Context.Name}\"");
                    break;
                case AuthTask _:
                    sb.AppendLine("Attempting to authenticate.");
                    break;
                default:
                    sb.AppendLine("Unknown action running.");
                    break;
            }

            sb.Append("Assembly reloading locked.");

            return sb.ToString();
        }

        private bool IsSelectedValid<T>(List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        private void DrawSpinner(float value, Rect rect)
        {
            // There are 11 frames in the spinner animation, 0 till 11.
            var imageId = Mathf.RoundToInt(value) % 12;
            var icon = EditorGUIUtility.IconContent($"d_WaitSpin{imageId:D2}");
            EditorGUI.DrawPreviewTexture(rect, icon.image, spinnerMaterial, ScaleMode.ScaleToFit, 1);
        }
    }
}
