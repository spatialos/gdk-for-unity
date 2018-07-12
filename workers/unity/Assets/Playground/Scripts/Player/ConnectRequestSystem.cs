using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    [UpdateBefore(typeof(ConnectClientSystem))]
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    internal class ConnectRequestSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<WorkerEntityTag> DenotesWorker;
        }

        public Button connectButton;
        private bool clicked;

        [Inject] private Data data;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            connectButton = GameObject.Find("ConnectButton").GetComponent<Button>();
            connectButton.onClick.AddListener(IsClicked);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                if (!clicked)
                {
                    continue;
                }

                PostUpdateCommands.AddComponent(data.Entity[i], new ConnectButtonClicked());
                connectButton.gameObject.SetActive(false);
                clicked = false;
            }
        }

        private void IsClicked()
        {
            clicked = true;
        }
    }
}
