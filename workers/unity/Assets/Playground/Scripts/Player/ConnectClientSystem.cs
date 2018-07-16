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
        private GameObject panel;
        private Button button;
        private Text error;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
            panel = GameObject.Find("ConnectionPanel");
            button = GameObject.Find("ConnectButton").GetComponent<Button>();
            error = GameObject.Find("ConnectionError").GetComponent<Text>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var entity = data.Entity[i];
                var isConnected = worker.Connect(worker.ConnectionConfig);
                if (isConnected)
                {
                    panel.SetActive(false);
                }
                else
                {
                    error.text = "Connection attempt failed. Please check the IP address is correct";
                    error.gameObject.SetActive(true);
                    button.gameObject.SetActive(true);
                }

                PostUpdateCommands.RemoveComponent<ConnectButtonClicked>(entity);
            }
        }
    }
}
