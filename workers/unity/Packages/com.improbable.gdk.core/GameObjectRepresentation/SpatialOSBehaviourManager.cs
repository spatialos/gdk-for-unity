using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core.MonoBehaviours
{

    /// <summary>
    ///     Invokes dispatcher callbacks on GameObjects that represent entities.
    /// </summary>
    internal class SpatialOSBehaviourManager
    {
        public GameObject GameObject;
        private Dictionary<uint, List<MonoBehaviour>> componentIdToReaderSpatialOSBehaviours;
        private Dictionary<uint, List<MonoBehaviour>> componentIdToWriterSpatialOSBehaviours;
        private Dictionary<MonoBehaviour, int> numUnsatisfiedComponents;
        private List<MonoBehaviour> spatialOSBehavioursToEnable;
        private List<MonoBehaviour> spatialOSBehavioursToDisable;
        private Dictionary<uint, Dictionary<MonoBehaviour, IReaderInternal>> readersWriters;

        public SpatialOSBehaviourManager(GameObject gameObject)
        {
            // Setup caches
        }

        public List<IReaderInternal> GetReadersWriters(uint componentId)
        {
            return null;
        }

        public void EnableSpatialOSBehaviours()
        {
            // inject + enable all spatialOSBehavioursToEnable
        }

        public void DisableSpatialOSBehaviours()
        {
            // disable + deinject all spatialOSBehavioursToDisable
        }

        public void AddComponent(uint componentId)
        {
            // Mark readers as to be enabled
        }

        public void RemoveComponent(uint componentId)
        {
            // Mark readers as to be disabled
        }

        public void ChangeAuthority(uint componentId, Authority authority)
        {
            // Mark writers as to be enabled/disabled
        }
    }
}
