using Improbable;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    internal class CubeMovementSystem : ComponentSystem
    {
        private ComponentGroup cubeGroup;

        private Vector3 origin;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            origin = World.GetExistingManager<WorkerSystem>().Origin;

            cubeGroup = GetComponentGroup(
                ComponentType.ReadWrite<CubeTargetVelocity.Component>(),
                ComponentType.ReadOnly<CubeTargetVelocity.ComponentAuthority>(),
                ComponentType.ReadWrite<Rigidbody>()
            );
            cubeGroup.SetFilter(CubeTargetVelocity.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var entities = cubeGroup.GetEntityArray();
            var cubeVelocityData = cubeGroup.GetComponentDataArray<CubeTargetVelocity.Component>();

            for (var i = 0; i < cubeVelocityData.Length; i++)
            {
                var rigidbody = EntityManager.GetComponentObject<Rigidbody>(entities[i]);
                var cubeComponent = cubeVelocityData[i];

                if (cubeComponent.TargetVelocity.X > 0 && rigidbody.position.x - origin.x > 10)
                {
                    cubeComponent.TargetVelocity = new Vector3f { X = -2.0f };
                    cubeVelocityData[i] = cubeComponent;
                }
                else if (cubeComponent.TargetVelocity.X < 0 && rigidbody.position.x - origin.x < -10)
                {
                    cubeComponent.TargetVelocity = new Vector3f { X = 2.0f };
                    cubeVelocityData[i] = cubeComponent;
                }

                rigidbody.MovePosition(rigidbody.position + cubeComponent.TargetVelocity.ToUnityVector() * Time.fixedDeltaTime);
            }
        }
    }
}
