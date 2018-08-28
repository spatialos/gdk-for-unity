using Generated.Improbable;
using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace Improbable.Gdk.TransformSynchronization
{
    public class SetNewlyCreatedGameObjectTransformSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialOSTransform> Transforms;
            [ReadOnly] public ComponentArray<GameObjectReference> GameObjectReferences;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyAddedTags;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var spatialPosition = data.Transforms[i].Location;
                var position = new Vector3(spatialPosition.X, spatialPosition.Y, spatialPosition.Z);

                var spatialRotation = data.Transforms[i].Rotation;
                var rotation
                    = new Quaternion(spatialRotation.X, spatialRotation.Y, spatialRotation.Z, spatialRotation.W);

                data.GameObjectReferences[i].transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
}
