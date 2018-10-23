using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectRepresentation.Editor
{
    /// <summary>
    ///     A prefab processor which disables any Monobehaviours on top-level GameObjects in the Resources folder
    ///     which have fields with the <see cref="RequireAttribute"/> on them.
    /// </summary>
    /// <remarks>
    ///     This processes the prefabs when you build or when you press play in the Unity Editor.
    /// </remarks>
    [InitializeOnLoad]
    public class PrefabPreprocessor : IPreprocessBuildWithReport
    {
        // Needed for IPreprocessBuildWithReport
        public int callbackOrder => 0;

        static PrefabPreprocessor()
        {
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            PreprocessPrefabs();
        }

        private static void OnPlaymodeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
            {
                PreprocessPrefabs();
            }
        }

        private static void PreprocessPrefabs()
        {
            var assetsUpdated = false;

            var allPrefabObjectsInResources = AssetDatabase.FindAssets("t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(assetPath => assetPath.Contains("Resources"))
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(prefabObject => prefabObject != null)
                .ToArray();

            foreach (var prefabObject in allPrefabObjectsInResources)
            {
                var prefabWasFixed = false;

                foreach (var monoBehaviour in prefabObject
                    .GetComponents<MonoBehaviour>()
                    .Where(DoesBehaviourNeedFixing))
                {
                    assetsUpdated = true;
                    prefabWasFixed = true;

                    Undo.RecordObject(monoBehaviour, "Disable MonoBehaviours with [Require] fields on prefabs.");

                    monoBehaviour.enabled = false;
                }

                if (prefabWasFixed)
                {
                    EditorUtility.SetDirty(prefabObject);
                }
            }

            if (assetsUpdated)
            {
                AssetDatabase.SaveAssets();
            }
        }

        private static bool IsBehaviourEnabledInEditor(Object obj)
        {
            return !ReferenceEquals(obj, null)
                   && EditorUtility.GetObjectEnabled(obj) == 1;
        }

        private static bool DoesBehaviourNeedConditionalEnabling(Type targetType)
        {
            return
                targetType.GetCustomAttribute<WorkerTypeAttribute>() != null ||
                targetType
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Any(field => Attribute.IsDefined(field, typeof(RequireAttribute), false));
        }

        private static bool DoesBehaviourNeedFixing(MonoBehaviour monoBehaviour)
        {
            return IsBehaviourEnabledInEditor(monoBehaviour) &&
                   DoesBehaviourNeedConditionalEnabling(monoBehaviour.GetType());
        }
    }
}
