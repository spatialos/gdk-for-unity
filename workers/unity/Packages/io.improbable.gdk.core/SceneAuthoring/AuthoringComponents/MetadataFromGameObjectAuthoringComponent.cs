using UnityEngine;

namespace Improbable.Gdk.Core.SceneAuthoring.AuthoringComponents
{
    [AddComponentMenu("SpatialOS/Authoring Components/Metadata From GameObject Authoring Component")]
    public class MetadataFromGameObjectAuthoringComponent : MonoBehaviour, ISpatialOsAuthoringComponent
    {
#pragma warning disable 649
        [SerializeField] private string writeAccess;
#pragma warning restore 649

        public void WriteTo(EntityTemplate template)
        {
            var metadata = new Metadata.Snapshot(gameObject.name);
            template.AddComponent(metadata, writeAccess);
        }
    }
}
