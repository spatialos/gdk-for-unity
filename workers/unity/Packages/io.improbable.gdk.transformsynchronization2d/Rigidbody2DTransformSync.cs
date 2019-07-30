using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.TransformSynchronization
{
    internal class Rigidbody2DTransformSync : ITransformSync<Rigidbody2D>
    {
        public void UpdateLatestTransform(ref TransformToSend transformToSend, Rigidbody2D rigidbody)
        {
            transformToSend = new TransformToSend
            {
                Position = rigidbody.position,
                Velocity = rigidbody.velocity,
                Orientation = Quaternion.Euler(0, 0, rigidbody.rotation)
            };
        }

        public void ApplyLatestTransform(ref TransformToSet transformToSet, Rigidbody2D rigidbody)
        {
            rigidbody.MovePosition(transformToSet.Position);
            rigidbody.MoveRotation(transformToSet.Orientation);
            rigidbody.AddForce((Vector2) transformToSet.Velocity - rigidbody.velocity, ForceMode2D.Impulse);
        }

        public void OnResetAuth(WorkerSystem worker, Entity entity, ref TransformInternal.Component transformComponent, Rigidbody2D rigidbody)
        {
            rigidbody.MovePosition(transformComponent.Location.ToUnityVector() + worker.Origin);
            rigidbody.MoveRotation(transformComponent.Rotation.ToUnityQuaternion());
            rigidbody.AddForce((Vector2) transformComponent.Velocity.ToUnityVector() - rigidbody.velocity,
                ForceMode2D.Impulse);
        }

        public void InitKinematicState(ref KinematicStateWhenAuth kinematicStateWhenAuth, Rigidbody2D rigidbody)
        {
            kinematicStateWhenAuth = new KinematicStateWhenAuth
            {
                KinematicWhenAuthoritative = rigidbody.isKinematic
            };

            rigidbody.isKinematic = true;
        }

        public void ApplyKinematicStateOnAuthChange(ref KinematicStateWhenAuth kinematicStateWhenAuth,
            AuthorityChangeReceived authChange, Rigidbody2D rigidbody)
        {
            switch (authChange.Authority)
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
        }
    }
}
