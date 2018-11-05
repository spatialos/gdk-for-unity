using Improbable.Common;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground
{
    public class CheckForPlayerCollision : MonoBehaviour
    {
        [Require] private CollisionsWriter collisionWriter;

        void OnTriggerEnter(Collider other)
        {
            collisionWriter?.SendPlayerCollidedEvent(new Empty());
        }
    }
}
