<%(TOC)%>

# Implement basic player creation

<%(Callout message="
Before reading this document, make sure you have read the following documentation on:

* [Player Lifecycle Feature Module]({{urlRoot}}/modules/player-lifecycle/overview)
* [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
* [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

By default, the module sends a player creation request as soon as the client-worker instance connects to SpatialOS. The server-worker instance receiving the request spawns a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity) to represent the player.

To change this behaviour, read the documentation on [custom player creation]({{urlRoot}}/modules/player-lifecycle/custom-player-creation).

## Set up your worker connector

You need to add the underlying player lifecycle systems to your worker. Open your [`WorkerConnector` implementations]({{urlRoot}}/workflows/monobehaviour/worker-connectors) and add one of the following lines to the `HandleWorkerConnectionEstablished` method.

**If this is a client-worker:**

```csharp
    PlayerLifecycleHelper.AddClientSystems(Worker.World);
```

**If this is a server-worker:**

```csharp
    PlayerLifecycleHelper.AddServerWorkers(Worker.World);
```

> **Note:** You may need to override the `HandleWorkerConnectionEstablished` method in your `WorkerConnector` implementation if you haven't already.

## Define the player entity template

The server-worker responsible for handling requests to spawn player entities needs to know which [entity template]({{urlRoot}}/reference/concepts/entity-templates) to use when sending the entity creation request to the [SpatialOS Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime).

Create a method that returns an [`EntityTemplate`]({{urlRoot}}/api/core/entity-template#entitytemplate-class) object and takes the following parameters:

* `string workerId`: The ID of the worker that wants to spawn this player entity.
*  `byte[] playerCreationArguments`: a serialized byte array of arguments provided by the worker instance sending the player creation request.

When defining the entity template, you need to use the [`PlayerLifecycleHelper.AddPlayerLifecycleComponents`]({{urlRoot}}/api/player-lifecycle/player-lifecycle-helper#static-methods) method. This method adds the SpatialOS components that are required by the Player Lifecycle Feature Module to the player entity template.

The following code snippet shows an example on how to implement such a method:

```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(EntityId entityId, string workerId, byte[] playerCreationArguments)
    {
        // Obtain unique client attribute of the client-worker that requested the player entity
        var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);

        var entityTemplate = new EntityTemplate();
        entityTemplate.AddComponent(new Position.Snapshot(new Coordinates()), "UnityGameLogic");
        // add all components that you want the player entity to have
        PlayerLifecycleHelper.AddPlayerLifecycleComponents(entityTemplate, workerId, "UnityGameLogic");

        return entityTemplate;
    }
}
```

## Configure the entity template delegate

You need to configure the Player Lifecycle Feature Module to use the player entity template function you defined above. To do this, set the `PlayerLifecycleConfig.CreateEntityTemplate` field to the function you defined.

For example, you may wish to set this in the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector):

```csharp
private async void Start()
{
    PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;
    await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
}
```
