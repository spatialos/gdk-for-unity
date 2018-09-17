using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
    {
        private struct ScoringData
        {
            public readonly int Length;
            public ComponentDataArray<Score.Component> Score;

            [ReadOnly] public ComponentDataArray<Launcher.CommandRequests.IncreaseScore> CommandRequests;
        }

        [Inject] private ScoringData scoringData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < scoringData.Length; i++)
            {
                var playerScore = scoringData.Score[i];
                foreach (var request in scoringData.CommandRequests[i].Requests)
                {
                    playerScore.Score += request.Payload.Amount;
                }

                scoringData.Score[i] = playerScore;
            }
        }
    }
}
