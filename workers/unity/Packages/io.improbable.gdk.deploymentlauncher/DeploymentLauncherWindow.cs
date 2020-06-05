using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Improbable.Gdk.Core.Editor;
using Improbable.Gdk.Tools;
using Improbable.Gdk.Tools.MiniJSON;
using Improbable.Worker.CInterop;
using UnityEditor;
using UnityEngine;
using AuthTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Tools.RedirectedProcessResult, int>;
using LaunchTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, (string, string, Improbable.Gdk.DeploymentLauncher.BaseDeploymentConfig)>;
using ListTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<System.Collections.Generic.List<Improbable.Gdk.DeploymentLauncher.DeploymentInfo>, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, string>;
using StopTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Core.Collections.Result<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.Ipc.Error>, Improbable.Gdk.DeploymentLauncher.DeploymentInfo>;
using UploadTask = Improbable.Gdk.DeploymentLauncher.Commands.WrappedTask<Improbable.Gdk.Tools.RedirectedProcessResult, Improbable.Gdk.DeploymentLauncher.AssemblyConfig>;

namespace Improbable.Gdk.DeploymentLauncher
{
    internal class DeploymentLauncherWindow : EditorWindow
    {
        private DeploymentLauncherWindowStyle style;

        private readonly TaskManager manager = new TaskManager();
        private readonly UIStateManager stateManager = new UIStateManager();

        private string[] allWorkers;

        private DeploymentLauncherConfig launcherConfig;
        private int selectedDeploymentIndex;
        private Vector2 scrollPos;
        private string projectName;

        private List<DeploymentInfo> listedDeployments = new List<DeploymentInfo>();
        private int selectedListedDeploymentIndex;

        // Minimum time required from last config change before saving to file
        private static readonly TimeSpan FileSavingInterval = TimeSpan.FromSeconds(1);
        private DateTime lastEditTime;

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
                MarkConfigAsDirty();
            }

