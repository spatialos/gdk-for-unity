using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

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
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Launcher.Component> Launcher;
            [ReadOnly] public ComponentDataArray<Recharging> Reloading;
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
