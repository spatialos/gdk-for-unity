[//]: # (Doc of docs reference 36)
[//]: # (TODO - technical writer review)

<%(TOC)%>
# Player Lifecycle Feature Module
_This document relates to both [MonoBehaviour and  ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with:

  * [Core Module and Feature Module overview]({{urlRoot}}/content/modules/core-and-feature-module-overview)
  * [Creating entity templates]({{urlRoot}}/content/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)
  * [Creating workers with WorkerConnector]({{urlRoot}}/content/gameobject/api-workerconnector)

The Player lifecycle module is a feature module providing you with a simple player spawning and player lifecycle management implementation.

### How to enable this module

To enable this module, you need to add the necessary systems to your workers in the `HandleWorkerConnectionEstablished()` of your [`WorkerConnector`]({{urlRoot}}/content/gameobject/creating-workers-with-workerconnector).
After your worker has been created, you need to use the following code snippet:

1. On a [client-worker]({{urlRoot}}/content/glossary#client-worker): `PlayerLifecycleHelper.AddClientSystems(Worker.World)`
1. On a [server-worker]({{urlRoot}}/content/glossary#server-worker): `PlayerLifecycleHelper.AddServerSystems(Worker.World)`

### How to spawn a player entity

To spawn a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) representing
a player, you need to:

1. Define the entity template for your player entity
1. Specify an entity template for player creation

The client-worker initiates the player entity creation once it has been successfully created.

#### 1. Define the player entity template

The module takes care of spawning the player entity as soon as a client-worker connects. To enable this behaviour, the server-worker responsible for handling requests to spawn player entities needs to know which [entity template]({{urlRoot}}/content/entity-templates) to use when sending the entity creation request to the [SpatialOS Runtime]({{urlRoot}}/content/glossary#spatialos-runtime).

Create a method that returns an `EntityTemplate` object and takes the following parameters to define your player [entity template]({{urlRoot}}/content/entity-templates):

* `string workerId`: The ID of the worker that wants to spawn this player entity
*  `Improbable.Vector3f position`: the position where  the client-worker requested to spawn the player entity

When defining the entity template, you need to use the `AddPlayerLifecycleComponents` method.
This methods adds the SpatialOS components to the player entity template necessary to manage the player lifecycle for this entity.

The following code snippet shows an example on how to implement such a method:
```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = $"workerId:{workerId}";
        // Obtain the attribute of your server-worker
        var serverAttribute = "UnityGameLogic";

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddPosition(new Position.Snapshot { Coords = new Coordinates(position.x, position.y, position.z) }, serverAttribute);
        // add all components that you want the player entity to have
        AddPlayerLifecycleComponents(entityTemplate, workerId, clientAttribute, serverAttribute);

        return entityTemplate;
    }
}
```

#### 2. Specify an entity template for player creation

The Player lifecycle module provides a configuration class, called `PlayerLifecycleConfig`, that needs to be configured to use the player entity template.
This can be done by setting the `PlayerLifecycleConfig.CreatePlayerEntityTemplate` field to `PlayerTemplate.CreatePlayerEntityTemplate`.

The following example is a code snippet using a script that runs once when starting the game to initialize the configuration for the player lifecycle module:
```csharp
public static class OneTimeInitialization
{
    private static bool initialized;

    // Using this attribute will trigger the Init() method whenever a Scene gets loaded.
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        if (initialized)
        {
            // ensures to only run it the first time a Scene gets loaded.
            return;
        }

        initialized = true;
        // initialize your game...
        // ...
        // Setup template to use for player on connecting client
        PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;
    }
}
```

### When is a player entity deleted?

To ensure that player entities of disconnected client-workers get deleted correctly,
the server-workers responsible to manage the player lifecycle sends a `PlayerHeartBeat` [command]({{urlRoot}}/content/world-component-commands-requests-responses) to the different player entities to check whether they are still connected. If a player entity fails to send a response three times in a row, the server-worker sends a request to the SpatialOS Runtime to delete this entity.
