using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private readonly EntityId playerCreatorEntityId = new EntityId(1);

        private struct NewEntityData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            [ReadOnly] public ComponentDataArray<OnConnected> DenotesJustConnected;
            public EntityArray Entities;
        }

        private struct SendData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandSenders.CreatePlayer> RequestSenders;
            [ReadOnly] public ComponentDataArray<ShouldRequestPlayerTag> DenotesShouldRequestPlayer;
            public EntityArray Entities;
        }

        private struct ResponseData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandResponses.CreatePlayer> Responses;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            public EntityArray Entities;
        }

        private ILogDispatcher logDispatcher;

        [Inject] private NewEntityData newEntityData;
        [Inject] private SendData sendData;
        [Inject] private ResponseData responseData;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            logDispatcher = World.GetExistingManager<WorkerSystem>().LogDispatcher;
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < newEntityData.Length; ++i)
            {
                PostUpdateCommands.AddComponent(newEntityData.Entities[i], new ShouldRequestPlayerTag());
            }

            for (var i = 0; i < sendData.Length; ++i)
            {
                var request = new CreatePlayerRequestType(new Improbable.Vector3f { X = 0, Y = 0, Z = 0 });
                var createPlayerRequest = PlayerCreator.CreatePlayer.CreateRequest(playerCreatorEntityId, request);

                sendData.RequestSenders[i].RequestsToSend
                    .Add(createPlayerRequest);
                PostUpdateCommands.RemoveComponent<ShouldRequestPlayerTag>(sendData.Entities[i]);
            }

            // Currently this has a race condition where you can receive two entites
            // The fix for this is more sophisticted server side handling of requests
            for (var i = 0; i < responseData.Length; ++i)
            {
                foreach (var receivedResponse in responseData.Responses[i].Responses)
                {
                    if (receivedResponse.StatusCode == StatusCode.AuthorityLost)
                    {
                        PostUpdateCommands.AddComponent(responseData.Entities[i], new ShouldRequestPlayerTag());
                    }
                    else if (receivedResponse.StatusCode != StatusCode.Success)
                    {
                        logDispatcher.HandleLog(LogType.Error, new LogEvent(
                            $"Create player request failed: {receivedResponse.Message}"));
                    }
                }
            }
        }
    }
}
