using System;
using System.Collections.Generic;
using Improbable.Gdk.ReactiveComponents;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class ReactiveComponentSystem : ComponentSystem
    {
        private readonly List<IReactiveComponentManager> managers = new List<IReactiveComponentManager>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(IReactiveComponentManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (IReactiveComponentManager) Activator.CreateInstance(type);
                    managers.Add(instance);
                }
            }
        }

        [Inject] private EntityManager entityManager;
        [Inject] private ComponentUpdateSystem updateSystem;
        [Inject] private WorkerSystem workerSystem;

        protected override void OnDestroyManager()
        {
            foreach (var manager in managers)
            {
                manager.Clean(World);
            }

            base.OnDestroyManager();
        }

        protected override void OnUpdate()
        {
            foreach (var manager in managers)
            {
                manager.PopulateReactiveComponents(updateSystem, entityManager, workerSystem, World);
            }
        }
    }
}
