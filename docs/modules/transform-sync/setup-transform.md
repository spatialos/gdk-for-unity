# Set up tranform synchronization for an entity

## Add the components to your EntityTemplate

When you have a SpatialOS entity that you want the transform to be synchronized, you need to ensure that the required components are present on that entity.

We provide a helper method that does this for you: `TransformSynchronizationHelper.AddTransformSynchronizationComponents`. See the [API reference]({{urlRoot}}/api/transform-synchronization/transform-synchronization-helper) for more information on this method.

```csharp
var serverAttribute = "UnityGameLogic";
var clientAttribute = "UnityClient";
 
var entityTemplate = new EntityTemplate();
entityTemplate.AddComponent(new Position.Snapshot(), serverAttribute);
entityTemplate.AddComponent(new Metadata.Snapshot { EntityType = "EntityThatCanMove"}, serverAttribute);
entityTemplate.SetReadAcl(serverAttribute, clientAttribute);
entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

// Add the transform components to the template.
TransformSynchronizationHelper.AddTransformSynchronizationComponents(entityTemplate, serverAttribute);
```

> **Note:** Typically you will want to delegate authority of the transform components to a server-worker. This prevents a client from changing its own position in the game world. This, of course, is subject to _your own_ design constraints.

## Add the `TransformSynchronization` MonoBehaviour to that entity's prefab

<%(Callout message="As the Transform Synchronization Feature Module supports GameObjects only, you will need to ensure that you have a linked GameObject to your SpatialOS entity.<br/><br/>See our [GameObject Creation Feature Module documentation]({{urlRoot}}/modules/game-object-creation/overview) to get this set up.")%>

Open the prefab which will be linked to your SpatialOS entity and add the `TransformSynchronization` MonoBehaviour to that entity. It should look like the following:

![]({{assetRoot}}assets/image-transform-feature-module-md-0.png)

<%(#Expandable title="What does <code>Set Kinematic When Not Authoritative Do</code>?")%>
If this option is selected and there is a Rigidbody on the GameObject, the Rigidbody will become [kinematic](https://docs.unity3d.com/Manual/Glossary.html#kinematics) when the given worker is not authoritative over that SpatialOS entity's `Transform` component. 

When authority is re-gained the Rigidbody will return to the state it was in before.
<%(/Expandable)%>

## Add transform strategies to the `TransformSynchronization` MonoBehaviour

The behaviour of the Transform Synchronization Feature Module is dictated by which strategies are attached to the `TransformSynchronization` MonoBehaviour. Each strategy can be classified as either a "receive strategy" or a "send strategy". See our complete documentation on the [transform strategies]({{urlRoot}}/modules/transform-sync/strategies) to learn what each of them does.

You may need to create instances of these strategies as they are scriptable objects. Select **Assets** > **Create** > **SpatialOS** > **Transforms** to see the available strategies.

### Recommended configuration

An arbritrary number of strategies can be specified, but only one per worker type, for either send or receive should be added. The `Worker Type` field on the scriptable object corresponds to which worker type it will apply to.

**Our recommended receive strategy setup**

* For client-workers, use the `InterpolationReceiveStrategy`.
* For server-workers, use the `DirectReceiveStrategy`.

**Our recommended send strategy setup**

Currently only `RateLimitedSendStrategy` is available and thus is the recommended strategy.