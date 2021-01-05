using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Restricted;
using Unity.Entities;
using UnityEngine;

namespace Playground.LoadBalancing
{
    public class BootstrapLoadBalancing : MonoBehaviour
    {
        [Require] private World world;
        [Require] private WorkerCommandSender workerCommandSender;

        public void OnEnable()
        {
            workerCommandSender.SendAssignPartitionCommand(world.GetExistingSystem<WorkerSystem>().WorkerEntityId, new AssignPartitionRequest(1));
        }
    }
}
