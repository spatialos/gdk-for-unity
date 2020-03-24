using System;
using System.Collections.Generic;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Holds mappings from entity types to the set of components expected before creating their GameObjects.
    /// </summary>
    public class EntityTypeExpectations
    {
        private Type[] defaultExpectation;

        private readonly Dictionary<string, Type[]> entityExpectations
            = new Dictionary<string, Type[]>();

        public void RegisterDefault(Type[] defaultComponentTypes)
        {
            defaultExpectation = defaultComponentTypes;
        }

        public void RegisterEntityType(string entityType, Type[] expectedComponentTypes)
        {
            entityExpectations.Add(entityType, expectedComponentTypes ?? new Type[] { });
        }

        internal Type[] GetExpectedTypes(string entityType)
        {
            if (!entityExpectations.TryGetValue(entityType, out var types))
            {
                return defaultExpectation;
            }

            return types;
        }
    }
}
