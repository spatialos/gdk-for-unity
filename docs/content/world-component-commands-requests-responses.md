[//]: # (Doc of docs reference 24)
[//]: # (TODO - technical author pass)
[//]: # (TODO - split into Commands intro doc + API doc)
[//]: # (TODO - Update the ECS Commands doc to refer to this for relevant info. It doesn’t seem to have much of this duplicated, so IMO this is just a matter of removing 2 sentences of details about the request/response types and adding 1 sentence with a link here. Could do a straightforward PR once this doc is in.)
<%(TOC)%>
# Commands: World and component command requests and responses
_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you know about the two different workflows ([GameObject- MonoBehaviour and ECS]({{urlRoot}}/content/intro-workflows-spatialos-entities)) and [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk). You should also be familiar with the terms; [schema]({{urlRoot}}/content/glossary#schema), [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) and [SpatialOS components]({{urlRoot}}/content/glossary#spatialos-component).

## About commands
Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers]({{urlRoot}}/content/workers/workers-in-the-gdk). Commands are relevant to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities).

There are two types of commands in SpatialOS:

* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities]({{urlRoot}}/content/glossary#spatialos-entity).
* **Component commands** you set up in your [schema]({{urlRoot}}/content/glossary#schema) for workers to invoke on any SpatialOS entity’s components.

You can find documentation for commands by following these links:

* [MonoBehaviour world commands]({{urlRoot}}/content/gameobject/world-commands)
* [ECS world commands]({{urlRoot}}/content/ecs/world-commands)
* MonoBehaviour component commands, see [GameObject: Sending and receiving commands]({{urlRoot}}/content/gameobject/sending-receiving-commands)
* MonoBehaviour component commands, see [ECS: Sending and receiving component commands]({{urlRoot}}/content/ecs/sending-receiving-component-commands)
* Both workflows - world and component command requests and responses - this document


### API - Component commands
For every component command that you specify in your schema, the GDK generates command request and response structs using the [code-generator]({{urlRoot}}/content/code-generator). These types are available in the `{namespace defined in schema}.{name of component}.{name of command}` class.  

For each component command, the GDK generates:

  * `Request`: This represents a command request that you wish to send.
  * `ReceivedRequest`: This represents a command request that you have received.
  * `Response`: This represents the command response that you wish to send.
  * `ReceivedResponse`: This represents a command response that you have received.


### Request
It stores the information of a request you want to send.

**Fields**

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| RequestId | `long` | A unique identifier for a command request on the worker sending the request. |
| Payload | `TRequest` | The payload you want to send with the command. Its type `TRequest` is the argument type of the command as defined in schema. |
| TargetEntityId | `EntityId` | The ID of the SpatialOS entity that you want to send the command to. |
| TimeoutMillis | `uint` | Optional. Specifies the amount of time to wait before the command fails with a timeout status. This is “null” by default which actually means it’s 5 seconds as this value comes from the lower-level [SpatialOS worker SDK](https://docs.improbable.io/reference/latest/csharpsdk/introduction) (SpaitalOS documentation). |
| AllowShortCircuiting | `bool` | Optional.  A boolean specifying whether or not a command which is sent to the same worker can be handled without going through the SpatialOS Runtime. The default is `false`. |
| Context | `System.Object` | Optional. An arbitrary object you can associate with the command which you also get back along with the response. This is useful to pass more information about the situation to the code handling the response. The default is `null`. |

**Methods**

```csharp
static Request CreateRequest(EntityId targetEntityId, TRequest request, uint? timeoutMillis = null, bool allowShortCircuiting = false, System.Object context = null)
```

Parameters:

  * `EntityId entityId` - The ID of the SpatialOS entity that you want to send the command to.
  * `TRequest payload` - This is the data which is sent to the worker with [write access]({{urlRoot}}/content/glossary#authority) with your command. The type `TRequest` of this payload depends on what you defined in your schema as the payload.
  * `uint timeoutMillis` - Optional. Specifies the amount of time to wait before the command fails with a timeout status. This is `null` by default which actually means it’s 5 seconds as this value comes from the lower-level [SpatialOS worker SDK](https://docs.improbable.io/reference/latest/csharpsdk/introduction).
  * `bool allowShortCircuiting` - Optional.
A boolean specifying whether or not a command which is sent to the same worker can be handled without going through the SpatialOS Runtime. The default is `false`.   * `object context` - Optional. An arbitrary object you can associate with the command which you also get back along with the response. This is useful to pass more information about the situation to the code handling the response. The default is `null`.

Returns: a `Request` object containing the data specified in the parameters.

### ReceivedRequest
Stores the received command request.

**Fields**

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| RequestId | `long` | Uniquely identifies this incoming command request on this worker. |
| Payload | `TRequest` | The data of the received command request. Its type `TRequest` is the argument type of the command as defined in schema. |
| CallerWorkerId | `string` | Uniquely identifies the worker that sent the request. |
| CallerAttributeSet | `List<string>` | The set of attributes configured for the worker that sent the request. |

### Response
It stores the information of a response you wish to send.

**Fields**

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| RequestId | `long` | The request ID of the incoming command request this response corresponds to. |
| Payload | `TResponse` | If the command succeeded, this stores the data of the command response. Otherwise `null`. The type `TResponse` is the return type of the command as defined in schema. |
| FailureMessage | `string` | The associated error message, if the command failed. Otherwise `null`. |

**Methods**

```csharp
MyComponent.MyCommand.Response.CreateResponse(ReceivedRequest req, <CommandName>CommandResponse payload);
```

Parameters:

  * `ReceivedRequest req` - The request that you want to send a response for.
  * `{name of command}CommandResponse payload` - This is the data of your command response. The type `T` of this payload depends on what you defined in your schema as the payload.

Returns: a `Response` object containing the data specified in the parameters.

```csharp
MyComponent.MyCommand.Response.CreateResponseFailure(ReceivedRequest req, string failureMessage);
```

Parameters:

  * `ReceivedRequest req` - The request that you want to send a response for.
  * `string failureMessage` - A string containing the reason for failing the command.

Returns: a `Response` object containing the data specified in the parameters.

### ReceivedResponse

Stores the received command response to enable you to handle it appropriately.

**Fields**

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| RequestId | `long` | The request ID of the original `Request` this response is reacting to. |
| EntityId | `EntityId` | The ID of the SpatialOS entity from where the command originated from. |
| StatusCode | [StatusCode](https://docs.improbable.io/reference/latest/csharpsdk/api-reference#fields-4) | A status code describing the outcome. |
| RequestPayload | `TRequest` | The data of the original command request. Its type `TRequest` is the argument type of the command as defined in schema. |
| ResponsePayload | `TResponse` | If the command succeeded, this stores the data of the command response. Otherwise it is `null`. The type `TResponse` is the return type of the command as defined in schema. |
| Message | `string` |The associated error message, if the command failed. Otherwise `null`. |
| Context | `System.Object` | The arbitrary context object given in the `Request` this response is reacting to. |
