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
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ResetForAuthorityGainedSystem : ComponentSystem
    {
        private struct RigidbodyData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<Rigidbody> Rigidbody;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> TransformComponent;
            [WriteOnly] public ComponentDataArray<TicksSinceLastTransformUpdate> TicksSinceLastUpdate;
            [WriteOnly] public BufferArray<BufferedTransform> TransformBuffer;

            public SubtractiveComponent<NewlyAddedSpatialOSEntity> DenotesNotNewlyAdded;

            [ReadOnly] public ComponentDataArray<AuthorityChanges<TransformInternal.Component>>
                DenotesAuthorityChanged;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>>
                DenotesAuthoritative;
        }

        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<UnityEngine.Transform> UnityTransform;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> TransformComponent;
            [WriteOnly] public ComponentDataArray<TicksSinceLastTransformUpdate> TicksSinceLastUpdate;
            [WriteOnly] public BufferArray<BufferedTransform> TransformBuffer;

            public SubtractiveComponent<Rigidbody> DenotesNoRigidbody;
            public SubtractiveComponent<NewlyAddedSpatialOSEntity> DenotesNotNewlyAdded;

            [ReadOnly] public ComponentDataArray<AuthorityChanges<TransformInternal.Component>>
                DenotesAuthorityChanged;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>>
                DenotesAuthoritative;
        }

        [Inject] private RigidbodyData rigidbodyData;
        [Inject] private TransformData transformData;
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            for (int i = 0; i < rigidbodyData.Length; ++i)
            {
                var t = rigidbodyData.TransformComponent[i];
                var rigidbody = rigidbodyData.Rigidbody[i];
                rigidbody.MovePosition(t.Location.ToUnityVector3() + worker.Origin);
                rigidbody.MoveRotation(t.Rotation.ToUnityQuaternion());
                rigidbody.AddForce(t.Velocity.ToUnityVector3() - rigidbody.velocity, ForceMode.VelocityChange);
                rigidbodyData.TransformBuffer[i].Clear();
                rigidbodyData.TicksSinceLastUpdate[i] = new TicksSinceLastTransformUpdate();
            }

            for (int i = 0; i < transformData.Length; ++i)
            {
                var t = transformData.TransformComponent[i];
                var unityTransform = transformData.UnityTransform[i];
                unityTransform.position = t.Location.ToUnityVector3() + worker.Origin;
                unityTransform.rotation = t.Rotation.ToUnityQuaternion();
                transformData.TransformBuffer[i].Clear();
                transformData.TicksSinceLastUpdate[i] = new TicksSinceLastTransformUpdate();
            }
        }
    }
}
