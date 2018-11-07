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

        private float timeElapsedSinceUpdate;

        private const float TimeBetweenMetricUpdatesSecs = 2.0f;
        private const int DefaultTargetFrameRate = 60;

        private float TargetFps;

        private float totalFpsCount;
        private int fpsMeasurements;

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

            timeElapsedSinceUpdate += Time.deltaTime;

            AddFpsSample();
            if (timeElapsedSinceUpdate >= TimeBetweenMetricUpdatesSecs)
            {
                timeElapsedSinceUpdate = 0;

                var dynamicFps = CalculateFps();
                Metrics.GaugeMetrics["Dynamic.FPS"] = dynamicFps;
                Metrics.GaugeMetrics["Unity used heap size"] = GC.GetTotalMemory(false);
                Metrics.Load = CalculateLoad(dynamicFps);

                connection.SendMetrics(Metrics);
            }
        }

        private float CalculateLoad(float dynamicFps)
        {
            return Mathf.Max(0.0f, 0.5f * TargetFps / dynamicFps);
        }

        private void AddFpsSample()
        {
            totalFpsCount += 1.0f / Time.deltaTime;
            fpsMeasurements++;
        }

        private float CalculateFps()
        {
            var fps = totalFpsCount / fpsMeasurements;
            totalFpsCount = 0;
            fpsMeasurements = 0;
            return fps;
        }
    }
}
