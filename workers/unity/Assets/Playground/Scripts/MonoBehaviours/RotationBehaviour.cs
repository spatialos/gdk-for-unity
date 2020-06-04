using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using SpatialQuaternion = Improbable.Gdk.TransformSynchronization.CompressedQuaternion;

public class RotationBehaviour : MonoBehaviour
{
    public bool RotatingClockWise = true;

    // Should only run when authoritative
    [Require] private TransformInternalWriter transformWriter;

    private void Update()
    {
        transform.rotation *=
            Quaternion.Euler(20.0f * Time.deltaTime * (RotatingClockWise ? Vector3.up : Vector3.down));
    }
}
