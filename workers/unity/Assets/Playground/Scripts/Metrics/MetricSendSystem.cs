using System;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Playground
{
    public class MetricSendSystem : ComponentSystem
    {
        private Connection connection;

        private DateTime timeOfNextUpdate;
        private DateTime timeOfLastUpdate;

        private const double TimeBetweenMetricUpdatesSecs = 2;
        private const int DefaultTargetFrameRate = 60;

        private double TargetFps;

        private int lastFrameCount;
        private double lastSentFps;

        // 0 <= smoothing < 1
        // larger value == more smoothing
        private const double smoothing = 0;

        private Improbable.Worker.Metrics Metrics;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            connection = World.GetExistingManager<WorkerSystem>().Connection;
            Metrics = new Improbable.Worker.Metrics();

            TargetFps = Application.targetFrameRate == -1
                ? DefaultTargetFrameRate
                : Application.targetFrameRate;
        }

        protected override void OnUpdate()
        {
            if (connection == null)
            {
                return;
            }

            if (DateTime.Now >= timeOfNextUpdate)
            {
                var dynamicFps = CalculateFps();
                Metrics.GaugeMetrics["Dynamic.FPS"] = dynamicFps;
                Metrics.GaugeMetrics["Unity used heap size"] = GC.GetTotalMemory(false);
                Metrics.Load = CalculateLoad(dynamicFps);

                connection.SendMetrics(Metrics);

                lastSentFps = dynamicFps;
                timeOfLastUpdate = DateTime.Now;
                timeOfNextUpdate = DateTime.Now.AddSeconds(TimeBetweenMetricUpdatesSecs);
            }
        }

        // Load defined as performance relative to target FPS.
        // i.e. a load of 0.5 means that the worker is hitting the target FPS
        // but achieving less than half the target FPS takes load above 1.0
        private double CalculateLoad(double dynamicFps)
        {
            return Math.Max(0.0d, 0.5d * TargetFps / dynamicFps);
        }

        private double CalculateFps()
        {
            var frameCount = Time.frameCount - lastFrameCount;
            lastFrameCount = Time.frameCount;
            var rawFps = frameCount / (DateTime.Now - timeOfLastUpdate).TotalSeconds;
            return (rawFps * (1 - smoothing)) + (lastSentFps * smoothing);
        }
    }
}
