using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    internal class ConnectClientSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            [ReadOnly] public ComponentDataArray<ConnectButtonClicked> DenotesClickedConnectButton;
        }

        [Inject] private Data data;

        private WorkerBase worker;
        private GameObject connectionPanel;
        private Button connectButton;
        private Text errorField;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
            connectionPanel = GameObject.FindWithTag("ConnectionPanel");
            connectButton = connectionPanel.transform.Find("ConnectButton").GetComponent<Button>();
            errorField = connectionPanel.transform.Find("ConnectionError").GetComponent<Text>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var entity = data.Entity[i];
                var isConnected = worker.Connect(worker.ConnectionConfig);
                if (isConnected)
                {
                    connectionPanel.SetActive(false);
                }
                else
                {
                    errorField.text = "Connection attempt failed. Please check the IP address is correct";
                    errorField.gameObject.SetActive(true);
                    connectButton.gameObject.SetActive(true);
                }

                PostUpdateCommands.RemoveComponent<ConnectButtonClicked>(entity);
            }
        }
    }
}
