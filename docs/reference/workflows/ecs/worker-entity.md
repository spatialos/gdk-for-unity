<%(TOC)%>

# ECS: Worker entity

<%(Callout message="
Before reading this document, make sure you have read:

* [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

Each worker has an ECS world to represent the entities currently in a [worker's view]({{urlRoot}}/reference/glossary#worker-s-view). Each [worker's ECS world]({{urlRoot}}/reference/concepts/worker#workers-and-ecs-worlds) contains a worker entity, which can be uniquely identified by the [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component attached to it.

The worker entity enables you to react to changes to the state of the Runtime connection. That is, whether the worker is connected to the [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) or not. The GDK adds the following [temporary components]({{urlRoot}}/reference/workflows/ecs/temporary-components) to your worker entity when these changes occur:

* [`OnConnected`]({{urlRoot}}/api/core/on-connected): the worker just connected to the SpatialOS Runtime.
* [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected): the worker just disconnected from the SpatialOS Runtime. This is an `ISharedComponentData` and stores the reason for the disconnection as a `string`.

## How to run logic when the worker has just connected

The GDK adds the `OnConnected` component to your worker entity when it has just connected.

**Example usage**

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

The GDK adds the `OnDisconnected` component to your worker entity when it has just disconnected.

**Example usage**

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
