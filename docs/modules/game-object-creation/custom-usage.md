# Set up custom spawning

The Game Object Creation Feature Module offers you a simple way to customize how GameObjects are created. You must:

1. Implement a class which inherits from the [`IEntityGameObjectCreator`]({{urlRoot}}/api/game-object-creation/i-entity-game-object-creator) interface.
2. Tell the GDK to use this implementation.

## Implement the `IEntityGameObjectCreator` interface

A good reference implementation of this interface can be found in the [`GameObjectCreatorFromMetadata`]({{urlRoot}}/api/game-object-creation/game-object-creator-from-metadata).

### OnEntityCreated

This method is called by the `GameObjectInitializationSystem` whenever a SpatialOS entity enters your view.

The system gives you an instance of a [`SpatialOSEntity`]({{urlRoot}}/api/game-object-creation/spatial-os-entity) class to represent the entity and an instance of the [`EntityGameObjectLinker`]({{urlRoot}}/api/subscriptions/entity-game-object-linker) class as parameters.

> **Note:** You should not rely on the authority of any of the components on the SpatialOS entity to conditionally create _or_ pick a GameObject to spawn. At the time this method is called, there is no guarantee that your worker has been delegated authority.

If you wish to create a GameObject to represent the SpatialOS entity, you must instantiate the GameObject yourself and call [`EntityGameObjectLinker.LinkGameObjectToSpatialOSEntity`]({{urlRoot}}/api/subscriptions/entity-game-object-linker#methods) with the entity ID and that GameObject.

The `LinkGameObjectToSpatialOS` call has a third parameter: `params Type[] componentTypesToAdd`. These are the types of MonoBehaviour components that you want the linker to copy references onto the underlying ECS entity.

> **Note:** If you also want to use the Transform Synchronization module, you must copy the `UnityEngine.Transform` and the `UnityEngine.Rigidbody` MonoBehaviour components.

### OnEntityRemoved

This method is called by the `GameObjectInitializationSystem` whenever a SpatialOS entity leaves a view. It is called _after_ any linked GameObjects have been unlinked.

The system gives you the entity ID of the SpatialOS entity that has left your view as a parameter.

It is your responsibility to destroy the GameObjects that have been linked to that SpatialOS entity.

## Use your custom `IEntityGameObjectCreator` implementation

Open your [`WorkerConnector` implementation]({{urlRoot}}/workflows/monobehaviour/worker-connectors) and add the following line to the `HandleWorkerConnectionEstablished` method.

```csharp
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, new MyGameObjectCreator());
```

> **Note:** You may need to override the `HandleWorkerConnectionEstablished` method in your `WorkerConnector` implementation if you haven't already.

## (Optional) Link a worker prefab to your worker instance

If you have a worker prefab that you wish to link to your worker instance, you should replace the line noted above with:

```csharp
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, new MyGameObjectCreator(), workerPrefabInstance);
```
