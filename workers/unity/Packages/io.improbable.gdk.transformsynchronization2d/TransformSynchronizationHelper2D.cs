using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper2D
    {
        public static void Enable2DMode(World world)
        {
            var defaultApplyLatestTransformSystem = world.GetOrCreateSystem<DefaultApplyLatestTransformSystem>();
            defaultApplyLatestTransformSystem.RegisterType<Rigidbody2D>(
                (ref TransformToSet transformToSet, Rigidbody2D rigidbody) =>
                {
                    rigidbody.MovePosition(transformToSet.Position);
                    rigidbody.MoveRotation(transformToSet.Orientation);
                    rigidbody.AddForce((Vector2) transformToSet.Velocity - rigidbody.velocity, ForceMode2D.Impulse);
                });

            var defaultUpdateLatestTransformSystem = world.GetOrCreateSystem<DefaultUpdateLatestTransformSystem>();
            defaultUpdateLatestTransformSystem.RegisterType<Rigidbody2D>(
                (ref TransformToSend transformToSend, Rigidbody2D rigidbody) =>
                {
                    transformToSend = new TransformToSend
                    {
                        Position = rigidbody.position,
                        Velocity = rigidbody.velocity,
                        Orientation = Quaternion.Euler(0, 0, rigidbody.rotation)
                    };
                });

            var authorityGainedSystem = world.GetOrCreateSystem<ResetForAuthorityGainedSystem>();
            authorityGainedSystem.RegisterType<Rigidbody2D>((
                WorkerSystem worker,
                Entity entity,
                ref TransformInternal.Component transformInternal,
                Rigidbody2D rigidbody) =>
            {
                rigidbody.MovePosition(TransformUtils.ToUnityVector3(transformInternal.Location) + worker.Origin);
                rigidbody.MoveRotation(TransformUtils.ToUnityQuaternion(transformInternal.Rotation));
                rigidbody.AddForce(
                    (Vector2) TransformUtils.ToUnityVector3(transformInternal.Velocity) - rigidbody.velocity,
                    ForceMode2D.Impulse);
            });

            var kinematicSystem = world.GetOrCreateSystem<SetKinematicFromAuthoritySystem>();
            kinematicSystem.RegisterType<Rigidbody2D>(
                (ref KinematicStateWhenAuth kinematicStateWhenAuth,
                    Rigidbody2D rigidbody) =>
                {
                    kinematicStateWhenAuth = new KinematicStateWhenAuth
                    {
                        KinematicWhenAuthoritative = rigidbody.isKinematic
                    };

                    rigidbody.isKinematic = true;
                },
                (ref KinematicStateWhenAuth kinematicStateWhenAuth,
                    AuthorityChangeReceived auth,
                    Rigidbody2D rigidbody) =>
                {
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
                            rigidbody.isKinematic = kinematicStateWhenAuth.KinematicWhenAuthoritative;
                            break;
                    }
                });
        }
    }
}
