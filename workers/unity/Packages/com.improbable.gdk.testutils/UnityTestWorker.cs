using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TestUtils
{
    public class UnityTestWorker : WorkerBase
    {
        public UnityTestWorker(EntityManager entityManager) : base(new ReceptionistConfig(), entityManager, new LoggingDispatcher(), Vector3.zero)
        {
        }
    }
}
