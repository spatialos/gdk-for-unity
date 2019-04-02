# How the Transform Synchronization Feature Module works

## What is Transform

There are two concepts of the location of a SpatialOS entity:

* The _transform_. This contains a location, rotation, and physics tick and is used by worker-instances to run physics simulations or render entities for a client.

* The _position_. This only contains a location. This is defined by the standard library component `Improbable.Position` and is used by the SpatialOS load balancer to determine which worker should be delegated authority over a given entity-component.

<%(#Expandable title="Why do we need two locations?")%>
There are a few reasons why you might want a separate `Transform` and `Position` component.

1. **Separation of responsibilies.** By separating out what worker-instances see as the location of the entity and what the load balancer sees as the location of the entity you conceptually separate these. A gameplay programmer would only need to be aware of the `Transform` component and wouldn't need to worry about the load balancing at all.
2. **Save bandwidth.** The `Position` represents location with 3 doubles. The `Transform` component represents location as 3 floats, which is the Unity native representation. This means that, ignoring rotation, a position update is twice as large as a transform update.<br/><br/>If `Transform` has a high frequency update rate and `Position` has a low frequency update rate then you have a net bandwidth saving over updating just `Position` at a high frequency!
3. **Atomicity.** The `Transform` component contains more than just the location. It also contains the rotation and physics tick. If these were on a separate component, you lose the guarantee that all three (location, rotation, physics tick) are update atomically.<br/><br/>Two updates sent in the same frame from one worker are _not_ guaranteed to be received on the same frame in another.
<%(/Expandable)%>

## How do my entities' transform get synchronized

The behaviour of the Transform Synchronization Feature Modules differs depending on whether your worker-instance is authoritative over the `Transform` _and_ `Position` components. This behaviour can be adjusted with [transform synchronization strategies]({{urlRoot}}/modules/transform-sync/strategies), but broadly goes as follows:

<%(#Expandable title="Do you support the Unity ECS Transforms package?")%>
We don't support the Unity ECS Transform package at this time.

If this is a feature that you strongly desire, please let us know in either: the [Discord](https://discord.gg/SCZTCYm) or our [forums](https://forums.improbable.io/latest?tags=unity-gdk).
<%(/Expandable)%>

##### On authoritative worker-instances

The Transform Synchronization Feature Module listens for transform changes in native Unity representations (`UnityEngine.Transform` or `UnityEngine.Rigidbody`) and translates these into SpatialOS component updates. 

This means that, as a user, you shouldn't need to manually update the `Transform` or `Position` components. You can simply move your GameObjects as you normally would and these changes will be synchronized as specified in the applied strategy.

##### On non-authoritative worker-instances

The Transform Synchronization Feature Module listens for SpatialOS component updates in the `Transform` component and applies these changes to the native Unity representation (`UnityEngine.Transform` or `UnityEngine.Rigidbody`) as specified in the applied strategy.
