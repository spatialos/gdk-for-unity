using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateBefore(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
    internal class CubeMovementSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<Rigidbody> Rigidbody;
            public SubtractiveComponent<SpatialOSPlayerInput> NoPlayerInput;
            public ComponentDataArray<Authoritative<SpatialOSTransform>> TransformAuthority;
        }

        [Inject] private Data data;

        private static Vector3 speed = new Vector3(2, 0, 0);

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var rigidbodyComponent = data.Rigidbody[i];
                if (rigidbodyComponent.position.x > 10)
                {
                    speed = new Vector3(-2, 0, 0);
                }

                if (rigidbodyComponent.position.x < -10)
                {
                    speed = new Vector3(2, 0, 0);
                }

                rigidbodyComponent.MovePosition(rigidbodyComponent.position + Time.deltaTime * speed);
            }
        }
    }
}
