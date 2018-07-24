using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ReaderWriterImpl
    {
        // This is placeholder code.
    }

    /// <summary>
    ///     Invokes dispatcher callbacks on GameObjects that represent entities.
    /// </summary>
    public class SpatialOSBehaviourManager
    {
        public GameObject GameObject;
        private Dictionary<uint, List<MonoBehaviour>> componentIdToReaderSpatialOSBehaviours;
        private Dictionary<uint, List<MonoBehaviour>> componentIdToWriterSpatialOSBehaviours;
        private Dictionary<MonoBehaviour, int> numUnsatisfiedComponents;
        private List<MonoBehaviour> spatialOsBehavioursToEnable;
        private List<MonoBehaviour> spatialOsBehavioursToDisable;
        private Dictionary<uint, Dictionary<MonoBehaviour, ReaderWriterImpl>> readersWriters;

        public SpatialOSBehaviourManager(GameObject gameObject)
        {
            // Setup caches
        }

        public List<ReaderWriterImpl> GetReadersWriters(uint componentId)
        {
            return null;
        }

        public void Tick()
        {
            // inject + enable all spatialOsBehavioursToEnable
            // disable + deinject all spatialOsBehavioursToDisable
        }

        public void OnAddComponent(uint componentId)
        {
            // Mark readers as to be enabled
        }

        public void OnRemoveComponent(uint componentId)
        {
            // Mark readers as to be disabled
        }

        public void OnAuthorityChange(uint componentId, Authority authority)
        {
            // Mark writers as to be enabled/disabled
        }
    }
}
