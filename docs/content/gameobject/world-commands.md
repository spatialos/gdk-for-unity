[//]: # (Doc of docs reference 9)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - explain what “handling the response based on the information contained in this object” means - see note below.)
[//]: # (TODO - link to status codes for error messages - see note below.)

<%(TOC)%>
# World commands
_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities#spatialos-entities)._

Before reading this document, make sure you are familiar with

  * [How to interact with SpatialOS using MonoBehaviours]({{urlRoot}}/content/gameobject/interact-spatialos-monobehaviours)
  * [Commands: World and component command requests and responses
]({{urlRoot}}/content/world-component-commands-requests-responses)
  * [SpatialOS entities: Creating entity templates]({{urlRoot}}/content/entity-templates)

## About commands
Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers]({{urlRoot}}/content/workers/workers-in-the-gdk). Commands are relevant to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities).<br/>

There are two types of commands in SpatialOS:

* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities]({{urlRoot}}/content/glossary#spatialos-entity).
* **Component commands** you set up in your [schema]({{urlRoot}}/content/glossary#schema) for workers to invoke on any SpatialOS entity’s components.

The commands documentation is:

* [MonoBehaviour world commands]({{urlRoot}}/content/gameobject/world-commands)
* [ECS world commands]({{urlRoot}}/content/ecs/world-commands)
* Monobehaviour component command, see [(MonoBehaviour)Sending and receiving component commands]({{urlRoot}}/content/gameobject/sending-receiving-commands)
* ECS component commands, see [ECS: Sending and receiving component commands]({{urlRoot}}/content/ecs/sending-receiving-component-commands)
* Both workflows - [World and component command requests and responses]({{urlRoot}}/content/world-component-commands-requests-responses)


## How to send and receive world commands
We provide the following type for sending and receiving world commands:

  * `WorldCommandSender`

> We do not provide a `WorldCommandReceiver` as the command will be directly handled by the SpatialOS Runtime.

The `WorldCommandSender` can be injected without any condition. A MonoBehaviour that requires only these will be enabled as soon as the associated GameObject is created.

If you would like to see how you can use these world commands to create or delete entities, we recommend you to read the [how to create and delete SpatialOS entities]({{urlRoot}}/content/gameobject/create-delete-spatialos-entities) document.

### CreateEntity
You can use the `WorldCommandSender.SendCreateEntityCommand` method to request the creation of a new SpatialOS entity. It has the following signature:

```csharp
void SendCreateEntityCommand(WorldCommands.CreateEntity.Request request, Action<WorldCommands.CreateEntity.ReceivedResponse> callback = null);
```

Parameters:

  * `WorldCommands.CreateEntity.Request request`: The command request payload.
  * `Action<WorldCommands.CreateEntity.ReceivedResponse> callback`: Optional. A callback that will be called when the command response is received.

[//]: # (TODO - explain what “handling the response based on the information contained in this object” means)

### ReserveEntityIds

Like other commands, a `CreateEntity` command response can time out, but the entity might still be created. As we do not know which ID was used by the SpatialOS Runtime to create this entity, the worker needs to retry the `CreateEntity` command request. This might lead to creating multiple entities as a new entity ID will be used by the SpatialOS Runtime, if the previous `CreateEntity` command did succeed.
To avoid this, the worker can reserve an entity ID before sending the `CreateEntity` command request. Depending on the command response, you should do the following:

  * The command succeeded: The entity got successfully created
  * The command failed with the status code `ApplicationError` and the following message: “'Entity reservation failed. The entity with Id <{id}> could not be found. The reservation might have expired or never existed”. If you have used the entity ID that you received from the `ReserveEntityIds` command response for sending the `CreateEntity` commands, then your entity has already been created and you don’t need to retry it anymore.
  * The command timed out or another error appeared: retry the command.
[//]: # (TODO - link to status codes for error messages.)

You can use the `WorldCommandSender.SendReserveEntityIdsCommand` method to send a command request to reserve entity ids. It has the following signature:

```csharp
void SendReserveEntityIdsCommand(WorldCommands.ReserveEntityIds.Request request, Action<WorldCommands.ReserveEntityIds.ReceivedResponse> callback = null);
```

Parameters:

  * `WorldCommands.ReserveEntityIds.Request request`: The command request payload.
  * `Action<WorldCommands.ReserveEntityIds.ReceivedResponse> callback`: Optional. A callback that will be called when the command response is received.

### DeleteEntity

You can use the `SendDeleteEntityCommand` method to request the deletion of a SpatialOS entity given its SpatialOS entity ID. It has the following signature:

```csharp
void SendDeleteEntityCommand(WorldCommands.DeleteEntity.Request request, Action<WorldCommands.DeleteEntity.ReceivedResponse> callback = null)
```

Parameters:

  * `WorldCommands.DeleteEntity.Request request`: The command request payload.
  * `Action<WorldCommands.DeleteEntity.ReceivedResponse> callback`: Optional. A callback that will be called when the command response is received.

>  Do not manually delete GameObjects representing entities after sending a `DeleteEntity` command. [You should wait until you receive a callback on ]({{urlRoot}}/content/gameobject/linking-spatialos-entities#the-creation-feature-module).


### EntityQuery

You can use entity queries to get information about entities in the SpatialOS game world.

You can use the `SendEntityQueryCommand` method to request information about the entities.It has the following signature:

```csharp
void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request, Action<WorldCommands.EntityQuery.ReceivedResponse> callback = null)
```

Parameters:

  * `WorldCommands.EntityQuery.Request request`: The command request payload.
  * `Action<WorldCommands.EntityQuery.ReceivedResponse> callback`: Optional. A callback that will be called when the command response is received.
