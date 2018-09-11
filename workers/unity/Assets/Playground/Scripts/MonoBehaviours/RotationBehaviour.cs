using Generated.Improbable.Transform;
using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;
using SpatialQuaternion = Generated.Improbable.Transform.Quaternion;
using Quaternion = UnityEngine.Quaternion;

public class RotationBehaviour : MonoBehaviour
{
    public bool RotatingClockWise = true;

    // Should only run when authoritative
    [Require] private TransformInternal.Requirable.Writer _;

    void Update()
    {
        transform.rotation *=
            Quaternion.Euler((RotatingClockWise ? Vector3.up : Vector3.down) * Time.deltaTime * 20.0f);
    }
}
