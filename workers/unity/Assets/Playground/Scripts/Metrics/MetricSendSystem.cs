using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class MetricSendSystem : ComponentSystem
    {
        private Worker worker;

        private float timeElapsedSinceUpdate;

        private readonly Queue<float> fpsMeasurements = new Queue<float>();
        private const int MaxFpsSamples = 50;
        private const float TimeBetweenMetricUpdatesSecs = 2.0f;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = Worker.GetWorkerFromWorld(World);
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
            if (timeElapsedSinceUpdate >= TimeBetweenMetricUpdatesSecs)
            {
                timeElapsedSinceUpdate = 0;
                var fps = CalculateFps();
                var load = DefaultLoadCalculation(fps);
                var metrics = new Improbable.Worker.Metrics
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
            if (fpsMeasurements.Count == MaxFpsSamples)
            {
                fpsMeasurements.Dequeue();
            }

            fpsMeasurements.Enqueue(1.0f / Time.deltaTime);
        }

        private float CalculateFps()
        {
            var fps = 0.0f;
            foreach (var measurement in fpsMeasurements)
            {
                fps += measurement;
            }

            fps /= fpsMeasurements.Count;
            return fps;
        }
    }
}
