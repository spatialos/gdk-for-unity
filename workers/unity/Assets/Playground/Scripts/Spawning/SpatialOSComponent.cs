using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class SpatialOSComponent : MonoBehaviour
    {
        public long SpatialEntityId;
        public Entity Entity;
        public World World;
    }
}
