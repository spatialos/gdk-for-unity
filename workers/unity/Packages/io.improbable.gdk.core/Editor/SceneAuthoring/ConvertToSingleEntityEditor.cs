using Improbable.Gdk.Core.SceneAuthoring;
using UnityEditor;

namespace Improbable.Gdk.Core.Editor.SceneAuthoring
{
    [CustomEditor(typeof(ConvertToSingleEntity))]
    public class ConvertToSingleEntityEditor : UnityEditor.Editor
    {
        private SerializedProperty useSpecificEntityIdProperty;
        private SerializedProperty desiredEntityIdProperty;

        private void OnEnable()
        {
            useSpecificEntityIdProperty = serializedObject.FindProperty(nameof(ConvertToSingleEntity.UseSpecificEntityId));
            desiredEntityIdProperty = serializedObject.FindProperty(nameof(ConvertToSingleEntity.DesiredEntityId));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(useSpecificEntityIdProperty);

            if (useSpecificEntityIdProperty.boolValue)
            {
                EditorGUILayout.PropertyField(desiredEntityIdProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
