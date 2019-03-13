using System;
using System.Threading.Tasks;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

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
            disconnectCallbackSystem = World.GetOrCreateManager<WorkerDisconnectCallbackSystem>();

            AddCoreSystems();
        }

        public static async Task<WorkerInWorld> CreateWorkerInWorldAsync(IConnectionHandlerBuilder connectionHandlerBuilder, string workerType,
            ILogDispatcher logDispatcher, Vector3 origin)
        {
            var handler = await connectionHandlerBuilder.CreateAsync();
            return new WorkerInWorld(handler, workerType, logDispatcher, origin);
        }

        private void AddCoreSystems()
        {
            World.CreateManager<WorkerSystem>(this, WorkerId, LogDispatcher, WorkerType, Origin);
            World.GetOrCreateManager<CommandSystem>();
            World.GetOrCreateManager<ComponentUpdateSystem>();
            World.GetOrCreateManager<EntitySystem>();
            World.GetOrCreateManager<ComponentSendSystem>();
            World.GetOrCreateManager<SpatialOSReceiveSystem>();
            World.GetOrCreateManager<SpatialOSSendSystem>();
            World.GetOrCreateManager<EcsViewSystem>();
            World.GetOrCreateManager<CleanTemporaryComponentsSystem>();

            // Subscriptions systems
            World.GetOrCreateManager<CommandCallbackSystem>();
            World.GetOrCreateManager<ComponentConstraintsCallbackSystem>();
            World.GetOrCreateManager<ComponentCallbackSystem>();
            World.GetOrCreateManager<RequireLifecycleSystem>();
            World.GetOrCreateManager<SubscriptionSystem>();

            // Reactive components
            ReactiveComponentsHelper.AddCommonSystems(World);
        }

        public override void Dispose()
        {
            World?.Dispose();
            World = null;
            base.Dispose();
        }
    }
}
