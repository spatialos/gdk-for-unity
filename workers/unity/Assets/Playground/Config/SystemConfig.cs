using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Playground
{
    public static class SystemConfig
    {
        public const string UnityClient = "UnityClient";
        public const string UnityGameLogic = "UnityGameLogic";

        public static readonly Type[] SpatialSystems =
        {
            typeof(SpatialOSReceiveSystem),
            typeof(SpatialOSSendSystem),
            typeof(CleanReactiveComponentsSystem)
        };

        public static readonly Type[] CommonSystems =
        {
            typeof(EntityManager),
            typeof(GameObjectInitializationSystem),
            typeof(ArchetypeInitializationSystem),
            typeof(DisconnectSystem)
        };

        public static List<Type> GetSystems(string workerType)
        {
            switch (workerType)
            {
                case UnityClient:
                    return GetClientSystems();
                case UnityGameLogic:
                    return GetGameLogicSystems();
                default:
                    throw new Exception("Worker type not known");
            }
        }

        public static List<Type> GetClientSystems()
        {
            var systems = new List<Type>(SpatialSystems);
            systems.AddRange(TransformSynchronizationSystemHelper.ClientSystems);
            systems.AddRange(PlayerLifecycleConfig.ClientSystems);
            systems.AddRange(CommonSystems);
            systems.AddRange(new[]
            {
                typeof(ProcessColorChangeSystem),
                typeof(LocalPlayerInputSync),
                typeof(InitCameraSystem),
                typeof(FollowCameraSystem),
                typeof(InitUISystem),
                typeof(UpdateUISystem),
                typeof(PlayerCommandsSystem),
                typeof(MetricSendSystem)
            });
            return systems;
        }

        public static List<Type> GetGameLogicSystems()
        {
            var systems = new List<Type>(SpatialSystems);
            systems.AddRange(TransformSynchronizationSystemHelper.ServerSystems);
            systems.AddRange(PlayerLifecycleConfig.ServerSystems);
            systems.AddRange(CommonSystems);
            systems.AddRange(new[]
            {
                typeof(CubeMovementSystem),
                typeof(MoveLocalPlayerSystem),
                typeof(TriggerColorChangeSystem),
                typeof(ProcessLaunchCommandSystem),
                typeof(ProcessRechargeSystem),
                typeof(MetricSendSystem),
                // TODO after rebase on master
                //typeof(ProcessScoresSystem),
                //typeof(CollisionProcessSystem)
            });
            return systems;
        }
    }
}
