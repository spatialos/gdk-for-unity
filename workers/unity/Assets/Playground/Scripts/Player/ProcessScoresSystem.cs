using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
    {
        private struct ScoringData
        {
#pragma warning disable 649
            public readonly int Length;
            public ComponentDataArray<SpatialOSScore> Score;

            [ReadOnly]
            public ComponentArray<CommandRequests<Generated.Playground.Launcher.IncreaseScore.Request>> CommandRequests;
#pragma warning restore 649
        }

        [Inject] private ScoringData scoringData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < scoringData.Length; i++)
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
