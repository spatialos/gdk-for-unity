using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    internal static class ReflectionUtility
    {
        /// <summary>
        ///     Get all non-abstract types that implement a given interface and have the specified attributes present.
        /// </summary>
        /// <param name="interfaceType">The interface that should be implemented by each type.</param>
        /// <param name="attributes">An array of attributes that must be present on each type.</param>
        public static List<Type> GetNonAbstractTypes(Type interfaceType, params Type[] attributes)
        {
            List<Type> matchingTypes = new List<Type>();

            // If there are no known types then search all loaded assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (TypeMatchesRequirements(type, interfaceType, attributes))
                        {
                            matchingTypes.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    // If the assembly contains types that couldn't be loaded print an error but still move forward with all types it could load
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Could not load all types from assembly {assembly.FullName}");
                    for (int i = 0; i < e.Types.Length; ++i)
                    {
                        // If type is loaded and matches then add it to the list
                        if (e.Types[i] != null && TypeMatchesRequirements(e.Types[i], interfaceType, attributes))
                        {
                            matchingTypes.Add(e.Types[i]);
                        }
                        else
                        {
                            sb.AppendLine($"Type \"{e.Types[i]}\" failed to load with error \"{e.LoaderExceptions[i].Message}\"");
                        }
                    }

                    Debug.LogError(sb.ToString());
                }
            }

            return matchingTypes;
        }

        private static bool TypeMatchesRequirements(Type targetType, Type interfaceType, Type[] attributes)
        {
            if (targetType.IsAbstract || !interfaceType.IsAssignableFrom(targetType))
            {
                return false;
            }

            // Check all specified attributes are present
            foreach (var attribute in attributes)
            {
                if (targetType.GetCustomAttribute(attribute, true) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
