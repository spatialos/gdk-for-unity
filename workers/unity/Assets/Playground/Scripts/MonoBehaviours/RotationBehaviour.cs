using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;
using SpatialQuaternion = Generated.Improbable.Transform.Quaternion;
using Quaternion = UnityEngine.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

public class RotationBehaviour : MonoBehaviour
{
    public bool RotatingClockWise = true;
    [Require] private Transform.Requirable.Writer writer;

    void Update()
    {
        transform.rotation *=
            Quaternion.Euler((RotatingClockWise ? Vector3.up : Vector3.down) * Time.deltaTime * 20.0f);
    }
}
