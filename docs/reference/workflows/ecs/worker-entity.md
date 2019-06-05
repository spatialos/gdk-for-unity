<%(TOC)%>

# ECS: Worker entity

<%(Callout message="
Before reading this document, make sure you have read:

* [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

Each of the workers in your project must have exactly one [ECS entity]({{urlRoot}}/reference/glossary#unity-ecs-entity) in its [worker-ECS world]({{urlRoot}}/reference/concepts/worker#workers-and-ecs-worlds) at any point in time. To uniquely identify the worker entity of your current worker, the worker entity has the [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component attached to it.

The worker’s worker entity performs certain tasks:

  * send and receive [commands](https://docs.improbable.io/reference/latest/shared/glossary#command) before the worker has checked out any SpatialOS entities.
  * register changes to the state of the Runtime connection (that is whether the worker is connected to the [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) or not) by filtering for the following [temporary components]({{urlRoot}}/reference/workflows/ecs/temporary-components):
     * [`OnConnected`]({{urlRoot}}/api/core/on-connected): the worker just connected to the SpatialOS Runtime.
     * [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected): the worker just disconnected from the SpatialOS Runtime. This is an `ISharedComponentData` and stores the reason for the disconnection as a `string`.

## How to run logic when the worker has just connected

You can use the worker to check in an ECS system to see whether the worker just
connected. This allows you to handle any initialization logic necessary.

**Example**

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class HandleConnectSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadOnly<OnConnected>(),
            ComponentType.ReadOnly<WorkerEntityTag>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref OnConnected onConnected, ref WorkerEntityTag workerEntityTag) =>
            {
                Debug.Log("Worker just connected!");
            });
    }
}
```

## How to run logic when the worker has just disconnected

You can use the worker to check in an ECS system to see whether the worker just disconnected. This allows you to handle any clean-up logic necessary.

**Example**

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class HandleDisconnectSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadOnly<OnDisconnected>(),
            ComponentType.ReadOnly<WorkerEntityTag>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (OnDisconnected onDisconnected, ref WorkerEntityTag workerEntityTag) =>
            {
                Debug.Log($"Got disconnected: {onDisconnected.ReasonForDisconnect}");
            });
    }
}
```
