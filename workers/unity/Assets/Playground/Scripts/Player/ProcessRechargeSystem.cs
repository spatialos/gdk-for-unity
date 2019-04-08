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
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            group = GetComponentGroup(
                ComponentType.Create<Launcher.Component>(),
                ComponentType.ReadOnly<Recharging>()
            );
        }

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;
            var launcherData = group.GetComponentDataArray<Launcher.Component>();
            var entities = group.GetEntityArray();

            for (var i = 0; i < entities.Length; i++)
            {
                var launcher = launcherData[i];

                launcher.RechargeTimeLeft -= dt;
                if (launcher.RechargeTimeLeft < 0.0f)
                {
                    launcher.RechargeTimeLeft = 0.0f;
                    launcher.EnergyLeft = 100;
                    PostUpdateCommands.RemoveComponent<Recharging>(entities[i]);
                }

                launcherData[i] = launcher;
            }
        }
    }
}
