using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;

namespace Playground
{
    public class CheckForPlayerCollision : MonoBehaviour
    {
        [Require] private Collisions.Requirable.Writer writer;

        void OnTriggerEnter(Collider other)
        {
            writer?.SendPlayerCollided(new Empty());
        }
    }
}
