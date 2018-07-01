using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Timing
{
    /// <summary>
    ///     Responsible for adding and removing timers
    /// </summary>
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    [UpdateBefore(typeof(AddComponentAtLocalTimeSystem))]
    public class LocalTimerLifecycleSystem : ComponentSystem
    {
        // Can add an extra layer in if we want to seperate out workers - likely a good idea for debugging
        internal static readonly Dictionary<uint, PriorityQueue<ComponentToAddAtLocalTime>> HandleToTimerQueue
            = new Dictionary<uint, PriorityQueue<ComponentToAddAtLocalTime>>();

        private static readonly Dictionary<LocalTimerLifecycleSystem, List<Action<EntityCommandBuffer>>> systemToDelayedAddTimerActions
            = new Dictionary<LocalTimerLifecycleSystem, List<Action<EntityCommandBuffer>>>();

        private static uint nextAvailableHandle = 1u;

        public struct RemovedEntityData
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<PersistantLocalTimerHandle> LocalTimerHandles;
            [ReadOnly] public SubtractiveComponent<HasHadLocalTimerTag> DenotesEntityRemoved;
            [ReadOnly] public EntityArray RemovedEntities;
        }

        public struct EmptyTimerData
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<ActiveTimer> ActiveTimers;
            [ReadOnly] public ComponentDataArray<CheckForEmptyTimer> DenotesShouldRemoveTimerHandle;
            [ReadOnly] public EntityArray Entites;
        }

        [Inject] private RemovedEntityData removedEntityData;
        [Inject] private EmptyTimerData emptyTimerData;

        protected override void OnUpdate()
        {
            List<Action<EntityCommandBuffer>> addTimerActions;
            if (systemToDelayedAddTimerActions.TryGetValue(this, out addTimerActions))
            {
                foreach (var addTimerAction in addTimerActions)
                {
                    addTimerAction(PostUpdateCommands);
                }

                addTimerActions.Clear();
            }

            for (int i = 0; i < removedEntityData.Length; ++i)
            {
                ReleaseHandle(removedEntityData.LocalTimerHandles[i].Handle);
                PostUpdateCommands.RemoveSystemStateComponent<PersistantLocalTimerHandle>(removedEntityData.RemovedEntities[i]);
            }

            for (int i = 0; i < emptyTimerData.Length; ++i)
            {
                if (emptyTimerData.ActiveTimers[i].Queue.Count() == 0)
                {
                    PostUpdateCommands.RemoveComponent<ActiveTimer>(emptyTimerData.Entites[i]);
                }
                PostUpdateCommands.RemoveComponent<CheckForEmptyTimer>(emptyTimerData.Entites[i]);
            }
        }

        // todo Could get rid of the command buffer argument by creating an add timer to entity system
        // could also just add the component via the entity manager
        internal void AddComponentAtLocalTime<T>(T component, float time, Entity entity)
            where T : struct, IComponentData
        {
            var manager = World.GetExistingManager<EntityManager>();

            uint timerHandle;
            if (manager.HasComponent<PersistantLocalTimerHandle>(entity))
            {
                timerHandle = manager.GetComponentData<PersistantLocalTimerHandle>(entity).Handle;
                if (!manager.HasComponent<ActiveTimer>(entity))
                {
                    var delayedAddTimerActions = GetelayedAddTimerAction();

                    delayedAddTimerActions.Add(commandBuffer =>
                    {
                        if (!manager.Exists(entity))
                        {
                            ReleaseHandle(timerHandle);
                            return;
                        }

                        if (!manager.HasComponent<ActiveTimer>(entity))
                        {
                            commandBuffer.AddComponent(entity, new ActiveTimer(timerHandle));
                        }
                    });
                }
            }
            else
            {
                timerHandle = GetAvailableHandle();
                var delayedAddTimerActions = GetelayedAddTimerAction();

                delayedAddTimerActions.Add(commandBuffer =>
                {
                    if (!manager.Exists(entity))
                    {
                        ReleaseHandle(timerHandle);
                        return;
                    }

                    if (!manager.HasComponent<ActiveTimer>(entity))
                    {
                        commandBuffer.AddComponent(entity, new ActiveTimer(timerHandle));
                        commandBuffer.AddComponent(entity, new PersistantLocalTimerHandle { Handle = timerHandle });
                        commandBuffer.AddComponent(entity, new HasHadLocalTimerTag());
                    }
                });
            }

            GetOrCreateComponentTimer(timerHandle).Push(ComponentToAddAtLocalTime.Create(component, entity, time));
        }

        internal static PriorityQueue<ComponentToAddAtLocalTime> GetOrCreateComponentTimer(uint handle)
        {
            PriorityQueue<ComponentToAddAtLocalTime> queue;
            if (!HandleToTimerQueue.TryGetValue(handle, out queue))
            {
                queue = new PriorityQueue<ComponentToAddAtLocalTime>();
                HandleToTimerQueue.Add(handle, queue);
            }

            return queue;
        }

        private uint GetAvailableHandle()
        {
            return nextAvailableHandle++;
        }

        private void ReleaseHandle(uint handle)
        {
            HandleToTimerQueue.Remove(handle);
        }

        private List<Action<EntityCommandBuffer>> GetelayedAddTimerAction()
        {
            List<Action<EntityCommandBuffer>> delayedAddTimerActions;
            if (!systemToDelayedAddTimerActions.TryGetValue(this, out delayedAddTimerActions))
            {
                delayedAddTimerActions = new List<Action<EntityCommandBuffer>>();
                systemToDelayedAddTimerActions.Add(this, delayedAddTimerActions);
            }

            return delayedAddTimerActions;
        }
    }
}
