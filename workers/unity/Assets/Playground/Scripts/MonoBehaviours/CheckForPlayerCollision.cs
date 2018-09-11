using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;
using Collisions = Generated.Playground.Collisions;
using Empty = Generated.Playground.Empty;


#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion


public class CheckForPlayerCollision : MonoBehaviour
{
    [Require] private Collisions.Requirable.Writer collisionsWriter;

    private void OnTriggerEnter(Collider other)
    {
        // <*>Trigger events are called even when the behaviour is disabled.
        collisionsWriter?.SendPlayerCollided(new Empty());
    }
}
