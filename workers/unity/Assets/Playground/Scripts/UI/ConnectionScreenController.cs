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
#pragma warning disable 649
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button localConnectButton;
        [SerializeField] private Button cloudConnectButton;
        [SerializeField] private Text errorMessage;
        [SerializeField] private GameObject clientWorkerPrefab;
#pragma warning restore 649

        private GameObject worker;

        private string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

        public void Awake()
        {
            var hostIp = GetReceptionistHostFromArguments();
            if (!string.IsNullOrEmpty(hostIp))
            {
                ipAddressInput.text = hostIp;
            }
            else
            {
                ipAddressInput.text = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
            }

            localConnectButton.onClick.AddListener(TryLocalConnect);
            cloudConnectButton.onClick.AddListener(TryCloudConnect);
        }

        public void OnDestroy()
        {
            localConnectButton.onClick.RemoveListener(TryLocalConnect);
            cloudConnectButton.onClick.RemoveListener(TryCloudConnect);
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

        public void OnDisconnected(string disconnectReason)
        {
            UnityObjectDestroyer.Destroy(worker);
            ipAddressInput.text = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
            connectionPanel.SetActive(true);
        }

        private IMobileConnectionController PrepareConnect()
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
            return workerConnector;
        }

        private void TryLocalConnect()
        {
            var workerConnector = PrepareConnect();
            workerConnector.TryConnectAsync(ConnectionService.Receptionist);
        }

        private void TryCloudConnect()
        {
            var workerConnector = PrepareConnect();
            workerConnector.TryConnectAsync(ConnectionService.AlphaLocator);
        }

        private string GetReceptionistHostFromArguments()
        {
#if UNITY_ANDROID
            var arguments = Improbable.Gdk.Mobile.Android.LaunchArguments.GetArguments();
            var hostIp =
                CommandLineUtility.GetCommandLineValue(arguments, RuntimeConfigNames.ReceptionistHost, string.Empty);
            return hostIp;
#else
            return string.Empty;
#endif
        }
    }
}
