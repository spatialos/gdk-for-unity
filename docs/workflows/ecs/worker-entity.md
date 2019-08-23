<%(TOC)%>

# ECS: Worker entity

<%(Callout message="
Before reading this document, make sure you have read:

* [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

Each worker has an ECS world to represent the entities currently in a [worker's view]({{urlRoot}}/reference/glossary#worker-s-view). This world contains a worker entity, which can be uniquely identified by the [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component attached to it.

The worker entity enables you to react to changes in the connection to the Runtime. That is, whether the worker is connected to the [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) or not.

## How to run logic when the worker has just connected

The GDK adds the [`OnConnected`]({{urlRoot}}/api/core/on-connected) [temporary component]({{urlRoot}}/workflows/ecs/concepts/temporary-components) to your worker entity when it has just connected.

**Example usage**

```csharp
public class HandleConnectSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadOnly<OnConnected>(),
            ComponentType.ReadOnly<WorkerEntityTag>()
        );
    }

    protected override void OnUpdate()
    {
        // You can iterate through the matching components using the ECS `.ForEach` syntax.
        Entities.With(query).ForEach(entity =>
        {
            Debug.Log("Worker just connected!");
        });
    }
}
```

## How to run logic when the worker has just disconnected

The GDK adds the [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected) [temporary component]({{urlRoot}}/workflows/ecs/concepts/temporary-components) to your worker entity when it has just disconnected. This component contains a single string field storing the reason for disconnecting.

**Example usage**

```csharp
public class HandleDisconnectSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadOnly<OnDisconnected>(),
            ComponentType.ReadOnly<WorkerEntityTag>()
        );
    }

    protected override void OnUpdate()
    {
        // You can iterate through the matching components using the ECS `.ForEach` syntax.
        Entities.With(query).ForEach((OnDisconnected onDisconnected) =>
        {
            Debug.Log($"Got disconnected: {onDisconnected.ReasonForDisconnect}");
        });
    }
}
```
