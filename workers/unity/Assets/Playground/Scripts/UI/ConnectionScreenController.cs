using Improbable.Gdk.Core;
using UnityEngine;

namespace Playground
{
    [ExecuteInEditMode]
    public class ConnectionScreenController : MonoBehaviour
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";
        private const string MissingWorkerConnectorMessage =
            "The WorkerConnector behaviour was not found on the worker prefab";

        private const int GuiPadding = 10;
        private const float VerticalPositionRatio = 0.4f;

        [SerializeField] private GameObject clientWorkerPrefab;
        [SerializeField] private Font font;
        [SerializeField] private float screenWidthFontRatio = 20;

        private string ipAddressText;
        private string errorMessage;
        private bool isConnecting;

        private GameObject worker;

        public void Awake()
        {
            ipAddressText = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
        }

        public void OnGUI()
        {
            GUI.enabled = !isConnecting;
            using (new GUILayout.AreaScope(new Rect(
                GuiPadding,
                Screen.height * VerticalPositionRatio,
                Screen.width - GuiPadding * 2,
                Screen.height - GuiPadding * 2)))
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
        }

        private void TryConnect()
        {
            errorMessage = string.Empty;
            isConnecting = true;
            worker = Instantiate(clientWorkerPrefab);
            var workerConnector = worker.GetComponent<IMobileConnectionController>();

            if (workerConnector == null)
            {
                isConnecting  = false;
                UnityObjectDestroyer.Destroy(worker);
                errorMessage = MissingWorkerConnectorMessage;
                throw new MissingComponentException(MissingWorkerConnectorMessage);
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
            errorMessage = $"Connection failed. Please check the IP address entered.\nSpatialOS error message:\n{connectionError}";
            isConnecting = false;
        }
    }
}
