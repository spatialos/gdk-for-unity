using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class UnityGameLogic : WorkerBase
    {
        public UnityGameLogic(ConnectionConfig config, EntityManager entityManager, ILogDispatcher logDispatcher, Vector3 origin) : base(config, entityManager, logDispatcher, origin)
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;

            RequiredSpatialSystems.AddRange(TransformSynchronizationSystemHelper.ServerSystems);
            RequiredSpatialSystems.AddRange(PlayerLifecycleConfig.ServerSystems);
            RequiredSpatialSystems.AddRange(new []
            {
                typeof(GameObjectInitializationSystem),
                typeof(ArchetypeInitializationSystem),
                typeof(DisconnectSystem),
                typeof(CubeMovementSystem),
                typeof(MoveLocalPlayerSystem),
                typeof(TriggerColorChangeSystem),
                typeof(ProcessLaunchCommandSystem),
                typeof(ProcessRechargeSystem),
                typeof(MetricSendSystem),
                typeof(ProcessScoresSystem),
                typeof(CollisionProcessSystem)
            });
        }
    }
}
