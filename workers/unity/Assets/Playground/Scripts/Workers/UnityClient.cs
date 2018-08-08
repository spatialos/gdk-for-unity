using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class UnityClient : WorkerBase
    {
        public UnityClient(ConnectionConfig config, EntityManager entityManager, ILogDispatcher logDispatcher, Vector3 origin) : base(config, entityManager, logDispatcher, origin)
        {
            RequiredSpatialSystems.AddRange(TransformSynchronizationSystemHelper.ClientSystems);
            RequiredSpatialSystems.AddRange(PlayerLifecycleConfig.ClientSystems);
            RequiredSpatialSystems.AddRange(new[]
            {
                typeof(GameObjectInitializationSystem),
                typeof(ArchetypeInitializationSystem),
                typeof(DisconnectSystem),
                typeof(ProcessColorChangeSystem),
                typeof(LocalPlayerInputSync),
                typeof(InitCameraSystem),
                typeof(FollowCameraSystem),
                typeof(InitUISystem),
                typeof(UpdateUISystem),
                typeof(PlayerCommandsSystem),
                typeof(MetricSendSystem)
            });
        }
    }
}
