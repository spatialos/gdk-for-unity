[//]: # (Doc of docs reference 9)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - explain what “handling the response based on the information contained in this object” means - see note below.)
[//]: # (TODO - link to status codes for error messages - see note below.)

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
We provide the following types for sending and receiving world commands:

  * `WorldCommands.Requirable.WorldCommandRequestSender`
  * `WorldCommands.Requirable.WorldCommandResponseHandler`

> We do not provide a `WorldCommands.Requirable.WorldCommandRequestHandler` as the command will be directly handled by the SpatialOS Runtime.

Both of these objects can be injected without any condition. A MonoBehaviour that requires only these will be enabled as soon as the associated GameObject is created.

If you would like to see how you can use these world commands to create or delete entities, we recommend you to read the [how to create and delete SpatialOS entities]({{urlRoot}}/content/gameobject/create-delete-spatialos-entities) document.

### CreateEntity
You can use the `WorldCommandRequestSender.CreateEntity` method to request the creation of a new SpatialOS entity. It has the following signature:

```csharp
long CreateEntity(Improbable.Worker.Core.Entity entityTemplate, EntityId? entityId = null, uint? timeoutMillis = null, object context = null)
```

Parameters:

  * `Improbable.Worker.Core.Entity entityTemplate`: The [template]({{urlRoot}}/content/entity-templates) of the entity that you want to create.
  * `EntityId entityId`: Optional. The ID that the new entity should have, if you [reserved one](#reserveentityids).
  * `uint timeoutMillis`: Optional. Specifies the amount of time in milliseconds to wait before the command fails with a timeout status. If not specified, the default of 5 seconds of the lower-level [Worker SDK (SpatialOS documentation)](https://docs.improbable.io/reference/latest/cppsdk/introduction) is used.
  * `object context`: Optional. An arbitrary object you can associate with the command. You get this object back along with the response. This is useful when handling the response based on the information contained in this object.
[//]: # (TODO - explain what “handling the response based on the information contained in this object” means)

Returns: the request Id for this command as a `long`.

The corresponding response callback is `WorldCommandResponseHandler.OnCreateEntityResponse`:

```csharp
event Action<CreateEntity.ReceivedResponse> OnCreateEntityResponse
```

Callback parameters:

  * `ReceivedResponse`: The data associated with the incoming response stored inside a [`ReceivedResponse`]({{urlRoot}}/content/world-component-commands-requests-responses#receivedresponse) struct.

The `CreateEntity.ReceivedResponse` struct contains the same fields as any `ReceivedResponse` struct. However, in this case, the `EntityId` field represents the entity ID of the newly-created entity. If the command failed, this field is null. You can additionally use the `ReserveEntityIds` world command before entity creation which makes it easier to create multiple entities in a group.

### ReserveEntityIds

Like other commands, a `CreateEntity` command response can time out, but the entity might still be created. As we do not know which ID was used by the SpatialOS Runtime to create this entity, the worker needs to retry the `CreateEntity` command request. This might lead to creating multiple entities as a new entity ID will be used by the SpatialOS Runtime, if the previous `CreateEntity` command did succeed.
To avoid this, the worker can reserve an entity ID before sending the `CreateEntity` command request. Depending on the command response, you should do the following:
The command succeeded: The entity got successfully created
The command failed with the status code `ApplicationError` and the following message: “'Entity reservation failed. The entity with Id <{id}> could not be found. The reservation might have expired or never existed”. If you have used the entity ID that you received from the `ReserveEntityIds` command response for sending the `CreateEntity` commands, then your entity has already been created and you don’t need to retry it anymore.
The command timed out or another error appeared: retry the command.
[//]: # (TODO - link to status codes for error messages.)


You can use the `WorldCommandRequestSender.ReserveEntityIds` method to send a command request to reserve entity ids. It has the following signature:

```csharp
long ReserveEntityIds(uint numberOfEntityIds, uint? timeoutMillis = null, object context = null)
```

Parameters:

  * `uint numberOfEntityIds`: The number of entity IDs that you want to reserve.
  * `uint timeoutMillis`: Optional. Specifies the amount of time in milliseconds to wait before the command fails with a timeout status. If not specified, the default of 5 seconds of the underlying Worker SDK is used.
  * `object context`: Optional. An arbitrary object you can associate with the command. You get this object back along with the response. This is useful when handling the response based on the information contained in this object.

Returns: the request Id for this command as a `long`.

The corresponding response callback is `WorldCommandResponseHandler.OnReserveEntityIdsResponse`:

```csharp
event Action<ReserveEntityIds.ReceivedResponse> OnReserveEntityIdsResponse
```
Callback parameters:
  * `ReceivedResponse`: The data associated with the incoming response stored inside a [`ReceivedResponse`]({{urlRoot}}/content/world-component-commands-requests-responses) struct.

The `ReserveEntityIds.ReceivedResponse` struct contains these additional properties:

| Name          	| Type 	| Description                                                                                                                                                            	|
|-------------------|----------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| FirstEntityId 	| `EntityId` | If successful, an ID which is the first in a contiguous range of newly allocated entity IDs. These are guaranteed to be unused in the current deployment. Otherwise, null. |
| NumberOfEntityIds | `int`  	| If successful, the number of IDs reserved in the contiguous range, otherwise 0.                                                                                        	|

### DeleteEntity
You can use the `DeleteEntity` method to request the deletion of a SpatialOS entity given its SpatialOS entity ID. It has the following signature:

```csharp
long DeleteEntity(EntityId entityId, uint? timeoutMillis = null, object context = null)
```

Parameters:

  * `EntityId entityId`: The ID of the SpatialOS entity that you want to delete.
  * `uint timeoutMillis`: Optional. Specifies the amount of time in milliseconds to wait before the command fails with a timeout status. If not specified, the default of 5 seconds of the underlying Worker SDK is used.
  * `object context`: Optional. An arbitrary object you can associate with the command. You get this object back along with the response. This is useful when handling the response based on the information contained in this object.

Returns: the request Id for this command as a `long`.

The corresponding response callback is `WorldCommandResponseHandler.OnDeleteEntityResponse`:

```csharp
event Action<DeleteEntity.ReceivedResponse> OnDeleteEntityResponse
```
Callback parameters:

  * `ReceivedResponse`: The data associated with the incoming response stored inside a [`ReceivedResponse`]({{urlRoot}}/content/world-component-commands-requests-responses#receivedresponse) struct.

The `DeleteEntity.ReceivedResponse` struct contains the same fields as any `ReceivedResponse` struct. However, in this case, the `EntityId` field represents the entity ID of the deleted entity. If the command failed, this field is null.

>  Do not manually delete GameObjects representing entities after sending a `DeleteEntity` command. [The GDK does this for you]({{urlRoot}}/content/gameobject/linking-spatialos-entities#the-creation-feature-module).


### EntityQuery
You can use entity queries to get information about entities in the SpatialOS game world. (See SpatialOs documentation on entity queries.) The GDK currently only offers a prototype of the entity queries. The methods for sending and receiving entity queries exist, but you can not yet safely access the information in the responses. It is not recommended to use them. Any entity queries that specify a `SnapshotResultType
` will be ignored.

You can use the `EntityQuery` method to request information about the entities.It has the following signature:

```csharp
long EntityQuery(Improbable.Worker.Query.EntityQuery entityQuery, uint? timeoutMillis = null, object context = null)
```

Parameters:

* `Improbable.Worker.Query.EntityQuery entityQuery`: The query that you want to send to the SpatialOS Runtime. See the documentation of the query object for more details.
* `uint timeoutMillis`: Optional. Specifies the amount of time in milliseconds to wait before the command fails with a timeout status. If not specified, the default of 5 seconds of the underlying Worker SDK is used.
* `object context`: Optional. An arbitrary object you can associate with the command. You get this object back along with the response. This is useful when handling the response based on the information contained in this object.to pass more information about the situation to the code handling the response.

Returns: the request Id for this command as a `long`.

The corresponding response callback is `WorldCommandResponseHandler.OnEntityQueryResponse`:

```csharp
event Action<EntityQuery.ReceivedResponse> OnEntityQueryResponse
```

Callback parameters:

  * `ReceivedResponse`: The data associated with the incoming response stored inside a [`ReceivedResponse`]({{urlRoot}}/content/world-component-commands-requests-responses#api-component-commands) struct.

The `EntityQuery.ReceivedResponse` struct contains the same fields as any `ReceivedResponse` struct. Additionally, it exposes the following fields:

| Name          	| Type 	| Description                                                                                                                                                            	|
|-------------------|----------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ResultCount 	| `int` | The number of entities that matched the query. <br/><br/> Note that a best-effort attempt is made to count the entities when the status code is ApplicationError. In this case, the count can still be non-zero, but should be considered a lower bound (i.e. there might be entities matching the query that were not counted). |
| Result | `Dictionary<EntityId, EntityQuerySnapshot>`  	| The result of a `SnapshotResultType` query. For `CountResultType` queries this will be `null`.<br/> </br> Note that a best-effort attempt is made to get results when the status code is ApplicationError. In this case, the result can still be non-empty, but there might be entities matching the query that were not returned.<br/><br/> 
