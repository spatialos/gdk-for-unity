using Improbable.Gdk.Core;
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
            public EntityArray Entity;
            public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            public ComponentDataArray<ConnectButtonClicked> DenotesClickedConnectButton;
        }

        [Inject] private Data data;

        private WorkerBase worker;
        private GameObject panel;
        private Button button;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
            panel = GameObject.Find("ConnectionPanel");
            button = GameObject.Find("ConnectButton").GetComponent<Button>();
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
                    // TODO: add a text on screen that config is wrong
                    button.gameObject.SetActive(true);
                }

                PostUpdateCommands.RemoveComponent<ConnectButtonClicked>(entity);
            }
        }
    }
}
