using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public abstract class TransformSynchronizationReceiveStrategy : ScriptableObject
    {
        public string WorkerType;

        internal abstract void Apply(Entity entity, World world, EntityCommandBuffer commandBuffer);
    }
}
