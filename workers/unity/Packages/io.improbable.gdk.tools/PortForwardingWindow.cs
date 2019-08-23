using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal class PortForwardingWindow : EditorWindow
    {
        [SerializeField] private string deploymentName = "";

        [SerializeField] private string workerId = "";

        [SerializeField] private int port;

        private Task<RedirectedProcessResult> activePortForwarding;
        private CancellationTokenSource tokenSrc;

        private bool isConnected;
        private bool portForwardingHasFailed;

        [MenuItem("SpatialOS/Port Forwarding", isValidateFunction: false, priority: MenuPriorities.PortForwarding)]
        public static void ShowWindow()
        {
            GetWindow<PortForwardingWindow>().Show();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Port Forwarding");
        }

        public void OnGUI()
        {
            using (new EditorGUI.DisabledScope(activePortForwarding != null))
            using (new EditorGUILayout.VerticalScope())
            {
                deploymentName = EditorGUILayout.TextField("Deployment Name", deploymentName);
                workerId = EditorGUILayout.TextField("Worker ID", workerId);
                port = EditorGUILayout.IntField("Port", port);

                var errors = GetErrors().ToList();

                if (errors.Any())
                {
                    EditorGUILayout.HelpBox(
                        $"Cannot forward port due to the following errors:\n\n{string.Join("\n", errors)}",
                        MessageType.Error);
                }

                using (new EditorGUI.DisabledScope(errors.Any()))
                {
                    if (GUILayout.Button("Forward port"))
                    {
                        portForwardingHasFailed = false;
                        isConnected = false;
                        activePortForwarding = RunPortForwarding();
                    }
                }
            }

            var messageType = MessageType.None;
            string message = null;

            if (isConnected)
            {
                messageType = MessageType.Info;
                message = $"Connected successfully to {workerId}:{port} on {deploymentName}!\n\nAssembly reloading locked.";
            }
            else if (activePortForwarding != null)
            {
                messageType = MessageType.Warning;
                message = "Establishing connection...\n\nAssembly reloading locked.";
            }
            else if (portForwardingHasFailed)
            {
                messageType = MessageType.Error;
                message = "Failed to establish connection. Check the Unity Console for more information.";
            }

            if (message != null)
            {
                EditorGUILayout.HelpBox(message, messageType);
            }

            if (activePortForwarding != null)
            {
                if (GUILayout.Button("Cancel"))
                {
                    tokenSrc.Cancel();
                }
            }
        }

        private void OnDisable()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Update()
        {
            if (activePortForwarding == null)
            {
                return;
            }

            if (!activePortForwarding.IsCompleted)
            {
                return;
            }

            // Cancelling or killing the process will result in a non-zero exit code, so ensure that we weren't
            // previously connected successfully.
            portForwardingHasFailed = activePortForwarding.Result.ExitCode != 0 && !isConnected;

            if (portForwardingHasFailed)
            {
                foreach (var error in activePortForwarding.Result.Stdout)
                {
                    Debug.LogError(error);
                }
            }

            // Reset state.
            isConnected = false;
            Cleanup();
        }

        private IEnumerable<string> GetErrors()
        {
            if (port > ushort.MaxValue || port < 1)
            {
                yield return "Port must be a number in the range of 1 to 65535.";
            }

            if (string.IsNullOrEmpty(deploymentName))
            {
                yield return "Deployment name cannot be empty.";
            }
            else if (!Regex.IsMatch(deploymentName, @"^[a-z0-9_]{2,32}$"))
            {
                yield return
                    $"Deployment Name \"{deploymentName}\" invalid. Must conform to the regex: ^[a-z0-9_]{{2,32}}$";
            }

            if (string.IsNullOrEmpty(workerId))
            {
                yield return "Worker ID cannot be empty.";
            }
        }

        private Task<RedirectedProcessResult> RunPortForwarding()
        {
            EditorApplication.LockReloadAssemblies();

            tokenSrc = new CancellationTokenSource();
            return RedirectedProcess.Command(Common.SpatialBinary)
                .InDirectory(Common.SpatialProjectRootDir)
                .WithArgs("project", "deployment", "worker", "port-forward",
                    "-d", deploymentName,
                    "-w", workerId,
                    "-p", $"{port}")
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .AddOutputProcessing(line => isConnected |= line.Contains("Established tunnel"))
                .RunAsync(tokenSrc.Token);
        }

        private void Cleanup()
        {
            tokenSrc?.Cancel();
            tokenSrc?.Dispose();
            tokenSrc = null;

            activePortForwarding?.Wait();
            activePortForwarding?.Dispose();
            activePortForwarding = null;

            EditorApplication.UnlockReloadAssemblies();
        }
    }
}
