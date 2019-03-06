using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Playground.Scripts.UI;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class UpdateUISystem : ComponentSystem
    {
        public struct PlayerDataLauncher
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Launcher.Component> Launcher;
            [ReadOnly] public ComponentDataArray<Launcher.ReceivedUpdates> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerAuth;
        }

        public struct PlayerDataScore
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Score.Component> Score;
            [ReadOnly] public ComponentDataArray<Score.ReceivedUpdates> Updates;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerAuth;
        }

        [Inject] private PlayerDataLauncher playerDataLauncher;
        [Inject] private PlayerDataScore playerDataScore;

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
