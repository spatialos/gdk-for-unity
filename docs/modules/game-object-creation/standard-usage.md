<%(TOC)%>

# Set up basic spawning

## Set up your GameObject prefabs

The [`GameObjectCreatorFromMetadata`]({{urlRoot}}/api/game-object-creation/game-object-creator-from-metadata) class is the default implementation for spawning GameObjects for your SpatialOS entities. This class expects your GameObject prefabs to be in a particular place in your project.

The `GameObjectCreatorFromMetadata` uses the `entity_type` field in the `Metadata` component along with the worker instance's type to determine which prefab to spawn.

<%(Callout message="You can set the `entity_type` field in the `Metadata` component when declaring your [EntityTemplate]({{urlRoot}}/reference/concepts/entity-templates.md).")%>

For a given worker type `<worker-type>` it will first look for a prefab at:

```text
Resources/Prefabs/<worker-type>/<Metadata.entity-type>
```

Then, if it can't find one there, it will look for a prefab at:

```text
Resources/Prefabs/Common/<Metadata.entity-type>
```

If the prefab is not found at either location, a GameObject will not be spawned for that SpatialOS entity

This leads to the following rules for representing a SpatialOS Entity as a GameObject:

* The prefab must be named `<Metadata.entity_type>`.
* For a specific worker, use the `Resources/Prefabs/<worker-type>` directory.
* For a prefab that is common to any worker, use the `Resources/Prefabs/Common`.

## Set up your worker connector

You then need to add the underlying systems to your worker. Open your [`WorkerConnector` implementation]({{urlRoot}}/reference/workflows/monobehaviour/worker-connectors) and add the following line to the `HandleWorkerConnectionEstablished` method.

```csharp
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
```

> **Note:** You may need to override the `HandleWorkerConnectionEstablished` method in your `WorkerConnector` implementation if you haven't already.

This adds the `GameObjectInitializationSystem` with an instance of the `GameObjectCreatorFromMetadata` class to your worker.

## (Optional) Link a worker prefab to your worker instance

If you have a worker prefab that you wish to link to your worker instance, you should replace the line noted above with:

```csharp
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, workerPrefabInstance);
```
