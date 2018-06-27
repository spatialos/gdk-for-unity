**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Temporary components

The attribute `Improbable.Gdk.Core.RemoveAtEndOfTick`, can be applied to any ECS component, extending either `IComponentData` or `ISharedComponentData`.
All components with this attribute will be removed from all entities during the `InternalSpatialOSCleanGroup` at the end of `SpatialOSSendGroup`.

Note: The `InternalSpatialOSCleanGroup` runs after both the `SpatialOSUpdateGroup` and the `CustomSpatialOSSendGroup`.

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
        int Length;
        [Readonly] ComponentDataArray<SomeTemporaryComponent> TemporaryComponent;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        // perform work related to the temporary component
    }
}
``` 

At the end of the update loop, `SomeTemporaryComponent` will automatically be removed from all entities.