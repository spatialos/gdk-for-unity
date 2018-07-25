using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    class ProcessScoresSystem : ComponentSystem
    {
        private struct ScoringData
        {
            public readonly int Length;
            public ComponentDataArray<SpatialOSLauncher> Launcher;

            [ReadOnly]
            public ComponentArray<CommandRequests<Generated.Playground.Launcher.ScoreIncrease.Request>> CommandRequests;
        }

        [Inject] private ScoringData scoringData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < scoringData.Length; i++)
            {
                var launcher = scoringData.Launcher[i];
                foreach (var request in scoringData.CommandRequests[i].Buffer)
                {
                    launcher.Score += request.RawRequest.AmountIncrease;
                }
                scoringData.Launcher[i] = launcher;
            }
        }
    }
}
