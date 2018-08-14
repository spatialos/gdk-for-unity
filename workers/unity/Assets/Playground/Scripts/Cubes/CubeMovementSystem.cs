using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateBefore(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
    internal class CubeMovementSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public ComponentArray<Rigidbody> Rigidbody;
            public SubtractiveComponent<SpatialOSPlayerInput> NoPlayerInput;
            public ComponentDataArray<Authoritative<SpatialOSTransform>> TransformAuthority;
        }

        [Inject] private Data data;

        public struct WorkerData
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<WorkerConfig> WorkerConfigs;
        }

        [Inject] private WorkerData workerData;

        private static Vector3 speed = new Vector3(2, 0, 0);

        protected override void OnUpdate()
        {
            if (workerData.Length == 0)
            {
                new LoggingDispatcher().HandleLog(LogType.Error, new LogEvent("This system should not have been run without a worker entity"));
            }

            var origin = workerData.WorkerConfigs[0].Worker.Origin;

            for (var i = 0; i < data.Length; i++)
            {
                var rigidbodyComponent = data.Rigidbody[i];
                if (rigidbodyComponent.position.x - origin.x > 10)
                {
                    speed = new Vector3(-2, 0, 0);
                }

                if (rigidbodyComponent.position.x - origin.x < -10)
                {
                    speed = new Vector3(2, 0, 0);
                }

                rigidbodyComponent.MovePosition(rigidbodyComponent.position + Time.deltaTime * speed);
            }
        }
    }
}
