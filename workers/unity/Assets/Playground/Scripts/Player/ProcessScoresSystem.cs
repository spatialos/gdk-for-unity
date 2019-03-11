using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
    {
        [Inject] private CommandSystem commandSystem;
        [Inject] private WorkerSystem workerSystem;

        protected override void OnUpdate()
        {
            var requests = commandSystem.GetRequests<Launcher.IncreaseScore.ReceivedRequest>();
            var scoreComponents = GetComponentDataFromEntity<Score.Component>();

            for (var i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                if (!workerSystem.TryGetEntity(request.EntityId, out var entity))
                {
                    continue;
                }

                var component = scoreComponents[entity];
                component.Score += request.Payload.Amount;
                scoreComponents[entity] = component;
            }
        }
    }
}
