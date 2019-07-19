using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourInspector : UnityEditor.Editor
    {
        private MonoBehaviour script;
        private Type scriptType;
        private bool isSpatialBehaviour;

        private Dictionary<FieldInfo, ISubscription> subscriptions;

        private string requiredWorkerTypesLabel;
        private string workerType;
        private bool? isWorkerType;

        private static GUIStyle availableStyle;
        private static GUIStyle unavailableStyle;

        private bool foldout;

        private void OnEnable()
        {
            // Create GUIStyles
            availableStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } };
            unavailableStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = Color.red } };

            // Get type info
            script = (MonoBehaviour) target;
            scriptType = script.GetType();
            isSpatialBehaviour = HasWorkerTypeAttribute(scriptType) || HasRequireAttributes(scriptType);
            var requiredWorkerTypes = GetRequiredWorkerTypes(scriptType);

            subscriptions = new Dictionary<FieldInfo, ISubscription>();
            var requiredFields = scriptType
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(field => Attribute.IsDefined(field, typeof(RequireAttribute), false));

            // Get subscription info when playing
            if (Application.isPlaying)
            {
                // Get the LinkedEntityComponent to make sure we only run on live GO's and get the world we're in.
                var linkedEntityComponent = script.GetComponent<LinkedEntityComponent>();
                if (linkedEntityComponent == null)
                {
                    return;
                }

                workerType = linkedEntityComponent.Worker.WorkerType;
                isWorkerType = requiredWorkerTypes?.Contains(workerType);
                if (isWorkerType.HasValue)
                {
                    requiredWorkerTypesLabel = string.Join(" || ", requiredWorkerTypes);
                }

                var subscriptionSystem = linkedEntityComponent.World.GetExistingSystem<SubscriptionSystem>();

                foreach (var fieldInfo in requiredFields)
                {
                    var subscription = subscriptionSystem.Subscribe(linkedEntityComponent.EntityId, fieldInfo.FieldType);
                    subscriptions.Add(fieldInfo, subscription);
                }
            }
            else
            {
                if (requiredWorkerTypes != null)
                {
                    requiredWorkerTypesLabel = string.Join(" || ", requiredWorkerTypes);
                }

                foreach (var fieldInfo in requiredFields)
                {
                    subscriptions.Add(fieldInfo, null);
                }
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying && subscriptions != null)
            {
                foreach (var pair in subscriptions)
                {
                    var subscription = pair.Value;
                    subscription?.Cancel();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (isSpatialBehaviour)
            {
                foldout = EditorGUILayout.Foldout(foldout, "SpatialOS", true);
                if (foldout)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        if (Application.isPlaying)
                        {
                            DrawLiveInspector();
                        }
                        else
                        {
                            DrawEditorInspector();
                        }
                    }
                }
            }

            DrawDefaultInspector();
        }

        private void DrawEditorInspector()
        {
            if (!string.IsNullOrEmpty(requiredWorkerTypesLabel))
            {
                EditorGUILayout.LabelField(requiredWorkerTypesLabel);
            }

            if (subscriptions != null)
            {
                foreach (var pair in subscriptions)
                {
                    var field = pair.Key;
                    EditorGUILayout.LabelField(field.FieldType.Name);
                }
            }
        }

        private void DrawLiveInspector()
        {
            if (isWorkerType.HasValue)
            {
                if (isWorkerType.Value)
                {
                    EditorGUILayout.LabelField(workerType, availableStyle);
                }
                else
                {
                    EditorGUILayout.LabelField(requiredWorkerTypesLabel, unavailableStyle);
                }
            }

            if (subscriptions != null)
            {
                foreach (var pair in subscriptions)
                {
                    var field = pair.Key;
                    var subscription = pair.Value;
                    EditorGUILayout.LabelField(field.FieldType.Name,
                        subscription.HasValue ? availableStyle : unavailableStyle);
                }
            }
        }

        private static string[] GetRequiredWorkerTypes(Type targetType)
        {
            return targetType.GetCustomAttribute<WorkerTypeAttribute>()?.WorkerTypes;
        }

        private static bool HasWorkerTypeAttribute(Type targetType)
        {
            return GetRequiredWorkerTypes(targetType) != null;
        }

        private static bool HasRequireAttributes(Type targetType)
        {
            return targetType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Any(field => Attribute.IsDefined(field, typeof(RequireAttribute), false));
        }
    }
}
