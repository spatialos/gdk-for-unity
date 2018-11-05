using Improbable.Gdk.Subscriptions;
using Improbable.Transform;
using UnityEngine;
using SpatialQuaternion = Improbable.Transform.Quaternion;
using Quaternion = UnityEngine.Quaternion;

#region Diagnostic control

#pragma warning disable 169

#endregion

public class RotationBehaviour : MonoBehaviour
{
    public bool RotatingClockWise = true;

    // Should only run when authoritative
    [Require] private TransformInternalWriter transformWriter;

    private void Update()
    {
        transform.rotation *=
            Quaternion.Euler((RotatingClockWise ? Vector3.up : Vector3.down) * Time.deltaTime * 20.0f);
    }
}
