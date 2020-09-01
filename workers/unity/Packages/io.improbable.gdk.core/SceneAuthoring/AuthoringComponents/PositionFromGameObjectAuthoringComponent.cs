using UnityEngine;

namespace Improbable.Gdk.Core.SceneAuthoring.AuthoringComponents
{
    [AddComponentMenu("SpatialOS/Authoring Components/Position From GameObject Authoring Component")]
    public class PositionFromGameObjectAuthoringComponent : MonoBehaviour, ISpatialOsAuthoringComponent
    {
#pragma warning disable 649
        [SerializeField] private string writeAccess;
#pragma warning restore 649

        public void WriteTo(EntityTemplate template)
        {
            var coords = Coordinates.FromUnityVector(transform.position);
            template.AddComponent(new Position.Snapshot(coords), writeAccess);
        }
    }
}
