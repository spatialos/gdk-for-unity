[//]: # (Doc of docs reference 8.2)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - Callback on what note below)
[//]: # (TODO - split API section into a different doc)
[//]: # (TODO - add how to do spawn logic in code example below)

<%(TOC)%>
# Component commands

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/which-workflow#spatialos-entities)._

Before reading this document, make sure you are familiar with:

  * [Readers and Writers]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/index)
  * [Read and write access]({{urlRoot}}/reference/glossary#authority)

### About commands

Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers]({{urlRoot}}/reference/concepts/worker). Commands are relevant to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/which-workflow).<br/>

There are two types of commands in SpatialOS:

* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities]({{urlRoot}}/reference/glossary#spatialos-entity).
* **Component commands** you set up in your [schema]({{urlRoot}}/reference/glossary#schema) for workers to invoke on any SpatialOS entity’s components.

This document is about MonoBehaviour component commands. The commands documentation is:

* [MonoBehaviour world commands]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/world-commands)
* [ECS world commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands)
* MonoBehaviour component commands - this document
* [ECS component commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/component-commands)
* Both workflows - [world commands API reference]({{urlRoot}}/api/core/commands/world-commands)

### How to send and receive component commands

To send and handle commands the GDK automatically generates the following types for each SpatialOS component with commands.

  * `{component name}CommandSender` for sending command requests and handling command responses.
  * `{component name}CommandReceiver` for handling command requests and sending command responses.

These can be injected into your MonoBehaviour scripts in the same way as [Readers and Writers]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/index) by defining a field of one of the above-mentioned types and decorating it with the `[Require]` attribute.

Any worker can send command requests and handle command responses for any command. However, only workers that have [write access]({{urlRoot}}/reference/glossary#authority) over the component that the command was defined in are able to handle command requests and send command responses.

### Examples

We use the following [schema]({{urlRoot}}/reference/glossary#schema) for all examples described in this documentation.
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
  * `CubeSpawnerReader`
  * `CubeSpawnerWriter`
  * `CubeSpawnerCommandSender`
  * `CubeSpawnerCommandReceiver`

#### How to send command requests

The following code snippet provides an example on how to send a command request and register a callback for the response.
This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.

```csharp
using Improbable.Gdk.Subscriptions;
using Playground;

public class BuildCommandSenderBehaviour : MonoBehaviour
{
    [Require] CubeSpawnerCommandSender commandSender;

    private void SendRequest(EntityId targetId)
    {
        if (commandSender != null)
        {
            commandSender.SendSpawnCubeCommand(targetId, new Empty(), OnSpawnCubeResponse);
        }
    }

    private void OnSpawnCubeResponse(CubeSpawner.SpawnCube.ReceivedResponse response) {
        // Do something with the response.
    }
}
```

#### How to respond to command requests

The following code snippet provides an examples on how to respond to command request.
This example MonoBehaviour would be enabled only on workers that have [write access]({{urlRoot}}/reference/glossary#write-access).

```csharp
using Improbable.Gdk.Subscriptions;
using Playground;

public class SpawnCubeCommandRequestHandlerBehaviour : MonoBehaviour
{
    [Require] CubeSpawnerCommandReceiver commandReceiver;

    void OnEnable()
    {
        // register callback for command OnBuildWallRequest
        commandReceiver.OnSpawnCubeRequestReceived += HandleSpawnCubeRequest;
    }

    void OnDisable()
    {
        // Do not deregister the callback here.
        // Any registered callback is automatically deregistered
        // when the MonoBehaviour gets disabled.
    }

    void HandleSpawnCubeRequest(CubeSpawner.SpawnCube.ReceivedRequest request)
    {
        // retrieve information about the request
        var payload = request.Payload;
        // do spawn logic....
        // send response and decide whether the command succeeded or failed
        if (SpawnSucceeded(out var entityId))
        {
            commandReceiver.SendSpawnCubeResponse(request.RequestId, new Improbable.Common.Empty());
        }
        else
        {
            responder.SendSpawnCubeFailure(request.RequestId, "Failed to create new cube!");
        }
    }
}
```
[//]: # (TODO - add how to do spawn logic in code example above)

> For an example of how to create an entity, you can look at the [how to create and delete entities]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/create-delete-spatialos-entities) document.

### API

#### CommandSender

The `CommandSender` is used to send command requests, it provides two methods per command. They are responsible for:

```csharp
void Send{name of command}Command(EntityId targetEntityId, TPayload payload, Action<ReceivedResponse> callback = null);
```

Parameters:

  * `EntityId entityId` - The id of the SpatialOS entity that you want to send the command to.
  * `TPayload payload` - This is the payload of your command. The type `TPayload` depends on what you defined in your schema as the payload.
  * `Action<ReceivedResponse> callback` - Optional. Registers a callback that will be called when the command response is received.

```csharp
void Send{name of command}Command(TRequest request, Action<ReceivedResponse> callback = null);
```

Parameters:

  * `TRequest request` - This is the code-generated request object for this command, the type will be `{component name}.{command name}.Request`.
  * `Action<ReceivedResponse> callback` - Optional. Registers a callback that will be called when the command response is received.

#### CommandReceiver

The `CommandReceiver` is used to handle command requests and send command responses.

Ensure that for each type of command request that you listen to, you only inject the corresponding `CommandReceiver` only once per GameObject. If injected multiple times, only one Handler will receive the request and be able to send a response.

It provides the following C# event for each command it is responsible for.

```csharp
event Action<{name of command}.ReceivedRequest> On{name of command}RequestReceived;
```

[//]: # (TODO - callback on what - JB comment)
Whenever a request is received, all callbacks registered to this event will be invoked with the `ReceivedRequest` object.

It also provides three methods per command it is responsible for:

```csharp
void Send{name of command}Response(TResponse response);
```

Parameters:
  * `TResponse response`: This is the code generated response object for this command, the type will be `{component name}.{command name}.Response`.

```csharp
void Send{name of command}Response(long requestId, TPayload response);
```

Parameters:
  * `TPayload payload` - This is the response payload of your command. The type `TPayload` depends on what you defined in your schema as the response payload.

```csharp
void Send{name of command}Failure(long requestId, string string);
```

Parameters:
  * `string failureMessage`: The message that you want to send to indicate the reason for failing the command.
