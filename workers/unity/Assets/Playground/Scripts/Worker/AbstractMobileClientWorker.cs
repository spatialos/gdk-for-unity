using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using Playground;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractMobileClientWorker : MobileWorkerConnector
{
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject level;
    [SerializeField] private InputField ipAddressInput;
    [SerializeField] private Button connectButton;
    [SerializeField] private Text errorMessage;

    private GameObject levelInstance;
    private bool connected;

    protected string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

    public void Awake()
    {
        ipAddressInput.text = PlayerPrefs.GetString("cachedIp");
        connectButton.onClick.AddListener(Connect);
    }

    public async void Connect()
    {
        errorMessage.text = "";
        await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void HandleWorkerConnectionEstablished()
    {
        WorkerUtils.AddClientSystems(Worker.World);
        if (level == null)
        {
            return;
        }

        levelInstance = Instantiate(level, transform);
        levelInstance.transform.SetParent(null);

        connected = true;
        connectionPanel.SetActive(false);

        PlayerPrefs.SetString("cachedIp", input.text);
        PlayerPrefs.Save();
    }

    protected override void HandleWorkerConnectionFailure()
    {
        errorMessage.text = "Connection failure. Please check the IP address";
    }

    protected string GetIpFromField()
    {
        return input.text;
    }

    public override void Dispose()
    {
        if (levelInstance != null)
        {
            Destroy(levelInstance);
        }

        if (connected)
        {
            Destroy(this);
        }

        Worker?.Dispose();
        Worker = null;
    }
}
