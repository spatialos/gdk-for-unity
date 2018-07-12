using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEditorInternal.VR;
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

        private InputField connectParam;
        private Button connectButton;
        private bool clicked;
        private WorkerBase worker;

        [Inject] private Data data;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            connectParam = GameObject.Find("ConnectParam").GetComponent<InputField>();
            connectButton = GameObject.Find("ConnectButton").GetComponent<Button>();
            connectButton.onClick.AddListener(IsClicked);
            worker = WorkerRegistry.GetWorkerForWorld(World);
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
            // if (!DeviceInfo.IsEmulator())
            // {
            //     SetConnectionParameters(GetInputString());
            // }
        }

        private string GetInputString()
        {
            return connectParam.text;
        }

        private void SetConnectionParameters(string param)
        {
            if (IsIpAddress(param))
            {
                worker.ConnectionConfig = ReceptionistConfig.CreateConnectionConfigForPhysicalAndroid(param);
            }

            // TODO: else -> cloud connection
        }

        private bool IsIpAddress(string param)
        {
            return param.Split('.').Length == 4;
        }
    }
}
