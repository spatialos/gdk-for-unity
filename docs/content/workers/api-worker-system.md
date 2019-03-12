[//]: # (Doc of docs reference 15.2)

<%(TOC)%>
#  Workers: API - Worker system

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

See first the documentation on [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk) and the [Worker API]({{urlRoot}}/content/workers/api-worker)

The `WorkerSystem` class stores information about a worker during [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime). You can use `WorkerSystem` to access information about the worker during Runtime; any system running in the same [ECS world]({{urlRoot}}/content/glossary#unity-ecs-world) as a worker can access the `WorkerSystem` class.

**Fields**

| Field             | Type                   | Description                    |
|-------------------|------------------------|--------------------------------|
| Connection    | [`Connection`]({{urlRoot}}/content/connecting-to-spatialos) | The connection to the [SpatialOS Runtime]({{urlRoot}}/content/glossary#spatialos-runtime). You can use it to send data and messages. |
| WorkerType    | `string`                 | The [type of this worker]({{urlRoot}}/content/glossary#worker-types). |
| Origin        | `Vector3`                | The vector by which we translate all ECS entities added to a worker. This is useful when running multiple workers in the same Scene. You can choose to set a [worker origin]({{urlRoot}}/content/glossary#worker-origin) to be large enough so that entities that are visible to or checked out by different workers don’t interact with each other. |
| WorkerEntity  | `Entity`                 | The corresponding [Worker entity]({{urlRoot}}/content/workers/worker-entity) which allows you to query the current state of the worker as well as send and receive commands. |
| LogDispatcher | `ILogDispatcher`         | A reference to the [logger]({{urlRoot}}/content/ecs/logging) that you can use to log to your Unity Editor’s console and the [SpatialOS Console]({{urlRoot}}/content/glossary#console) |

**Methods**

```csharp
bool TryGetEntity(EntityId entityId, out Entity entity);
```

Parameters:

  * `EntityId entityId`: The entity ID of the SpatialOS entity that you want to retrieve.
  * `Entity entity`: The ECS entity that represents the SpatialOS entity that you want to retrieve. It will be `Entity.Null`, if this ECS entity does not exist.

Returns: `true`, if the queried SpatialOS entity is checked out on this worker, `false` otherwise.


```csharp
bool HasEntity(EntityId entityId);
```

Parameters:

  * `EntityId entityId`: The entity ID of the SpatialOS entity you want to check.

Returns: `true` if the SpatialOS entity is checked out on this worker, `false` otherwise.

### ECS: How to access the WorkerSystem in the ECS workflow

You can inject the worker system in any `ComponentSystem` that is in the same [ECS world]({{urlRoot}}/content/glossary#unity-ecs-world) as the `WorkerSystem`.
See the code example below for a guide on how to inject it:

**Example**

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class YourSystem : ComponentSystem
{
    [Inject] WorkerSystem Worker;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        // The injected WorkerSystem will already be available here
    }

    protected override void OnUpdate()
    {
        // Do something
    }
}
```

### GameObject: How to access the WorkerSystem in the MonoBehaviour workflow

**Example**

```csharp
public class CubeSpawnerInputBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
      var spatialOSComponent = GetComponent<SpatialOSComponent>();
      var worker = spatialOSComponent.Worker;
      // do something with your worker
    }
}
```
