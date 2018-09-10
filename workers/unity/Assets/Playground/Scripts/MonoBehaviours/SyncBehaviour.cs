using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Quaternion = Generated.Improbable.Transform.Quaternion;
using UnityQuaternion = UnityEngine.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

namespace Playground.MonoBehaviours
{
    public class SyncBehaviour : MonoBehaviour
    {
        [Require] private Transform.Requirable.Reader reader;

        void Update()
        {
            Quaternion rot = reader.Data.Rotation;
            gameObject.transform.rotation = new UnityQuaternion(rot.X, rot.Y, rot.Z, rot.W);
        }
    }
}
