**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

## ECS: Custom replication systems

### What the SpatialOS GDK's replication system does

By default, the SpatialOS GDK for Unity (GDK) automatically replicates ECS components to SpatialOS whenever you modify an ECS component (that corresponds to a SpatialOS component)'s properties.

#### For properties

Each ECS component has an internal bool named `DirtyBit`. When a worker sets any property of a SpatialOS component, in the corresponding ECS component, the GDK sets the `DirtyBit` to `true`. The `SpatialOSSendSystem`, which runs at the end of every frame, then checks the `DirtyBit` of each ECS component. If `DirtyBit` is `true`, the GDK pushes a SpatialOS component update and sets `DirtyBit` back to `false`.

#### For events

When a worker sends a SpatialOS event, the GDK puts the event object into an internal buffer. When it's time to replicate a component, the GDK sends all buffered events and clears the buffer.

### Writing your own replication system

If some ECS components need more complex replication logic, you can create custom replication systems on a per-component basis. To do this:

* Your custom replication system must extend the `Improbable.Gdk.Core.CustomSpatialOSSendSystem<T>` class (where `T` is a SpatialOS component). Note that this disables the standard replication for `T` and ensures the system runs at the correct point in the update lifecycle. For more information about the update lifecycle see [System Update Order](./system-update-order.md).

* Handle replication of properties:

    If you write a custom replication system that works with properties, it needs to handle the testing and setting of the `DirtyBit` explictly, because standard replication won't happen. This means that you must manually set `DirtyBit` back to `false`. See [TransformSendSystem.cs](../../../workers/unity/Packages/com.improbable.gdk.transformsynchronization/Systems/TransformSendSystem.cs) for an example.

* Handle replication of events:

    If you write a custom replication system for a component that has events, it needs to take ownership of sending events and clearing the buffer.

* Register the system:

    You need to register a custom replication system by attaching the system to a world. For example:

    ```csharp
    // This means the default replication code will not be executed.
    World.GetOrCreateManager<TransformSendSystem>();
    ```

### Examples

Here's an example of a custom replication system for a `Transform` component:

```csharp
public class TransformSendSystem : CustomSpatialOSSendSystem<Transform.Component>
{
    private struct TransformData
    {
        public readonly int Length;
        public ComponentDataArray<Transform.Component> Transforms;
        [ReadOnly] public ComponentDataArray<Authoritative<Transform.Component>> TransformAuthority;
        [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
    }

    [Inject] private TransformData transformData;

    // Number of transform sends per second.
    private const float SendRateHz = 30.0f;

    private float timeSinceLastSend = 0.0f;

    protected override void OnUpdate()
    {
        // Send update at SendRateHz.
        timeSinceLastSend += Time.deltaTime;
        if (timeSinceLastSend < 1.0f / SendRateHz)
        {
            return;
        }

        timeSinceLastSend = 0.0f;

        for (var i = 0; i < transformData.Length; i++)
        {
            var component = transformData.Transforms[i];

            if (component.DirtyBit != true)
            {
                continue;
            }

            var entityId = transformData.SpatialEntityIds[i].EntityId;

            var update = new SchemaComponentUpdate(component.ComponentId);
            Generated.Improbable.Transform.Transform.Serialization.Serialize(component,
                update.GetFields());
            worker.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));

            component.DirtyBit = false;
            transformData.Transforms[i] = component;
        }
    }
}
```

> **Note**: You need to create the `SchemaComponentUpdate` object with the correct component ID. We provide serialization methods to add data to this object automatically.

Here's an example custom replication system for a component called `CubeColor`. The component has one event called `change_color` of the type `ColorData` and has no fields.

```csharp
public class CubeColorSendSystem : CustomSpatialOSSendSystem<CubeColor.Component>
{
    public struct ColorData
    {
        public readonly int Length;
        public ComponentDataArray<CubeColor.Component> CubeColors;
        public ComponentDataArray<CuberColor.EventSender.ChangeColor> ChangeColorEventSenders;
        public ComponentDataArray<Authoritative<CubeColor.Component>> CubeColorAuthority;
        public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
    }

    [Inject] private CubeColorData cubeColorData;

    protected override void OnUpdate()
    {
        for (var i = 0; i < cubeColorData.Length; i++)
        {
            var entityId = cubeColorData.SpatialEntityIds[i];
            var component = cubeColorData.CubeColors[i];
            var changeColorEvents = cubeColorData.ChangeColorEventSenders[i].Events;

            if(changeColorEvents.Count == 0)
            {
                continue;
            }

            var update = new SchemaComponentUpdate(component.ComponentId);
            var eventsObj = update.GetEvents();
            foreach(var event in changeColorEvents)
            {
                var eventObj = eventsObj.AddObject(1); // NOTE: 1 corresponds to the event index in schema.
                global::Generated.Playground.ColorData.Serialization.Serialize(event, eventObj)
            }

            worker.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));
            changeColorEvents.Clear();
        }
    }
}
```

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).
