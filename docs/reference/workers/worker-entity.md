[//]: # (Doc of docs reference 15.2a)
[//]: # (TODO: Move to the ecs folder)

<%(TOC)%>
# Worker entity
_This document relates to the [ECS workflow](\{\{urlRoot\}\}/reference/intro-workflows-spatialos-entities)._

Before reading this document, see the documentation on [workers in the GDK](\{\{urlRoot\}\}/reference/workers/workers-in-the-gdk).

Each of the workers in your project must have exactly one [ECS entity](\{\{urlRoot\}\}/reference/glossary#unity-ecs-entity) in its [worker-ECS world](\{\{urlRoot\}\}/reference/workers/workers-in-the-gdk#workers-and-ecs-worlds) at any point in time. To uniquely identify the worker entity of your current worker, the worker entity has the `WorkerEntityTag` component attached to it.

The worker’s worker entity performs certain tasks:

  * send and receive [commands](https://docs.improbable.io/reference/latest/shared/glossary#command) before the worker has checked out any SpatialOS entities.
  * register changes to the state of the Runtime connection (that is whether the worker is connected to the [Runtime](\{\{urlRoot\}\}/reference/glossary#spatialos-runtime) or not) by filtering for the following [temporary components](\{\{urlRoot\}\}/reference/ecs/temporary-components):
     * `OnConnected`: the worker just connected to the SpatialOS Runtime.
     * `OnDisconnected`: the worker just disconnected from the SpatialOS Runtime. This is an `ISharedComponentData` and stores the reason for the disconnection as a `string`.

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
    private struct Data
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<OnConnected> OnConnected;
        [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        Debug.Log("Worker just connected!");
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
    private struct Data
    {
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<OnDisconnected> OnDisconnected;
        [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        var reasonForDisconnect = data.OnDisconnected[0].ReasonForDisconnect;
        Debug.Log($"Got disconnected: {reasonForDisconnect}");
    }
}
```

## How to send a command using the worker entity

The worker entity has all [command sender components](\{\{urlRoot\}\}/reference/ecs/commands) attached to it.
By filtering for these components, you are able to send commands even if you don't have any [SpatialOS entities](\{\{urlRoot\}\}/reference/glossary#spatialos-entity) which is [checked out](\{\{urlRoot\}\}/reference/glossary#checking-out).

```csharp
public class CreateCreatureSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        var requestSender = data.CreateEntitySender[0];
        var entity = CreatureTemplate.CreateCreatureEntityTemplate(new Coordinates(0, 0, 0));
        requestSender.RequestsToSend.Add(new WorldCommands.CreateEntity.Request
        (
            entity
        ));
        data.CreateEntitySender[0] = requestSender;
    }
}
```
