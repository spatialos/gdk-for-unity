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

        private byte[] serializedArgumentsCache;

        private bool playerRequestQueued;
        private int playerCreationAttempts = 0;

        private int playerCreatorQueryAttempts = 0;
        private long? playerCreatorQueryId;

        private readonly List<EntityId> playerCreatorEntityIds = new List<EntityId>();

        private readonly EntityQuery playerCreatorQuery = new EntityQuery
        {
            Constraint = new ComponentConstraint(PlayerCreator.ComponentId),
            ResultType = new SnapshotResultType()
        };

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            workerSystem = World.GetExistingManager<WorkerSystem>();
            commandSystem = World.GetExistingManager<CommandSystem>();
            logDispatcher = workerSystem.LogDispatcher;

            QueryForPlayerCreators();

            if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
            {
                RequestPlayerCreation();
            }
        }

        private void QueryForPlayerCreators()
        {
            playerCreatorQueryId = commandSystem.SendCommand(new WorldCommands.EntityQuery.Request
            {
                EntityQuery = playerCreatorQuery
            });
            ++playerCreatorQueryAttempts;
        }

        public void RequestPlayerCreation(byte[] serializedArguments = null)
        {
            serializedArgumentsCache = serializedArguments;
            playerRequestQueued = true;
        }

        private void SendCreatePlayerRequest()
        {
            commandSystem.SendCommand(new PlayerCreator.CreatePlayer.Request(
                playerCreatorEntityIds[Random.Range(0, playerCreatorEntityIds.Count)],
                new CreatePlayerRequest(serializedArgumentsCache)
            ));
            playerRequestQueued = false;
        }

        private void RetryCreatePlayerRequest()
        {
            if (playerCreationAttempts < PlayerLifecycleConfig.MaxPlayerCreationAttempts)
            {
                ++playerCreationAttempts;

                logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                    $"Retrying player creation request, attempt {playerCreationAttempts}."
                ));

                SendCreatePlayerRequest();
            }
            else
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(
                    $"Unable to create player after {playerCreationAttempts} attempts."
                ));
            }
        }

        protected override void OnUpdate()
        {
            if (playerCreatorEntityIds.Count > 0)
            {
                if (playerRequestQueued)
                {
                    SendCreatePlayerRequest();
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
            else
            {
                var entityQueryResponses = commandSystem.GetResponses<WorldCommands.EntityQuery.ReceivedResponse>();
                for (var i = 0; i < entityQueryResponses.Count; i++)
                {
                    ref readonly var response = ref entityQueryResponses[i];
                    if (response.RequestId == playerCreatorQueryId)
                    {
                        if (response.StatusCode == StatusCode.Success && response.Result != null)
                        {
                            playerCreatorQueryId = null;
                            playerCreatorEntityIds.AddRange(response.Result.Keys);
                        }
                        else if (playerCreatorQueryAttempts > PlayerLifecycleConfig.MaxPlayerCreatorQueryAttempts)
                        {
                            logDispatcher.HandleLog(LogType.Error, new LogEvent(
                                $"Unable to find player creator after {playerCreatorQueryAttempts} attempts."
                            ));
                        }
                        else
                        {
                            logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                                $"Retrying player creator query, attempt {playerCreatorQueryAttempts}."
                            ));

                            QueryForPlayerCreators();
                        }

                        break;
                    }
                }
            }
        }
    }
}
