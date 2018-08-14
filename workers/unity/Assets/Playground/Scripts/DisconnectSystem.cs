using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class DisconnectSystem : ComponentSystem
    {
        public struct DisconnectData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectMessage;
            [ReadOnly] public SharedComponentDataArray<WorkerConfig> Configs;
        }

        [Inject] private DisconnectData data;

        protected override void OnUpdate()
        {
            data.Configs[0].Worker.LogDispatcher.HandleLog(
                LogType.Warning,
                new LogEvent($"Disconnected from SpatialOS with reason: \"{data.DisconnectMessage[0].ReasonForDisconnect}\""
                ));
            Worker.Disconnect(data.Configs[0].Worker);
        }
    }
}
