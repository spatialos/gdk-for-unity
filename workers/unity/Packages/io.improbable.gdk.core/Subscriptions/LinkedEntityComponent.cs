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
        public bool IsValid;

        internal EntityGameObjectLinker Linker;

        private void OnDestroy()
        {
            if (IsValid)
            {
                Linker.UnlinkGameObjectFromEntity(EntityId, gameObject);
            }
        }
    }
}
