**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Custom replication systems

By default in the Unity GDK, components are automatically replicated to SpatialOS whenever a component's fields are modified. However if some components require more complex replication logic, the Unity GDK supports creating custom replication systems on a per component basis by extending the `CustomSpatialOSSendSystem<T>` class, where `T` is a component.

For standard replication, each component has an internal bool named `DirtyBit`. When any property within the component is set, the dirty bit is set to `true`. The `SpatialOSSendSystem` then checks the `DirtyBit` of each component. If set to `true`, a component update is pushed and `DirtyBit` is set to `false`.

A custom replication system needs to handle the testing and setting of the `DirtyBit` explictly since the standard replication is not being executed.

Example of a custom replication system for a `Transform` component:

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

Note: The update objects are generated types from the C# Worker SDK.

To register a custom replication system, attach this system to a world and the default replication code will not be executed.

```
World.GetOrCreateManager<TransformSendSystem>();
```

### Custom Replication with Events

In standard replication, when an event is sent the event object is put into an internal buffer. When it comes time to replicate a component, all buffered events are sent and the buffer is cleared. A custom replication system needs to take ownership of sending events and clearing this buffer.

Below is an example custom replication system for a component called `CubeColor` with one event called `change_color` which has the type `ColorData`.

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