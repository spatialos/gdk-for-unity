using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation.Types
{
    [Serializable]
    public class AuthoritativeEntityResolver : IEntityRepresentationResolver
    {
        public string EntityType => entityType;
        public IEnumerable<uint> RequiredComponents => requiredComponents.Append(authComponentId);

#pragma warning disable 649
        [SerializeField] private string entityType;
        [SerializeField] private GameObject ownedPrefab;
        [SerializeField] private GameObject unownedPrefab;
        [SerializeField] private uint authComponentId;
        [SerializeField] private uint[] requiredComponents = { };
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            var authComponent = ComponentDatabase.GetMetaclass(authComponentId).Authority;
            return manager.HasComponent(entityInfo.Entity, authComponent)
                ? ownedPrefab
                : unownedPrefab;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(ownedPrefab);
            referencedPrefabs.Add(unownedPrefab);
        }
    }
}
