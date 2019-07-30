using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.TransformSynchronization
{
    internal class RigidbodyTransformSync : ITransformSync<Rigidbody>
    {
        public void UpdateLatestTransform(ref TransformToSend transformToSend, Rigidbody rigidbody)
        {
            transformToSend = new TransformToSend
            {
                Position = rigidbody.position,
                Velocity = rigidbody.velocity,
                Orientation = rigidbody.rotation
            };
        }

        public void ApplyLatestTransform(ref TransformToSet transformToSet, Rigidbody rigidbody)
        {
            rigidbody.MovePosition(transformToSet.Position);
            rigidbody.MoveRotation(transformToSet.Orientation);
            rigidbody.AddForce(transformToSet.Velocity - rigidbody.velocity, ForceMode.VelocityChange);
        }

        public void OnResetAuth(WorkerSystem worker, Entity entity, ref TransformInternal.Component transformComponent, Rigidbody rigidbody)
        {
            rigidbody.MovePosition(transformComponent.Location.ToUnityVector() + worker.Origin);
            rigidbody.MoveRotation(transformComponent.Rotation.ToUnityQuaternion());
            rigidbody.AddForce(transformComponent.Velocity.ToUnityVector() - rigidbody.velocity,
                ForceMode.VelocityChange);
        }

        public void InitKinematicState(ref KinematicStateWhenAuth kinematicStateWhenAuth, Rigidbody rigidbody)
        {
            kinematicStateWhenAuth = new KinematicStateWhenAuth
            {
                KinematicWhenAuthoritative = rigidbody.isKinematic
            };

            rigidbody.isKinematic = true;
        }

        public void ApplyKinematicStateOnAuthChange(ref KinematicStateWhenAuth kinematicStateWhenAuth,
            AuthorityChangeReceived authChange, Rigidbody rigidbody)
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
