using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private readonly EntityId playerCreatorEntityId = new EntityId(1);

        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandSenders.CreatePlayer> RequestSenders;
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
            data.RequestSenders[0].RequestsToSend
                .Add(new PlayerCreator.CreatePlayer.Request(playerCreatorEntityId, request));
        }
    }
}
