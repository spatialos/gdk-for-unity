using System.Text.RegularExpressions;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using Improbable.Gdk.Android;

#endif

namespace Playground
{
    [UpdateBefore(typeof(ConnectClientSystem))]
    internal class ConnectButtonClickSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorker;
        }

        private static string ipRegEx =
            @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

        private InputField connectParamInputField;
        private Button connectButton;
        private bool connectionRequested;
        private WorkerBase worker;
        private Text errorField;

        [Inject] private Data data;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
            var connectionPanel = GameObject.FindGameObjectWithTag("ConnectionPanel");
            connectParamInputField = connectionPanel.transform.Find("ConnectParam").GetComponent<InputField>();
            connectButton = connectionPanel.transform.Find("ConnectButton").GetComponent<Button>();
            connectButton.onClick.AddListener(IsClicked);
            errorField = connectionPanel.transform.Find("ConnectionError").GetComponent<Text>();
            if (!Application.isMobilePlatform)
            {
                connectParamInputField.gameObject.SetActive(false);
            }
        }

        protected override void OnUpdate()
        {
            if (connectionRequested)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    PostUpdateCommands.AddComponent(data.Entity[i], new ConnectButtonClicked());
                    connectButton.gameObject.SetActive(false);
                    errorField.text = "";
                    connectionRequested = false;
                }
            }
        }

        private void IsClicked()
        {
            connectionRequested = true;
#if UNITY_ANDROID
            if (Application.isMobilePlatform)
            {
                if (DeviceInfo.IsAndroidStudioEmulator() && connectParamInputField.text.Equals(string.Empty))
                {
                    worker.ConnectionConfig = ReceptionistConfig.CreateConnectionConfigForAndroidEmulator();
                }
                else
                {
                    SetConnectionParameters(connectParamInputField.text);
                }
            }
#endif
        }


        private void SetConnectionParameters(string param)
        {
            if (IsIpAddress(param))
            {
                worker.ConnectionConfig = ReceptionistConfig.CreateConnectionConfigForPhysicalAndroid(param);
            }
            else
            {
                connectionRequested = false;
                errorField.text = "Entered text is not a valid IP address.";
            }

            // TODO: UTY-558 else -> cloud connection
        }

        private static bool IsIpAddress(string param)
        {
            return Regex.Match(param, ipRegEx).Success;
        }
    }
}
