using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation.EditmodeTests
{
    public class MockGameObjectCreator : IEntityGameObjectCreator
    {
        public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
        {
        }

        public void OnEntityCreated(string entityType, SpatialOSEntity entity, EntityGameObjectLinker linker)
        {
            throw new NotImplementedException();
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            throw new NotImplementedException();
        }
    }
}
