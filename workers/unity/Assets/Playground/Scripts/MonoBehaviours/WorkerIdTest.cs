using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground
{
    public class WorkerIdTest : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private WorkerId workerId;
#pragma warning restore 649

        private void OnEnable()
        {
            Debug.Log($"Testing [Require] WorkerId: {workerId.Id}");
        }
    }
}
