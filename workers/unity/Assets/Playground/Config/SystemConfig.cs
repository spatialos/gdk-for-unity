using System;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Playground
{
    public static class SystemConfig
    {
        public const string UnityClient = "UnityClient";
        public const string UnityGameLogic = "UnityGameLogic";

        public static void AddSystems(World world, string workerType)
        {
            switch (workerType)
            {
                case UnityClient:
                    AddClientSystems(world);
                    break;
                case UnityGameLogic:
                    AddGameLogicSystems(world);
                    break;
                default:
                    throw new Exception("Worker type not known");
            }
        }

        public static void AddCommonSystems(World world)
        {
            world.GetOrCreateManager<EntityManager>();
            world.GetOrCreateManager<GameObjectInitializationSystem>();
            world.GetOrCreateManager<ArchetypeInitializationSystem>();
            world.GetOrCreateManager<DisconnectSystem>();
        }

        public static void AddClientSystems(World world)
        {
            AddCommonSystems(world);
            CoreSystemHelper.AddSystems(world);
            TransformSynchronizationSystemHelper.AddSystems(world);
            PlayerLifecycleConfig.AddClientSystems(world);
            world.GetOrCreateManager<ProcessColorChangeSystem>();
            world.GetOrCreateManager<LocalPlayerInputSync>();
            world.GetOrCreateManager<InitCameraSystem>();
            world.GetOrCreateManager<FollowCameraSystem>();
            world.GetOrCreateManager<InitUISystem>();
            world.GetOrCreateManager<UpdateUISystem>();
            world.GetOrCreateManager<PlayerCommandsSystem>();
            world.GetOrCreateManager<MetricSendSystem>();
        }

        public static void AddGameLogicSystems(World world)
        {
            AddCommonSystems(world);
            CoreSystemHelper.AddSystems(world);
            TransformSynchronizationSystemHelper.AddSystems(world);
            PlayerLifecycleConfig.AddServerSystems(world);
            world.GetOrCreateManager<CubeMovementSystem>();
            world.GetOrCreateManager<MoveLocalPlayerSystem>();
            world.GetOrCreateManager<TriggerColorChangeSystem>();
            world.GetOrCreateManager<ProcessLaunchCommandSystem>();
            world.GetOrCreateManager<ProcessRechargeSystem>();
            world.GetOrCreateManager<MetricSendSystem>();
            world.GetOrCreateManager<ProcessScoresSystem>();
            world.GetOrCreateManager<CollisionProcessSystem>();
        }
    }
}
