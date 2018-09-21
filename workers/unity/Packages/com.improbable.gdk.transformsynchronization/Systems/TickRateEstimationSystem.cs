using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class TickRateEstimationSystem : ComponentSystem
    {
        // Estimate of the the number of physics ticks that happen per second according the the system clock
        public float PhysicsTicksPerRealSecond;

        private const int NumberOfSamples = 20;

        private readonly Queue<long> samples = new Queue<long>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            PhysicsTicksPerRealSecond = 1.0f / Time.fixedDeltaTime;
        }

        protected override void OnUpdate()
        {
            long sample = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            samples.Enqueue(sample);
            if (samples.Count == NumberOfSamples)
            {
                var oldestSample = samples.Dequeue();
                float secondsPast = 0.001f * (float) (sample - oldestSample);
                PhysicsTicksPerRealSecond = (float) NumberOfSamples / secondsPast;
            }
        }
    }
}
