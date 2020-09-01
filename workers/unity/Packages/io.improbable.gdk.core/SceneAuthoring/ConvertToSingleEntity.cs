using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core.SceneAuthoring
{
    [AddComponentMenu("SpatialOS/Authoring Components/Convert to Single Entity")]
    public class ConvertToSingleEntity : MonoBehaviour, IConvertGameObjectToSpatialOsEntity
    {
        [SerializeField] internal bool UseSpecificEntityId;
        [SerializeField] internal long DesiredEntityId;
        [SerializeField] internal string[] ReadAcl = { };

        public List<ConvertedEntity> Convert()
        {
            var components = GetComponents<ISpatialOsAuthoringComponent>();
            var template = new EntityTemplate();

            foreach (var component in components)
            {
                component.WriteTo(template);
            }

            template.SetReadAccess(ReadAcl);

            var entity = UseSpecificEntityId
                ? new ConvertedEntity(new EntityId(DesiredEntityId), template)
                : new ConvertedEntity(template);

            return new List<ConvertedEntity> { entity };
        }
    }
}
