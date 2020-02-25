using UnityEditor;
using UnityEngine;

namespace Improbable.Worker.CInterop
{
    internal class DeploymentLauncherWindowStyle
    {
        private const string BuiltInErrorIcon = "console.erroricon.sml";
        private const string BuiltInRefreshIcon = "Refresh";
        private const string BuiltInWebIcon = "BuildSettings.Web.Small";
        private const string BuiltInEditIcon = "editicon.sml";

        public readonly Vector2 SmallIconSize = new Vector2(x: 12, y: 12);
        public readonly Color HorizontalLineColor = new Color(r: 0.3f, g: 0.3f, b: 0.3f, a: 1);

        public readonly GUIContent ProjectRefreshButtonContents;
        public readonly GUIContent EditRuntimeVersionButtonContents;
        public readonly GUIContent DeploymentConfigurationErrorContents;
        public readonly GUIContent OpenDeploymentButtonContents;
        public readonly GUIContent RemoveDeploymentConfigurationButtonContents;
        public readonly GUIContent RemoveSimPlayerDeploymentButtonContents;

        private Material spinnerMaterial;

        public DeploymentLauncherWindowStyle()
        {
            ProjectRefreshButtonContents = new GUIContent(EditorGUIUtility.IconContent(BuiltInRefreshIcon))
            {
                tooltip = "Refresh your project name."
            };

            EditRuntimeVersionButtonContents = new GUIContent(EditorGUIUtility.IconContent(BuiltInEditIcon))
            {
                tooltip = "Edit the Runtime version."
            };

            DeploymentConfigurationErrorContents = new GUIContent(EditorGUIUtility.IconContent(BuiltInErrorIcon))
            {
                tooltip = "One or more errors in deployment configuration."
            };

            OpenDeploymentButtonContents = new GUIContent(EditorGUIUtility.IconContent(BuiltInWebIcon))
            {
                tooltip = "Open this deployment in your browser."
            };

            RemoveDeploymentConfigurationButtonContents = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
            {
                tooltip = "Remove deployment configuration"
            };

            RemoveSimPlayerDeploymentButtonContents = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
            {
                tooltip = "Remove simulated player deployment"
            };

            spinnerMaterial = new Material(Shader.Find("UI/Default"));
        }

        public void DrawSpinner(float value, Rect rect)
        {
            // There are 11 frames in the spinner animation, 0 till 11.
            var imageId = Mathf.RoundToInt(value) % 12;
            var icon = EditorGUIUtility.IconContent($"d_WaitSpin{imageId:D2}");
            EditorGUI.DrawPreviewTexture(rect, icon.image, spinnerMaterial, ScaleMode.ScaleToFit, imageAspect: 1);
        }
    }
}
