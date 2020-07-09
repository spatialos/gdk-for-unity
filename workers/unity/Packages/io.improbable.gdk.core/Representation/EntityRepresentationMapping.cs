using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    [CreateAssetMenu(menuName = "SpatialOS/Entity Representation Mapping")]
    public class EntityRepresentationMapping : ScriptableObject
    {
        [SerializeReference]
        public List<IEntityRepresentationResolver> EntityRepresentationResolvers
            = new List<IEntityRepresentationResolver>();
    }
}
