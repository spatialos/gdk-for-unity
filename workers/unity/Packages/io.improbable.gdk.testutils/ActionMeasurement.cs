using System;
using System.Collections.Generic;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.TestUtils
{
    public class ActionMeasurement
    {
        private readonly Action action;

        private readonly List<SampleGroup> sampleGroups = new List<SampleGroup>();

        private int warmupCount = 5;
        private int measurementCount = 10;
        private int iterationsPerMeasurement = 1;

        private ActionMeasurement(Action action)
        {
            this.action = action;
        }

        public static ActionMeasurement Measure(Action action)
        {
            return new ActionMeasurement(action);
        }

        public ActionMeasurement ProfilerMarkers(IEnumerable<string> profilerMarkers)
        {
            foreach (var marker in profilerMarkers)
            {
                var sampleGroup = new SampleGroup(marker, Unity.PerformanceTesting.SampleUnit.Nanosecond);
                sampleGroup.GetRecorder().enabled = false;
                sampleGroups.Add(sampleGroup);
            }

            return this;
        }

        public ActionMeasurement WarmupCount(int warmupCount)
        {
            this.warmupCount = warmupCount;
            return this;
        }

        public ActionMeasurement MeasurementCount(int measurementCount)
        {
            this.measurementCount = measurementCount;
            return this;
        }

        public ActionMeasurement IterationsPerMeasurement(int iterationsPerMeasurement)
        {
            this.iterationsPerMeasurement = iterationsPerMeasurement;
            return this;
        }

        private void StartMeasurements()
        {
            foreach (var sampleGroup in sampleGroups)
            {
                sampleGroup.GetRecorder().enabled = true;
            }
        }

        private void EndMeasurements()
        {
            foreach (var sampleGroup in sampleGroups)
            {
                var recorder = sampleGroup.GetRecorder();
                recorder.enabled = false;
                Unity.PerformanceTesting.Measure.Custom(sampleGroup, (double) recorder.elapsedNanoseconds / recorder.sampleBlockCount);
            }
        }

        public void Run()
        {
            for (var i = 0; i < warmupCount; i++)
            {
                action();
            }

            for (var i = 0; i < measurementCount; i++)
            {
                StartMeasurements();

                for (var j = 0; j < iterationsPerMeasurement; j++)
                {
                    action();
                }

                EndMeasurements();
            }
        }
    }
}
