using System;
using System.Reflection;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    internal static class EntityManagerExtensions
    {
        // Reflection magic to get the internal method "SetComponentObject" on the specific EntityManager instance. This is required to add Components to Entities at runtime
        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        private static readonly Action<EntityManager, Entity, ComponentType, object> setComponentObjectAction =
            (Action<EntityManager, Entity, ComponentType, object>) Delegate.CreateDelegate(
                typeof(Action<EntityManager, Entity, ComponentType, object>), setComponentObjectMethodInfo);

        public static void SetComponentObject<T>(this EntityManager entityManager, Entity entity, T component)
        {
            setComponentObjectAction(entityManager, entity, typeof(T), component);
        }

        public static void SetComponentObject(this EntityManager entityManager, Entity entity,
            ComponentType componentType, object component)
        {
            setComponentObjectAction(entityManager, entity, componentType, component);
        }
    }
}
