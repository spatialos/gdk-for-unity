using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class WorkerFlagTest : MonoBehaviour
    {
        [Require] private WorkerFlagReader workerFlagReader;

        private void OnEnable()
        {
            workerFlagReader.OnWorkerFlagChange += (name, value) =>
            {
                Debug.Log($"Worker flag change detected: {name} with value {value}");
            };
        }
    }
}
