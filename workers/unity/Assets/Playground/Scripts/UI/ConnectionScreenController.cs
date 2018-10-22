using Improbable.Gdk.Core;
using UnityEngine;

namespace Playground
{
    [ExecuteInEditMode]
    public class ConnectionScreenController : MonoBehaviour
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";
        private const int GuiPadding = 10;

        [SerializeField] private GameObject clientWorkerPrefab;
        [SerializeField] private Font font;
        [SerializeField] private int wantedFontSize = 20;

        private bool showConnectionGui = true;
        private bool isConnecting = false;
        private string ipAddressText = null;
        private string errorMessage = "";

        private GameObject worker;

        public void Awake()
        {
            ipAddressText = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
        }

        public void OnGUI()
        {
            if (!showConnectionGui)
            {
                return;
            }

            using (new GUILayout.AreaScope(new Rect(
                GuiPadding,
                GuiPadding,
                Screen.width - GuiPadding * 2,
                Screen.height - GuiPadding * 2)))
            {
                var guiEnabled = GUI.enabled;

                GUI.enabled = !isConnecting;
                try
                {
                    using (new ResizedGui(font, wantedFontSize,
                        GUI.skin.label,
                        GUI.skin.textField,
                        GUI.skin.button,
                        GUI.skin.textArea
                    ))
                    {
                        ResizedGui.Label("Enter IP address:");
                        ipAddressText = ResizedGui.TextField(ipAddressText);

                        if (ResizedGui.Button("Connect"))
                        {
                            if (Application.isPlaying)
                            {
                                TryConnect();
                            }
                        }

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            ResizedGui.Label("Error:");

                            ResizedGui.TextArea(errorMessage);
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
            errorMessage = "";
            isConnecting = true;

            worker = Instantiate(clientWorkerPrefab);
            var workerConnector = worker.GetComponent<IMobileConnectionController>();

            if (workerConnector == null)
            {
                UnityObjectDestroyer.Destroy(worker);
                throw new MissingComponentException("The WorkerConnector behaviour was not found on the worker prefab");
            }

            workerConnector.IpAddress = ipAddressText;
            workerConnector.ConnectionScreenController = this;
            workerConnector.TryConnect();
        }

        public void OnConnectionSucceeded()
        {
            PlayerPrefs.SetString(HostIpPlayerPrefsKey, ipAddressText);
            PlayerPrefs.Save();

            showConnectionGui = false;
            isConnecting = false;
        }

        public void OnConnectionFailed(string connectionError)
        {
            UnityObjectDestroyer.Destroy(worker);
            errorMessage = $"Connection failed. Please check the IP address entered. {connectionError}";
            isConnecting = false;
        }
    }
}
