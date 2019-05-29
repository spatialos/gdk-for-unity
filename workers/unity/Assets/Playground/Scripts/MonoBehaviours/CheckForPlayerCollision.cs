using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground
{
    public class CheckForPlayerCollision : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private CollisionsWriter collisionWriter;
#pragma warning restore 649

        void OnTriggerEnter(Collider other)
        {
            collisionWriter?.SendPlayerCollidedEvent(new Empty());
        }
    }
}
