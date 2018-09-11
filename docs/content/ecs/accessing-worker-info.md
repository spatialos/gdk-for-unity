**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

## ECS: Accessing information about the worker during runtime 

### Worker System

The WorkerSystem can be injected in any `ComponentSystem` that is in the same ECS world as the worker to access the data relevant to the worker. See the code example below on how to inject it:

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace YourProject
{
    public class YourSystem : ComponentSystem
    {
        [Inject] WorkerSystem Worker;

        protected override void OnUpdate()
            // Do something
        }
    }
}
```

The `WorkerSystem` stores the following data:

* `Connection`: The connection to the SpatialOS Runtime. You can use it to send data and messages.

* `World`: The ECS World that this worker will run its logic in.

* `LogDispatcher`: A reference to the [logger](logging.md) that can be used to log to the Unity Console and the SpatialOS Runtime.

* `WorkerType`: The type of this worker.

* `WorkerId`: The ID of this worker. The worker uses the ID to connect to the SpatialOS Runtime.

* `Origin`: The vector by which we translate all entities added to a worker. This is useful when running multiple workers in the same scene. You can choose to set a worker origin to be large enough so that entities that are visible to or checked out by different workers don’t bump into each other.

### Worker Entity

The Worker Entity provides the API to send and receive [commands](commands.md).

To check whether the worker just connected or disconnected, you can filter for the following [temporary components](temporary-components.md):

* `OnConnected`: the worker just connected to the SpatialOS Runtime. 

* `OnDisconnected`: the worker just disconnected from the SpatialOS Runtime. This is an `ISharedComponentData` and stores the reason for the disconnection as a `string`.

To use the worker entity, you can inject it in your system by filtering for the `WorkerEntityTag` component data as shown below.

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace YourProject
{
    public class YourSystem : ComponentSystem
    {
	
        private struct Data 
        {
            Public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }

        [Inject] Data data;

        protected override void OnUpdate()
            // Do something
        }
    }
}
```

----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).