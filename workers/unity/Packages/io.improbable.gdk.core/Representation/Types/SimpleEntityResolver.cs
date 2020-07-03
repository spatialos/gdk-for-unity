using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation.Types
{
    [Serializable]
    public class SimpleEntityResolver : IEntityRepresentationResolver
    {
        public SimpleEntityResolver()
        {
        }

        public SimpleEntityResolver(string entityType, GameObject prefab)
        {
            this.entityType = entityType;
            this.prefab = prefab;
        }

        public string EntityType => entityType;
        public IEnumerable<uint> RequiredComponents => requiredComponents;

#pragma warning disable 649
        [SerializeField] private string entityType;
        [SerializeField] private GameObject prefab;
        [SerializeField] private uint[] requiredComponents = { };
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            return prefab;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(prefab);
        }
    }
}
