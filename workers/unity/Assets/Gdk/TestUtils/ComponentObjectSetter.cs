using System;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TestUtils
{
    public static class ComponentObjectSetter
    {
        // Reflection magic to get the internal method "SetComponentObject" on the specific EntityManager instance. This is required to add Components to Entities at runtime
        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        public static void AddAndSetComponentObject<T>(Entity entity, T component, EntityManager entityManager)
            where T : Component
        {
            if (!entityManager.HasComponent<T>(entity))
            {
                entityManager.AddComponent(entity, typeof(T));
            }

            setComponentObjectMethodInfo.Invoke(entityManager,
                new object[] { entity, (ComponentType) typeof(T), component });
        }
    }
}
