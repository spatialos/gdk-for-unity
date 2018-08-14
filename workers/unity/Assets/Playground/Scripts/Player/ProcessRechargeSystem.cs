using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public struct Recharging : IComponentData
    {
    }

    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ProcessRechargeSystem : ComponentSystem
    {
        private struct Data
        {
#pragma warning disable 649
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Recharging> Reloading;
            public ComponentDataArray<SpatialOSLauncher> Launcher;
#pragma warning restore 649
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;
            for (var i = 0; i < data.Length; i++)
            {
                var launcher = data.Launcher[i];
                launcher.RechargeTimeLeft -= dt;
                if (launcher.RechargeTimeLeft < 0.0f)
                {
                    launcher.RechargeTimeLeft = 0.0f;
                    launcher.EnergyLeft = 100;
                    PostUpdateCommands.RemoveComponent<Recharging>(data.Entity[i]);
                }

                data.Launcher[i] = launcher;
            }
        }
    }
}
