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

        private long timeOfNextUpdate;
        private long timeOfLastUpdate;

        private const double TimeBetweenMetricUpdatesSecs = 2;
        private const int DefaultTargetFrameRate = 60;

        private double TargetFps;

        private float lastSentFps;
        private int lastFrameCount;

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

            if (DateTime.Now.Ticks >= timeOfNextUpdate)
            {
                var dynamicFps = CalculateFps();
                Metrics.GaugeMetrics["Dynamic.FPS"] = dynamicFps;
                Metrics.GaugeMetrics["Unity used heap size"] = GC.GetTotalMemory(false);
                Metrics.Load = CalculateLoad(dynamicFps);

                connection.SendMetrics(Metrics);

                timeOfLastUpdate = DateTime.Now.Ticks;
                timeOfNextUpdate = DateTime.Now.AddSeconds(TimeBetweenMetricUpdatesSecs).Ticks;
            }
        }

        private double CalculateLoad(double dynamicFps)
        {
            return Math.Max(0.0d, 0.5d * TargetFps / dynamicFps);
        }

        private double CalculateFps()
        {
            var elapsedTime = DateTime.Now.Ticks - timeOfLastUpdate;
            var frames = Time.frameCount - lastFrameCount;
            lastFrameCount = Time.frameCount;
            return frames / TimeSpan.FromTicks(elapsedTime).TotalSeconds;
        }
    }
}
