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

        private ILogDispatcher logDispatcher;

        private PlayerCreationContext currentContext;

        protected override void OnCreate()
        {
            base.OnCreate();

            commandSystem = World.GetExistingSystem<CommandSystem>();
            logDispatcher = World.GetExistingSystem<WorkerSystem>().LogDispatcher;

            if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
            {
                RequestPlayerCreation();
            }
        }

        protected override void OnUpdate()
        {
            if (currentContext == null)
            {
                return;
            }

            if (currentContext.Step())
            {
                currentContext = null;
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
            if (currentContext != null)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                    $"Unable to perform player creation request as one has already been requested."
                ));
                return;
            }

            currentContext = new PlayerCreationContext(commandSystem,
                logDispatcher,
                serializedArguments ?? new byte[] { },
                callback);
        }


        private class PlayerCreationContext
        {
            private readonly CommandSystem commandSystem;
            private readonly ILogDispatcher logDispatcher;

            private readonly byte[] serializedArgs;
            private readonly Action<PlayerCreator.CreatePlayer.ReceivedResponse> callback;

            private int retries;
            private CommandRequestId? currentRequest;

            public PlayerCreationContext(CommandSystem commandSystem, ILogDispatcher logDispatcher,
                byte[] serializedArgs,
                Action<PlayerCreator.CreatePlayer.ReceivedResponse> callback)
            {
                this.commandSystem = commandSystem;
                this.logDispatcher = logDispatcher;
                this.serializedArgs = serializedArgs;
                this.callback = callback;
            }

            /// <returns>True if the player creation loop is complete, false otherwise.</returns>
            public bool Step()
            {
                if (!currentRequest.HasValue)
                {
                    SendCommand();
                    return false;
                }

                var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();

                for (var i = 0; i < responses.Count; i++)
                {
                    ref readonly var response = ref responses[i];

                    if (response.RequestId != currentRequest.Value)
                    {
                        continue;
                    }

                    switch (response.StatusCode)
                    {
                        case StatusCode.Success:
                            break;
                        case StatusCode.AuthorityLost:
                        case StatusCode.InternalError:
                        case StatusCode.Timeout:
                            return RetryCommand();
                        default:
                            logDispatcher.Error(
                                $"Create player request failed with status code {response.StatusCode}: '{response.Message}'");
                            break;
                    }

                    callback?.Invoke(response);
                    return true;
                }

                return false;
            }

            private bool RetryCommand()
            {
                if (retries < PlayerLifecycleConfig.MaxPlayerCreationRetries)
                {
                    retries += 1;

                    logDispatcher.HandleLog(LogType.Warning, new LogEvent(
                        $"Retrying player creation request, attempt {retries}."
                    ));

                    SendCommand();
                    return false;
                }

                var retryText = retries == 0 ? "1 attempt" : $"{retries + 1} attempts";
                logDispatcher.Error($"Unable to create player after {retryText}.");
                return true;
            }

            private void SendCommand()
            {
                currentRequest = commandSystem.SendCommand(new PlayerCreator.CreatePlayer.Request(
                    PlayerLifecycleConfig.PlayerCreatorEntityId,
                    new CreatePlayerRequest(serializedArgs)
                ));
            }
        }
    }
}
