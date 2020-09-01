using Improbable.Gdk.Core;
using Improbable.Gdk.Core.SceneAuthoring;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [AddComponentMenu("SpatialOS/Authoring Components/Transform From GameObject Authoring Component")]
    public class TransformFromGameObjectAuthoringComponent : MonoBehaviour, ISpatialOsAuthoringComponent
    {
#pragma warning disable 649
        [SerializeField] private string writeAccess;
#pragma warning restore 649

        public void WriteTo(EntityTemplate template)
        {
            var transformSnapshot = TransformUtils.CreateTransformSnapshot(transform.position, transform.rotation);
            template.AddComponent(transformSnapshot, writeAccess);
        }
    }
}
