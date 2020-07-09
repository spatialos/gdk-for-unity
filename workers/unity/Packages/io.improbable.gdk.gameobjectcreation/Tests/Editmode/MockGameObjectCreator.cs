using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation.EditmodeTests
{
    public class MockGameObjectCreator : IEntityGameObjectCreator
    {
        public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
        {
        }

        public void OnEntityCreated(SpatialOSEntityInfo entityInfo, GameObject prefab, EntityManager entityManager,
            EntityGameObjectLinker linker)
        {
            throw new NotImplementedException();
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            throw new NotImplementedException();
        }
    }
}