            Application.quitting += OnExit;
        }

        private void OnDestroy()
        {
            OnExit();

            Application.quitting -= OnExit;
        }

        private void OnExit()
        {
            AssetDatabase.SaveAssets();

            manager.Cancel();
        }

        private void Update()
        {
            TrySaveChanges();

            manager.Update();

            foreach (var wrappedTask in manager.CompletedTasks.OfType<UploadTask>())
            {
                if (wrappedTask.Task.Result.ExitCode != 0)
                {
                    UnityEngine.Debug.LogError($"Upload of {wrappedTask.Context.AssemblyName} failed.");
                }
                else
                {
                    UnityEngine.Debug.Log($"Upload of {wrappedTask.Context.AssemblyName} succeeded.");
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<LaunchTask>())
            {
                var (cachedProjectName, assemblyName, config) = wrappedTask.Context;

                var result = wrappedTask.Task.Result;
                if (result.IsOkay)
                {
                    var host = GetHostForEnvironment();
                    Application.OpenURL($"{host}/projects/{cachedProjectName}/deployments/{config.Name}/overview");
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
                        UnityEngine.Debug.LogError($"Launch of {config.Name} failed. Code: {error.Code} Message: {error.Message}");
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
                        UnityEngine.Debug.LogError($"Failed to list deployments in project {wrappedTask.Context}. Code: {error.Code} Message: {error.Message}");
                    }
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<StopTask>())
            {
                var result = wrappedTask.Task.Result;
                var info = wrappedTask.Context;
                if (result.IsOkay)
                {
                    UnityEngine.Debug.Log($"Stopped deployment: \"{info.Name}\" successfully.");

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
                        UnityEngine.Debug.LogError($"Failed to stop deployment: \"{info.Name}\". Code: {error.Code} Message: {error.Message}.");
                    }
                }
            }

            foreach (var wrappedTask in manager.CompletedTasks.OfType<AuthTask>())
            {
                var result = wrappedTask.Task.Result;

                if (result.ExitCode == 0)
                {
                    UnityEngine.Debug.Log("Successfully authenticated with SpatialOS. Retrying previous action.");
                }
                else
                {
                    // Stop the potential infinite loop of retries.
                    manager.Cancel();
                    UnityEngine.Debug.LogError("Failed to authenticate with SpatialOS. Please run \"spatial auth login\" manually.");
                }
            }

            manager.ClearResults();
        }

        private static string GetHostForEnvironment()
        {
            var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
            return toolsConfig.EnvironmentPlatform == "cn-production"
                ? "https://console.spatialoschina.com"
                : "https://console.improbable.io";
        }

        private void OnGUI()
        {
            if (style == null)
            {
                style = new DeploymentLauncherWindowStyle();
            }

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
                        if (GUILayout.Button(style.ProjectRefreshButtonContents, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                        {
                            projectName = GetProjectName();
                            launcherConfig.SetProjectName(projectName);
                            MarkConfigAsDirty();
                        }
                    }

                    EditorGUILayout.LabelField("Project Name", projectName);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(manager.IsActive))
                    {
                        if (GUILayout.Button(style.EditRuntimeVersionButtonContents, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                        {
                            GdkToolsConfigurationWindow.ShowWindow();
                        }
                    }

                    var config = GdkToolsConfiguration.GetOrCreateInstance();

                    EditorGUILayout.LabelField("Runtime Version", config.RuntimeVersion);
                }

                CommonUIElements.DrawHorizontalLine(5, style.HorizontalLineColor);

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

                CommonUIElements.DrawHorizontalLine(5, style.HorizontalLineColor);
                GUILayout.Label("Live Deployments", EditorStyles.boldLabel);
                DrawDeploymentList();

                scrollPos = scrollView.scrollPosition;

                if (check.changed)
                {
                    MarkConfigAsDirty();
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
                    style.DrawSpinner(Time.realtimeSinceStartup * 10, rect);

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
                    UnityEngine.Debug.Log("Cancelled task.");
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Cannot cancel task as it has already reached completion.");
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
                    /* Responsive Layout, Intuitive API! */
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

            CommonUIElements.DrawHorizontalLine(3, style.HorizontalLineColor);

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

                    using (new EditorGUIUtility.IconSizeScope(style.SmallIconSize))
                    {
                        if (errors.Any())
                        {
                            GUILayout.Label(style.DeploymentConfigurationErrorContents);
                        }

                        if (GUILayout.Button(style.RemoveDeploymentConfigurationButtonContents, EditorStyles.miniButton))
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

            CommonUIElements.DrawHorizontalLine(5, style.HorizontalLineColor);

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

            var foldoutState = stateManager.GetStateObjectOrDefault<bool>($"{source.Name}region".GetHashCode());

            foldoutState = EditorGUILayout.Foldout(foldoutState, new GUIContent("Deployment location"));
            stateManager.SetStateObject($"{source.Name}region".GetHashCode(), foldoutState);

            if (foldoutState)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (EditorGUILayout.Toggle("Region",
                            source.DeploymentLocationType == DeploymentLocationType.Region, EditorStyles.radioButton))
                        {
                            dest.DeploymentLocationType = DeploymentLocationType.Region;
                        }

                        using (new EditorGUI.DisabledScope(dest.DeploymentLocationType == DeploymentLocationType.Cluster))
                        {
                            dest.Region = (DeploymentRegionCode) EditorGUILayout.EnumPopup(source.Region);
                        }
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        if (EditorGUILayout.Toggle("Cluster",
                            dest.DeploymentLocationType == DeploymentLocationType.Cluster, EditorStyles.radioButton))
                        {
                            dest.DeploymentLocationType = DeploymentLocationType.Cluster;
                        }

                        using (new EditorGUI.DisabledScope(dest.DeploymentLocationType == DeploymentLocationType.Region))
                        {
                            dest.Cluster = EditorGUILayout.TextField(source.Cluster);
                        }
                    }
                }
            }

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

                    using (new EditorGUIUtility.IconSizeScope(style.SmallIconSize))
                    {
                        if (GUILayout.Button(style.RemoveSimPlayerDeploymentButtonContents, EditorStyles.miniButton))
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
                UnityEngine.Debug.LogException(e);
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

                        if (GUILayout.Button(style.OpenDeploymentButtonContents, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                        {
                            var host = GetHostForEnvironment();
                            Application.OpenURL($"{host}/projects/{projectName}/deployments/{deplInfo.Name}/overview/{deplInfo.Id}");
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

                        CommonUIElements.DrawHorizontalLine(3, style.HorizontalLineColor);
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

        private string GetProjectName()
        {
            var spatialJsonFile = Path.Combine(Common.SpatialProjectRootDir, "spatialos.json");

            if (!File.Exists(spatialJsonFile))
            {
                UnityEngine.Debug.LogError($"Could not find a spatialos.json file located at: {spatialJsonFile}");
                return null;
            }

            var data = Json.Deserialize(File.ReadAllText(spatialJsonFile));

            if (data == null)
            {
                UnityEngine.Debug.LogError($"Could not parse spatialos.json file located at: {spatialJsonFile}");
                return null;
            }

            try
            {
                return (string) data["name"];
            }
            catch (KeyNotFoundException e)
            {
                UnityEngine.Debug.LogError($"Could not find a \"name\" field in {spatialJsonFile}.\n Raw exception: {e.Message}");
                return null;
            }
        }

        private bool TryAuthAndRetry(Ipc.Error error)
        {
            if (error.Code != Ipc.ErrorCode.Unauthenticated)
            {
                return false;
            }

            UnityEngine.Debug.Log("Attempting to authenticate...");
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

        private void MarkConfigAsDirty()
        {
            EditorUtility.SetDirty(launcherConfig);
            lastEditTime = DateTime.Now;
        }

        private void TrySaveChanges()
        {
            var timeSinceLastEdit = DateTime.Now - lastEditTime;
            if (EditorUtility.GetDirtyCount(launcherConfig) <= 0 || timeSinceLastEdit <= FileSavingInterval)
            {
                return;
            }

            AssetDatabase.SaveAssets();
        }

        private bool IsSelectedValid<T>(List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }
    }
}
