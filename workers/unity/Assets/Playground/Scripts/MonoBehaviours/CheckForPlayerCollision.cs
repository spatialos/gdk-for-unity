using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Collisions = Generated.Playground.Collisions;
using Empty = Generated.Playground.Empty;

public class CheckForPlayerCollision : MonoBehaviour
{
    [Require] private Collisions.Requirables.Writer writer;

    void OnTriggerEnter(Collider other)
    {
        if (writer != null)
        {
            writer.SendPlayerCollided(new Empty());
        }
    }
}
