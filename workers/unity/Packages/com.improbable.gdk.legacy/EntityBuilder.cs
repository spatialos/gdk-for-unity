using System;
using System.Collections.Generic;
using Improbable.Collections;
using Improbable.Worker;

// This file was taken from the Unity SDK and is temporarily being used
namespace Improbable.Gdk.Legacy
{
    public class EntityBuilder
    {
        private readonly Entity entity;
        private Acl entityAcl;
        private bool hasBuiltOnce;

        public static EntityBuilder Begin()
        {
            return new EntityBuilder();
        }

        protected EntityBuilder()
        {
            entity = new Entity();
        }

        public EntityBuilder AddPositionComponent(Coordinates position, WorkerRequirementSet writeAcl)
        {
            entity.Add(new Position.Data(position));
            entityAcl.SetWriteAccess<Position>(writeAcl);
            return this;
        }

        public EntityBuilder SetPersistence(bool persistence)
        {
            if (persistence)
            {
                entity.Add(new Persistence.Data());
            }

            return this;
        }

        public EntityBuilder SetReadAcl(WorkerRequirementSet readAcl)
        {
            entityAcl.SetReadAccess(readAcl);
            return this;
        }

        public EntityBuilder SetEntityAclComponentWriteAccess(WorkerRequirementSet writeAcl)
        {
            entityAcl.SetWriteAccess<EntityAcl>(writeAcl);
            return this;
        }

        public EntityBuilder AddComponent<T>(IComponentData<T> data, WorkerRequirementSet writeAcl)
            where T : IComponentMetaclass
        {
            entity.Add(data);
            entityAcl.SetWriteAccess<T>(writeAcl);
            return this;
        }

        public Entity Build()
        {
            if (hasBuiltOnce)
            {
                throw new InvalidOperationException("Cannot call Build() multiple times on an EntityBuilder");
            }

            hasBuiltOnce = true;
            entity.Add(entityAcl.ToData());
            return entity;
        }
    }

    public struct Acl
    {
        private static readonly HashSet<uint> allComponentIds = Dynamic.GetComponentIds();

        private Map<uint, WorkerRequirementSet> writePermissions;
        private WorkerRequirementSet readPermissions;

        public static Acl Build()
        {
            return new Acl();
        }

        public Acl SetWriteAccess<TComponent>(WorkerRequirementSet requirementSet)
            where TComponent : IComponentMetaclass
        {
            EnsureWritePermissionsAllocated();

            var id = Dynamic.GetComponentId<TComponent>();
            writePermissions[id] = requirementSet;
            return this;
        }

        public Acl SetWriteAccess(uint componentId, WorkerRequirementSet requirementSet)
        {
            if (!allComponentIds.Contains(componentId))
            {
                throw new InvalidOperationException(string.Format("{0} is an unknown component id", componentId));
            }

            EnsureWritePermissionsAllocated();
            writePermissions[componentId] = requirementSet;

            return this;
        }

        public Acl SetReadAccess(WorkerRequirementSet requirementSet)
        {
            readPermissions = requirementSet;
            return this;
        }

        public EntityAcl.Data ToData()
        {
            return new EntityAcl.Data(new EntityAclData(readPermissions,
                writePermissions == null
                    ? new Map<uint, WorkerRequirementSet>()
                    : writePermissions));
        }

        public EntityAcl.Update ToUpdate()
        {
            return new EntityAcl.Update().SetReadAcl(readPermissions).SetComponentWriteAcl(
                writePermissions == null
                    ? new Map<uint,
                        WorkerRequirementSet
                    >()
                    : writePermissions);
        }

        public static Acl MergeIntoAcl(EntityAclData otherAcl, Acl newAcl)
        {
            var mergedAcl = new Acl();

            mergedAcl.readPermissions = otherAcl.readAcl;
            if (newAcl.readPermissions.attributeSet != null)
            {
                mergedAcl.readPermissions = newAcl.readPermissions;
            }

            var mergedWritePermissions = otherAcl.componentWriteAcl;
            if (newAcl.writePermissions != null)
            {
                foreach (var key in newAcl.writePermissions.Keys)
                {
                    mergedWritePermissions[key] = newAcl.writePermissions[key];
                }
            }

            mergedAcl.writePermissions = mergedWritePermissions;

            return mergedAcl;
        }

        public static WorkerAttributeSet MakeAttributeSet(string attribute1, params string[] attributes)
        {
            var list = new Improbable.Collections.List<string>(attributes.Length + 1);
            foreach (var attribute in Enumerate(attribute1, attributes))
            {
                list.Add(attribute);
            }

            return new WorkerAttributeSet(list);
        }

        public static WorkerRequirementSet MakeRequirementSet(WorkerAttributeSet attribute1,
            params WorkerAttributeSet[] attributes)
        {
            var list = new Improbable.Collections.List<WorkerAttributeSet>(attributes.Length + 1);
            foreach (var attribute in Enumerate(attribute1, attributes))
            {
                list.Add(attribute);
            }

            return new WorkerRequirementSet(list);
        }

        private static IEnumerable<T> Enumerate<T>(T element1, IEnumerable<T> elements)
        {
            yield return element1;

            using (var enumerator = elements.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        private void EnsureWritePermissionsAllocated()
        {
            writePermissions = writePermissions ?? new Map<uint, WorkerRequirementSet>();
        }
    }
}
