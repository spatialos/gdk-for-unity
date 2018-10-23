using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class DefaultUpdateLatestTransformSystem : ComponentSystem
    {
        private struct RigidbodyData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<Rigidbody> Rigidbody;
            public ComponentDataArray<TransformToSend> TransformToSend;
            [ReadOnly] public ComponentDataArray<GetTransformFromGameObjectTag> DenotesShouldGetTransformFromGameObject;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>>
                DenotesAuthoritative;
        }

        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<UnityEngine.Transform> Transform;
            public ComponentDataArray<TransformToSend> TransformToSend;
            public SubtractiveComponent<Rigidbody> DenotesNoRigidbody;
            [ReadOnly] public ComponentDataArray<GetTransformFromGameObjectTag> DenotesShouldGetTransformFromGameObject;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>>
                DenotesAuthoritative;
        }

        [Inject] private RigidbodyData rigidbodyData;
        [Inject] private TransformData transformData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < rigidbodyData.Length; ++i)
            {
                var rigidbody = rigidbodyData.Rigidbody[i];
                var transformToSend = new TransformToSend
                {
                    Position = rigidbody.position,
                    Velocity = rigidbody.velocity,
                    Orientation = rigidbody.rotation
                };
                rigidbodyData.TransformToSend[i] = transformToSend;
            }

            for (int i = 0; i < transformData.Length; ++i)
            {
                var transform = transformData.Transform[i];
                var transformToSend = new TransformToSend
                {
                    Position = transform.position,
                    Velocity = Vector3.zero,
                    Orientation = transform.rotation
                };
                transformData.TransformToSend[i] = transformToSend;
            }
        }
    }
}
