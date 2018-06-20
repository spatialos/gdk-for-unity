**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Receiving updates from SpatialOS: reactive components

To represent state changes or messages from SpatialOS, the Unity GDK uses something we're calling "reactive components": ECS components that it adds to the relevant ECS entity for the duration of a tick.

When the Unity GDK receives an update or message from SpatialOS, it places a "reactive component" on the associated ECS entity until the end of the tick. This reactive component contains a list of all the updates or messages received, so they can be processed by any system that you want to react to the change or message.

At the end of the tick, the Unity GDK removes the reactive component. 

### Reactive component types

Reactive components inherit from `MessagesReceived<T>` (where `T` is the generated component type associated with the message or the state update). `MessagesReceived<T>` gives you the field `Buffer`, a list of all currently stored messages.

These are the types of reactive component available:

1. `ComponentsUpdated`:  All local and received [SpatialOS component updates](https://docs.improbable.io/reference/13.0/shared/design/operations#component-related-operations) for the current SpatialOS entity, except the initial value.
2. `AuthoritiesChanged`: Updates to the [authority](https://docs.improbable.io/reference/13.0/shared/design/understanding-access#understanding-read-and-write-access-authority) the current worker instance has over a SpatialOS component. See [Authority](authority.md) for information on how this works.
3. `EventsReceived`: All received [events](https://docs.improbable.io/reference/13.0/shared/design/object-interaction#events) for the current entity. See [Events](events.md) for information on how this works.
4. `CommandRequests`: All received [command](https://docs.improbable.io/reference/13.0/shared/design/commands) requests. See [Commands](commands.md) for information on how this works.
5. `CommandResponses`: All received [command](https://docs.improbable.io/reference/13.0/shared/design/commands) responses. See [Commands](commands.md) for information on how this works.

Here's an example of a system using a reactive component:

```csharp
public class ReactiveSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<ComponentsUpdated<SpatialOSPosition>> PositionUpdated;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var componentUpdates = data.PositionUpdated[i].Buffer;
            foreach (var componentUpdate in componentUpdates) 
            {
                // Do something
            }            
        }
    }
}
```

### Removal of reactive components

The Unity GDK automatically removes reactive components from the ECS entity as soon as `CleanReactiveComponentsSystem` is run. This means that you must run any logic processing that reactive component _before_ `CleanReactiveComponentsSystem`. (This system is run at the end of each frame.)


----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).