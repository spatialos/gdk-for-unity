using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class MetricSendSystem : ComponentSystem
    {
        private WorkerSystem worker;

        private DateTime timeOfNextUpdate;
        private DateTime timeOfLastUpdate;

        private const double TimeBetweenMetricUpdatesSecs = 2;
        private const int DefaultTargetFrameRate = 60;

        private double targetFps;

        private int lastFrameCount;
        private double calculatedFps;

        // We use exponential smoothing for the FPS metric
        // larger value == more smoothing, 0 = no smoothing
        // 0 <= smoothing < 1
        private const double smoothing = 0;

        private static readonly Metrics WorkerMetrics = new Metrics();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();

            targetFps = Application.targetFrameRate == -1
                ? DefaultTargetFrameRate
                : Application.targetFrameRate;

            lastFrameCount = Time.frameCount;
            calculatedFps = targetFps;

            timeOfLastUpdate = DateTime.Now;
            timeOfNextUpdate = timeOfLastUpdate.AddSeconds(TimeBetweenMetricUpdatesSecs);
        }

        protected override void OnUpdate()
        {
            if (DateTime.Now >= timeOfNextUpdate)
            {
                CalculateFps();
                WorkerMetrics.GaugeMetrics["Dynamic.FPS"] = calculatedFps;
                WorkerMetrics.GaugeMetrics["Unity used heap size"] = GC.GetTotalMemory(false);
                WorkerMetrics.Load = CalculateLoad();

                worker.SendMetrics(WorkerMetrics);

                timeOfLastUpdate = DateTime.Now;
                timeOfNextUpdate = timeOfLastUpdate.AddSeconds(TimeBetweenMetricUpdatesSecs);
            }
        }

        // Load defined as performance relative to target FPS.
        // i.e. a load of 0.5 means that the worker is hitting the target FPS
        // but achieving less than half the target FPS takes load above 1.0
        private double CalculateLoad()
        {
            return Math.Max(0.0d, 0.5d * targetFps / calculatedFps);
        }

        private void CalculateFps()
        {
            var frameCount = Time.frameCount - lastFrameCount;
            lastFrameCount = Time.frameCount;
            var rawFps = frameCount / (DateTime.Now - timeOfLastUpdate).TotalSeconds;
            calculatedFps = (rawFps * (1 - smoothing)) + (calculatedFps * smoothing);
        }
    }
}
