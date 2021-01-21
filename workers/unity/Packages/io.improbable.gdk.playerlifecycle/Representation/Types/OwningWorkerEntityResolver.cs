using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle.Representation.Types
{
    [Serializable]
    public class OwningWorkerEntityResolver : IEntityRepresentationResolver
    {
        public string EntityType => entityType;
        public IEnumerable<uint> RequiredComponents => requiredComponents.Append(OwningWorker.ComponentId);

#pragma warning disable 649
        [SerializeField] private string entityType;
        [SerializeField] private GameObject ownedPrefab;
        [SerializeField] private GameObject unownedPrefab;
        [SerializeField] private uint[] requiredComponents = { };
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            var owningWorkerEntityId = manager.GetComponentData<OwningWorker.Component>(entityInfo.Entity).WorkerEntityId;
            var myWorkerEntityId = manager.World.GetExistingSystem<WorkerSystem>().WorkerEntityId;

            return owningWorkerEntityId == myWorkerEntityId
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
