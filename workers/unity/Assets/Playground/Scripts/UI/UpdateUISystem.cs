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
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialOSLauncher> Launcher;
            [ReadOnly] public ComponentArray<ComponentsUpdated<SpatialOSLauncher.Update>> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerAuth;
        }

        public struct PlayerDataScore
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialOSScore> Score;
            [ReadOnly] public ComponentArray<ComponentsUpdated<SpatialOSScore.Update>> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerAuth;
        }

        [Inject] private PlayerDataLauncher playerDataLauncher;
        [Inject] private PlayerDataScore playerDataScore;

        protected override void OnUpdate()
        {
            for (int i = 0; i < playerDataLauncher.Length; i++)
            {
                var launcher = playerDataLauncher.Launcher[i];

                if (launcher.RechargeTimeLeft > 0.0f)
                {
                    UIComponent.Main.TestText.text = "Recharging";
                }
                else
                {
                    UIComponent.Main.TestText.text = $"Energy: {launcher.EnergyLeft}";
                }
            }

            for (int i = 0; i < playerDataScore.Length; i++)
            {
                var score = playerDataScore.Score[i];
                UIComponent.Main.ScoreText.text = $"Score: {score.Score}";
            }
        }
    }
}
