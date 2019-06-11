using Improbable;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    internal class CubeMovementSystem : ComponentSystem
    {
        private EntityQuery cubeGroup;

        private Vector3 origin;

        protected override void OnCreate()
        {
            base.OnCreate();

            origin = World.GetExistingSystem<WorkerSystem>().Origin;

            cubeGroup = GetEntityQuery(
                ComponentType.ReadWrite<CubeTargetVelocity.Component>(),
                ComponentType.ReadOnly<CubeTargetVelocity.ComponentAuthority>(),
                ComponentType.ReadWrite<Rigidbody>()
            );
            cubeGroup.SetFilter(CubeTargetVelocity.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(cubeGroup).ForEach((Entity entity, ref CubeTargetVelocity.Component cubeComponent, Rigidbody rigidbody) =>
            {
                if (cubeComponent.TargetVelocity.X > 0 && rigidbody.position.x - origin.x > 10)
                {
                    cubeComponent.TargetVelocity = new Vector3f { X = -2.0f };
                }
                else if (cubeComponent.TargetVelocity.X < 0 && rigidbody.position.x - origin.x < -10)
                {
                    cubeComponent.TargetVelocity = new Vector3f { X = 2.0f };
                }

                rigidbody.MovePosition(rigidbody.position + cubeComponent.TargetVelocity.ToUnityVector() * Time.fixedDeltaTime);
            });
        }
    }
}
