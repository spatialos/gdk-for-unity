using System;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.Core.NewWorld
{
    public class SpatialOSWorld : World
    {
        internal SpatialOSWorker SpatialWorker { get; }

        public SpatialOSWorld(SpatialOSWorker worker) : base(worker.WorkerId)
        {
            SpatialWorker = worker;
            worker.WorkerEntity = EntityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            AddCoreSystems();
        }

        private void AddCoreSystems()
        {
            GetOrCreateSystem<UpdateWorldTimeSystem>();
            GetOrCreateSystem<WorkerSystem>();
            GetOrCreateSystem<CommandSystem>();
            GetOrCreateSystem<ComponentUpdateSystem>();
            GetOrCreateSystem<EntitySystem>();
            GetOrCreateSystem<ComponentSendSystem>();
            GetOrCreateSystem<SpatialOSReceiveSystem>();
            GetOrCreateSystem<SpatialOSSendSystem>();
            GetOrCreateSystem<EcsViewSystem>();
            GetOrCreateSystem<CleanTemporaryComponentsSystem>();

            // Subscriptions systems
            GetOrCreateSystem<CommandCallbackSystem>();
            GetOrCreateSystem<ComponentConstraintsCallbackSystem>();
            GetOrCreateSystem<ComponentCallbackSystem>();
            GetOrCreateSystem<WorkerFlagCallbackSystem>();
            GetOrCreateSystem<RequireLifecycleSystem>();
            GetOrCreateSystem<SubscriptionSystem>();
        }
    }

    public static class WorldExtensions
    {
        public static SpatialOSWorker GetWorker(this World world)
        {
            if (world is SpatialOSWorld spatialWorld)
            {
                return spatialWorld.SpatialWorker;
            }

            throw new InvalidCastException("This world does not seem to be a SpatialOSWorld");
        }
    }
}
