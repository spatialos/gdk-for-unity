using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;

namespace Playground
{
    public class UnityGameLogic : WorkerBase
    {
        public const string WorkerType = "UnityGameLogic";

        public override string GetWorkerType => WorkerType;

        private readonly ForwardingDispatcher forwardingDispatcher;

        public UnityGameLogic(string workerId, Vector3 origin) : base(workerId, origin, new ForwardingDispatcher())
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;
            forwardingDispatcher = (ForwardingDispatcher) View.LogDispatcher;
        }

        public override void Connect(ConnectionConfig config)
        {
            base.Connect(config);
            forwardingDispatcher.SetConnection(Connection);
        }

        public override void RegisterSystems()
        {
            base.RegisterSystems();
            TransfromSynchronizationSystemHelper.RegisterServerSystems(World);
            PlayerLifecycleConfig.RegisterServerSystems(World);

            // Entity initialization systems
            World.GetOrCreateManager<GameObjectInitializationSystem>();
            World.GetOrCreateManager<ArchetypeInitializationSystem>();

            // SpatailOS systems
            World.GetOrCreateManager<DisconnectSystem>();

            // Server cube movement systems
            World.GetOrCreateManager<CubeMovementSystem>();

            // Server player movement systems
            World.GetOrCreateManager<MoveLocalPlayerSystem>();

            // Server test event systems
            World.GetOrCreateManager<TriggerColorChangeSystem>();

            // Server test command systems
            World.GetOrCreateManager<ProcessLaunchCommandSystem>();
            World.GetOrCreateManager<ProcessRechargeSystem>();

            // Metric sending system
            World.GetOrCreateManager<MetricSendSystem>();

            // Score update systems
            World.GetOrCreateManager<ProcessScoresSystem>();
            World.GetOrCreateManager<CollisionProcessSystem>();
        }
    }
}
