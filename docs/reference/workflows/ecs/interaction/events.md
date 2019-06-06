<%(TOC)%>

# ECS: Events

Events are one of the possible things contained in a [SpatialOS component](https://docs.improbable.io/reference/latest/shared/glossary#component). Unlike properties, they're transient, so (effectively) they let a SpatialOS entity broadcast a transient message about something that has happened to it.

Events are for broadcasting information between worker instances about a transient occurrence relating to a particular SpatialOS entity. Only the worker instance with authority over the relevant SpatialOS component can send an event, but any worker instance interested in the component receives the event.

> For more information about what events are and what their purpose is, see [this section on events](https://docs.improbable.io/reference/latest/shared/design/object-interaction#events) in the SpatialOS documentation.


## Sending events

A worker instance can send an event using the `SendEvent` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system).

It requires you to pass in a populated event struct of type `{component name}.{event name}.Event`, and the entity ID of the entity that you want to trigger the event on.

<%(#Expandable title="Example schema")%>

The code generated from the below schema will define a `CubeColor.ChangeColor.Event` struct, which needs to be constructed when sending `change_color` events. The struct type can also be used to determine the type of events to watch for by event receivers.

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

<%(/Expandable)%>

```csharp


public class SendChangeColorEvent : ComponentSystem
{
    private ComponentUpdateSystem componentUpdateSystem;

    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        componentUpdateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

        query = GetEntityQuery(
            ComponentType.ReadOnly<SpatialEntityId>(),
            ComponentType.ReadOnly<CubeColor>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach((ref SpatialEntityId entityId, CubeColor cubeColor) =>
        {
            componentUpdateSystem.SendEvent(
                new CubeColor.ChangeColor.Event(new ColorData(Color.GREEN)),
                entityId.EntityId);
        });
    }
}
```

## Receiving events

<!-- TODO explain that events are propagated? -->

The `GetEventsReceived<T>` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system) allows you to retrieve a list of all the events of a given type that have been received since the last tick.

The example below shows how to use this method to handle events a worker receives.

```csharp
public class ChangeColorEventReceiveSystem : ComponentSystem
{
    private ComponentUpdateSystem componentUpdateSystem;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        componentUpdateSystem = World.GetExistingSystem<ComponentUpdateSystem>();
    }

    protected override void OnUpdate()
    {
        var changeColorEvents = componentUpdateSystem.GetEventsReceived<CubeColor.ChangeColor.Event>();

        for (var i = 0; i < changeColorEvents.Count; i++)
        {
            var colorData = changeColorEvents[i].Event.Payload;

            // Do something with the payload
        }
    }
}
```
