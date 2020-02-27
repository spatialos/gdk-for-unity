using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public abstract class TransformSynchronizationSendStrategy : ScriptableObject
    {
        public string WorkerType;

        internal abstract void Apply(Entity entity, World world, EntityCommandBuffer commandBuffer);

        internal abstract void Remove(Entity entity, World world, EntityCommandBuffer commandBuffer);
    }
}
