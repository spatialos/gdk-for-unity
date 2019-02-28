using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    public class LinkedEntityComponent : MonoBehaviour
    {
        public EntityId EntityId;
        public World World;
        public WorkerSystem Worker;

        internal bool IsValid;
    }
}
