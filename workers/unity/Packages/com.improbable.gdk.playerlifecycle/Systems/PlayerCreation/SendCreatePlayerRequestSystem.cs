using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;
        private ILogDispatcher logDispatcher;

        private ComponentGroup initializationGroup;

        private byte[] serializedArgumentsCache;

        private List<EntityId> playerCreatorEntityIds;
        private long? playerCreatorQueryId;

        private bool sendAutoPlayerCreationRequest;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            workerSystem = World.GetExistingManager<WorkerSystem>();
            commandSystem = World.GetExistingManager<CommandSystem>();
            logDispatcher = workerSystem.LogDispatcher;

            playerCreatorEntityIds = new List<EntityId>();

            initializationGroup = GetComponentGroup(
                ComponentType.ReadOnly<WorkerEntityTag>(),
                ComponentType.ReadOnly<OnConnected>()
            );
        }

        public bool RequestPlayerCreation(byte[] serializedArguments = null)
        {
            serializedArgumentsCache = serializedArguments;

            var playerCreatorCount = playerCreatorEntityIds.Count;
            if (playerCreatorCount > 0)
            {
                commandSystem.SendCommand(new PlayerCreator.CreatePlayer.Request(
                    playerCreatorEntityIds[Random.Range(0, playerCreatorCount)],
                    new CreatePlayerRequestType(serializedArgumentsCache)));
                return true;
            }
            else
            {
                Debug.LogWarning("Unable to send player creation request: no player creator entities found yet.");
                return false;
            }
        }

        private void RetryCreatePlayerRequest()
        {
            RequestPlayerCreation(serializedArgumentsCache);
        }

        protected override void OnUpdate()
        {
            if (PlayerLifecycleConfig.AutoRequestPlayerCreation && !initializationGroup.IsEmptyIgnoreFilter)
            {
                sendAutoPlayerCreationRequest = true;
            }

            if (playerCreatorEntityIds.Count == 0)
            {
                if (!playerCreatorQueryId.HasValue)
                {
                    playerCreatorQueryId = commandSystem.SendCommand(new WorldCommands.EntityQuery.Request
                    {
                        EntityQuery = new EntityQuery
                        {
                            Constraint = new ComponentConstraint(PlayerCreator.ComponentId),
                            ResultType = new SnapshotResultType()
                        }
                    });
                }
                else
                {
                    var entityQueryResponses = commandSystem.GetResponses<WorldCommands.EntityQuery.ReceivedResponse>();
                    for (var i = 0; i < entityQueryResponses.Count; i++)
                    {
                        ref readonly var response = ref entityQueryResponses[i];
                        if (response.RequestId == playerCreatorQueryId)
                        {
                            playerCreatorQueryId = null;

                            if (response.Result != null)
                            {
                                playerCreatorEntityIds.AddRange(response.Result.Keys);
                            }
                        }
                    }
                }

                return;
            }

            if (sendAutoPlayerCreationRequest)
            {
                RequestPlayerCreation();
                sendAutoPlayerCreationRequest = false;
            }

            // Currently this has a race condition where you can receive two entities
            // The fix for this is more sophisticated server side handling of requests
            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (response.StatusCode == StatusCode.AuthorityLost)
                {
                    RetryCreatePlayerRequest();
                }
                else if (response.StatusCode != StatusCode.Success)
                {
                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                        $"Create player request failed: {response.Message}"
                    ));
                }
            }
        }
    }
}
