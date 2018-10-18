using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class ConnectionScreenController : MonoBehaviour
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";

        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button connectButton;
        [SerializeField] private Text errorMessage;
        [SerializeField] private GameObject clientWorkerPrefab;

        private GameObject worker;

        private string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

        public void Awake()
        {
            ipAddressInput.text = PlayerPrefs.GetString(HostIpPlayerPrefsKey);
            connectButton.onClick.AddListener(TryConnect);
        }

        private void TryConnect()
        {
            worker = Instantiate(clientWorkerPrefab);
            var workerConnector = worker.GetComponent<IMobileConnectionController>();

            if (workerConnector == null)
            {
                UnityObjectDestroyer.Destroy(worker);
                throw new MissingComponentException("The WorkerConnector behaviour was not found on the worker prefab");
            }

            workerConnector.IpAddress = IpAddress;
            workerConnector.ConnectionScreenController = this;
            workerConnector.TryConnect();
        }

        public void OnConnectionSucceeded()
        {
            PlayerPrefs.SetString(HostIpPlayerPrefsKey, IpAddress);
            PlayerPrefs.Save();

            connectionPanel.SetActive(false);
        }

        public void OnConnectionFailed()
        {
            UnityObjectDestroyer.Destroy(worker);
            errorMessage.text = "Connection failed. Please check the IP address entered.";
        }
    }
}
