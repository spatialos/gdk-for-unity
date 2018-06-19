using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;

namespace Playground
{
    public class UnityClient : WorkerBase
    {
        public const string WorkerType = "UnityClient";

        public override string GetWorkerType => WorkerType;

        public UnityClient(string workerId, Vector3 origin) : base(workerId, origin)
        {
        }

        public override void RegisterSystems()
        {
            base.RegisterSystems();
            TransfromSynchronizationSystemHelper.RegisterClientSystems(World);
            PlayerLifecycleConfig.RegisterClientSystems(World);

            // Entity initialization systems
            World.GetOrCreateManager<GameObjectInitializationSystem>();
            World.GetOrCreateManager<ArchetypeInitializationSystem>();

            // SpatailOS systems
            World.GetOrCreateManager<DisconnectSystem>();

            // Client test event systems
            World.GetOrCreateManager<ProcessColorChangeSystem>();

            // Client player movement systems
            World.GetOrCreateManager<LocalPlayerInputSync>();
            World.GetOrCreateManager<InitCameraSystem>();
            World.GetOrCreateManager<FollowCameraSystem>();

            // Client player UI systems
            World.GetOrCreateManager<InitUISystem>();
            World.GetOrCreateManager<UpdateUISystem>();

            // Client player commands system
            World.GetOrCreateManager<PlayerCommandsSystem>();
        }
    }
}
