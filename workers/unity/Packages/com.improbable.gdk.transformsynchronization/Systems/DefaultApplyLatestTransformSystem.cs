using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

#region Diagnostic control

#pragma warning disable 169
#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private struct RigidbodyData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformToSet> CurrentTransform;
            public ComponentArray<Rigidbody> Rigidbody;
            [ReadOnly] public ComponentDataArray<SetTransformToGameObjectTag> DenotesShouldUpdateAutomatically;

            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>>
                DenotesNotAuthoritative;
        }

        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformToSet> CurrentTransform;
            [ReadOnly] public ComponentArray<UnityEngine.Transform> Transform;
            public SubtractiveComponent<Rigidbody> DenotesNoRigidbody;
            [ReadOnly] public ComponentDataArray<SetTransformToGameObjectTag> DenotesShouldUpdateAutomatically;

            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>>
                DenotesNotAuthoritative;
        }

        [Inject] private RigidbodyData rigidbodyData;
        [Inject] private TransformData transformData;

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
