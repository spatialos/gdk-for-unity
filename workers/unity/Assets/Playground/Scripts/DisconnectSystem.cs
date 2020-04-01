using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    internal class DisconnectSystem : ComponentSystem
    {
        private EntityQuery group;

        protected override void OnCreate()
        {
            base.OnCreate();

            group = GetEntityQuery(
                ComponentType.ReadOnly<OnDisconnected>(),
                ComponentType.ReadOnly<WorkerEntityTag>()
            );
        }

        protected override void OnUpdate()
        {
            Entities.With(group).ForEach((OnDisconnected data) =>
            {
                Debug.LogWarning($"Disconnected from SpatialOS with reason: {data.ReasonForDisconnect}");
            });
        }
    }
}
