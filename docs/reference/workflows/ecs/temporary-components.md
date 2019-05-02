<%(TOC)%>

# Temporary components

_This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/overview)._

When working with entities, you might need components that only exist for one frame to execute certain logic depending on those components. For this purpose, we introduce the concept of temporary components. If a temporary component is added to an entity, the component will be removed at the end of the update loop, i.e. when the systems inside the `InternalSpatialOSCleanGroup` have been run. See [System update order]({{urlRoot}}/reference/workflows/ecs/system-update-order) for more details on the different update groups.

To create a temporary component, we provide you with the [`Improbable.Gdk.Core.RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute, which can be applied to any ECS component, extending either `IComponentData` or `ISharedComponentData`.

The following code snippet shows an example of how to annotate your ECS component with the [`RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute.

```csharp
[RemoveAtEndOfTick]
struct SomeTemporaryComponent : IComponentData
{
}
```

When using temporary components, you must consider the ordering of your systems carefully. Any system which runs logic predicated on the temporary component should be ran after the temporary component may be added and before it will be removed (in `InternalSpatialOSCleanGroup`).

The following code snippet shows an example how you can annotate your system to ensure it is run in the correct order.
`AddComponentSystem` is the system that adds `SomeTemporaryComponent ` to your entities, while `ReadComponentSystem` filters for all entities containing `SomeTemporaryComponent `.

```csharp
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class AddComponentSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        public EntityArray Entities;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        PostUpdateCommands.AddComponent(data.Entities[i], new SomeTemporaryComponent());
    }
}

// ensure that the temporary component has already been
// added by running this system after AddComponentSystem
[UpdateAfter(typeof(AddComponentSystem))]
// By running this system during the SpatialOSUpdateGroup, it will
// definitely run before the InternalSpatialOSCleanGroup group.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class ReadComponentSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        [Readonly] public ComponentDataArray<SomeTemporaryComponent> TemporaryComponent;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        // perform work related to the temporary component
    }
}
```
