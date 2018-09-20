using System.Collections.Generic;
using Improbable;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Transform;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

// ReSharper disable PossibleInvalidOperationException

#endregion

namespace Playground.MonoBehaviours
{
    public class SpawnCubeCommandReceiver : MonoBehaviour
    {
        [Require] private TransformInternal.Requirable.Reader transformReader;
        [Require] private CubeSpawner.Requirable.CommandRequestHandler cubeSpawnerCommandRequestHandler;
        [Require] private CubeSpawner.Requirable.Writer cubeSpawnerWriter;
        [Require] private WorldCommands.Requirable.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.Requirable.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;

        public void OnEnable()
        {
            logDispatcher = GetComponent<SpatialOSComponent>().Worker.LogDispatcher;
            cubeSpawnerCommandRequestHandler.OnSpawnCubeRequest += OnSpawnCubeRequest;
            worldCommandResponseHandler.OnReserveEntityIdsResponse += OnEntityIdsReserved;
            worldCommandResponseHandler.OnCreateEntityResponse += OnEntityCreated;
        }

        private void OnSpawnCubeRequest(CubeSpawner.SpawnCube.RequestResponder requestResponder)
        {
            requestResponder.SendResponse(new Empty());

            worldCommandRequestSender.ReserveEntityIds(1, context: this);
        }

        private void OnEntityIdsReserved(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            if (!ReferenceEquals(this, response.Context))
            {
                // This response was not for a command from this behaviour.
                return;
            }

            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent("ReserveEntityIds failed.")
                        .WithField("Reason", response.Message));

                return;
            }

            var location = transformReader.Data.Location;
            var cubeEntityTemplate =
                CubeTemplate.CreateCubeEntityTemplate(new Coordinates(location.X, location.Y + 2, location.Z));
            var expectedEntityId = response.FirstEntityId.Value;

            worldCommandRequestSender.CreateEntity(cubeEntityTemplate, expectedEntityId, context: this);
        }

        private void OnEntityCreated(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            if (!ReferenceEquals(this, response.Context))
            {
                // This response was not for a command from this behaviour.
                return;
            }

            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent("CreateEntity failed.")
                        .WithField(LoggingUtils.EntityId, response.RequestPayload.EntityId)
                        .WithField("Reason", response.Message));

                return;
            }

            var spawnedCubesCopy =
                new List<EntityId>(cubeSpawnerWriter.Data.SpawnedCubes);
            var newEntityId = response.EntityId.Value;

            spawnedCubesCopy.Add(newEntityId);

            cubeSpawnerWriter.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubesCopy
            });
        }
    }
}
