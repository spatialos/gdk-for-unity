using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Legacy;
using Improbable.Worker;

namespace Playground
{
    public static class PlayerTemplate
    {
        private static readonly WorkerRequirementSet GameLogicSet =
            WorkerRegistry.GetWorkerRequirementSet(typeof(UnityGameLogic));

        private static readonly WorkerRequirementSet AllWorkersSet =
            WorkerRegistry.GetWorkerRequirementSet(typeof(UnityClient), typeof(UnityGameLogic));

        public static Entity CreatePlayerEntityTemplate(List<string> clientAttributeSet,
            Generated.Improbable.Vector3f position)
        {
            var clientSet = new WorkerRequirementSet(new Improbable.Collections.List<WorkerAttributeSet>()
            {
                new WorkerAttributeSet(clientAttributeSet as Improbable.Collections.List<string>)
            });

            var location = new Improbable.Transform.Location(position.X, position.Y, position.Z);
            var rotation = new Improbable.Transform.Quaternion(1, 0, 0, 0);
            var coordinates = new Coordinates(0, 0, 0);
            var transformData = new Improbable.Transform.Transform.Data(location, rotation, 0);
            var metadata = new Metadata.Data(ArchetypeConfig.CharacterArchetype);
            var playerInput = new Playground.PlayerInput.Data(0, 0, false);
            var playerHeartbeatClient = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.Data();
            var playerHeartbeatServer = new Improbable.PlayerLifecycle.PlayerHeartbeatServer.Data();
            var prefab = new Playground.Prefab.Data(ArchetypeConfig.CharacterArchetype);
            var archetype = new Playground.ArchetypeComponent.Data(ArchetypeConfig.CharacterArchetype);
            var launcher = new Playground.Launcher.Data(100, 0);
            var score = new Playground.Score.Data(0);

            return EntityBuilder.Begin()
                .AddPositionComponent(coordinates, GameLogicSet)
                .AddComponent(metadata, GameLogicSet)
                .SetPersistence(false)
                .SetReadAcl(AllWorkersSet)
                .AddComponent(transformData, GameLogicSet)
                .AddComponent(playerInput, clientSet)
                .AddComponent(playerHeartbeatClient, clientSet)
                .AddComponent(playerHeartbeatServer, GameLogicSet)
                .AddComponent(prefab, GameLogicSet)
                .AddComponent(archetype, GameLogicSet)
                .AddComponent(launcher, GameLogicSet)
                .AddComponent(score, GameLogicSet)
                .Build();
        }
    }
}
