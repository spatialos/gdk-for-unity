using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class DisconnectSystem : ComponentSystem
    {
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            group = GetComponentGroup(
                ComponentType.ReadOnly<OnDisconnected>(),
                ComponentType.ReadOnly<WorkerEntityTag>()
            );
        }

        protected override void OnUpdate()
        {
            Entities.With(group).ForEach((OnDisconnected data) =>
            {
                Debug.LogWarningFormat("Disconnected from SpatialOS with reason: \"{0}\"",
                    data.ReasonForDisconnect);
            });
        }
    }
}
