using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private const long PlayerCreatorEntityId = 1;

        public struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<CommandRequestSender<SpatialOSPlayerCreator>> RequestSenders;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            [ReadOnly] public ComponentDataArray<OnConnected> DenotesJustConnected;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            var request = new CreatePlayerRequestType
            {
                Position = new Generated.Improbable.Vector3f { X = 0, Y = 0, Z = 0 }
            };
            data.RequestSenders[0].SendCreatePlayerRequest(PlayerCreatorEntityId, request);
        }
    }
}
