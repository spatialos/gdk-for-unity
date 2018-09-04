using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Google.LongRunning;
using Improbable.SpatialOS.Deployment.V1Alpha1;
using Improbable.SpatialOS.Platform.Common;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    internal class DeploymentWindow : EditorWindow
    {
        private Deployment deployment;
        private readonly DeploymentServiceClient localDeploymentClient = DeploymentServiceClient.Create(SpatialdEndpoint);
        private readonly List<Deployment> deployments = new List<Deployment>();

        private const int SpatialDPort = 9876;

        private static readonly PlatformApiEndpoint SpatialdEndpoint = new PlatformApiEndpoint
        (
            "localhost",
            SpatialDPort,
            true
        );

        private string projectName;

        private Operation<Deployment, CreateDeploymentMetadata> localDeploymentOperation;

        [MenuItem("Window/SpatialOS/Launch Deployments")]
        private static void DeploymentWindowMenu()
        {
            var window = GetWindow<DeploymentWindow>();
            window.Show();
        }

        public void Awake()
        {
            titleContent = new GUIContent("Launch deployment");
        }

        private void OnEnable()
        {
            var dict = MiniJSON.Json.Deserialize(File.ReadAllText(Path.Combine(LocalLaunch.SpatialProjectRootDir, "spatialos.json"), Encoding.UTF8));
            projectName = (string) dict["name"];

            GetDeployments();
        }

        private async void GetDeployments()
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

                var dpls = await task.ReadPageAsync(50);
                deployments.AddRange(dpls);
                nextPageToken = dpls.NextPageToken;
            }
            while (!string.IsNullOrEmpty(nextPageToken));

            deployment = deployments.FirstOrDefault(dpl =>
                dpl.Status != Deployment.Types.Status.Stopped);
        }

        public void OnGUI()
        {            
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Spatial service log"))
                    {
                        if (Application.platform == RuntimePlatform.WindowsEditor)
                        {                         
                            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".improbable", "spatiald", "log", "spatiald.log");
                            Debug.Log(path);
                            Process.Start(path);
                        }
                    }

                    if (GUILayout.Button("Runtime log"))
                    {
                        if (Application.platform == RuntimePlatform.WindowsEditor)
                        {
                            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".improbable", "spatiald", "log", "local", "runtime.log");
                            Debug.Log(path);
                            Process.Start(path);
                        }
                    }
                }

                if (deployment == null)
                {
                    if (GUILayout.Button("Local launch"))
                    {
                        DoLocalLaunch();
                    }
                }
                else if(deployment.Status == Deployment.Types.Status.Starting)
                {
                    if (GUILayout.Button("Cancel"))
                    {
                        StopDeployment(deployment);
                    }
                }
                else if (deployment.Status == Deployment.Types.Status.Running)
                {
                    if (GUILayout.Button("Stop"))
                    {
                        StopDeployment(deployment);
                    }
                }

                if (deployment != null)
                {
                    GUILayout.Label($"Name: {deployment.Name}");
                    GUILayout.Label($"Id: {deployment.Id}");
                    GUILayout.Label($"Status: {deployment.Status}");
                }                
            }
        }

        private async void StopDeployment(Deployment dpl)
        {
            var result =
                localDeploymentClient.StopDeploymentAsync(new StopDeploymentRequest
                {
                    Id = dpl.Id,
                    ProjectName = projectName
                });

            GetDeployments();

            await result;

            GetDeployments();
        }

        private async void DoLocalLaunch()
        {
            var configJson = File.ReadAllText(Path.Combine(LocalLaunch.SpatialProjectRootDir, "default_launch.json"), Encoding.UTF8);
            localDeploymentOperation = localDeploymentClient.CreateDeployment(new CreateDeploymentRequest(
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

            GetDeployments();

            var result = await localDeploymentOperation.PollUntilCompletedAsync();

            if (result.IsFaulted)
            {
                Debug.LogException(result.Exception);
            }
            else if (result.IsCompleted)
            {
                deployment = result.Result;
            }

            localDeploymentOperation = null;
        }
    }
}
