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

        public static void SetWorkerForWorld(WorkerBase worker)
        {
            if (WorldToWorker.ContainsKey(worker.World))
            {
                throw new ArgumentException($"A worker is already stored for world '{worker.World.Name}'");
            }

            WorldToWorker[worker.World] = worker;
        }

        public static void UnsetWorkerForWorld(WorkerBase worker)
        {
            WorkerBase workerForWorld;

            if (WorldToWorker.TryGetValue(worker.World, out workerForWorld) && workerForWorld == worker)
            {
                WorldToWorker.Remove(worker.World);
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

            WorkerTypeToInitializationFunction.Add(workerType, CreateWorker<T>);
        }

        public static T CreateWorker<T>(string workerId, Vector3 origin) where T : WorkerBase
        {
            var worker = (T) Activator.CreateInstance(typeof(T), workerId, origin);
            worker.RegisterSystems();
            return worker;
        }

        public static WorkerBase CreateWorker(string workerType, string workerId, Vector3 origin)
        {
            Func<string, Vector3, WorkerBase> createWorker;
            if (!WorkerTypeToInitializationFunction.TryGetValue(workerType, out createWorker))
            {
                throw new ArgumentException("No worker found for worker type '{0}'", workerType);
            }

            return createWorker(workerId, origin);
        }
    }
}
