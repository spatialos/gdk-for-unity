using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;

namespace Playground
{
    public static class PlayerTemplate
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position,
            Option<byte[]> playerCreationArguments)
        {
            if (playerCreationArguments.HasValue)
            {
                try
                {
                    var deserializedArguments = PlayerLifecycleHelper.DeserializeArguments<PlayerCreationParams>(
                        playerCreationArguments.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to deserialize arguments. {e.Message}");
                    throw;
                }
            }

            var clientAttribute = $"workerId:{workerId}";

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), clientAttribute);
            template.AddComponent(new Metadata.Snapshot { EntityType = "Character" }, WorkerUtils.UnityGameLogic);
            template.AddComponent(new PlayerInput.Snapshot(), clientAttribute);
            template.AddComponent(new Launcher.Snapshot { EnergyLeft = 100, RechargeTimeLeft = 0 },
                WorkerUtils.UnityGameLogic);
            template.AddComponent(new Score.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeSpawner.Snapshot { SpawnedCubes = new List<EntityId>() },
                WorkerUtils.UnityGameLogic);
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute);
            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, clientAttribute,
                WorkerUtils.UnityGameLogic);

            template.SetReadAccess(WorkerUtils.UnityClient, WorkerUtils.UnityGameLogic, WorkerUtils.AndroidClient,
                WorkerUtils.iOSClient);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

            return template;
        }
    }
}
