using System.Collections;
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
        internal EntityGameObjectLinker Linker;

        private void OnDisable()
        {
            if (IsValid)
            {
                Linker.UnlinkGameObjectFromEntity(EntityId, gameObject);
            }
        }

        internal void Invalidate()
        {
            // Can't delete immediately as the GameObject might be linked to another entity before the end of the frame
            IsValid = false;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DeleteIfInvalid());
            }
        }

        private IEnumerator DeleteIfInvalid()
        {
            yield return new WaitForEndOfFrame();
            if (!IsValid)
            {
                Destroy(this);
            }
        }
    }
}
