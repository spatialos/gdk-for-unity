using Generated.Improbable;
using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Quaternion = Generated.Improbable.Transform.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(TransformSynchronizationGroup))]
    public class LocalTransformSyncSystem : ComponentSystem
    {
        private struct TransformData
        {
            public readonly int Length;
            public ComponentDataArray<Transform.Component> Transform;
            public ComponentDataArray<Position.Component> Position;
            [ReadOnly] public ComponentArray<Rigidbody> GameObjectRigidBody;

            [ReadOnly] public ComponentDataArray<Authoritative<Transform.Component>> TransformAuthority;
            [ReadOnly] public ComponentDataArray<Authoritative<Position.Component>> PositionAuthority;
        }

        [Inject] private TransformData transformData;

        private Vector3 origin;

        private TickSystem tickSystem;

        private const float ToleranceMeters = 0.001f;
        private const float ToleranceAngle = 0.001f;

        private const uint MaxTicksUntilSync = 60;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            origin = World.GetExistingManager<WorkerSystem>().Origin;

            tickSystem = World.GetOrCreateManager<TickSystem>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformData.Length; i++)
            {
                var rigidbody = transformData.GameObjectRigidBody[i];
                var transform = transformData.Transform[i];

                var nativePosition = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z);

                var positionDifference = (rigidbody.position - origin - nativePosition).sqrMagnitude;

                // Sync if sufficient difference or if sufficient time has passed
                if (positionDifference <= ToleranceMeters)
                {
                    var nativeRotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                        transform.Rotation.Z, transform.Rotation.W);
                    var rotationDifference = UnityEngine.Quaternion.Angle(rigidbody.rotation, nativeRotation);
                    if (rotationDifference <= ToleranceAngle)
                    {
                        var differencesGreaterThanZero = positionDifference == 0.0f && rotationDifference == 0.0f;
                        if (differencesGreaterThanZero || tickSystem.GlobalTick - transform.Tick < MaxTicksUntilSync)
                        {
                            continue;
                        }
                    }
                }

                var position = rigidbody.position - origin;
                var location = new Location { X = position.x, Y = position.y, Z = position.z };
                var rotation = new Quaternion
                {
                    W = rigidbody.rotation.w,
                    X = rigidbody.rotation.x,
                    Y = rigidbody.rotation.y,
                    Z = rigidbody.rotation.z
                };

                var newTransform = new Transform.Component
                {
                    Location = location,
                    Rotation = rotation,
                    Tick = tickSystem.GlobalTick
                };

                var newPosition = new Position.Component
                {
                    Coords = new Coordinates
                    {
                        X = newTransform.Location.X,
                        Y = newTransform.Location.Y,
                        Z = newTransform.Location.Z
                    }
                };

                transformData.Transform[i] = newTransform;
                transformData.Position[i] = newPosition;
            }
        }
    }
}
