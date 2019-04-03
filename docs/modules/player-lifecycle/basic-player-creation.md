<%(TOC)%>
# Implementing basic player creation

<%(Callout message="
Before reading this document, make sure you have read the following documentation on:

* [Player Lifecycle Feature Module]({{urlRoot}}/modules/player-lifecycle/overview)
* [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
* [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

By default, the module sends a player creation request as soon as the client-worker instance connects to SpatialOS. The server-worker instance receiving the request spawns a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity) to represent the player.The player entity gets deleted, if the server-worker fails to receive multiple [heartbeats]({{urlRoot}}/modules/player-lifecycle/heartbeating) consecutively.

## Set up your worker connector

You need to add the underlying player lifecycle systems to your worker. Open your [`WorkerConnector` implementations]({{urlRoot}}/reference/workflows/monobehaviour/creating-workers) and add one of the following lines to the `HandleWorkerConnectionEstablished` method.

**If this is a client-worker:**

```csharp
    PlayerLifecycleHelper.AddClientSystems(Worker.World);
```

**If this is a server-worker:**

```csharp
    PlayerLifecycleHelper.AddServerWorkers(Worker.World);
```

To change this behaviour, read the documentation on [custom player creation]({{urlRoot}}/modules/player-lifecycle/custom-player-creation).

## Define the player entity template

The server-worker responsible for handling requests to spawn player entities needs to know which [entity template]({{urlRoot}}/reference/concepts/entity-templates) to use when sending the entity creation request to the [SpatialOS Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime).

Create a method that returns an `EntityTemplate` object and takes the following parameters to define your player [entity template]({{urlRoot}}/reference/concepts/entity-templates):

* `string workerId`: The ID of the worker that wants to spawn this player entity.
*  `byte[] playerCreationArguments`: a serialized byte array of arguments provided by the worker instance sending the player creation request.

When defining the entity template, you need to use the `AddPlayerLifecycleComponents` method. This method adds SpatialOS components to the player entity template that are used by the Player Lifecycle Feature Module to manage the lifecycle of this entity.

The following code snippet shows an example on how to implement such a method:

```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] playerCreationArguments)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddPosition(new Position.Snapshot(new Coordinates()), "UnityGameLogic");
        // add all components that you want the player entity to have
        AddPlayerLifecycleComponents(entityTemplate, workerId, "UnityGameLogic");

        return entityTemplate;
    }
}
```

## Configure the entity template delegate

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
