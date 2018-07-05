using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ScriptableWorkerConfiguration : ScriptableObject
    {
        public static readonly string AssetPath = "Assets/Gdk/Core/Config/ScriptableWorkerConfiguration.asset";

        public List<WorkerConfiguration> WorkerConfigurations = new List<WorkerConfiguration>();

        public bool UseExternalIp = false;
    }
}
