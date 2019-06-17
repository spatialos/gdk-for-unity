using System.Collections.Generic;
using Improbable.Gdk.Core.Editor;
using Improbable.Gdk.Tools;
using UnityEngine;

namespace Improbable.Gdk.DeploymentLauncher
{
    [CreateAssetMenu(fileName = "SpatialOS Deployment Launcher Config", menuName = EditorConfig.ParentMenu + "/Deployment Launcher Config")]
    internal class DeploymentLauncherConfig : SingletonScriptableObject<DeploymentLauncherConfig>
    {
        [SerializeField] public AssemblyConfig AssemblyConfig = new AssemblyConfig();
        [SerializeField] public List<DeploymentConfig> DeploymentConfigs = new List<DeploymentConfig>();

        public void SetProjectName(string projectName)
        {
            AssemblyConfig.ProjectName = projectName;

            foreach (var config in DeploymentConfigs)
            {
                config.ProjectName = projectName;
            }
        }
    }
}
