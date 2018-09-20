using Improbable.Common;
using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;

namespace Playground
{
    public class CheckForPlayerCollision : MonoBehaviour
    {
        [Require] private Collisions.Requirable.Writer collisionWriter;

        void OnTriggerEnter(Collider other)
        {
            collisionWriter?.SendPlayerCollided(new Empty());
        }
    }
}
