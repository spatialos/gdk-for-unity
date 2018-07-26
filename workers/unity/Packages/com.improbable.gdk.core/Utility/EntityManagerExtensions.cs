using System;
using System.Diagnostics;
using System.Reflection;
using Unity.Entities;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Improbable.Gdk.Core
{
#if UNITY_EDITOR
    // Attempt to validate the method as soon as possible when in the editor.
    [InitializeOnLoad]
#endif
    internal static class EntityManagerExtensions
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

            ValidateParameters(setComponentObjectMethodInfo);

            SetComponentObjectAction =
                (SetComponentObjectDelegate) Delegate.CreateDelegate(typeof(SetComponentObjectDelegate),
                    setComponentObjectMethodInfo);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        private static void ValidateParameters(MethodInfo setComponentObjectMethodInfo)
        {
            const string errorMessage =
                "Could not find the EntityManager.SetComponentObject(Entity, ComponentType, object) method.\n" +
                "Please ensure you are using a compatible version of the Unity.Entities package.";

            if (setComponentObjectMethodInfo == null)
            {
                throw new MissingMethodException(errorMessage);
            }

            var parameters = setComponentObjectMethodInfo.GetParameters();

            if (parameters.Length == 3 &&
                parameters[0].ParameterType == typeof(Entity) &&
                parameters[1].ParameterType == typeof(ComponentType) &&
                parameters[2].ParameterType == typeof(object))
            {
                // All good!
                return;
            }

            throw new MissingMethodException(errorMessage);
        }

        public static void SetComponentObject<T>(this EntityManager entityManager, Entity entity, T component)
        {
            SetComponentObjectAction(entityManager, entity, typeof(T), component);
        }

        public static void SetComponentObject(this EntityManager entityManager, Entity entity,
            ComponentType componentType, object component)
        {
            SetComponentObjectAction(entityManager, entity, componentType, component);
        }
    }
}
