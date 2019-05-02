<%(TOC)%>

# ECS: Events
 _This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/overview)._


Events are one of the possible things contained in a [SpatialOS component](https://docs.improbable.io/reference/latest/shared/glossary#component). Unlike properties, they're transient, so (effectively) they let a SpatialOS entity broadcast a transient message about something that has happened to it.

Events are for broadcasting information between worker instances about a transient occurrence relating to a particular SpatialOS entity. Only the worker instance with authority over the relevant SpatialOS component can send an event.

> For more information about what events are and what their purpose is, see [this section on events](https://docs.improbable.io/reference/latest/shared/design/object-interaction#events) in the SpatialOS documentation.

## Sending events

A worker instance can send an event using a `ComponentName.EventSenders.EventName` ECS component, where `ComponentName` is the name of the component that the event is defined in, and `EventName` is the name of the event in schema.

For each SpatialOS component containing an event, the SpatialOS GDK for Unity (GDK) attaches a `ComponentName.EventSenders.EventName` ECS component when (and only when) the worker instance has authority over the SpatialOS component `ComponentName`.

For each event in the SpatialOS component, there will be a corresponding method to send that event.

Example schema:

```schemalang
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

Given the example schema, the GDK generates these types:

* `ColorData` - Equivalent of the schema type.
* `CubeColor.EventSenders.ChangeColor` - The event sender type.

The GDK attaches `CubeColor.EventSenders.ChangeColor` to all ECS entities that have a `CubeColor` SpatialOS component that the worker instance has authority over. See [Authority]({{urlRoot}}/reference/workflows/ecs/interaction/authority) for more on how authority works in the GDK.

On the `CubeColor.EventSenders.ChangeColor` ECS component, there is a list of type `ColorData`. To send an event, add a `ColorData` struct to the list.

```csharp
public class SendChangeColorEvent : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<CubeColor> Color;
        public ComponentDataArray<CubeColor.EventSenders.ChangeColor> ChangeColorEventSender;
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

            eventSender.Events.Add(colorData);
        }
    }
}
```

## Receiving events

<!-- TODO explain that events are propagated? -->

When a worker instance receives an event, this is represented with reactive ECS components.

For the SpatialOS entity that the event was sent on, the GDK attaches a `ComponentName.ReceivedEvents.EventName` component to the corresponding ECS entity, where `ComponentName` is the name of the component that the event is defined in, and `EventName` is the name of the event in schema.

Given the same schema as above, `change_color` events are stored in a list of `ColorData`s on a `CubeColor.ReceivedEvents.ChangeColor` component.

Here's an example of receiving an event so the worker instance can handle it:

```csharp
public class ChangeColorEventReceiveSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<CubeColor.ReceivedEvents.ChangeColor> ChangeColorEvents;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var events = data.ChangeColorEvents[i];

            foreach (var colorData in events.Events)
            {
                // Do something with the payload
            }
        }
    }
}
```
