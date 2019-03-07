[//]: # (Doc of docs reference 5.1)

<%(TOC)%>
# SpatialOS entities: How to link SpatialOS entities with GameObjects

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with:

* The SpatialOS entity section of [MonoBehaviour and ECS workflows: SpatialOS entities]({{urlRoot}}/content/intro-workflows-spatialos-entities)
* [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)

As described in the [SpatialOS entity background documentation]({{urlRoot}}/content/intro-workflows-spatialos-entities), you represent SpatialOS entities with GameObjects in a Scene by creating the SpatialOS entity first, then linking it to a GameObject in a Scene.

This document is a guide on how to link a SpatialOS entity with a GameObject using the GameObject Creation Feature Module.

You create SpatialOS entities by creating setting up entity templates. Find out how to do this in the [Entity templates]({{urlRoot}}/content/entity-templates) documentation.

When you are using the MonoBehaviour workflow, you interact with SpatialOS entities in the [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) by using Readers and Writers. Find out how to use interact with SpatialOS entities in the [Reader and Writers]({{urlRoot}}/content/gameobject/readers-writers).

## The Creation Feature Module

### How to use the GameObject Creation Feature Module

The GameObject Creation Feature Module contains an implementation of how to link GameObjects to SpatialOS entities along with a default implementation of how to create GameObjects.
To enable the GameObject Creation Feature Module in your project:

1. Set up your project to use the GameObject Creation Feature Module.
  * In your Unity project’s `Packages` directory, locate the Unity Packages manifest `manifest.json`. Add the GameObject Creation Feature Module to the manifest by adding: `"com.improbable.gdk.gameobjectcreation": "file:<path-to-gdk>/workers/unity/Packages/com.improbable.gdk.gameobjectcreation"`
1. Set up your workers to use the GameObject Creation Feature Module when the [WorkerConnector]({{urlRoot}}/content/gameobject/creating-workers-with-workerconnector) initializes systems.
  * To use the default implementation of GameObject creation, in your implementation of `WorkerConnector.HandleWorkerConnectionEstablished`, call `GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World)`.
1. In your project, create a prefab for any SpatialOS entity you want to represent as a GameObject. Where you store the prefab depends on which worker is going to create the GameObject.
  * For any worker, use the `Resources/Prefabs/Common/` directory.
  * For specific worker types, use the `Resources/Prefabs/<WorkerType>` directory where `<WorkerType>` is the type of worker which is going to make this prefab. (Using this directory makes the prefab available for only for a specific worker type.)
1. For the SpatialOS entity, you want to represent as a GameObject, in its [entity template]({{urlRoot}}/content/entity-templates), set the value of its `Metadata` component to the name of the prefab you have just set up.

### How to customize the GameObject Creation Feature Module

To customize the creation of GameObjects, implement the `IEntityGameObjectCreator` interface. This interface provides the following methods that the GDK calls:

### IEntityGameObjectCreator interface

```csharp
void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker);
```

Fields:

  * `SpatialOSEntity entity`: A SpatialOS entity that entered your [worker’s view]({{urlRoot}}/content/glossary#worker-s-view).
  * `EntityGameObjectLinker linker`: An instance of the linker. Ensure you call `linker.LinkGameObjectToSpatialOSEntity(EntityId entityId, GameObject gameObject, params Type[] componentTypesToAdd)` to link the `GameObject` and SpatialOS entity.

```csharp
void OnEntityRemoved(EntityId entityId);
```

Fields:

  * `EntityId entityId`: The entity ID of the SpatialOS entity that was just removed from your worker.

### SpatialOSEntity

This is a wrapper around an ECS entity which you can use to easily access its data.

```csharp
bool HasComponent<T>()
```

Returns: True, if the ECS entity contains the specified ECS component representing a SpatialOS component, denoted as `T`.

```csharp
T GetComponent<T>()
```

Returns: If the ECS entity has it, returns the ECS component, denoted as `T`. If it doesn’t have the ECS component, it returns a struct of type `T` with all its fields initialized with their default values.

#### Example GameObject creator

This is an example of a simple implementation of a GameObject creator.

```csharp
 public class YourGameObjectCreator : IEntityGameObjectCreator
{
    private Dictionary<EntityId, GameObject> gameObjectMapping = new Dictionary<EntityId, GameObject>();
    
    public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.HasComponent<Metadata.Component>())
        {
            return null;
        }

        var prefabName = entity.GetComponent<Metadata.Component>().EntityType;
        var prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", prefabName));
        if (prefab == null)
        {
            return;
        }

        var gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

        linker.LinkGameObjectToSpatialOSEntity(entity.EntityId, gameObject);
        gameObjectMapping.Add(entity.EntityId, gameObject);
    }

    public void OnEntityRemoved(EntityId entityId)
    {
      if (!gameObjectMapping.TryGetValue(entityId, out var go))
      {
          return;
      }

      Object.Destroy(go);
      entityIdToGameObject.Remove(entityId);
    }
}
```

To use your own `IEntityGameObjectCreator` implementation in the GameObject Creation Feature Module, you must call `EnableStandardGameObjectCreation(Worker.World, new YourEntityGameObjectCreator())` when initializing your worker.
