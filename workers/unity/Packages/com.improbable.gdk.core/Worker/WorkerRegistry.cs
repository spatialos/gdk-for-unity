using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public static class WorkerRegistry
    {
        private static readonly Dictionary<World, WorkerBase> WorldToWorker = new Dictionary<World, WorkerBase>();

        private static readonly Dictionary<string, Type> WorkerTypes = new Dictionary<string, Type>();

        private static readonly Dictionary<Type, WorkerAttributeSet> WorkerTypeToAttributeSet =
            new Dictionary<Type, WorkerAttributeSet>();

        public static void SetWorkerForWorld(WorkerBase worker, World world)
        {
            if (WorldToWorker.ContainsKey(world))
            {
                throw new ArgumentException($"A Worker is already stored for world '{world.Name}'");
            }

            WorldToWorker[world] = worker;
        }

        public static void UnsetWorkerForWorld(WorkerBase worker, World world)
        {
            WorkerBase workerForWorld;

            if (WorldToWorker.TryGetValue(world, out workerForWorld) && workerForWorld == worker)
            {
                WorldToWorker.Remove(world);
            }
        }

        public static WorkerBase GetWorkerForWorld(World world)
        {
            WorldToWorker.TryGetValue(world, out var worker);
            return worker;
        }

        public static void RegisterWorkerType<T>() where T : WorkerBase
        {
            WorkerTypes.Add(typeof(T).Name, typeof(T));
            WorkerTypeToAttributeSet.Add(
                typeof(T),
                new WorkerAttributeSet(new Improbable.Collections.List<string> { typeof(T).Name })
            );

        }

        public static T CreateWorker<T>(ConnectionConfig config, EntityManager entityManager, ILogDispatcher logDispatcher, Vector3 origin) where T : WorkerBase
        {
            return (T) Activator.CreateInstance(typeof(T), config, entityManager, logDispatcher, origin);
        }

        public static WorkerBase CreateWorker(ConnectionConfig config, EntityManager entityManager, ILogDispatcher logDispatcher, Vector3 origin)
        {
            if (!WorkerTypes.ContainsKey(config.WorkerType))
            {
                throw new ArgumentException($"No worker found for worker type '{config.WorkerType}'", nameof(config.WorkerType));
            }

            return (WorkerBase)Activator.CreateInstance(WorkerTypes[config.WorkerType], config, entityManager, logDispatcher, origin);
        }

        public static WorkerRequirementSet GetWorkerRequirementSet(Type workerType, params Type[] workerTypes)
        {
            var workerAttributes = new Improbable.Collections.List<WorkerAttributeSet>();
            workerAttributes.Add(WorkerTypeToAttributeSet[workerType]);
            foreach (var nextType in workerTypes)
            {
                workerAttributes.Add(WorkerTypeToAttributeSet[nextType]);
            }

            return new WorkerRequirementSet(workerAttributes);
        }
    }
}
