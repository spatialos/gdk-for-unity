using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private bool playerCreationRequestQueued;
        private long? playerCreationRequestId;
        private int playerCreationRetries;

        private int playerCreatorQueryRetries;
        private long? playerCreatorEntityQueryId;

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

            SendPlayerCreatorEntityQuery();

            if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
            {
                RequestPlayerCreation();
            }
        }

        private void SendPlayerCreatorEntityQuery()
        {
            playerCreatorEntityQueryId = commandSystem.SendCommand(new WorldCommands.EntityQuery.Request
            {
                EntityQuery = playerCreatorQuery
            });
        }

        private void HandleEntityQueryResponses()
        {
            var entityQueryResponses = commandSystem.GetResponses<WorldCommands.EntityQuery.ReceivedResponse>();
            for (var i = 0; i < entityQueryResponses.Count; i++)
            {
                ref readonly var response = ref entityQueryResponses[i];
                if (response.RequestId != playerCreatorEntityQueryId)
                {
                    continue;
                }

                playerCreatorEntityQueryId = null;

                if (response.StatusCode == StatusCode.Success)
                {
                    playerCreatorEntityIds.AddRange(response.Result.Keys);
                }
                else if (playerCreatorQueryRetries < PlayerLifecycleConfig.MaxPlayerCreatorQueryRetries)
                {
                    logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                        $"Retrying player creator query, attempt {playerCreatorQueryRetries}."
                    ));

                    SendPlayerCreatorEntityQuery();

                    ++playerCreatorQueryRetries;
                }
                else
                {
                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                        $"Unable to find player creator after {playerCreatorQueryRetries} retries."
                    ));
                }

                break;
            }
        }

        public void RequestPlayerCreation(byte[] serializedArguments = null)
        {
            if (playerCreationRequestId.HasValue)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                    $"Unable to perform player creation request as one has already been requested."
                ));
                return;
            }

            playerCreationRetries = 0;
            serializedArgumentsCache = serializedArguments;
            playerCreationRequestQueued = true;
        }

        private void SendCreatePlayerRequest()
        {
            playerCreationRequestId = commandSystem.SendCommand(new PlayerCreator.CreatePlayer.Request(
                playerCreatorEntityIds[Random.Range(0, playerCreatorEntityIds.Count)],
                new CreatePlayerRequest(serializedArgumentsCache)
            ));

            playerCreationRequestQueued = false;
        }

        private void RetryCreatePlayerRequest()
        {
            if (playerCreationRetries < PlayerLifecycleConfig.MaxPlayerCreationRetries)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                    $"Retrying player creation request, attempt {playerCreationRetries}."
                ));

                ++playerCreationRetries;
                SendCreatePlayerRequest();
            }
            else
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(
                    $"Unable to create player after {playerCreationRetries} attempts."
                ));
            }
        }

        private void HandlePlayerCreation()
        {
            if (playerCreationRequestQueued)
            {
                SendCreatePlayerRequest();
            }

            if (!playerCreationRequestId.HasValue)
            {
                return;
            }

            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (response.RequestId != playerCreationRequestId)
                {
                    continue;
                }

                playerCreationRequestId = null;

                switch (response.StatusCode)
                {
                    case StatusCode.Success:
                        break;
                    case StatusCode.AuthorityLost:
                    case StatusCode.InternalError:
                    case StatusCode.Timeout:
                        RetryCreatePlayerRequest();
                        break;
                    default:
                        logDispatcher.HandleLog(LogType.Error, new LogEvent(
                            $"Create player request failed: {response.Message}"
                        ));
                        break;
                }

                break;
            }
        }

        protected override void OnUpdate()
        {
            if (playerCreatorEntityIds.Count > 0)
            {
                HandlePlayerCreation();
            }
            else
            {
                HandleEntityQueryResponses();
            }
        }
    }
}
