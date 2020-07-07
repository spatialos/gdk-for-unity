using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    public interface IEntityRepresentationResolver
    {
        /// <summary>
        /// The type of the entity (meta-data string)
        /// </summary>
        string EntityType { get; }

        /// <summary>
        ///  Required components before spawning (same as the current `EntityTypeExpectations`)
        /// </summary>
        IEnumerable<uint> RequiredComponents { get; }

        GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager);
        void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs);
    }
}
