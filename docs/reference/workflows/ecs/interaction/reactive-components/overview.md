<%(TOC)%>

# ECS: Reactive components

<%(Callout type="warn" message="
**Please note that reactive components will be removed in a future release. We recommend that you do not use them.**

Please also note that the FPS and Blank starter projects disable this functionality by default, as reactive components have a **significant overhead** and **noticeable performance degradation** over time.

You can enable reactive components and their related systems by adding `USE_LEGACY_REACTIVE_COMPONENTS` to your **Scripting Define Symbols**.
")%>

When the GDK receives an update or message from SpatialOS, a _reactive component_  is placed on the associated ECS entity at the start of the frame. A reactive component contains a list of all the updates or messages received, so they can be processed by any system that you want to _react_ to the change or message.

Reactive components are removed by the GDK at the end of each frame, as they are implemented as a [temporary component]({{urlRoot}}/reference/workflows/ecs/concepts/temporary-components).

## Reactive component types

These are the types of reactive component available:

* `AuthorityChanges`: All updates to the [authority](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/authority-and-interest/introduction) the current worker instance has over a SpatialOS component.
* `ReceivedUpdates`:  All received [SpatialOS component updates](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/operations#component-related-operations) for the current SpatialOS entity.
* `ReceivedEvents`: All received [events](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/object-interaction#events) for the current entity.
* `CommandRequests`: All received [command](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/commands) requests.
* `CommandResponses`: All received [command](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/commands) responses.
* `ComponentAdded<T>`: Denotes that a SpatialOS component of type `T` has been added to this entity.
* `ComponentRemoved<T>`: Denotes that a SpatialOS component of type `T` has been removed to this entity.

## Example of a system using a reactive component

The following snippet shows how to retrieve and iterate through all the `PlayerCollided` events received in a frame. You can iterate through the matching components using the ECS `.ForEach` syntax.

```csharp
public class ReactiveSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Collisions.ReceivedEvents.PlayerCollided>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref Collisions.ReceivedEvents.PlayerCollided playerCollisionEvents) =>
            {
                foreach (var playerCollision in playerCollisionEvents.Events)
                {
                    // Do something
                }
            });
    }
}
```

## Removal of reactive components

The GDK automatically removes reactive components from the ECS entity as soon as the `CleanReactiveComponentsSystem` is run at the end of each frame. This means that you must run any logic for processing that reactive component _before_ `CleanReactiveComponentsSystem`.


