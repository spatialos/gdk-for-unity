using System;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Subscriptions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class CustomBehaviourInspector : Editor
{
    private MonoBehaviour script;
    private Type scriptType;
    private bool isSpatialBehaviour;

    private void OnEnable()
    {
        script = target as MonoBehaviour;
        scriptType = script.GetType();
        isSpatialBehaviour = HasWorkerTypeAttribute(scriptType) || HasRequireAttributes(scriptType);
    }

    public override void OnInspectorGUI()
    {
        if (isSpatialBehaviour)
        {
            EditorGUILayout.LabelField("Spatial MonoBehaviour", EditorStyles.boldLabel);
        }

        DrawDefaultInspector();
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
