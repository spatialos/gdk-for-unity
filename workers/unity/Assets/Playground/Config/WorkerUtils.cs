using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using System.Collections.Generic;
using Unity.Entities;

namespace Playground
{
    public static class WorkerUtils
    {
        public const string UnityClient = "UnityClient";
        public const string UnityGameLogic = "UnityGameLogic";
        public const string AndroidClient = "AndroidClient";
        public const string iOSClient = "iOSClient";

        public static readonly List<string> AllWorkerAttributes =
            new List<string>
            {
                UnityGameLogic,
                UnityClient,
                AndroidClient,
                iOSClient
            };

        public static void AddMobileClientSystems(World world)
        {
            AddCommonClientSystems(world);
        }

        public static void AddPCClientSystems(World world)
        {
            AddCommonClientSystems(world);
            world.GetOrCreateManager<PlayerCommandsSystem>();
        }

        public static void AddCommonClientSystems(World world)
        {
            AddLifecycleSystems(world);
            TransformSynchronizationHelper.AddClientSystems(world);
            PlayerLifecycleHelper.AddClientSystems(world);
            GameObjectRepresentationHelper.AddSystems(world);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(world);
            world.GetOrCreateManager<ProcessColorChangeSystem>();
            world.GetOrCreateManager<LocalPlayerInputSync>();
            world.GetOrCreateManager<InitCameraSystem>();
            world.GetOrCreateManager<FollowCameraSystem>();
            world.GetOrCreateManager<InitUISystem>();
            world.GetOrCreateManager<UpdateUISystem>();
            world.GetOrCreateManager<MetricSendSystem>();
        }

        public static void AddGameLogicSystems(World world)
        {
            AddLifecycleSystems(world);
            TransformSynchronizationHelper.AddServerSystems(world);
            PlayerLifecycleHelper.AddServerSystems(world);
            GameObjectRepresentationHelper.AddSystems(world);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(world);
            world.GetOrCreateManager<CubeMovementSystem>();
            world.GetOrCreateManager<TriggerColorChangeSystem>();
            world.GetOrCreateManager<ProcessLaunchCommandSystem>();
            world.GetOrCreateManager<ProcessRechargeSystem>();
            world.GetOrCreateManager<MetricSendSystem>();
            world.GetOrCreateManager<ProcessScoresSystem>();
            world.GetOrCreateManager<CollisionProcessSystem>();
            world.GetOrCreateManager<MoveLocalPlayerSystem>();
        }

        private static void AddLifecycleSystems(World world)
        {
            world.GetOrCreateManager<DisconnectSystem>();
        }
    }
}
