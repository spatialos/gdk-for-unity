using Improbable.Gdk.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class ConnectionScreenController : MonoBehaviour
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";

        private const string MissingWorkerConnectorMessage =
            "The WorkerConnector behaviour was not found on the worker prefab";

        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button connectButton;
        [SerializeField] private Text errorMessage;
        [SerializeField] private GameObject clientWorkerPrefab;

        private GameObject worker;

        private string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

        public void Awake()
        {
            var hostIp = GetIPFromExtras();
            if (!string.IsNullOrEmpty(hostIp))
            {
                ipAddressInput.text = hostIp;
                TryConnect();
                return;
            }

            ipAddressInput.text = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
            connectButton.onClick.AddListener(TryConnect);
        }

        public void OnDestroy()
        {
            connectButton.onClick.RemoveListener(TryConnect);
        }

        public void OnConnectionSucceeded()
        {
            PlayerPrefs.SetString(HostIpPlayerPrefsKey, IpAddress);
            PlayerPrefs.Save();

            connectionPanel.SetActive(false);
        }

        public void OnConnectionFailed(string connectionError)
        {
            UnityObjectDestroyer.Destroy(worker);
            errorMessage.text =
                $"Connection failed. Please check the IP address entered.\nSpatialOS error message:\n{connectionError}";
        }

        private void TryConnect()
        {
            errorMessage.text = string.Empty;
            worker = Instantiate(clientWorkerPrefab);
            var workerConnector = worker.GetComponent<IMobileConnectionController>();

            if (workerConnector == null)
            {
                UnityObjectDestroyer.Destroy(worker);
                errorMessage.text = MissingWorkerConnectorMessage;
                throw new MissingComponentException(MissingWorkerConnectorMessage);
            }

            workerConnector.IpAddress = IpAddress;
            workerConnector.ConnectionScreenController = this;
            workerConnector.TryConnect();
        }

        private string GetIPFromExtras()
        {
            var arguments = CommandLineUtility.GetArguments();
            var hostIp = CommandLineUtility.GetCommandLineValue(arguments, RuntimeConfigNames.ReceptionistHost, string.Empty);
            return hostIp;
        }
    }
}
