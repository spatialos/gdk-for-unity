using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private readonly EntityId playerCreatorEntityId = new EntityId(1);

        private struct SendData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandSenders.CreatePlayer> RequestSenders;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            [ReadOnly] public ComponentDataArray<OnConnected> DenotesJustConnected;
        }

        [Inject] private SendData sendData;

        private struct ResponseData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandResponses.CreatePlayer> Responses;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }

        [Inject] private ResponseData responseData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < sendData.Length; ++i)
            {
                var request = new CreatePlayerRequestType
                {
                    Position = new Generated.Improbable.Vector3f { X = 0, Y = 0, Z = 0 }
                };

                sendData.RequestSenders[i].RequestsToSend
                    .Add(PlayerCreator.CreatePlayer.CreateRequest(playerCreatorEntityId, request));
            }

            for (var i = 0; i < responseData.Length; ++i)
            {
                foreach (var receivedResponse in responseData.Responses[i].Responses)
                {
                    Debug.Log($"Create player response code: {receivedResponse.StatusCode}");

                    if (receivedResponse.StatusCode != StatusCode.Success)
                    {
                        Debug.LogError($"Create player failed: {receivedResponse.Message}");
                    }
                }
            }
        }
    }
}
