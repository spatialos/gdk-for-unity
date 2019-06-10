<%(TOC)%>

# ECS: Events

<%(Callout message="
Before reading this document, make sure you are familiar with

  * [SpatialOS events](https://docs.improbable.io/reference/latest/shared/design/object-interaction#events)
")%>

## Sending events

A worker instance can send an event using the `SendEvent` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system).

You must instantiante and populate an instance of the event payload, the type of which is `{component name}.{event name}.Event`, when sending the event.

The `SendEvent` method requires you to pass in this populated event, and the entity ID of the entity that you want to trigger the event on.

> **Note:** You can only send events from entity-component pairs which your worker instance is authoritative over. Make sure to filter your ECS query for authority as shown below.

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
            ComponentType.ReadOnly<CubeColor>(),
            ComponentType.ReadOnly<CubeColor.ComponentAuthority>()
        );

        query.SetFilter(CubeColor.ComponentAuthority.Authoritative);
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

The `GetEventsReceived<T>` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system) allows you to retrieve a list of all the events, given the event type `T`, that have been received since the last tick.

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
