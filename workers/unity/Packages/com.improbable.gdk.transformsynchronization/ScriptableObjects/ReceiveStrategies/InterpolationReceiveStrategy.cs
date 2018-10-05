using System;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [CreateAssetMenu(menuName = "SpatialOS/Transforms/Receive Strategies/Interpolation")]
    public class InterpolationReceiveStrategy : TransformSynchronizationReceiveStrategy
    {
        public int TargetBufferSize = TransformDefaults.InterpolationTargetBufferSize;
        public int MaxBufferSize = TransformDefaults.InterpolationMaxBufferSize;

        internal override void Apply(Entity entity, World world, EntityCommandBuffer commandBuffer)
        {
            if (MaxBufferSize < TargetBufferSize)
            {
                throw new InvalidOperationException(
                    $"In {name} the Max Buffer Size must be larger than the Target Buffer Size");
            }

            commandBuffer.AddSharedComponent(entity, new InterpolationConfig
            {
                TargetBufferSize = TargetBufferSize,
                MaxLoadMatchedBufferSize = MaxBufferSize
            });
        }
    }
}
