using System;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;
#if !DISABLE_REACTIVE_COMPONENTS
using Improbable.Gdk.ReactiveComponents;
#endif

namespace Improbable.Gdk.Core
{
    public class WorkerInWorld : Worker
    {
        /// <summary>
        ///     The origin of the worker in global Unity space.
        /// </summary>
        public readonly Vector3 Origin;


        /// <summary>
        ///     The ECS world associated with this worker.
        /// </summary>
        public World World { get; internal set; }

        /// <summary>
        ///     An event that triggers when the worker is disconnected.
        /// </summary>
        public event Action<string> OnDisconnect
        {
            add => disconnectCallbackSystem.OnDisconnected += value;
            remove => disconnectCallbackSystem.OnDisconnected -= value;
        }

        // todo should really not be a system and should be on the parent
        private readonly WorkerDisconnectCallbackSystem disconnectCallbackSystem;

        protected WorkerInWorld(IConnectionHandler connectionHandler, string workerType, ILogDispatcher logDispatcher,
            Vector3 origin) : base(connectionHandler, workerType, logDispatcher)
        {
            Origin = origin;

            World = new World(WorkerId);

            // This isn't a core system, this is for an easy disconnect event
            disconnectCallbackSystem = World.GetOrCreateSystem<WorkerDisconnectCallbackSystem>();

            AddCoreSystems();
        }

        public static async Task<WorkerInWorld> CreateWorkerInWorldAsync(IConnectionHandlerBuilder connectionHandlerBuilder, string workerType,
            ILogDispatcher logDispatcher, Vector3 origin, CancellationToken? token = null)
        {
            var handler = await connectionHandlerBuilder.CreateAsync(token);
            return new WorkerInWorld(handler, workerType, logDispatcher, origin);
        }

        private void AddCoreSystems()
        {
            World.CreateSystem<WorkerSystem>(this);
            World.GetOrCreateSystem<CommandSystem>();
            World.GetOrCreateSystem<ComponentUpdateSystem>();
            World.GetOrCreateSystem<EntitySystem>();
            World.GetOrCreateSystem<ComponentSendSystem>();
            World.GetOrCreateSystem<SpatialOSReceiveSystem>();
            World.GetOrCreateSystem<SpatialOSSendSystem>();
            World.GetOrCreateSystem<EcsViewSystem>();
            World.GetOrCreateSystem<CleanTemporaryComponentsSystem>();

            // Subscriptions systems
            World.GetOrCreateSystem<CommandCallbackSystem>();
            World.GetOrCreateSystem<ComponentConstraintsCallbackSystem>();
            World.GetOrCreateSystem<ComponentCallbackSystem>();
            World.GetOrCreateSystem<WorkerFlagCallbackSystem>();
            World.GetOrCreateSystem<RequireLifecycleSystem>();
            World.GetOrCreateSystem<SubscriptionSystem>();

#if !DISABLE_REACTIVE_COMPONENTS
            // Reactive components
            ReactiveComponentsHelper.AddCommonSystems(World);
#endif
        }

        public override void Dispose()
        {
            if (World != null && World.IsCreated)
            {
                World.Dispose();
                World = null;
            }

            base.Dispose();
        }
    }
}
