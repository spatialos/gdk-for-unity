using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
    {
        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            commandSystem = World.GetExistingManager<CommandSystem>();
            workerSystem = World.GetExistingManager<WorkerSystem>();
        }

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
