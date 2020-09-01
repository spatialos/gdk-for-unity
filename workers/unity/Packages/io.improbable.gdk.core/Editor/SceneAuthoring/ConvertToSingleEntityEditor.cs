using Improbable.Gdk.Core.SceneAuthoring;
using UnityEditor;

namespace Improbable.Gdk.Core.Editor.SceneAuthoring
{
    [CustomEditor(typeof(ConvertToSingleEntity))]
    public class ConvertToSingleEntityEditor : UnityEditor.Editor
    {
        private SerializedProperty useSpecificEntityIdProperty;
        private SerializedProperty desiredEntityIdProperty;
        private SerializedProperty readAclProperty;

        private void OnEnable()
        {
            useSpecificEntityIdProperty = serializedObject.FindProperty(nameof(ConvertToSingleEntity.UseSpecificEntityId));
            desiredEntityIdProperty = serializedObject.FindProperty(nameof(ConvertToSingleEntity.DesiredEntityId));
            readAclProperty = serializedObject.FindProperty(nameof(ConvertToSingleEntity.ReadAcl));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(useSpecificEntityIdProperty);

            if (useSpecificEntityIdProperty.boolValue)
            {
                EditorGUILayout.PropertyField(desiredEntityIdProperty);
            }

            EditorGUILayout.PropertyField(readAclProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
