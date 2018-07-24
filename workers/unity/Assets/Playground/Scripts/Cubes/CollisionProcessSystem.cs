using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class CollisionProcessSystem : ComponentSystem
    {
        private struct LaunchableData
        {
            public readonly int Length;
            public ComponentDataArray<SpatialOSLaunchable> Launchable;
            public ComponentDataArray<CollisionComponent> Collision;
            [ReadOnly] public ComponentDataArray<CommandRequestSender<SpatialOSLauncher>> Sender;
        }

        [Inject] private LaunchableData launchableData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < launchableData.Length; i++)
            {
                var collision = launchableData.Collision[i];
                var first = launchableData.Launchable[i];
                var second = collision.other;
                var first_launcher = first.MostRecentLauncher;
                var second_launcher = second.MostRecentLauncher;
                if (first_launcher == 0 && second_launcher == 0)
                {
                    continue;
                }
                if (first_launcher != 0 && second_launcher != 0 && first_launcher != second_launcher)
                {
                    first.MostRecentLauncher = 0;
                    second.MostRecentLauncher = 0;
                    launchableData.Launchable[i] = first;
                    collision.other = second;
                    launchableData.Collision[i] = collision;
                    continue;
                }
                if (first_launcher == 0 && second_launcher != 0)
                {
                    first.MostRecentLauncher = second_launcher;
                    launchableData.Launchable[i] = first;
                    continue;
                }
                if (first_launcher != 0 && second_launcher == 0)
                {
                    second.MostRecentLauncher = first_launcher;
                    collision.other = second;
                    launchableData.Collision[i] = collision;
                    continue;
                }
                if (first_launcher == second_launcher)
                {
                    launchableData.Sender[i].SendScoreIncreaseRequest(first_launcher, new Generated.Playground.ScoreIncreaseRequest()
                    {
                        LauncherToScore = first_launcher,
                        AmountIncrease = 1,
                    });
                }
            }
        }
    }
}
