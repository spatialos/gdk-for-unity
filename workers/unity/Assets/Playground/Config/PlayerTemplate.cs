using System;
using System.Collections.Generic;
using System.Linq;
using Generated.Improbable.PlayerLifecycle;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class PlayerTemplate
    {
        public static Entity CreatePlayerEntityTemplate(List<string> clientAttributeSet,
            Generated.Improbable.Vector3f position)
        {
            var clientAttribute = clientAttributeSet.First(attribute => attribute != WorkerUtils.UnityClient);

            if (clientAttribute == null)
            {
                throw new InvalidOperationException(
                    "Expected an attribute that is not \"UnityClient\" but none was found.");
            }

            const string CharacterType = "Character";

            var transform =
                TransformInternal.Component.CreateSchemaComponentData(new Location(),
                    new Quaternion { W = 1, X = 0, Y = 0, Z = 0 }, new Velocity(0.0f, 0.0f, 0.0f), 0, 0.0f);
            var playerInput = PlayerInput.Component.CreateSchemaComponentData(0, 0, false);
            var launcher = Launcher.Component.CreateSchemaComponentData(100, 0);
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            var score = Score.Component.CreateSchemaComponentData(0);
            var cubeSpawner = CubeSpawner.Component.CreateSchemaComponentData(new List<EntityId>(), 0);

            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, WorkerUtils.UnityGameLogic)
                .AddMetadata(CharacterType, WorkerUtils.UnityGameLogic)
                .SetPersistence(false)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .SetEntityAclComponentWriteAccess(WorkerUtils.UnityGameLogic)
                .AddComponent(transform, clientAttribute)
                .AddComponent(playerInput, clientAttribute)
                .AddComponent(launcher, WorkerUtils.UnityGameLogic)
                .AddComponent(clientHeartbeat, clientAttribute)
                .AddComponent(serverHeartbeat, WorkerUtils.UnityGameLogic)
                .AddComponent(score, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeSpawner, WorkerUtils.UnityGameLogic);

            return entityBuilder.Build();
        }
    }
}
