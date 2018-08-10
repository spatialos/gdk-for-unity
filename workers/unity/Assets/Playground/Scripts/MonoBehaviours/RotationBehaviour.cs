using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using UnityEngine;
using Quaternion = Generated.Improbable.Transform.Quaternion;
using UnityQuaternion = UnityEngine.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

public class RotationBehaviour : MonoBehaviour
{
    [Require] private Transform.Writer writer;

    private UnityQuaternion uRot;

    void Update()
    {
        Quaternion rot = writer.Data.Rotation;
        uRot.Set(rot.X, rot.Y, rot.Z, rot.W);
        uRot *= UnityQuaternion.Euler(Vector3.up * Time.deltaTime * 20);
        rot.X = uRot.x;
        rot.Y = uRot.y;
        rot.Z = uRot.z;
        rot.W = uRot.w;
        writer.Send(new SpatialOSTransform.Update(){ Rotation = new Option<Quaternion>(rot)});
    }
}
