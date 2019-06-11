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
        private EntityQuery group;

        protected override void OnCreate()
        {
            base.OnCreate();

            group = GetEntityQuery(
                ComponentType.ReadWrite<Launcher.Component>(),
                ComponentType.ReadOnly<Recharging>()
            );
        }

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            Entities.With(group).ForEach((Entity entity, ref Launcher.Component launcher) =>
            {
                launcher.RechargeTimeLeft -= dt;
                if (launcher.RechargeTimeLeft < 0.0f)
                {
                    launcher.RechargeTimeLeft = 0.0f;
                    launcher.EnergyLeft = 100;
                    PostUpdateCommands.RemoveComponent<Recharging>(entity);
                }
            });
        }
    }
}
