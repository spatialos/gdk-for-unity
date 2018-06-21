**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

----

## Authority

**Authority** is how SpatialOS represents which worker instances can write to each specific [SpatialOS component](https://docs.improbable.io/reference/13.0/shared/glossary#component).

> If you don't know about authority, you should read [Understanding authority](https://docs.improbable.io/reference/13.0/shared/design/understanding-access) in the SpatialOS documentation.
> 
> Crucially: at runtime, your worker instance may or may not have authority over a given SpatialOS component.

### How authority is represented in Unity's ECS

For every [SpatialOS component](https://docs.improbable.io/reference/13.0/shared/glossary#component) attached to a SpatialOS entity, the Unity GDK attaches a corresponding _ECS component tag_ (an ECS `IComponentData` component with no fields) to the ECS entity. We call these "authority tags".

The authority tags in the Unity GDK are (where `T` is a [SpatialOS component](https://docs.improbable.io/reference/13.0/shared/glossary#component)):

* Whether or not the worker instance has authority:
    * `Authoritative<T>`: the worker instance has authority over the SpatialOS component
    * `NotAuthoritative<T>`: the worker instance does not have authority over the SpatialOS component

    > For each SpatialOS component, there will only be one of these tags at a time
* If you're using `AuthorityLossImminent` notifications, when there is an `Authoritative<T>` tag, you could also have:
    * `AuthorityLossImminent<T>`: the worker instance will shortly lose authority over the SpatialOS component

        For more information on `AuthorityLossImminent`, [see the SpatialOS documentation](https://docs.improbable.io/reference/13.0/shared/design/understanding-access#enabling-and-configuring-authoritylossimminent-notifications).

> **Note**: This is _different_ to the [behaviour around AuthorityLossImminent notifications in the SpatialOS SDKs](https://docs.improbable.io/reference/13.0/shared/design/understanding-access#authority-states), where Authority can only be in one of three states at a time.

Here's an example of how to write a system that runs when a worker instance has authority over the SpatialOS component Position: 

```csharp
public class AuthoritativePositionSystem : ComponentSystem
{
    public struct Data
    {
        // This system will only run where the worker instance has authority over the SpatialOS component Position.

        public int Length;
        // An ECS component representing the SpatialOS component Position.
        public ComponentDataArray<SpatialOSPosition> Position; 
        // An ECS component tag representing the worker instance's authority over the SpatialOS component Position.
        public ComponentDataArray<Authoritative<SpatialOSPosition>> PositionAuthority; 
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

### What happens when a SpatialOS entity enters a worker instance's view

> This section is just to tell you how the system works: all of this is handled automatically by the Unity GDK, and you don't need to do anything to ensure that authority is correctly registered in authority tags.

When a SpatialOS entity enters the worker instance's view, the Unity GDK:

- creates an ECS entity to correspond to that SpatialOS entity
- for each SpatialOS component `T` on the SpatialOS entity:
    - attaches an ECS component to the ECS entity representing the SpatialOS component `T`
    - attaches  a `NotAuthoritative<T>` authority tag to represent the worker instance's authority over the SpatialOS component `T`

This means entities _may_ initially have `NotAuthoritative<T>` attached for a tick, even when the worker instance is actually authoritative over `T`, because the `AuthorityChangeOp` hasn't arrived yet.

### What happens when a worker instance gains or loses authority over a SpatialOS entity

When authority changes over a SpatialOS component the Unity GDK automatically adds an `AuthoritiesChanged<T>` reactive ECS component to the corresponding ECS entity (for `T`, the component that changed authority).

`AuthoritiesChanged<T>` holds a list of all authority changes that have happened to that SpatialOS component since the last tick.

> **Note**: `AuthoritiesChanged<T>` is a `Component`, not an `IComponentData`. This means you must use a `ComponentArray<T>` for injection rather than a `ComponentDataArray<T>`.

Here's an example of doing something when a worker instance gets authority over a SpatialOS component:

```csharp
public class OnPlayerSpawnSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<SpatialOSPlayerInput> PlayerInput;
        public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInputAuthority;
        
        // This system will only run when there has been a change of authority over PlayerInput in the last tick
        public ComponentArray<AuthoritiesChanged<SpatialOSPlayerInput>> PlayerInputAuthorityChange;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            // The data being injected is for being authoritative over the SpatialOS component PlayerInput, and for changes to authority.
            // So, write code here that you want to run when the worker instance receives authority over PlayerInput
        }
    }
}
```

The GDK automatically adds and removes authority tags: see [Reactive components](reactive-components.md) for more information.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).
