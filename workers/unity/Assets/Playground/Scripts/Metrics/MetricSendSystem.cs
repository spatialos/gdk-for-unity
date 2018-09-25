using System.Collections.Generic;
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

        private readonly Queue<float> fpsMeasurements = new Queue<float>();
        private const int MaxFpsSamples = 50;
        private const float TimeBetweenMetricUpdatesSecs = 2.0f;
        private const int DefaultTargetFrameRate = 60;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            connection = World.GetExistingManager<WorkerSystem>().Connection;
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
                var fps = CalculateFps();
                var load = DefaultLoadCalculation(fps);
                var metrics = new Improbable.Worker.Metrics
                {
                    Load = load
                };
                connection.SendMetrics(metrics);
            }
        }

        private static float DefaultLoadCalculation(float fps)
        {
            float targetFps = Application.targetFrameRate;

            if (targetFps == -1)
            {
                targetFps = DefaultTargetFrameRate;
            }

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
