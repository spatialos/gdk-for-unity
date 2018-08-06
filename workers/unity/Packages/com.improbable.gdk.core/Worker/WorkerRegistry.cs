using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public static class WorkerRegistry
    {
        private static readonly Dictionary<World, WorkerBase> WorldToWorker = new Dictionary<World, WorkerBase>();

        private static readonly Dictionary<string, Func<string, Vector3, WorkerBase>> WorkerTypeToInitializationFunction
            = new Dictionary<string, Func<string, Vector3, WorkerBase>>();

        private static readonly Dictionary<Type, WorkerAttributeSet> WorkerTypeToAttributeSet =
            new Dictionary<Type, WorkerAttributeSet>();

        public static void SetWorkerForWorld(WorkerBase worker, World world)
        {
            if (WorldToWorker.ContainsKey(world))
            {
                throw new ArgumentException($"A worker is already stored for world '{world.Name}'");
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
            WorkerBase worker;
            WorldToWorker.TryGetValue(world, out worker);
            return worker;
        }

        public static void RegisterWorkerType<T>() where T : WorkerBase
        {
            string workerType = (string) typeof(T).GetField("WorkerType").GetValue(null);
            WorkerTypeToAttributeSet.Add(
                typeof(T),
                new WorkerAttributeSet(new Improbable.Collections.List<string> { workerType })
            );

            WorkerTypeToInitializationFunction.Add(workerType, CreateWorker<T>);
        }

        public static T CreateWorker<T>(string workerId, Vector3 origin) where T : WorkerBase
        {
            var worker = (T) Activator.CreateInstance(typeof(T), workerId, origin);
            return worker;
        }

        public static WorkerBase CreateWorker(ConnectionConfig config, Vector3 origin)
        {
            Func<string, Vector3, WorkerBase> createWorker;
            if (!WorkerTypeToInitializationFunction.TryGetValue(config.WorkerType, out createWorker))
            {
                throw new ArgumentException($"No worker found for worker type '{config.WorkerType}'", nameof(config.WorkerType));
            }

            return createWorker(config.WorkerId, origin);
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
