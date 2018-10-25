using Improbable.Gdk.Core;
using UnityEngine;

namespace Playground
{
    [ExecuteInEditMode]
    public class ConnectionScreenController : MonoBehaviour
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";
        private const int GuiPadding = 10;
        private const float VerticalPositionRatio = 0.4f;

        [SerializeField] private GameObject clientWorkerPrefab;
        [SerializeField] private Font font;
        [SerializeField] private float screenWidthFontRatio = 20;

        private bool isConnecting;
        private string ipAddressText;
        private string errorMessage = string.Empty;

        private GameObject worker;

        public void Awake()
        {
            ipAddressText = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
        }

        public void OnGUI()
        {
            using (new GUILayout.AreaScope(new Rect(
                GuiPadding,
                Screen.height * VerticalPositionRatio,
                Screen.width - GuiPadding * 2,
                Screen.height - GuiPadding * 2)))
            {
                var guiEnabled = GUI.enabled;

                GUI.enabled = !isConnecting;
                try
                {
                    using (new ResizedGui(font, screenWidthFontRatio,
                        GUI.skin.label,
                        GUI.skin.textField,
                        GUI.skin.button,
                        GUI.skin.textArea
                    ))
                    {
                        ResizedGui.Label("Enter IP address:");
                        ipAddressText = ResizedGui.TextField(ipAddressText);

                        if (ResizedGui.Button("Connect") && Application.isPlaying)
                        {
                            TryConnect();
                        }

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            ResizedGui.TextArea($"Error: {errorMessage}");
                        }
                    }
                }
                finally
                {
                    GUI.enabled = guiEnabled;
                }
            }
        }

        private void TryConnect()
        {
            errorMessage = string.Empty;
            isConnecting = true;

            worker = Instantiate(clientWorkerPrefab);
            var workerConnector = worker.GetComponent<IMobileConnectionController>();

            if (workerConnector == null)
            {
                isConnecting = false;
                UnityObjectDestroyer.Destroy(worker);
                errorMessage = "The WorkerConnector behaviour was not found on the worker prefab";
                throw new MissingComponentException("The WorkerConnector behaviour was not found on the worker prefab.");
            }

            workerConnector.IpAddress = ipAddressText;
            workerConnector.ConnectionScreenController = this;
            workerConnector.TryConnect();
        }

        public void OnConnectionSucceeded()
        {
            PlayerPrefs.SetString(HostIpPlayerPrefsKey, ipAddressText);
            PlayerPrefs.Save();

            Destroy(gameObject);
        }

        public void OnConnectionFailed(string connectionError)
        {
            UnityObjectDestroyer.Destroy(worker);
            errorMessage = $"Connection failed. Please check the IP address entered.\nError received by SpatialOS:\n{connectionError}";
            isConnecting = false;
        }
    }
}
