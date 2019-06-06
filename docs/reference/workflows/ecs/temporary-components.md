<%(TOC)%>

# ECS: Temporary components

When working with entities, you might need components that only exist for one frame to execute certain logic depending on those components. For this purpose, we introduce the concept of temporary components.

If a temporary component is added to an entity, the component will be removed at the end of the update loop, i.e. when the systems inside the `InternalSpatialOSCleanGroup` have been run. See [System update order]({{urlRoot}}/reference/workflows/ecs/system-update-order) for more details on the different update groups.

To create a temporary component, we provide you with the [`Improbable.Gdk.Core.RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute, which can be applied to any ECS component, extending either `IComponentData` or `ISharedComponentData`.

The following code snippet shows an example of how to annotate your ECS component with the [`RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute.

```csharp
[RemoveAtEndOfTick]
struct SomeTemporaryComponent : IComponentData
{
}
```

When using temporary components, you must consider the ordering of your systems carefully. Any system which runs logic predicated on the temporary component should be run after the temporary component is added and before it is removed (in `InternalSpatialOSCleanGroup`).

The following code snippets shows an example of how you can annotate your system to ensure it is run in the correct order. `AddComponentSystem` is a system that adds `SomeTemporaryComponent ` to your entities, while `ReadComponentSystem` filters for all entities containing `SomeTemporaryComponent`.

**Example of how to add a temporary component to entities**

```csharp
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class AddComponentSystem : ComponentSystem
{
    private CommandSystem commandSystem;
    private WorkerSystem workerSystem;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        commandSystem = World.GetExistingSystem<CommandSystem>();
        workerSystem = World.GetExistingSystem<WorkerSystem>();
    }

    protected override void OnUpdate()
    {
        var requests = commandSystem.GetRequests<Launcher.LaunchEntity.ReceivedRequest>();

        // Add `SomeTemporaryComponent` to an entity if the request has `RechargeNow` enabled
        for (var i = 0; i < requests.Count; i++)
        {
            ref readonly var request = ref requests[i];
            if (!workerSystem.TryGetEntity(request.EntityId, out var entity))
            {
                continue;
            }

            if (request.RechargeNow)
            {
                PostUpdateCommands.AddComponent(entity, new SomeTemporaryComponent());
            }
        }
    }
}
```

**Example of how to filter for all entities with a temporary component**
```csharp
// Ensure that the temporary component has already been added
// by running this system after AddComponentSystem
[UpdateAfter(typeof(AddComponentSystem))]
// By running this system during the SpatialOSUpdateGroup, it will
// definitely be run before the InternalSpatialOSCleanGroup group.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class ReadComponentSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadOnly<SomeTemporaryComponent>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref SomeTemporaryComponent someTemporaryComponent) =>
            {
                // perform work related to your temporary component
            });
    }
}
```
