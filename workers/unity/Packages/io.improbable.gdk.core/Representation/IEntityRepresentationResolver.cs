using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    public interface IEntityRepresentationResolver
    {
        /// <summary>
        /// The type of the entity.
        /// </summary>
        string EntityType { get; }

        /// <summary>
        ///  Required components before this resolver can resolve the GameObject
        /// </summary>
        IEnumerable<uint> RequiredComponents { get; }

        GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager);
        void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs);
    }
}
