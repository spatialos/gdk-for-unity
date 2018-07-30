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
            public ComponentDataArray<SpatialOSScore> Score;

            [ReadOnly]
            public ComponentArray<CommandRequests<Generated.Playground.Launcher.IncreaseScore.Request>> CommandRequests;
        }

        [Inject] private ScoringData scoringData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < scoringData.Length; i++)
            {
                var playerScore = scoringData.Score[i];
                foreach (var request in scoringData.CommandRequests[i].Buffer)
                {
                    playerScore.Score += request.RawRequest.Amount;
                }

                scoringData.Score[i] = playerScore;
            }
        }
    }
}
