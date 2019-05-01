<%(TOC)%>

# How the Transform Synchronization Feature Module works

## What is transform

The transform of an entity describes its location and rotation. Worker-instances generally use it to run physics simulations or render entities for a client. Note that the SpatialOS representation of transform is _not_ the same as Unity's representation of transform.

In the Transform Synchronization Feature Module, we represent the Transform as:

```schemalang
package improbable.transform;

type Location {
    float x = 1;
    float y = 2;
    float z = 3;
}

type Velocity {
    float x = 1;
    float y = 2;
    float z = 3;
}

type Quaternion {
    float w = 1;
    float x = 2;
    float y = 3;
    float z = 4;
}

component TransformInternal {
    id = 11000;
    Location location = 1;
    Quaternion rotation = 2;
    Velocity velocity = 3;
    uint32 physics_tick = 4;
    float ticks_per_second = 5;
}
```

> **Note:** The `TransformInternal` component contains additional fields such as `velocity` and `physics_tick`. These are implementation details of the Transform Synchronization Feature Module.

<%(#Expandable title="Why not reuse the <code>Improbable.Position</code> SpatialOS component?")%>
There are a few reasons why you might want different `Transform` and `Position` components.

1. **Separation of responsibilies.** The `Improbable.Position` component is the load balancer's representation of location and the `TransformInternal` component is the workers' representation of location. This allows you to abstract the concept of location from the load balancer. A gameplay programmer would only need to be aware of the `TransformInternal` component without having to know about the load balancer at all.
2. **Save bandwidth.** The `Position` component represents location with 3 doubles. The `TransformInternal` component represents location as 3 floats, which is the Unity native representation. This means that, ignoring rotation, a position update is twice as large as a transform update. If the `TransformInternal` component has a high frequency update rate and the `Position` component has a low frequency update rate then you have a net bandwidth saving over updating just the `Position` component at a high frequency!
3. **Atomicity.** The `Transform` component contains more than just the location. If the other fields were on a separate component, you lose the guarantee that all fields are updated atomically.<br/><br/>Two updates sent in the same frame from one worker are _not_ guaranteed to be received on the same frame in another.
<%(/Expandable)%>

## How do my entities' transform get synchronized

The behaviour of the Transform Synchronization Feature Modules differs depending on whether your worker-instance is authoritative over the `TransformInternal` _and_ `Position` components. This behaviour can be adjusted with [transform synchronization strategies]({{urlRoot}}/modules/transform-sync/strategies), but broadly goes as follows:

##### On authoritative worker-instances

The Transform Synchronization Feature Module listens for changes in the native Unity transform representation (`UnityEngine.Transform` or `UnityEngine.Rigidbody`) and translates these into `TransformInternal` and `Position` component updates.

This means that, as a user, you shouldn't manually update the `TransformInternal` or `Position` components. You can simply move your GameObjects as you normally would and these changes will be synchronized as specified in the applied strategy.

##### On non-authoritative worker-instances

The Transform Synchronization Feature Module listens for SpatialOS component updates in the `TransformInternal` component and applies these changes to the native Unity transform representation (`UnityEngine.Transform` or `UnityEngine.Rigidbody`) as specified in the applied strategy.
