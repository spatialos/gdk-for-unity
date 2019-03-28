<%(TOC)%>
# ECS: Authority
 _This document relates to the [ECS workflow](\{\{urlRoot\}\}/reference/workflows/which-workflow.md)._

**Authority** is how SpatialOS represents which worker instances can write to each specific [SpatialOS component](\{\{urlRoot\}\}/reference/glossary#spatialos-component).

> If you don't know about authority, you should read [Understanding access](https://docs.improbable.io/reference/latest/shared/design/understanding-access) in the SpatialOS documentation.
>
> Crucially: at runtime, your worker instance may or may not have authority over a given SpatialOS component.

## How authority is represented in Unity's ECS

### Authority shared components

For every [SpatialOS component](\{\{urlRoot\}\}/reference/glossary#spatialos-component) attached to a SpatialOS entity, the GDK attaches an _ECS shared component_ (an ECS `ISharedComponentData`) to the ECS entity. This component is called `ComponentName.ComponentAuthority`, where `ComponentName` is the name of the SpatialOS component.

This component contains a single field, `HasAuthority`, a `bool` that indicates whether the worker instance has authority over the SpatialOS component.

> Note that this component does not contain information about `AuthorityLossImminent` notifications. For information on how a worker instance gets notified about these, see [Authority reactive components](#authority-reactive-components) below.

### Authority reactive components

For every [SpatialOS component](\{\{urlRoot\}\}/reference/glossary#spatialos-component) attached to a SpatialOS entity, the SpatialOS GDK for Unity (GDK) attaches a corresponding _ECS component tag_ (an ECS `IComponentData` component with no fields) to the ECS entity. We call these "authority tags".

The authority tags in the GDK are (where `T` is a [SpatialOS component](\{\{urlRoot\}\}/reference/glossary#spatialos-component)):

* Whether or not the worker instance has authority:
    * `Authoritative<T>`: the worker instance has authority over the SpatialOS component
    * `NotAuthoritative<T>`: the worker instance does not have authority over the SpatialOS component

    > For each SpatialOS component, there will only be one of these tags at a time
* If you're using `AuthorityLossImminent` notifications, when there is an `Authoritative<T>` tag, you could also have:
    * `AuthorityLossImminent<T>`: the worker instance will shortly lose authority over the SpatialOS component

        For more information on `AuthorityLossImminent`, [see the SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/design/understanding-access#enabling-and-configuring-authoritylossimminent-notifications).

> This is _different_ to the [behaviour around AuthorityLossImminent notifications in the SpatialOS SDKs](https://docs.improbable.io/reference/latest/shared/design/understanding-access#authority-states), where Authority can only be in one of three states at a time.

Here's an example of how to write a system that runs when a worker instance has authority over the `Position` SpatialOS component:

```csharp
public class AuthoritativePositionSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        // An ECS component representing the SpatialOS component Position.
        public ComponentDataArray<Position.Component> Position;
        // An ECS component tag representing the worker's authority over the SpatialOS component Position.
        public ComponentDataArray<Authoritative<Position.Component>> PositionAuthority;
    }

    [Inject] Data data;

    // This system will only run where the worker instance has authority over the SpatialOS component Position.
    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            // This will run when the worker instance is authoritative over a Position component.
        }
    }
}
```

## What happens when a SpatialOS entity enters a worker instance's view

> This section is just to tell you how the system works: all of this is handled automatically by the GDK, and you don't need to do anything to ensure that authority is correctly registered in authority tags.

When a SpatialOS entity enters the [worker instance's view](\{\{urlRoot\}\}/reference/glossary#worker-s-view), the GDK:

- creates an ECS entity to correspond to that SpatialOS entity
- for each SpatialOS component `T` on the SpatialOS entity:
    - attaches an ECS component to the ECS entity representing the SpatialOS component `T`
    - attaches  a `NotAuthoritative<T>` authority tag to represent the worker instance's authority over the SpatialOS component `T`

This means entities _may_ initially have `NotAuthoritative<T>` attached for a tick, even when the worker instance is actually authoritative over `T`, because the `AuthorityChangeOp` hasn't arrived yet.

### What happens when a worker instance gains or loses authority over a SpatialOS entity

When authority changes over a SpatialOS component the GDK automatically adds an `AuthorityChanges<T>` reactive ECS component to the corresponding ECS entity (where `T` is the component that changed authority).

`AuthorityChanges<T>` holds a list of all authority changes that have happened to that SpatialOS component since the last tick.

Here's an example of doing something when a worker instance gets authority over a SpatialOS component:

```csharp
public class OnPlayerSpawnSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<PlayerInput.Component> PlayerInput;
        public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerInputAuthority;
        // This system  only runs when there has been a change of authority over PlayerInput in the last tick
        public ComponentDataArray<AuthorityChanges<PlayerInput.Component>> PlayerInputAuthorityChange;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            // The data being injected is for being authoritative over the 
            // SpatialOS component PlayerInput, and for changes to authority.
            // So, write code here that you want to run when the worker instance 
            // receives authority over PlayerInput
        }
    }
}
```

The GDK automatically adds and removes authority tags: see [Reactive components](\{\{urlRoot\}\}/reference/workflows/ecs/reactive-components) for more information.
