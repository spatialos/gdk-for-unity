using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Subscriptions;
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Improbable.Gdk.Debug
{
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    public class MonoBehaviourInspector :
#if ODIN_INSPECTOR
        OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        private MonoBehaviour script;
        private LinkedEntityComponent linkedEntityComponent;
        private Type scriptType;
        private bool isSpatialBehaviour;

        private Dictionary<FieldInfo, ISubscription> subscriptions;

        private string requiredWorkerTypesLabel;
        private string workerType;
        private bool? isWorkerType;

        private bool foldout;

        private GUIContent enabledContent;
        private GUIContent disabledContent;

#if ODIN_INSPECTOR
        protected override void OnEnable()
#else
        protected void OnEnable()
#endif
        {
            // Fetch styles
            enabledContent = EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo");
            disabledContent = EditorGUIUtility.IconContent("sv_icon_dot0_pix16_gizmo");

            // Get type info
            script = (MonoBehaviour) target;
            scriptType = script.GetType();
            isSpatialBehaviour = HasWorkerTypeAttribute(scriptType) ||
                RequiredSubscriptionsDatabase.HasRequiredSubscriptions(scriptType);
            if (!isSpatialBehaviour)
            {
                return;
            }

            linkedEntityComponent = script.GetComponent<LinkedEntityComponent>();
            var requiredWorkerTypes = GetRequiredWorkerTypes(scriptType);

            subscriptions = new Dictionary<FieldInfo, ISubscription>();
            var subscriptionsInfo = RequiredSubscriptionsDatabase.GetOrCreateRequiredSubscriptionsInfo(scriptType);

            // Get subscription info when playing
            if (Application.isPlaying && linkedEntityComponent != null)
            {
                workerType = linkedEntityComponent.Worker.WorkerType;
                isWorkerType = requiredWorkerTypes?.Contains(workerType);
                if (isWorkerType.HasValue)
                {
                    requiredWorkerTypesLabel = string.Join(" || ", requiredWorkerTypes);
                }

                var subscriptionSystem = linkedEntityComponent.World.GetExistingSystem<SubscriptionSystem>();

                foreach (var fieldInfo in subscriptionsInfo.RequiredFields)
                {
                    var subscription =
                        subscriptionSystem.Subscribe(linkedEntityComponent.EntityId, fieldInfo.FieldType);
                    subscriptions.Add(fieldInfo, subscription);
                }
            }
            else
            {
                if (requiredWorkerTypes != null)
                {
                    requiredWorkerTypesLabel = string.Join(" || ", requiredWorkerTypes);
                }

                foreach (var fieldInfo in subscriptionsInfo.RequiredFields)
                {
                    subscriptions.Add(fieldInfo, null);
                }
            }
        }

#if ODIN_INSPECTOR
        protected override void OnDisable()
#else
        protected void OnDisable()
#endif
        {
            if (subscriptions != null)
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
                        if (Application.isPlaying && linkedEntityComponent != null)
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

            base.OnInspectorGUI();
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
                var guiContent = isWorkerType.Value ? enabledContent : disabledContent;
                guiContent.text = isWorkerType.Value ? workerType : requiredWorkerTypesLabel;
                EditorGUILayout.LabelField(guiContent);
            }

            if (subscriptions != null)
            {
                foreach (var pair in subscriptions)
                {
                    var field = pair.Key;
                    var subscription = pair.Value;
                    var style = subscription.HasValue ? enabledContent : disabledContent;
                    style.text = field.FieldType.Name;
                    EditorGUILayout.LabelField(style);
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
    }
}
