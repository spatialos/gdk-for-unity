using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
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
