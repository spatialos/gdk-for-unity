using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Improbable.Gdk.Core;

namespace Playground
{
    public class MetricSendSystem : ComponentSystem
    {
        private WorkerBase worker;

        private float timeElapsedSinceUpdate = 0.0f;

        private List<float> fpsMeasurements = new List<float>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            var connection = worker.Connection;

            timeElapsedSinceUpdate += Time.deltaTime;
            AddFpsSample();
            if (timeElapsedSinceUpdate >= MetricConfig.TimeBetweenMetricUpdatesSecs)
            {
                timeElapsedSinceUpdate = 0;
                float fps = CalculateFps();
                var load = MetricConfig.CalculateLoad == null ?
                    DefaultLoadCalculation(fps) : MetricConfig.CalculateLoad(fps);
                Improbable.Worker.Metrics metrics = new Improbable.Worker.Metrics
                {
                    Load = load
                };
                connection.SendMetrics(metrics);
            }
        }

        private float DefaultLoadCalculation(float fps)
        {
            float targetFps = Application.targetFrameRate;
            return Mathf.Max(0.0f, (targetFps - fps) / (0.5f * targetFps));
        }

        private void AddFpsSample()
        {
            if (fpsMeasurements.Count == MetricConfig.MaxFpsSamples)
            {
                fpsMeasurements.RemoveAt(0);
            }

            fpsMeasurements.Add(1.0f / Time.deltaTime);
        }

        private float CalculateFps()
        {
            float fps = 0.0f;
            foreach (var measurement in fpsMeasurements)
            {
                fps += measurement;
            }

            fps /= fpsMeasurements.Count;
            return fps;
        }
    }
}
