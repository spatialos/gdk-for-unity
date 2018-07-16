**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

## Writing a custom replication system

### What the Unity GDK's replication system does

By default, the Unity GDK automatically replicates ECS components to SpatialOS whenever you modify an ECS component (that corresponds to a SpatialOS component)'s properties.

#### For properties

Each ECS component has an internal bool named `DirtyBit`. When a worker sets any property of a SpatialOS component, in the corresponding ECS component, the Unity GDK sets `DirtyBit` to `true`. The `SpatialOSSendSystem`, which runs at the end of every frame, then checks the `DirtyBit` of each ECS component. If `DirtyBit` is `true`, the Unity GDK pushes a SpatialOS component update and sets `DirtyBit` back to `false`.

#### For events

When a worker sends a SpatialOS event, the Unity GDK puts the event object into an internal buffer. When it's time to replicate a component, the GDK sends all buffered events and clears the buffer.

### Writing your own replication system

If some ECS components need more complex replication logic, you can create custom replication systems on a per-component basis. To do this:

* Your custom replication system must extend the `CustomSpatialOSSendSystem<T>` class (where `T` is a SpatialOS component). Note that this will disable the standard replication for `T`.

* Handle replication of properties:

    If you write a custom replication system that works with properties, it needs to handle the testing and setting of the `DirtyBit` explictly, because standard replication won't happen. This means that you must manually set `DirtyBit` back to `false`. See [TransformSendSystem.cs](../../workers/unity/Assets/Gdk/TransformSynchronization/Systems/TransformSendSystem.cs) for an example.

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
[UpdateInGroup(typeof(UpdateGroupSpatialOSSend))]
[UpdateBefore(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class TransformSendSystem : CustomSpatialOSSendSystem<SpatialOSTransform>
{
    public struct TransformData
    {
        public int Length;
        public ComponentDataArray<SpatialOSTransform> Transforms;
        public ComponentDataArray<Authoritative<SpatialOSTransform>> TransformAuthority;
        public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
    }

    [Inject] private TransformData transformData;

    protected override void OnUpdate()
    {
        if (World.GetExistingManager<TickSystem>().GlobalTick % 2 != 0) //Update every other tick
        {
            return;
        }

        for (var i = 0; i < transformData.Length; i++)
        {
            var component = transformData.Transforms[i];

            if (component.DirtyBit != true)
            {
                continue;
            }

            var entityId = transformData.SpatialEntityIds[i].EntityId;

            var update = new global::Improbable.Transform.Transform.Update(); // Generated type from Worker SDK 
            update.SetLocation(global::Generated.Improbable.Transform.Location.ToSpatial(component.Location));
            update.SetRotation(global::Generated.Improbable.Transform.Quaternion.ToSpatial(component.Rotation));
            update.SetTick(component.Tick);

            SpatialOSTransformTranslation.SendComponentUpdate(worker.Connection, entityId, update);

            component.DirtyBit = false;
            transformData.Transforms[i] = component;
        }
    }
}
```

> **Note**: The update objects are generated types from the [SpatialOS C# SDK](https://docs.improbable.io/reference/latest/csharpsdk/introduction).

Here's an example custom replication system for a component called `CubeColor`. The component has one event called `change_color` of the type `ColorData`.

```csharp
[UpdateInGroup(typeof(UpdateGroupSpatialOSSend))]
[UpdateBefore(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class CubeColorSendSystem : CustomSpatialOSSendSystem<SpatialOSCubeColor>
{
    public struct ColorData
    {
        public int Length;
        public ComponentDataArray<SpatialOSCubeColor> CubeColors;
        public ComponentDataArray<EventSender<SpatialOSCubeColor>> CubeColorEventSenders;
        public ComponentDataArray<Authoritative<SpatialOSCubeColor>> CubeColorAuthority;
        public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
    }

    [Inject] private CubeColorData cubeColorData;

    protected override void OnUpdate()
    {
        for (var i = 0; i < cubeColorData.Length; i++)
        {
            var entityId = cubeColorData.SpatialEntityIds[i];
            var eventSender = cubeColorData.CubeColorEventSenders[i];
            var changeColorEvents = eventSender.GetChangeColorEvents();

            if(changeColorEvents.Count == 0)
            {
                continue;
            }

            var update = new global::DemoGame.CubeColor.Update();

            foreach(var event in changeColorEvents)
            {
                update.changeColor.Add(global::Generated.DemoGame.ColorData.ToSpatial(event));
            }

            SpatialOSCubeColorTranslation.SendComponentUpdate(worker.Connection, entityId, update);

            eventSender.ClearChangeColorEvents();
        }
    }
}
```

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).
