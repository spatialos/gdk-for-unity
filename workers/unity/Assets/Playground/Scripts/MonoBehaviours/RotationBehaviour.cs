using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using SpatialQuaternion = Generated.Improbable.Transform.Quaternion;
using Quaternion = UnityEngine.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

public class RotationBehaviour : MonoBehaviour
{
    public bool RotatingClockWise = true;
    [Require] private Transform.Requirables.Writer writer;

    private static SpatialQuaternion UnityToSpatialQuaternion(Quaternion q)
    {
        return new SpatialQuaternion
        {
            X = q.x,
            Y = q.y,
            Z = q.z,
            W = q.w
        };
    }

    private Quaternion uRot;

    void Update()
    {
        SpatialQuaternion rot = writer.Data.Rotation;
        uRot.Set(rot.X, rot.Y, rot.Z, rot.W);
        uRot *= Quaternion.Euler((RotatingClockWise ? Vector3.up : Vector3.down) * Time.deltaTime * 20);
        writer.Send(new Transform.Update
        {
            Rotation = new Option<SpatialQuaternion>(UnityToSpatialQuaternion(uRot))
        });
    }
}
