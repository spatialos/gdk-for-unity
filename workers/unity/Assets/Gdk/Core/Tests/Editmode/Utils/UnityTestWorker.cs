using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Utils
{
    internal class UnityTestWorker : WorkerBase
    {
        public const string WorkerType = "UnityTestWorker";

        public override string GetWorkerType => WorkerType;

        public UnityTestWorker(string workerId, Vector3 origin) : base(workerId, origin)
        {
        }
    }
}
