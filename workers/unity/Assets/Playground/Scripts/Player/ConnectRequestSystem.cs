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
        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<WorkerEntityTag> Worker;
        }

        public Button ConnectButton;
        private bool clicked;

        [Inject] private Data data;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            ConnectButton = GameObject.Find("ConnectButton").GetComponent<Button>();
            ConnectButton.onClick.AddListener(IsClicked);
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
                ConnectButton.gameObject.SetActive(false);
                clicked = false;
            }
        }

        private void IsClicked()
        {
            clicked = true;
        }
    }
}
