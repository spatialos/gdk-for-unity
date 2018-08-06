**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Sending and receiving events

Events are one of the possible things contained in a [SpatialOS component](https://docs.improbable.io/reference/13.0/shared/glossary#component). Unlike properties, they're transient, so (effectively) they let a SpatialOS entity broadcast a transient message about something that has happened to it.

Events are for broadcasting information between worker instances about a transient occurrence relating to a particular SpatialOS entity. Only the worker instance with authority over the relevant SpatialOS component can send an event.

> For more information about what events are and what their purpose is, see [this section on events](https://docs.improbable.io/reference/13.0/shared/design/object-interaction#events) in the SpatialOS documentation.

### Sending events

A worker instance can send an event using a `EventSender<T>` ECS component (where `T` is the SpatialOS component that the event is defined in).

For each SpatialOS component containing an event, the Unity GDK automatically attaches an `EventSender<T>` ECS component when (and only when) the worker instance has authority over the SpatialOS component `T`.

For each event in the SpatialOS component, there will be a corresponding method to send that event.

Example schema:

```
package playground;

enum Color {
    YELLOW = 0;
    GREEN = 1;
    BLUE = 2;
    RED = 3;
}

type ColorData {
    Color color = 1;
}

component CubeColor {
    id = 12000;
    event ColorData change_color;
}
```

Given the example schema, the Unity GDK generates these types:

* `ColorData` - Equivalent of the schema type.
* `ChangeColorEvent` - The corresponding type for the event which contains a `ColorData` within it. This type exists to distinguish between multiple events that use the same type.

The Unity GDK attaches `EventSender<SpatialOSCubeColor>` to all ECS entities that have a `CubeColor` SpatialOS component that the worker instance has authority over. See [Authority](authority.md) for more on how authority works in the Unity GDK.

On the `EventSender<SpatialOSCubeColor>` ECS component, there is a `SendColorChangeEvent(ColorData colorData)` method which sends a `change_color` event when called.

```csharp
public class SendChangeColorEvent : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<CubeColor> Color;
        public ComponentDataArray<EventSender<CubeColor>> CubeColorEventSender;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var eventSender = data.CubeColorEventSender[i];

            var colorData = new ColorData
            {
                Color = Color.GREEN
            };

            eventSender.SendChangeColorEvent(colorData);
        }
    }
}
```

### Receiving events

<!-- TODO explain that events are propagated? -->

When a worker instance receives an event, this is represented with reactive ECS components. 

For SpatialOS entity that the event was sent on, the Unity GDK attaches an `EventsReceived<T>` component to the corresponding ECS entity (where `T` is the generated type associated with the event). The ECS component holds a list of `T`, each one being an event.

Given the same schema as above, `change_color` events are stored in an `EventsReceived<ChangeColorEvent>` and have a list of `ChangeColorEvent`s. Each `ChangeColorEvent` contains a `Payload`, which is a `ColorData` object.

> **Note**: `EventsReceived<T>` is a `Component`, not an `IComponentData`. This means you must use a `ComponentArray<T>` for injection rather than a `ComponentDataArray<T>`.

Here's an example of receiving an event so the worker instance can respond to it:

```csharp
public class ChangeColorEventReceiveSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentArray<EventsReceived<ChangeColorEvent>> ChangeColorEvents;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var events = data.ChangeColorEvents[i];

            foreach (var ev in events.Buffer)
            {
                var colorData = ev.Payload;

                // Do something with the event
            }
        }
    }
}
```

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).