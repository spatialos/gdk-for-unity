using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;

namespace Playground
{
    public static class PlayerTemplate
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string clientWorkerId, byte[] playerCreationArguments)
        {
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(clientWorkerId);

            var template = new EntityTemplate();

            template.AddComponent(new Position.Snapshot(), clientAttribute);
            template.AddComponent(new Metadata.Snapshot("Character"), WorkerUtils.UnityGameLogic);
            template.AddComponent(new PlayerInput.Snapshot(), clientAttribute);
            template.AddComponent(new Launcher.Snapshot(100, 0), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Score.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeSpawner.Snapshot(new List<EntityId>()), WorkerUtils.UnityGameLogic);

            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute);
            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, clientWorkerId, WorkerUtils.UnityGameLogic);

            template.SetReadAccess(WorkerUtils.UnityClient, WorkerUtils.UnityGameLogic, WorkerUtils.MobileClient);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

            return template;
        }
    }
}
