[//]: # (Doc of docs reference 8.2)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - Callback on what note below)
[//]: # (TODO - split API section into a different doc)
[//]: # (TODO - add how to do spawn logic in code example below)

<%(TOC)%>
# Commands: Sending and receiving component commands
_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities#spatialos-entities)._

Before reading this document, make sure you are familiar with:

  * [World and component command requests and responses]({{urlRoot}}/content/world-component-commands-requests-responses)
  * [Readers and Writers]({{urlRoot}}/content/gameobject/readers-writers)
  * [Read and write access]({{urlRoot}}/content/glossary#authority)

### About commands
Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers]({{urlRoot}}/content/workers/workers-in-the-gdk). Commands are relevant to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities).<br/>

There are two types of commands in SpatialOS:

* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities]({{urlRoot}}/content/glossary#spatialos-entity).
* **Component commands** you set up in your [schema]({{urlRoot}}/content/glossary#schema) for workers to invoke on any SpatialOS entity’s components.

This document is about MonoBehaviour component commands. The commands documentation is:

* Both workflows - [world and component command requests and responses]({{urlRoot}}/content/world-component-commands-requests-responses)
* [MonoBehaviour world commands]({{urlRoot}}/content/gameobject/world-commands)
* [ECS world commands]({{urlRoot}}/content/ecs/world-commands)
* Monobehaviour component commands - this document
* ECS component commands, see [ECS: Sending and receiving component commands]({{urlRoot}}/content/ecs/sending-receiving-component-commands)


### How to send and receive component commands
To send and handle commands the GDK automatically generates the following types

  * `CommandRequestSender` for sending command requests
  * `CommandRequestHandler` for handling received command requests
  * `RequestResponder` for handling received command requests and sending command responses
  * `CommandResponseHandler` for handling command responses

These can be injected into your MonoBehaviour scripts in the same way as [Readers and Writers]({{urlRoot}}/content/gameobject/readers-writers) by defining a field of one of the above-mentioned types and decorating it with the `[Require]` attribute.

Any worker can send command requests and handle command responses for any command. However, only workers that have [write access]({{urlRoot}}/content/glossary#authority) over the component that the command was defined in are able to handle command requests and send command responses.

### Examples
We use the following [schema]({{urlRoot}}/content/glossary#schema) for all examples described in this documentation.
```
package playground;

component CubeSpawner
{
    id = 12002;
    command improbable.common.Empty spawn_cube(improbable.common.Empty);
}
```
The GDK generates the following classes in the `Playground` namespace:

  * `CubeSpawner.SpawnCube.Request`
  * `CubeSpawner.SpawnCube.ReceivedRequest`
  * `CubeSpawner.SpawnCube.Response`
  * `CubeSpawner.SpawnCube.ReceivedResponse`
  * `CubeSpawner.Requirable.Reader`
  * `CubeSpawnerder.Requirable.Writer`
  * `CubeSpawner.Requirable.CommandRequestSender`
  * `CubeSpawner.Requirable.CommandRequestHandler`
  * `CubeSpawner.SpawnCube.RequestResponder`
  * `CubeSpawner.Requirable.CommandResponseHandler`

#### How to send command requests
The following code snippet provides an examples on how to send a command request.
This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.
```csharp
using Playground;

public class BuildCommandSenderBehaviour : MonoBehaviour
{
    [Require] CubeSpawner.Requirable.CommandRequestSender commandSender;

    private void SendRequest(EntityId targetId)
    {
        if (commandSender != null)
        {
            commandSender.SendSpawnCubeRequest(targetId, new Empty());
        }
    }
}
```


#### How to respond to command requests
The following code snippet provides an examples on how to respond to command request.
This example MonoBehaviour would be enabled only on workers that have [write access]({{urlRoot}}/content/glossary#write-access).

```csharp
using Playground;

public class SpawnCubeCommandRequestHandlerBehaviour : MonoBehaviour
{
    [Require] Builder.Requirable.CommandRequestHandler requestHandler;

    void OnEnable()
    {
        // register callback for command OnBuildWallRequest
        requestHandler.OnSpawnCubeRequest += HandleSpawnCubeRequest;
    }

    void OnDisable()
    {
        // Do not deregister the callback here.
        // Any registered callback is automatically deregistered
        // when the MonoBehaviour gets disabled.
    }

    void HandleSpawnCubeRequest(CubeSpawner.SpawnCube.RequestResponder responder)
    {
        // retrieve information about the request
        var payload = responder.Request.Payload;
        // do spawn logic....
        // send response and decide whether the command succeeded or failed
        if (SpawnSucceeded(out var entityId))
        {
            responder.SendResponse(new SpawnCubeResponse
            {
                CreatedEntityId = entityId
            });
        }
        else
        {
            responder.SendResponseFailure("Failed to create new cube!");
        }
    }
}
```
[//]: # (TODO - add how to do spawn logic in code example above)

> For an example of how to create an entity, you can look at the [how to create and delete entities]({{urlRoot}}/content/gameobject/create-delete-spatialos-entities) document.

#### How to receive command responses
You can only listen to command responses on the entity that you sent the request from. This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.

```csharp
using Playground;

public class SpawnCubeCommandResponseHandlerBehaviour : MonoBehaviour
{
    [Require] CubeSpawner.Requirable.CommandResponseHandler responseHandler;

    void OnEnable()
    {
        responseHandler.OnSpawnCubeResponse += OnSpawnCubeResponse;
    }

    void OnSpawnCubeResponse(CubeSpawner.SpawnCube.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            // Command succeeded. Handle the response
            var payload = response.Payload;
        }
        else
        {
            // Command failed. Handle the failure
            // The payload will be null in this case.
            Debug.Log(response.Message);
        }
    }
}
```
[//]: # (TODO - split this section into a different doc)
### API

#### CommandRequestSender
The `CommandRequestSender` is used to send command requests and provides one method per command it is responsible for:
```csharp
long Send{name of command}Request(EntityId entityId, TPayload payload, uint? timeoutMillis, bool allowShortCircuiting, object context);
```
Parameters:

  * `EntityId entityId` - The id of the SpatialOS entity that you want to send the command to.
  * `TPayload payload` - This is the payload of your command. The type `TPayload` depends on what you defined in your schema as the payload.
  * `uint? timeoutMillis` - Optional. Specifies after how many milliseconds this command should time out. This is null by default implying that the default of 5 seconds of the underlying Worker SDK will be used.
  * `bool allowShortCircuiting` - Optional. A boolean describing whether or not the command can be handled without going through the Runtime if it would go to the same worker. See the [SpatialOS documentation](https://docs.improbable.io/reference/latest/csharpsdk/using/sending-data#sending-component-commands) for more details.
  * `object context` - Optional. An arbitrary object you can associate with the command which you also get back along with the response. This is useful to pass more information about the situation to the code handling the response.

Returns: the request id, stored as a `long`, corresponding to the request.


#### CommandRequestHandler
The `CommandRequestHandler` is used to handle command requests and send command responses.
Ensure that for each type of command request that you listen to, you only inject the corresponding `CommandRequestHandler` only once per GameObject.
If injected multiple times, only one Handler will receive the request and be able to send a response.

It provides the following for each command it is responsible for.
```csharp
event Action<{name of command}.RequestResponder> On{name of command}Request;
```

[//]: # (TODO - callback on what - JB comment)
Whenever a request gets received, a callback on this delegate of `CommandRequestHandler` will be triggered with a `RequestResponder`object.

#### RequestResponder
This object provides you with the capability to view the received request and respond to the command request.
The API is as follows

**Fields:**

| Field         	| Type 	| Description                        	|
|-------------------|----------|----------------------------------------|
| Request | [ReceivedRequest]({{urlRoot}}/content/world-component-commands-requests-responses) | Contains information about the received command request. |

**Methods:**
```csharp
void SendResponse(TPayload payload);
```
Parameters:

  * `TPayload payload`: The payload of the command response that you want to send. The exact type will be code-generated according to the response type specified for the command type within the schema declaration for this component.

```csharp
void SendResponseFailure(string message);
```
Parameters:

  * `string message`: The message that you want to send to indicate the reason for failing the command.

> You are only able to send either a success response or a failed response once.
Calling `SendResponse` or `SendResponseFailure` multiple times will not result in sending multiple responses.

#### CommandResponseHandler
The `CommandResponseHandler`is used to handle command responses and provides one event for each command it is responsible for.
You can register callbacks by subscribing to the events declared for this class.
To learn more about the API of a `ReceivedResponse` API go to [this doc]({{urlRoot}}/content/world-component-commands-requests-responses).

**Methods:**

```csharp
event Action<{name of command}.ReceivedResponse> On{name of command}Response;
```
