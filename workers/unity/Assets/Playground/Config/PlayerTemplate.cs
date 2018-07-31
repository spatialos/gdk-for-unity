using System;
using System.Collections.Generic;
using System.Linq;
using Generated.Improbable.PlayerLifecycle;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;

namespace Playground
{
    public static class PlayerTemplate
    {
        private static readonly string GameLogicAttribute = UnityGameLogic.WorkerType;

        private static readonly List<string> AllWorkerAttributes =
            new List<string> { UnityGameLogic.WorkerType, UnityClient.WorkerType };

        public static Entity CreatePlayerEntityTemplate(List<string> clientAttributeSet,
            Generated.Improbable.Vector3f position)
        {
            var clientAttribute = clientAttributeSet.First(attribute => attribute != UnityClient.WorkerType);

            if (clientAttribute == null)
            {
                throw new InvalidOperationException("Expected an attribute that is not \"UnityClient\" but none was found.");
            }

            var transform =
                SpatialOSTransform.CreateSchemaComponentData(rotation: new Quaternion { W = 1, X = 0, Y = 0, Z = 0 });
            var playerInput = SpatialOSPlayerInput.CreateSchemaComponentData();
            var prefab = SpatialOSPrefab.CreateSchemaComponentData(ArchetypeConfig.CharacterArchetype);
            var archetype = SpatialOSArchetypeComponent.CreateSchemaComponentData(ArchetypeConfig.CharacterArchetype);
            var launcher = SpatialOSLauncher.CreateSchemaComponentData(energyLeft: 100);
            var clientHeartbeat = SpatialOSPlayerHeartbeatClient.CreateSchemaComponentData();
            var serverHeartbeat = SpatialOSPlayerHeartbeatServer.CreateSchemaComponentData();

            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, GameLogicAttribute)
                .SetPersistence(false)
                .SetReadAcl(AllWorkerAttributes)
                .SetEntityAclComponentWriteAccess(GameLogicAttribute)
                .AddComponent(transform, GameLogicAttribute)
                .AddComponent(playerInput, clientAttribute)
                .AddComponent(prefab, GameLogicAttribute)
                .AddComponent(archetype, GameLogicAttribute)
                .AddComponent(launcher, GameLogicAttribute)
                .AddComponent(clientHeartbeat, clientAttribute)
                .AddComponent(serverHeartbeat, GameLogicAttribute);

            return entityBuilder.Build();

        }
    }
}
