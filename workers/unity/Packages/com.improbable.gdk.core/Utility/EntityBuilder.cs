using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Core
{
    public class EntityBuilder
    {
        private readonly Entity entity;
        private Acl acl;
        private bool hasBuiltOnce;

        private readonly HashSet<uint> componentsAdded = new HashSet<uint>();

        public static EntityBuilder Begin()
        {
            return new EntityBuilder();
        }

        protected EntityBuilder()
        {
            entity = new Entity();
        }

        public EntityBuilder AddComponent(ComponentData componentData, string writeAccess)
        {
            if (componentsAdded.Contains(componentData.ComponentId))
            {
                throw new InvalidOperationException(
                    $"Cannot add multiple components of the same type to the same entity. " +
                    $"Attempted to add componentId: {componentData.ComponentId} more than once.");
            }

            entity.Add(componentData);
            componentsAdded.Add(componentData.ComponentId);
            acl.SetComponentWriteAccess(componentData.ComponentId, writeAccess);
            return this;
        }

        public EntityBuilder AddPosition(double x, double y, double z, string writeAccess)
        {
            var schemaData = new SchemaComponentData(WellKnownComponents.Position.ComponentId);
            var fields = schemaData.GetFields();
            fields.AddDouble(1, x);
            fields.AddDouble(2, y);
            fields.AddDouble(3, z);

            return AddComponent(new ComponentData(schemaData), writeAccess);
        }

        public EntityBuilder SetPersistence(bool persistence)
        {
            if (persistence)
            {
                var schemaData = new SchemaComponentData(WellKnownComponents.Persistence.ComponentId);
                entity.Add(new ComponentData(schemaData));
            }
            else
            {
                if (entity.Get(WellKnownComponents.Persistence.ComponentId).HasValue)
                {
                    entity.Remove(WellKnownComponents.Persistence.ComponentId);
                }
            }

            return this;
        }

        public EntityBuilder AddMetadata(string metadata, string writeAccess)
        {
            var schemaData = new SchemaComponentData(WellKnownComponents.Metadata.ComponentId);
            var fields = schemaData.GetFields();
            fields.AddString(1, metadata);

            return AddComponent(new ComponentData(schemaData), writeAccess);
        }

        public EntityBuilder SetReadAcl(string attribute, params string[] attributes)
        {
            acl.AddReadAccess(attribute);
            foreach (var attr in attributes)
            {
                acl.AddReadAccess(attr);
            }

            return this;
        }

        public EntityBuilder SetReadAcl(List<string> attributes)
        {
            foreach (var attribute in attributes)
            {
                acl.AddReadAccess(attribute);
            }

            return this;
        }

        public EntityBuilder SetEntityAclComponentWriteAccess(string attribute)
        {
            acl.SetComponentWriteAccess(WellKnownComponents.EntityAcl.ComponentId, attribute);
            return this;
        }

        public Entity Build()
        {
            // Build and add the entity acl
            entity.Add(acl.Build());
            CheckRequiredComponents();
            return entity;
        }

        private void CheckRequiredComponents()
        {
            foreach (var requiredComponent in WellKnownComponents.RequiredComponents)
            {
                if (!componentsAdded.Contains(requiredComponent.ComponentId))
                {
                    throw new InvalidEntityException($"Entity is invalid. Missing required component: {requiredComponent.ComponentId}");
                }
            }
        }

        private class InvalidEntityException : Exception
        {
            public InvalidEntityException(string message) : base(message)
            {
            }
        }

        private struct WellKnownComponent
        {
            public uint ComponentId;
            public string Name;
        }

        private static class WellKnownComponents
        {
            public static readonly WellKnownComponent Position = new WellKnownComponent
            {
                ComponentId = 54,
                Name = "Position"
            };

            public static readonly WellKnownComponent EntityAcl = new WellKnownComponent
            {
                ComponentId = 50,
                Name = "EntityAcl"
            };

            public static readonly WellKnownComponent Persistence = new WellKnownComponent
            {
                ComponentId = 55,
                Name = "Persistence"
            };

            public static readonly WellKnownComponent Metadata = new WellKnownComponent
            {
                ComponentId = 53,
                Name = "Metadata"
            };

            public static readonly List<WellKnownComponent> RequiredComponents = new List<WellKnownComponent>{Position, EntityAcl};
        }

        private struct Acl
        {

            private Dictionary<uint, string> writePermissions;
            private List<string> readPermissions;

            public void SetComponentWriteAccess(uint componentId, string attribute)
            {
                writePermissions[componentId] = attribute;
            }

            public void AddReadAccess(string attribute)
            {
                readPermissions.Add(attribute);
            }

            public ComponentData Build()
            {
                var schemaComponentData = new SchemaComponentData(WellKnownComponents.EntityAcl.ComponentId);
                var fields = schemaComponentData.GetFields();

                // Write the read acl
                var workerRequirementSet = fields.AddObject(1);
                foreach (var attr in readPermissions)
                {
                    // Add another WorkerAttributeSet to the list
                    var set = workerRequirementSet.AddObject(1);
                    set.AddString(1, attr);
                }

                // Write the component write acl
                foreach (var writePermission in writePermissions)
                {
                    var keyValuePair = fields.AddObject(2);
                    keyValuePair.AddUint32(1, writePermission.Key);
                    var containedRequirementSet = keyValuePair.AddObject(2);
                    var containedAttributeSet = containedRequirementSet.AddObject(1);
                    containedAttributeSet.AddString(1, writePermission.Value);

                }

                return new ComponentData(schemaComponentData);
            }
        }
    }
}
