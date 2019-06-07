<%(TOC)%>

# ECS: Events

Events are one of the possible things contained in a [SpatialOS component](https://docs.improbable.io/reference/latest/shared/glossary#component). Unlike properties, they're transient, so (effectively) they let a SpatialOS entity broadcast a transient message about something that has happened to it.

Events are for broadcasting information between worker instances about a transient occurrence relating to a particular SpatialOS entity. Only the worker instance with authority over the relevant SpatialOS component can send an event, but any worker instance interested in the component receives the event.

> For more information about what events are and what their purpose is, see [this section on events](https://docs.improbable.io/reference/latest/shared/design/object-interaction#events) in the SpatialOS documentation.


## Sending events

A worker instance can send an event using the `SendEvent` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system).

The generated code from your schema will include a `{component name}.{event name}.Event` type, which needs to be constructed and given a payload when sending events.

The `SendEvent` method requires you to pass in a populated event, and the entity ID of the entity that you want to trigger the event on.

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

The `GetEventsReceived<T>` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system) allows you to retrieve a list of all the events, given the type `T`, that have been received since the last tick.

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
