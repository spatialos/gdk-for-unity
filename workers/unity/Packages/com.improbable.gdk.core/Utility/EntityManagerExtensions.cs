using System;
using System.Reflection;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
#if UNITY_EDITOR
    // Attempt to validate the method as soon as possible when in the editor.
    [UnityEditor.InitializeOnLoad]
#endif
    public static class EntityManagerExtensions
    {
        private delegate void SetComponentObjectDelegate(EntityManager entityManager,
            Entity entity, ComponentType componentType, object componentObject);

        private static readonly SetComponentObjectDelegate SetComponentObjectAction;

        static EntityManagerExtensions()
        {
            var setComponentObjectMethodInfo =
                typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                    new ParameterModifier[] { });

            if (setComponentObjectMethodInfo == null)
            {
                throw new MissingMethodException(
                    "Could not find the EntityManager.SetComponentObject(Entity, ComponentType, object) method.\n" +
                    "Please ensure you are using a compatible version of the Unity.Entities package.");
            }

            SetComponentObjectAction =
                (SetComponentObjectDelegate) Delegate.CreateDelegate(typeof(SetComponentObjectDelegate),
                    setComponentObjectMethodInfo);
        }

        public static void SetComponentObject<T>(this EntityManager entityManager, Entity entity, T component)
        {
            if (!entityManager.HasComponent<T>(entity))
            {
                entityManager.AddComponent(entity, typeof(T));
            }
            SetComponentObjectAction(entityManager, entity, ComponentType.Create<T>(), component);
        }

        public static void SetComponentObject(this EntityManager entityManager, Entity entity,
            ComponentType componentType, object component)
        {
            if (!entityManager.HasComponent(entity, componentType))
            {
                entityManager.AddComponent(entity, componentType);
            }

            SetComponentObjectAction(entityManager, entity, componentType, component);
        }
    }
}
