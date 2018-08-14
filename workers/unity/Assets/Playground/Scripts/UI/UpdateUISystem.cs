using Generated.Playground;
using Improbable.Gdk.Core;
using Playground.Scripts.UI;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class UpdateUISystem : ComponentSystem
    {
        public struct PlayerDataLauncher
        {
#pragma warning disable 649
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialOSLauncher> Launcher;
            [ReadOnly] public ComponentArray<ComponentsUpdated<SpatialOSLauncher.Update>> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerAuth;
#pragma warning restore 649
        }

        public struct PlayerDataScore
        {
#pragma warning disable 649
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialOSScore> Score;
            [ReadOnly] public ComponentArray<ComponentsUpdated<SpatialOSScore.Update>> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerAuth;
#pragma warning restore 649
        }

#pragma warning disable 649
        [Inject] private PlayerDataLauncher playerDataLauncher;
        [Inject] private PlayerDataScore playerDataScore;
#pragma warning restore 649

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerDataLauncher.Length; i++)
            {
                var launcher = playerDataLauncher.Launcher[i];

                UIComponent.Main.TestText.text =
                    launcher.RechargeTimeLeft > 0.0f ? "Recharging" : $"Energy: {launcher.EnergyLeft}";
            }

            for (var i = 0; i < playerDataScore.Length; i++)
            {
                var score = playerDataScore.Score[i];
                UIComponent.Main.ScoreText.text = $"Score: {score.Score}";
            }
        }
    }
}
