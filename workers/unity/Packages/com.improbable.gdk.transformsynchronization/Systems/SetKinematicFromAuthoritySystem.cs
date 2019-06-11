using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(ResetForAuthorityGainedSystem))]
    public class SetKinematicFromAuthoritySystem : ComponentSystem
    {
        private ComponentUpdateSystem updateSystem;

        private EntityQuery newEntityGroup;
        private EntityQuery authChangeGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            newEntityGroup = GetEntityQuery(
                ComponentType.ReadWrite<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            newEntityGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            authChangeGroup = GetEntityQuery(
                ComponentType.ReadWrite<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<NewlyAddedSpatialOSEntity>()
            );
        }

        protected override void OnUpdate()
        {
            UpdateNewEntityGroup();
            UpdateAuthChangeGroup();
        }

        private void UpdateNewEntityGroup()
        {
            Entities.With(newEntityGroup).ForEach((ref KinematicStateWhenAuth kinematicStateWhenAuth, Rigidbody rigidbody) =>
            {
                kinematicStateWhenAuth = new KinematicStateWhenAuth
                {
                    KinematicWhenAuthoritative = rigidbody.isKinematic
                };

                rigidbody.isKinematic = true;
            });
        }

        private void UpdateAuthChangeGroup()
        {
            Entities.With(newEntityGroup).ForEach(
                (Rigidbody rigidbody, ref KinematicStateWhenAuth kinematicStateWhenAuth, ref SpatialEntityId spatialEntityId) =>
                {
                    var changes = updateSystem.GetAuthorityChangesReceived(spatialEntityId.EntityId,
                        TransformInternal.ComponentId);
                    if (changes.Count == 0)
                    {
                        return;
                    }

                    var auth = changes[changes.Count - 1];
                    switch (auth.Authority)
                    {
                        case Authority.NotAuthoritative:
                            kinematicStateWhenAuth = new KinematicStateWhenAuth
                            {
                                KinematicWhenAuthoritative = rigidbody.isKinematic
                            };
                            rigidbody.isKinematic = true;
                            break;
                        case Authority.Authoritative:
                        case Authority.AuthorityLossImminent:
                            rigidbody.isKinematic = kinematicStateWhenAuth.KinematicWhenAuthoritative;
                            break;
                    }
                });
        }
    }
}
