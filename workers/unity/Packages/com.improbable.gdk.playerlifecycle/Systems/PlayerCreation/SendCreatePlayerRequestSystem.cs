using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Unity.Entities;
using UnityEngine;
using EntityQuery = Improbable.Worker.CInterop.Query.EntityQuery;
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
        private Action<PlayerCreator.CreatePlayer.ReceivedResponse> playerCreationCallback;

        private int playerCreatorQueryRetries;
        private long? playerCreatorEntityQueryId;

        private readonly List<EntityId> playerCreatorEntityIds = new List<EntityId>();

        private readonly EntityQuery playerCreatorQuery = new EntityQuery
        {
            Constraint = new ComponentConstraint(PlayerCreator.ComponentId),
            ResultType = new SnapshotResultType()
        };

        protected override void OnCreate()
        {
            base.OnCreate();

            workerSystem = World.GetExistingSystem<WorkerSystem>();
            commandSystem = World.GetExistingSystem<CommandSystem>();
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
                    ++playerCreatorQueryRetries;

                    logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                        $"Retrying player creator query, attempt {playerCreatorQueryRetries}.\n{response.Message}"
                    ));

                    SendPlayerCreatorEntityQuery();
                }
                else
                {
                    var retryText = playerCreatorQueryRetries == 0
                        ? "1 attempt"
                        : $"{playerCreatorQueryRetries + 1} attempts";

                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                        $"Unable to find player creator after {retryText}."
                    ));
                }

                break;
            }
        }

        /// <summary>
        ///     Queues a request for player creation, if none are currently in progress. If player creation entities
        ///     have already been found, the request is sent in the next tick.
        /// </summary>
        /// <param name="serializedArguments">A serialized byte array of arbitrary player creation arguments.</param>
        /// <param name="callback">
        ///     An action to be invoked when the worker receives a response to the player creation request.
        /// </param>
        public void RequestPlayerCreation(byte[] serializedArguments = null,
            Action<PlayerCreator.CreatePlayer.ReceivedResponse> callback = null)
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
            playerCreationCallback = callback;
            playerCreationRequestQueued = true;
        }

        // We only enter this method if playerCreatorEntityIds.Count is greater than 0, meaning that there will
        // always be at least one element in the list of Player Creator entity IDs.
        private void SendCreatePlayerRequest()
        {
            // Here we construct our CreatePlayer request, and choose a random Player Creator entity to send it to.
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
                ++playerCreationRetries;

                logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                    $"Retrying player creation request, attempt {playerCreationRetries}."
                ));

                SendCreatePlayerRequest();
            }
            else
            {
                var retryText = playerCreationRetries == 0
                    ? "1 attempt"
                    : $"{playerCreationRetries + 1} attempts";

                logDispatcher.HandleLog(LogType.Error, new LogEvent(
                    $"Unable to create player after {retryText}."
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

                // Clears the player creation request ID to indicate that we have received a
                // response for the player creation request that we last sent.
                playerCreationRequestId = null;

                playerCreationCallback?.Invoke(response);

                switch (response.StatusCode)
                {
                    case StatusCode.Success:
                        playerCreationCallback = null;
                        break;
                    case StatusCode.AuthorityLost:
                    case StatusCode.InternalError:
                    case StatusCode.Timeout:
                        RetryCreatePlayerRequest();
                        break;
                    default:
                        playerCreationCallback = null;
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
