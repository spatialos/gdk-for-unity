using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Playground
{
    public static class WorkerUtils
    {
        public const string UnityClient = "UnityClient";
        public const string UnityGameLogic = "UnityGameLogic";
        public const string MobileClient = "MobileClient";

        public static void AddClientSystems(World world, EntityRepresentationMapping entityRepresentationMapping)
        {
            AddLifecycleSystems(world);
            TransformSynchronizationHelper.AddClientSystems(world);
            PlayerLifecycleHelper.AddClientSystems(world);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(world, entityRepresentationMapping);
            world.GetOrCreateSystem<ProcessColorChangeSystem>();
            world.GetOrCreateSystem<LocalPlayerInputSync>();
            world.GetOrCreateSystem<MoveLocalPlayerSystem>();
            world.GetOrCreateSystem<InitCameraSystem>();
            world.GetOrCreateSystem<FollowCameraSystem>();
            world.GetOrCreateSystem<UpdateUISystem>();
            world.GetOrCreateSystem<PlayerCommandsSystem>();
            world.GetOrCreateSystem<MetricSendSystem>();
        }

        public static void AddGameLogicSystems(World world, EntityRepresentationMapping entityRepresentationMapping)
        {
            AddLifecycleSystems(world);
            TransformSynchronizationHelper.AddServerSystems(world);
            PlayerLifecycleHelper.AddServerSystems(world);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(world, entityRepresentationMapping);

            world.GetOrCreateSystem<TriggerColorChangeSystem>();
            world.GetOrCreateSystem<ProcessLaunchCommandSystem>();
            world.GetOrCreateSystem<ProcessRechargeSystem>();
            world.GetOrCreateSystem<MetricSendSystem>();
            world.GetOrCreateSystem<ProcessScoresSystem>();
            world.GetOrCreateSystem<CollisionProcessSystem>();
            world.GetOrCreateSystem<CubeMovementSystem>();
        }

        private static void AddLifecycleSystems(World world)
        {
            world.GetOrCreateSystem<DisconnectSystem>();
        }
    }
}
