using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Core.Editor
{
    /// <summary>
    ///     Base object for a singleton scriptable object.
    /// </summary>
    /// <remarks>
    ///     This differs from Unity's <see cref="ScriptableSingleton{T}"/> in that: null is returned if no instance of
    ///     <see cref="TSelf"/> is found. In Unity's implementation, an instance of <see cref="TSelf"/> will be created
    ///     for you.
    /// </remarks>
    /// <typeparam name="TSelf">The type to make a singleton scriptable object.</typeparam>
    public abstract class SingletonScriptableObject<TSelf> : ScriptableObject
        where TSelf : SingletonScriptableObject<TSelf>
    {
        private static readonly List<TSelf> Instances =
            new List<TSelf>();

        public virtual void OnEnable()
        {
            if (!IsAnAsset())
            {
                // This is not an asset, so don't register it as an instance.
                return;
            }

            var self = (TSelf) this;

            if (Instances.Find(instance => instance != self))
            {
                Debug.LogErrorFormat("There are multiple copies of {0} present. Please pick one and delete the other.",
                    SelfType);
            }

            if (!Instances.Contains(self))
            {
                Instances.Add(self);
            }
        }

        protected bool IsAnAsset()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);

            // If there is an asset path, it is in assets.
            return !string.IsNullOrEmpty(assetPath);
        }

        public void OnDisable()
        {
            if (!IsAnAsset())
            {
                return;
            }

            var self = (TSelf) this;

            if (Instances.Contains(self))
            {
                Instances.Remove(self);
            }
        }

        private static readonly Type SelfType = typeof(TSelf);

        /// <summary>
        ///     Finds the instance of <see cref="TSelf"/>
        /// </summary>
        /// <remarks>
        ///     An error will be logged if more than one instance is found. If more than one instance is found, only
        ///     the first is returned.
        /// </remarks>
        /// <returns>The instance of <see cref="TSelf"/> if one exists, null otherwise.</returns>
        public static TSelf GetInstance()
        {
            // Clean up dead ones.
            Instances.RemoveAll(item => item == null);

            if (Instances.Count > 0)
            {
                return Instances[0];
            }

            if (SingletonScriptableObjectLoader.LoadingInstances.Contains(SelfType))
            {
                return null;
            }

            SingletonScriptableObjectLoader.LoadingInstances.Add(SelfType);

            try
            {
                var allInstanceGuidsInAssetDatabase =
                    AssetDatabase.FindAssets("t:" + SelfType.Name);

                foreach (var instanceGUID in allInstanceGuidsInAssetDatabase)
                {
                    var instancePath = AssetDatabase.GUIDToAssetPath(instanceGUID);

                    var loadedInstance = AssetDatabase.LoadAssetAtPath<TSelf>(instancePath);

                    // onload should have been called here, but if not, ensure it's in the list.
                    if (loadedInstance == null)
                    {
                        continue;
                    }

                    if (Instances.Find(instance => instance != loadedInstance))
                    {
                        Debug.LogErrorFormat(
                            "There are multiple copies of {0} present. Please pick one and delete the other.",
                            SelfType);
                    }

                    if (!Instances.Contains(loadedInstance))
                    {
                        Instances.Add(loadedInstance);
                    }
                }
            }
            finally
            {
                SingletonScriptableObjectLoader.LoadingInstances.Remove(SelfType);
            }

            return Instances.FirstOrDefault();
        }
    }
}
