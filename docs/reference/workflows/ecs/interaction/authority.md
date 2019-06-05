<%(TOC)%>

# ECS: Authority

 _This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/overview.md)._

**Authority** is how SpatialOS represents which worker instances can write to each specific [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component).

> If you don't know about authority, you should read [Understanding access](https://docs.improbable.io/reference/latest/shared/design/understanding-access) in the SpatialOS documentation.
>
> Crucially: at runtime, your worker instance may or may not have authority over a given SpatialOS component.

## How authority is represented in Unity's ECS

### Authority shared components

For every [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component) attached to a SpatialOS entity, the GDK attaches an _ECS shared component_ (an ECS `ISharedComponentData`) to the ECS entity. This component is called `ComponentName.ComponentAuthority`, where `ComponentName` is the name of the SpatialOS component.

This component contains a single field, `HasAuthority`, a `bool` that indicates whether the worker instance has authority over the SpatialOS component.

> Note that this component does not contain information about `AuthorityLossImminent` notifications. For information on how a worker instance gets notified about these, see [Authority reactive components](#authority-reactive-components) below.

### Authority reactive components

For every [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component) attached to a SpatialOS entity, the SpatialOS GDK for Unity (GDK) attaches a corresponding _ECS component tag_ (an ECS `IComponentData` component with no fields) to the ECS entity. We call these "authority tags".

The authority tags in the GDK are (where `T` is a [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component)):

* Whether or not the worker instance has authority:
    * [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative): the worker instance has authority over the SpatialOS component
    * [`NotAuthoritative<T>`]({{urlRoot}}/api/reactive-components/not-authoritative): the worker instance does not have authority over the SpatialOS component

    > For each SpatialOS component, there will only be one of these tags at a time
* If you're using `AuthorityLossImminent` notifications, when there is an [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative) tag, you could also have:
    * [`AuthorityLostImminent<T>`]({{urlRoot}}/api/reactive-components/authority-loss-imminent): the worker instance will shortly lose authority over the SpatialOS component

        For more information on `AuthorityLossImminent`, [see the SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/design/understanding-access#enabling-and-configuring-authoritylossimminent-notifications).

> This is _different_ to the [behaviour around AuthorityLossImminent notifications in the SpatialOS SDKs](https://docs.improbable.io/reference/latest/shared/design/understanding-access#authority-states), where Authority can only be in one of three states at a time.

Here's an example of how to write a system that runs when a worker instance has authority over the `Position` SpatialOS component:

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

> This section is just to tell you how the system works: all of this is handled automatically by the GDK, and you don't need to do anything to ensure that authority is correctly registered in authority tags.

When a SpatialOS entity enters the [worker instance's view]({{urlRoot}}/reference/glossary#worker-s-view), the GDK:

- creates an ECS entity to correspond to that SpatialOS entity
- for each SpatialOS component `T` on the SpatialOS entity:
    - attaches an ECS component to the ECS entity representing the SpatialOS component `T`
    - attaches a [`NotAuthoritative<T>`]({{urlRoot}}/api/reactive-components/not-authoritative) authority tag to represent the worker instance's authority over the SpatialOS component `T`

This means entities _may_ initially have [`NotAuthoritative<T>`]({{urlRoot}}/api/reactive-components/not-authoritative) attached for a tick, even when the worker instance is actually authoritative over `T`, because the `AuthorityChangeOp` hasn't arrived yet.

### What happens when a worker instance gains or loses authority over a SpatialOS entity

When authority changes over a SpatialOS component the GDK automatically adds an [`AuthorityChanges<T>`]({{urlRoot}}/api/reactive-components/authority-changes) reactive ECS component to the corresponding ECS entity (where `T` is the component that changed authority).

[`AuthorityChanges<T>`]({{urlRoot}}/api/reactive-components/authority-changes) holds a list of all authority changes that have happened to that SpatialOS component since the last tick.

Here's an example of doing something when a worker instance gets authority over a SpatialOS component:

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

The GDK automatically adds and removes authority tags: see [Reactive components]({{urlRoot}}/reference/workflows/ecs/reactive-components) for more information.
