**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Temporary components

The attribute `Improbable.Gdk.Core.RemoveAtEndOfTick`, can be applied to any ECS component, extending either `IComponentData` or `ISharedComponentData`.
All components with this attribute will be removed from all entities during the `InternalSpatialOSCleanGroup` at the end of `SpatialOSSendGroup`. See [System update order](system-update-order.md) for more details.

### Example
```csharp
[RemoveAtEndOfTick]
struct SomeTemporaryComponent : IComponentData
{
}
```

If a system, `AddComponentSystem`, adds `SomeTemporaryComponent` to some entities, then `ReadComponentSystem.OnUpdate()` (see the sample below) will run during the same update loop.

```csharp
[UpdateAfter(typeof(AddComponentSystem))]
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
class ReadComponentSystem : ComponentSystem
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

At the end of the update loop, `SomeTemporaryComponent` will automatically be removed from all entities.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Editorial review status: Engineer review only)
[//]: # (Questions to deal with: need to describe what a tick is. Needs to link to a doc on system execution order. Needs to link to docs describing the existing components which use this)
[//]: # (1. Editorial review  - JIRA TICKET: UTY-628)
