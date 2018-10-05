using Improbable.Gdk.Core;
using Improbable.Transform;
using Improbable.Worker.Core;
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
    [UpdateBefore(typeof(ResetForAuthorityGainedSystem))]
    public class SetKinematicFromAuthoritySystem : ComponentSystem
    {
        private struct NewEntityData
        {
            public readonly int Length;
            public ComponentArray<Rigidbody> Rigidbody;

            // If authority is not gained on the first tick there will be no auth changed component
            public SubtractiveComponent<Authoritative<TransformInternal.Component>> DenotesNotAuthoritative;
            public ComponentDataArray<KinematicStateWhenAuth> KinematicStateWhenAuth;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> DenotesNewEntity;
            [ReadOnly] public ComponentDataArray<ManageKinematicOnAuthorityChangeTag> DenotesShouldManageRigidbody;
        }

        private struct AuthChangeData
        {
            public readonly int Length;
            public ComponentArray<Rigidbody> Rigidbody;
            public ComponentDataArray<KinematicStateWhenAuth> KinematicStateWhenAuth;
            [ReadOnly] public ComponentDataArray<AuthorityChanges<TransformInternal.Component>> TransformAuthority;
            [ReadOnly] public ComponentDataArray<ManageKinematicOnAuthorityChangeTag> DenotesShouldManageRigidbody;
        }

        [Inject] private AuthChangeData authChangeData;
        [Inject] private NewEntityData newEntityData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < newEntityData.Length; ++i)
            {
                var rigidbody = authChangeData.Rigidbody[i];
                newEntityData.KinematicStateWhenAuth[i] = new KinematicStateWhenAuth
                {
                    KinematicWhenAuthoritative = rigidbody.isKinematic
                };
                rigidbody.isKinematic = true;
            }

            for (int i = 0; i < authChangeData.Length; ++i)
            {
                var rigidbody = authChangeData.Rigidbody[i];
                var changes = authChangeData.TransformAuthority[i].Changes;
                var auth = changes[changes.Count - 1];
                switch (auth)
                {
                    case Authority.NotAuthoritative:
                        authChangeData.KinematicStateWhenAuth[i] = new KinematicStateWhenAuth
                        {
                            KinematicWhenAuthoritative = rigidbody.isKinematic
                        };
                        rigidbody.isKinematic = true;
                        break;
                    case Authority.Authoritative:
                    case Authority.AuthorityLossImminent:
                        rigidbody.isKinematic = authChangeData.KinematicStateWhenAuth[i].KinematicWhenAuthoritative;
                        break;
                }
            }
        }
    }
}
