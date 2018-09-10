using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdate))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private struct RigidbodyData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformToSet> CurrentTransform;
            public ComponentArray<Rigidbody> Rigidbody;

            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>>
                DenotesNotAuthoritative;
        }

        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformToSet> CurrentTransform;
            [ReadOnly] public ComponentArray<Transform> Transform;
            public SubtractiveComponent<Rigidbody> DenotesNoRigidbody;

            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>>
                DenotesNotAuthoritative;
        }

        [Inject] private RigidbodyData rigidbodyData;
        [Inject] private TransformData transformData;
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            for (int i = 0; i < rigidbodyData.Length; ++i)
            {
                var transform = rigidbodyData.CurrentTransform[i];
                var rigidbody = rigidbodyData.Rigidbody[i];
                rigidbody.MovePosition(transform.Position);
                rigidbody.MoveRotation(transform.Orientation);
                rigidbody.AddForce(transform.Velocity - rigidbody.velocity, ForceMode.VelocityChange);
            }

            for (int i = 0; i < transformData.Length; ++i)
            {
                var transform = transformData.CurrentTransform[i];
                transformData.Transform[i].localPosition = transform.Position;
                transformData.Transform[i].localRotation = transform.Orientation;
            }
        }
    }
}
