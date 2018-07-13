using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(TransformSynchronizationGroup))]
    public class InterpolateTransformSystem : ComponentSystem
    {
        private const uint TargetTickOffset = 2;
        private const uint MaxBufferSize = 4;

        private TickSystem tickSystem;
        private long serverTickOffset;
        private bool tickOffsetSet;

        private Vector3 origin;

        public struct TransformData
        {
            public readonly int Length;
            public ComponentArray<BufferedTransform> BufferedTransform;
            public ComponentArray<Rigidbody> Rigidbody;
            public ComponentDataArray<NotAuthoritative<SpatialOSTransform>> transformAuthority;
        }

        [Inject] private TransformData transformData;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            origin = worker.Origin;

            tickSystem = World.GetOrCreateManager<TickSystem>();
        }

        /*
         * This system receives transform updates from the server and applies them on the client.
         * Updates are not applied immedately due to the network sending updates at an inconsistent rate
         * By keeping a buffer of updates, motion can still look smooth at the cost of being
         * slightly behind the true position of the object on the server.
         *
         * The strategy used is:
         * 1. Drop any updates that are too far in the past
         * 2. If the time the update is supposed to be applied matches the client tick time, apply it.
         * 3. If the next update is supposed to be applied in the future, interpolate the position for the
         * current tick using the last position and the next update.
         *
         * The Unity ECS Transform component contains the latest transfrom update and does not accurately
         * reflect the position of the object in the present. The rigid body transform is the true rendered
         * transform of the object.
         */
        protected override void OnUpdate()
        {
            for (var i = 0; i < transformData.Length; i++)
            {
                var transformQueue = transformData.BufferedTransform[i].TransformUpdates;
                if (transformQueue.Count == 0)
                {
                    continue;
                }

                if (!tickOffsetSet)
                {
                    serverTickOffset = (long) transformQueue[0].Tick - tickSystem.GlobalTick;
                    tickOffsetSet = true;
                }

                // Recieved too many updates. Drop to latest update and interpolate from there.
                if (transformQueue.Count >= MaxBufferSize)
                {
                    transformQueue.RemoveRange(0, transformQueue.Count - 1);
                    serverTickOffset = (long) transformQueue[0].Tick - tickSystem.GlobalTick;
                }

                var serverTickToApply = tickSystem.GlobalTick - TargetTickOffset + serverTickOffset;

                // Our time is too far ahead need to reset to server tick
                if (transformQueue[0].Tick < serverTickToApply)
                {
                    serverTickOffset = (long) transformQueue[0].Tick - tickSystem.GlobalTick;
                    serverTickToApply = tickSystem.GlobalTick - TargetTickOffset + serverTickOffset;
                }

                // Apply update if update tick matches local tick, otherwise interpolate
                var nextTransform = transformQueue[0];
                var rigidBody = transformData.Rigidbody[i];

                if (nextTransform.Tick == serverTickToApply)
                {
                    transformQueue.RemoveAt(0);

                    var newPosition = new Vector3(nextTransform.Location.X, nextTransform.Location.Y,
                        nextTransform.Location.Z);
                    var newRotation = new UnityEngine.Quaternion(nextTransform.Rotation.X, nextTransform.Rotation.Y,
                        nextTransform.Rotation.Z, nextTransform.Rotation.W);

                    rigidBody.MovePosition(newPosition + origin);
                    rigidBody.MoveRotation(newRotation);
                }
                else // Interpolate from current transform to next transform in the future.
                {
                    var t = (float) 1.0 / (nextTransform.Tick - (serverTickToApply - 1));

                    var currentLocation = transformData.Rigidbody[i].position - origin;
                    var currentRotation = transformData.Rigidbody[i].rotation;

                    var newPosition = new Vector3(nextTransform.Location.X, nextTransform.Location.Y,
                        nextTransform.Location.Z);
                    var newRotation = new UnityEngine.Quaternion(nextTransform.Rotation.X, nextTransform.Rotation.Y,
                        nextTransform.Rotation.Z, nextTransform.Rotation.W);

                    var interpolateLocation = Vector3.Lerp(currentLocation, newPosition, t);
                    var interpolateRotation = UnityEngine.Quaternion.Slerp(currentRotation, newRotation, t);

                    rigidBody.MovePosition(interpolateLocation + origin);
                    rigidBody.MoveRotation(interpolateRotation);
                }
            }
        }
    }
}
