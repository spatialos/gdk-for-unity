[//]: # (Doc of docs reference 36)
[//]: # (TODO - technical writer review)

<%(TOC)%>
# Player Lifecycle Feature Module

_This document relates to both [MonoBehaviour and  ECS workflow]({{urlRoot}}/reference/workflows/which-workflow)._

Before reading this document, make sure you are familiar with:

  * [Core Module and Feature Module overview]({{urlRoot}}/modules/core-and-feature-module-overview)
  * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)

The Player lifecycle module is a feature module providing you with a simple player spawning and player lifecycle management implementation.

## How to enable this module

To enable this module, you need to add the necessary systems to your workers in the `HandleWorkerConnectionEstablished()` of your [`WorkerConnector`]({{urlRoot}}/reference/workflows/monobehaviour/creating-workers).
After your worker has been created, you need to use the following code snippet:

1. On a [client-worker]({{urlRoot}}/reference/glossary#client-worker): `PlayerLifecycleHelper.AddClientSystems(Worker.World)`
  * By default, the module automatically sends a player creation request as soon as the client-worker instance connects to SpatialOS. To manually initiate player creation or provide arbitrary data in a player creation request, add an extra `false` argument when adding client systems: `PlayerLifecycleHelper.AddClientSystems(Worker.World, false);`
1. On a [server-worker]({{urlRoot}}/reference/glossary#server-worker): `PlayerLifecycleHelper.AddServerSystems(Worker.World)`

## How to spawn a player entity

To spawn a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity) representing
a player, you need to:

1. Define the entity template for your player entity
1. Specify an entity template for player creation
1. (Optional) Manually request player creation

>**NOTE:** To pass in arbitrary serialized data with the player creation request, set `AutoRequestPlayerCreation` to false.

If `AutoRequestPlayerCreation` is set to false you must manually initiate player creation, as described in Step 3.

##### 1. Define the player entity template

The module takes care of spawning the player entity as soon as a client-worker connects. To enable this behaviour, the server-worker responsible for handling requests to spawn player entities needs to know which [entity template]({{urlRoot}}/reference/concepts/entity-templates) to use when sending the entity creation request to the [SpatialOS Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime).

Create a method that returns an `EntityTemplate` object and takes the following parameters to define your player [entity template]({{urlRoot}}/reference/concepts/entity-templates):

* `string workerId`: The ID of the worker that wants to spawn this player entity.
*  `byte[] playerCreationArguments`: a serialized byte array of arguments provided by the worker instance sending the player creation request.

When defining the entity template, you need to use the `AddPlayerLifecycleComponents` method. This method adds lifecycle SpatialOS components to the player entity template; these components are necessary to manage the player lifecycle for this entity.

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

You need to configure the `PlayerLifecycleConfig` configuration class to use the player entity template. Do this by setting the `PlayerLifecycleConfig.CreatePlayerEntityTemplate` field to `PlayerTemplate.CreatePlayerEntityTemplate`.

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

## How to use arbitrary data in the player creation loop

##### 1. Set AutoRequestPlayerCreation to false

First, ensure that `AutoRequestPlayerCreation` is set to false.

##### 2. Call RequestPlayerCreation with serialized data

Then, call `RequestPlayerCreation`. Ensure that your arbitrary data is serialized into a byte array.

```csharp
var myArguments = new SampleArgumentsObject { PlayerName = "playerName", SpawnPosition = new Coordinates(50, 0, 75)) };
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
var serializedArguments = SerializeArguments(myArguments);
playerCreationSystem.RequestPlayerCreation(serializedArguments);
```

Where an example implementation of `SerializeArguments` could be:

```csharp
private byte[] SerializeArguments(object playerCreationArguments)
{
    using (var memoryStream = new MemoryStream())
    {
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, playerCreationArguments);
        return memoryStream.ToArray();
    }
}
```

##### 3. Deserialize data into the same type you originally serialized from

Lastly, ensure that you are deserializing the byte array into the same type of object you serialized it from. For example:

```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] playerCreationArguments)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = $"workerId:{workerId}";
        // Obtain the attribute of your server-worker
        var serverAttribute = "UnityGameLogic";

        var deserializedArguments = DeserializeArguments<SampleArgumentsObject>(playerCreationArguments);

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddPosition(new Position.Snapshot { Coords = deserializedArguments.SpawnPosition }, serverAttribute);
        entityTemplate.AddComponent(new PlayerName.Snapshot { PlayerName = deserializedArguments.PlayerName }, serverAttribute);
        // add all components that you want the player entity to have
        AddPlayerLifecycleComponents(entityTemplate, workerId, clientAttribute, serverAttribute);

        return entityTemplate;
    }

    // An example implementation of DeserializeArguments
    private T DeserializeArguments<T>(byte[] serializedArguments)
    {
        using (var memoryStream = new MemoryStream())
        {
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(serializedArguments, 0, serializedArguments.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T) binaryFormatter.Deserialize(memoryStream);
        }
    }
}
```

## When is a player entity deleted?

To ensure that player entities of disconnected client-worker instances get deleted correctly, the server-worker instances responsible for managing the player lifecycle sends a `PlayerHeartBeat` command to the different player entities to check whether they are still connected. If a player entity fails to send a response three times in a row, the server-worker instance sends a request to the SpatialOS Runtime to delete this entity.
