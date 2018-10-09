using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Improbable.SpatialOS.Deployment.V1Alpha1;
using Improbable.SpatialOS.Platform.Common;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    internal class DeploymentWindow : EditorWindow
    {
        private enum Status
        {
            Stopped,
            Starting,
            Running,
            Stopping,
            Orphaned
        };

        // Well-known file locations.
        private readonly string lockFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".improbable",
                "spatiald.lock");

        private readonly string pidFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".improbable",
                "spatiald.pid");

        private readonly string spatialLogFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ".improbable", "spatiald", "log", "spatiald.log");

        private readonly string runtimeLogFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ".improbable", "spatiald", "log", "local", "runtime.log");

        private Deployment RunningDeployment
        {
            get => runningDeployment;
            set
            {
                runningDeployment = value;
                Repaint();
            }
        }

        private DeploymentServiceClient localDeploymentClient;
        private readonly List<Deployment> deployments = new List<Deployment>();
        private string projectName;
        private Status serviceStatus;
        private Deployment runningDeployment;

        private Status ServiceStatus
        {
            get => serviceStatus;
            set
            {
                serviceStatus = value;
                Repaint();
            }
        }

        [MenuItem("Window/SpatialOS/Launch Deployments")]
        private static void DeploymentWindowMenu()
        {
            var window = GetWindow<DeploymentWindow>();
            window.Show();
        }

        public async void Awake()
        {
            titleContent = new GUIContent("Launch deployment");

            await GetInitialState();
        }

        private async Task GetInitialState()
        {
            ServiceStatus = GetServiceStatus();

            if (serviceStatus == Status.Running)
            {
                var endpoint = GetSpatialEndpoint();
                if (endpoint != null)
                {
                    localDeploymentClient = DeploymentServiceClient.Create(endpoint);
                    RunningDeployment = await GetRunningDeployment();
                }
            }
        }

        private void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAssemblyReload;

            var dict = MiniJSON.Json.Deserialize(
                File.ReadAllText(Path.Combine(LocalLaunch.SpatialProjectRootDir, "spatialos.json"), Encoding.UTF8));
            projectName = (string) dict["name"];
        }

        private async void OnAssemblyReload()
        {
            await GetInitialState();
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAssemblyReload;
        }

        private async Task<Deployment> GetRunningDeployment()
        {
            deployments.Clear();

            var nextPageToken = string.Empty;
            do
            {
                var task = localDeploymentClient.ListDeploymentsAsync(new ListDeploymentsRequest
                {
                    ProjectName = projectName,
                    PageToken = nextPageToken,
                    PageSize = 50
                });

                var pagedDeployments = await task.ReadPageAsync(50);
                deployments.AddRange(pagedDeployments);
                nextPageToken = pagedDeployments.NextPageToken;
            }
            while (!string.IsNullOrEmpty(nextPageToken));

            return deployments.FirstOrDefault(dpl =>
                dpl.Status == Deployment.Types.Status.Running || dpl.Status == Deployment.Types.Status.Starting ||
                dpl.Status == Deployment.Types.Status.Stopping);
        }

        public async void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.HelpBox($"Spatial service is: {ServiceStatus}", MessageType.Info);

                    if (GUILayout.Button("Refresh"))
                    {
                        ServiceStatus = GetServiceStatus();
                    }

                    if (ServiceStatus == Status.Starting || ServiceStatus == Status.Running)
                    {
                        if (GUILayout.Button("Stop"))
                        {
                            await StopSpatialServiceAsync().ConfigureAwait(false);
                        }
                    }
                    else if (GUILayout.Button("Start"))
                    {
                        await StartSpatialServiceAsync().ConfigureAwait(false);
                    }

                    if (GUILayout.Button("View log"))
                    {
                        if (Application.platform == RuntimePlatform.WindowsEditor)
                        {
                            Process.Start(spatialLogFile);
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Runtime log"))
                    {
                        if (Application.platform == RuntimePlatform.WindowsEditor)
                        {
                            Process.Start(runtimeLogFile);
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    var runtimeStatus = RunningDeployment == null ? "Stopped" : RunningDeployment.Status.ToString();

                    EditorGUILayout.HelpBox($"Spatial runtime is: {runtimeStatus}", MessageType.Info);

                    switch (RunningDeployment)
                    {
                        case null:
                        {
                            if (GUILayout.Button("Start"))
                            {
                                await StartRuntimeAsync().ConfigureAwait(false);
                            }

                            break;
                        }
                        default:
                        {
                            if (GUILayout.Button("Stop"))
                            {
                                await StopDeploymentAsync(RunningDeployment).ConfigureAwait(false);
                            }

                            break;
                        }
                    }

                    if (GUILayout.Button("View log"))
                    {
                        if (Application.platform == RuntimePlatform.WindowsEditor)
                        {
                            Process.Start(runtimeLogFile);
                        }
                    }
                }
            }
        }

        private async Task StopSpatialServiceAsync()
        {
            localDeploymentClient = null;

            var tcs = new TaskCompletionSource<bool>();

            try
            {
                var startInfo = new ProcessStartInfo(Common.SpatialBinary, "service stop")
                    { CreateNoWindow = true, UseShellExecute = false };
                var process = Process.Start(startInfo);
                if (process == null)
                {
                    throw new Exception($"{nameof(process)} is null");
                }

                process.Exited += (sender, args) =>
                {
                    tcs.SetResult(true);
                    process.Dispose();
                };

                process.EnableRaisingEvents = true;
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            ServiceStatus = Status.Stopping;
            await tcs.Task;
            ServiceStatus = GetServiceStatus();
        }

        private async Task StopDeploymentAsync(Deployment dpl)
        {
            var result =
                localDeploymentClient.StopDeploymentAsync(new StopDeploymentRequest
                {
                    Id = dpl.Id,
                    ProjectName = projectName
                });

            RunningDeployment = await GetRunningDeployment().ConfigureAwait(false);

            await result.ConfigureAwait(false);

            RunningDeployment = await GetRunningDeployment().ConfigureAwait(false);
        }

        private async Task StartRuntimeAsync()
        {
            var configJson = File.ReadAllText(Path.Combine(LocalLaunch.SpatialProjectRootDir, "default_launch.json"),
                Encoding.UTF8);

            var operation = localDeploymentClient.CreateDeploymentAsync(new CreateDeploymentRequest(
                new CreateDeploymentRequest
                {
                    Deployment = new Deployment
                    {
                        Name = "local",
                        ProjectName = projectName,
                        LaunchConfig = new LaunchConfig
                        {
                            ConfigJson = configJson
                        }
                    }
                }));

            RunningDeployment = await GetRunningDeployment().ConfigureAwait(false);

            var result = await operation.ConfigureAwait(false);

            if (result.IsFaulted)
            {
                Debug.LogException(result.Exception);
            }
            else if (result.IsCompleted)
            {
                RunningDeployment = result.Result;
            }
        }

        private Status GetServiceStatus()
        {
            if (!File.Exists(pidFile))
            {
                return Status.Stopped;
            }

            var pidContent = File.ReadAllText(pidFile).Trim();
            try
            {
                using (Process.GetProcessById(int.Parse(pidContent)))
                {
                    // This will throw if the process doesn't exist.
                }

                return Status.Running;
            }
            catch
            {
                return Status.Orphaned;
            }
        }

        private PlatformApiEndpoint GetSpatialEndpoint()
        {
            try
            {
                var pidContent = File.ReadAllText(pidFile).Trim();
                var pid = int.Parse(pidContent);

                using (Process.GetProcessById(pid))
                {
                    // This will throw if the process doesn't exist.
                }

                var data = MiniJSON.Json.Deserialize(File.ReadAllText(lockFile));
                var port = (int) (long) data["port"];

                return new PlatformApiEndpoint
                (
                    "localhost",
                    port,
                    true
                );
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }

        private Task<bool> StartSpatialServiceAsync()
        {
            if (ServiceStatus == Status.Orphaned)
            {
                try
                {
                    File.Delete(pidFile);
                    File.Delete(lockFile);
                }
                catch
                {
                    // Ignore failed cleanup attempts.
                }
            }

            if (File.Exists(pidFile))
            {
                return Task.FromResult(true);
            }

            var fileWatcherCompletionSource = new TaskCompletionSource<bool>();
            var watcher = new FileSystemWatcher(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".improbable"),
                "spatiald.pid");

            watcher.Created += (sender, args) =>
            {
                Debug.LogFormat("Created {0}", args.FullPath);
                fileWatcherCompletionSource.SetResult(true);
                watcher.Dispose();
            };
            watcher.EnableRaisingEvents = true;

            var startInfo = new ProcessStartInfo(Common.SpatialBinary, "service start")
            {
                WorkingDirectory = LocalLaunch.SpatialProjectRootDir,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            try
            {
                var process = Process.Start(startInfo);
                if (process != null)
                {
                    process.Exited += (sender, args) =>
                    {
                        try
                        {
                            if (process.ExitCode == 0)
                            {
                                return;
                            }

                            var builder = new StringBuilder();
                            builder.AppendLine(process.StandardOutput.ReadToEnd());
                            builder.AppendLine(process.StandardError.ReadToEnd());

                            Debug.LogError(builder.ToString());
                        }
                        finally
                        {
                            process.Dispose();
                        }
                    };
                    process.EnableRaisingEvents = true;
                }
                else
                {
                    throw new Exception("spatial was not started.");
                }
            }
            catch (Exception e)
            {
                fileWatcherCompletionSource.SetException(e);
            }

            return fileWatcherCompletionSource.Task;
        }
    }
}
