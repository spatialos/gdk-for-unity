[//]: # (Doc of docs reference 36)
[//]: # (TODO - technical writer review)

<%(TOC)%>
# Player Lifecycle Feature Module
_This document relates to both [MonoBehaviour and  ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with:

  * [Core Module and Feature Module overview]({{urlRoot}}/content/modules/core-and-feature-module-overview)
  * [Creating entity templates]({{urlRoot}}/content/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)

The Player lifecycle module is a feature module providing you with a simple player spawning and player lifecycle management implementation.

## How to enable this module

To enable this module, you need to add the necessary systems to your workers in the `HandleWorkerConnectionEstablished()` of your [`WorkerConnector`]({{urlRoot}}/content/gameobject/creating-workers-with-workerconnector).
After your worker has been created, you need to use the following code snippet:

1. On a [client-worker]({{urlRoot}}/content/glossary#client-worker): `PlayerLifecycleHelper.AddClientSystems(Worker.World)`
  * By default, the module automatically sends a player creation request as soon as the client worker connects to SpatialOS. To manually initiate player creation or provide arbitrary data in a player creation request, add an extra `false` argument when adding client systems: `PlayerLifecycleHelper.AddClientSystems(Worker.World, false);`
1. On a [server-worker]({{urlRoot}}/content/glossary#server-worker): `PlayerLifecycleHelper.AddServerSystems(Worker.World)`

The Player lifecycle module provides a `PlayerLifecycleConfig` configuration class, containing a field called `AutoRequestPlayerCreation`. By default this is set to true, in which case the client-worker initiates the player entity creation as soon as it has connected to SpatialOS.

Calling `PlayerLifecycleHelper.AddClientSystems(Worker.World, false)` sets this value to false, in which case you must manually initiate player creation.

## How to spawn a player entity

To spawn a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) representing
a player, you need to:

1. Define the entity template for your player entity
1. Specify an entity template for player creation
1. (Optional) Manually request player creation

>**NOTE: To pass in arbitrary serialized data with the player creation request, `AutoRequestPlayerCreation` must be set to false.**

If `AutoRequestPlayerCreation` is set to false you must manually initiate player creation, as described in Step 3.

##### 1. Define the player entity template

The module takes care of spawning the player entity as soon as a client-worker connects. To enable this behaviour, the server-worker responsible for handling requests to spawn player entities needs to know which [entity template]({{urlRoot}}/content/entity-templates) to use when sending the entity creation request to the [SpatialOS Runtime]({{urlRoot}}/content/glossary#spatialos-runtime).

Create a method that returns an `EntityTemplate` object and takes the following parameters to define your player [entity template]({{urlRoot}}/content/entity-templates):

* `string workerId`: The ID of the worker that wants to spawn this player entity.
*  `byte[] playerCreationArguments`: a serialized byte array of arguments provided by the worker sending the player creation request.

When defining the entity template, you need to use the `AddPlayerLifecycleComponents` method.
This methods adds the SpatialOS components to the player entity template necessary to manage the player lifecycle for this entity.

The following code snippet shows an example on how to implement such a method:
```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] playerCreationArguments)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = $"workerId:{workerId}";
        // Obtain the attribute of your server-worker
        var serverAttribute = "UnityGameLogic";

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddPosition(new Position.Snapshot { Coords = new Coordinates() }, serverAttribute);
        // add all components that you want the player entity to have
        AddPlayerLifecycleComponents(entityTemplate, workerId, clientAttribute, serverAttribute);

        return entityTemplate;
    }
}
```

##### 2. Specify an entity template for player creation

The `PlayerLifecycleConfig` configuration class needs to be configured to use the player entity template.
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

##### 3. (Optional) Manually request player creation

If `AutoRequestPlayerCreation` is set to false, you must manually call `RequestPlayerCreation` in the `SendCreatePlayerRequestSystem`:
```csharp
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
playerCreationSystem.RequestPlayerCreation();
```

## How to pass arbitrary data into a player creation request

##### 1. Set AutoRequestPlayerCreation to false
First, ensure that `AutoRequestPlayerCreation` is set to false.

##### 2. Serialize before calling RequestPlayerCreation
Then, ensure that it is serialized into a byte array before calling `RequestPlayerCreation`:
```csharp
var myArguments = new SampleArgumentsObject { PlayerName = "playerName", SpawnPosition = new Coordinates(50, 0, 75)) };
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
var serializedArguments = PlayerLifecycleHelper.SerializeArguments(myArguments);
playerCreationSystem.RequestPlayerCreation(serializedArguments);
```
##### 3. Deserialize into the same type you originally serialized from
And lastly, ensure that you are deserializing the byte array into the same type of object you serialized it from. For example:
```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] playerCreationArguments)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = $"workerId:{workerId}";
        // Obtain the attribute of your server-worker
        var serverAttribute = "UnityGameLogic";

        var deserializedArguments = PlayerLifecycleHelper.DeserializeArguments<SampleArgumentsObject>(playerCreationArguments);

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddPosition(new Position.Snapshot { Coords = deserializedArguments.SpawnPosition }, serverAttribute);
        entityTemplate.AddComponent(new PlayerName.Snapshot { PlayerName = deserializedArguments.PlayerName }, serverAttribute);
        // add all components that you want the player entity to have
        AddPlayerLifecycleComponents(entityTemplate, workerId, clientAttribute, serverAttribute);

        return entityTemplate;
    }
}
```

## When is a player entity deleted?

To ensure that player entities of disconnected client-workers get deleted correctly, the server-workers responsible to manage the player lifecycle sends a `PlayerHeartBeat` [command]({{urlRoot}}/content/world-component-commands-requests-responses) to the different player entities to check whether they are still connected. If a player entity fails to send a response three times in a row, the server-worker sends a request to the SpatialOS Runtime to delete this entity.
