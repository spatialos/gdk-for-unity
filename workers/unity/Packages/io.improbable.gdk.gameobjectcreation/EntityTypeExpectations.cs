using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Holds mappings from entity types to the set of components expected before creating their GameObjects.
    /// </summary>
    public class EntityTypeExpectations
    {
        private ComponentType[] defaultExpectation;

        private readonly Dictionary<string, ComponentType[]> entityExpectations
            = new Dictionary<string, ComponentType[]>();

        public void RegisterDefault(Type[] defaultComponentTypes = null)
        {
            defaultExpectation = defaultComponentTypes?
                .Select(type => new ComponentType(type, ComponentType.AccessMode.ReadOnly))
                .ToArray();
        }

        public void RegisterEntityType(string entityType, Type[] expectedComponentTypes = null)
        {
            var expectedTypes = expectedComponentTypes?
                .Select(type => new ComponentType(type, ComponentType.AccessMode.ReadOnly))
                .ToArray();
            entityExpectations.Add(entityType, expectedTypes);
        }

        internal ComponentType[] GetExpectedTypes(string entityType)
        {
            if (!entityExpectations.TryGetValue(entityType, out var types))
            {
                return defaultExpectation;
            }

            return types ?? new ComponentType[] { };
        }
    }
}
