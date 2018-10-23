using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class PlayerTemplate
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position)
        {
            var clientAttribute = $"workerId:{workerId}";

            var playerInput = PlayerInput.Component.CreateSchemaComponentData(0, 0, false);
            var launcher = Launcher.Component.CreateSchemaComponentData(100, 0);

            var score = Score.Component.CreateSchemaComponentData(0);
            var cubeSpawner = CubeSpawner.Component.CreateSchemaComponentData(new List<EntityId>());

            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, clientAttribute)
                .AddMetadata("Character", WorkerUtils.UnityGameLogic)
                .SetPersistence(false)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .SetEntityAclComponentWriteAccess(WorkerUtils.UnityGameLogic)
                .AddComponent(playerInput, clientAttribute)
                .AddComponent(launcher, WorkerUtils.UnityGameLogic)
                .AddComponent(score, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeSpawner, WorkerUtils.UnityGameLogic)
                .AddTransformSynchronizationComponents(clientAttribute)
                .AddPlayerLifecycleComponents(workerId, clientAttribute, WorkerUtils.UnityGameLogic);

            return entityBuilder.Build();
        }
    }
}
