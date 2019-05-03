using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class WorkerFlagTest : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private WorkerFlagReader workerFlagReader;
#pragma warning restore 649

        private void OnEnable()
        {
            workerFlagReader.OnWorkerFlagChange += (name, value) =>
            {
                Debug.Log($"Worker flag change detected: {name} with value {value}");
            };
        }
    }
}
