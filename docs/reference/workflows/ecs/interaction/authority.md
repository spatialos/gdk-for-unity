<%(TOC)%>

# ECS: Authority

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Understanding access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)
* [Authority](https://docs.improbable.io/reference/latest/shared/concepts/interest-authority#authority-also-known-as-write-access)
")%>

**Authority** is how SpatialOS represents which worker instances can write to each specific [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component).

It is crucial to note that at runtime, your worker instance may or may not have authority over a given SpatialOS component.

## How authority is represented in Unity's ECS

For every [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component) attached to a SpatialOS entity, the GDK attaches a shared component (that implements `ISharedComponentData`) to the ECS entity. This component is called `ComponentName.ComponentAuthority`, where `ComponentName` is the name of the SpatialOS component.

This component contains a single field, `HasAuthority`, a `bool` that indicates whether the worker instance has authority over the SpatialOS component.

> Note that this component does not contain information about `AuthorityLossImminent` notifications.

Below is an example of how to write a system that runs when a worker instance has authority over the `Position` SpatialOS component:

```csharp
public class AuthoritativePositionSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Position.Component>(),
            ComponentType.ReadOnly<Position.ComponentAuthority>()
        );
        query.SetFilter(Position.ComponentAuthority.Authoritative);
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach((ref Position.Component position) =>
        {
            // This will only run when the worker instance is authoritative over a Position component.
        });
    }
}
```

## What happens when a SpatialOS entity enters a worker instance's view

When a SpatialOS entity enters the [worker instance's view]({{urlRoot}}/reference/glossary#worker-s-view), the GDK:

- creates an ECS entity to correspond to that SpatialOS entity
- attaches an ECS component to the ECS entity for each SpatialOS component on the SpatialOS entity

## What happens when a worker instance gains or loses authority over a SpatialOS entity

The `GetAuthorityChangesReceived` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system) allows you to retrieve a list of all the authority changes that have occured since the last tick, given an entity ID and a component ID.

Below is an example of doing something when a worker instance gains authority over a SpatialOS component:

```csharp
public class OnPlayerSpawnSystem : ComponentSystem
{
    private ComponentUpdateSystem updateSystem;

    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

        query = GetEntityQuery(
            ComponentType.ReadOnly<SpatialEntityId>(),
            ComponentType.ReadWrite<PlayerInput.Component>(),
            ComponentType.ReadOnly<PlayerInput.ComponentAuthority>()
        );
        query.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
    }

    protected override void OnUpdate()
    {
        // We iterate over all entities with the PlayerInput Component that we are authoritative over.
        Entities.With(query).ForEach(
            (ref SpatialEntityId spatialEntityId, ref PlayerInput.Component playerInput) =>
            {
                var authorityChanges = updateSystem.GetAuthorityChangesReceived(
                    spatialEntityId.EntityId,
                    PlayerInput.ComponentId);

                // Skip if there were no authority changes.
                if (authorityChanges.Count <= 0)
                {
                    return;
                }

                for (var i = 0; i < authorityChanges.Count; i++)
                {
                    // In here we iterate through entities with a PlayerInput component
                    // which we have authority over, and where there have been changes
                    // of authority. Therefore, write code here that you want to run
                    // when the worker instance receives authority over PlayerInput.
                }
            });
    }
}
```
