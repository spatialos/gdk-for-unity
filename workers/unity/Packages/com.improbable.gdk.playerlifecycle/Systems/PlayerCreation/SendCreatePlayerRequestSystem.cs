using System.Collections.Generic;
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

        private readonly Dictionary<long, float> requestTimeMap = new Dictionary<long, float>();
        private ILogDispatcher logDispatcher;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            logDispatcher = Improbable.Gdk.Core.Worker.GetWorkerFromWorld(World).LogDispatcher;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < sendData.Length; ++i)
            {
                var request = new CreatePlayerRequestType(new Generated.Improbable.Vector3f { X = 0, Y = 0, Z = 0 });

                var createPlayerRequest = PlayerCreator.CreatePlayer.CreateRequest(playerCreatorEntityId, request);

                requestTimeMap[createPlayerRequest.RequestId] = Time.realtimeSinceStartup;

                sendData.RequestSenders[i].RequestsToSend
                    .Add(createPlayerRequest);
            }

            for (var i = 0; i < responseData.Length; ++i)
            {
                foreach (var receivedResponse in responseData.Responses[i].Responses)
                {
                    var requestId = receivedResponse.RequestId;

                    if (requestTimeMap.TryGetValue(requestId, out var requestTime))
                    {
                        requestTimeMap.Remove(requestId);

                        logDispatcher.HandleLog(LogType.Log,
                            new LogEvent(
                                $"Time taken to be spawned : {Time.realtimeSinceStartup - requestTime:0.00}s."));

                        continue;
                    }
                    else
                    {
                        logDispatcher.HandleLog(LogType.Error,
                            new LogEvent($"Failed to get timing information for request with ID {requestId}."));
                    }

                    if (receivedResponse.StatusCode != StatusCode.Success)
                    {
                        logDispatcher.HandleLog(LogType.Error, new LogEvent(
                            $"Create player request (with id {requestId}) failed: {receivedResponse.Message}"));
                    }
                }
            }
        }
    }
}
