using Improbable.Gdk.Core;
using Improbable.Transform;
using Improbable.Worker.CInterop;
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
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(ResetForAuthorityGainedSystem))]
    public class SetKinematicFromAuthoritySystem : ComponentSystem
    {
        private ComponentUpdateSystem updateSystem;

        private ComponentGroup newEntityGroup;
        private ComponentGroup authChangeGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            newEntityGroup = GetComponentGroup(
                ComponentType.Create<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());

            authChangeGroup = GetComponentGroup(
                ComponentType.Create<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Subtractive<NewlyAddedSpatialOSEntity>());
        }

        protected override void OnUpdate()
        {
            UpdateNewEntityGroup();
            UpdateAuthChangeGroup();
        }

        private void UpdateNewEntityGroup()
        {
            newEntityGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            var rigidbodyArray = newEntityGroup.GetComponentArray<Rigidbody>();
            var kinematicStateWhenAuthArray = newEntityGroup.GetComponentDataArray<KinematicStateWhenAuth>();

            for (int i = 0; i < rigidbodyArray.Length; ++i)
            {
                var rigidbody = rigidbodyArray[i];
                kinematicStateWhenAuthArray[i] = new KinematicStateWhenAuth
                {
                    KinematicWhenAuthoritative = rigidbody.isKinematic
                };
                rigidbody.isKinematic = true;
            }
        }

        private void UpdateAuthChangeGroup()
        {
            var rigidbodyArray = authChangeGroup.GetComponentArray<Rigidbody>();
            var kinematicStateWhenAuthArray = authChangeGroup.GetComponentDataArray<KinematicStateWhenAuth>();
            var spatialEntityIdArray = authChangeGroup.GetComponentDataArray<SpatialEntityId>();

            for (int i = 0; i < rigidbodyArray.Length; ++i)
            {
                var rigidbody = rigidbodyArray[i];
                var changes = updateSystem.GetAuthorityChangesReceived(spatialEntityIdArray[i].EntityId,
                    TransformInternal.ComponentId);
                if (changes.Count == 0)
                {
                    continue;
                }

                var auth = changes[changes.Count - 1];
                switch (auth.Authority)
                {
                    case Authority.NotAuthoritative:
                        kinematicStateWhenAuthArray[i] = new KinematicStateWhenAuth
                        {
                            KinematicWhenAuthoritative = rigidbody.isKinematic
                        };
                        rigidbody.isKinematic = true;
                        break;
                    case Authority.Authoritative:
                    case Authority.AuthorityLossImminent:
                        rigidbody.isKinematic = kinematicStateWhenAuthArray[i].KinematicWhenAuthoritative;
                        break;
                }
            }
        }
    }
}
