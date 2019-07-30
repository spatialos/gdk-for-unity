using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    internal interface ITransformSync<TTransform> where TTransform : class
    {
        void UpdateLatestTransform(ref TransformToSend transformToSend, TTransform transform);

        void ApplyLatestTransform(ref TransformToSet transformToSet, TTransform transform);

        void OnResetAuth(WorkerSystem worker, Entity entity, ref TransformInternal.Component transformComponent,
            TTransform transform);

        void InitKinematicState(ref KinematicStateWhenAuth kinematicStateWhenAuth, TTransform transform);

        void ApplyKinematicStateOnAuthChange(ref KinematicStateWhenAuth kinematicStateWhenAuth,
            AuthorityChangeReceived authChange, TTransform transform);
    }
}
