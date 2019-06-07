<%(TOC)%>

# ECS: Component commands

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Commands](https://docs.improbable.io/reference/latest/shared/design/commands)
")%>

Component commands are defined in your [schema]({{urlRoot}}/reference/glossary#schema) for workers to invoke on any SpatialOS entity's components.

For each command defined in your schema components, the GDK generates the following types:

* `{component name}.{command name}.Request`
* `{component name}.{command name}.ReceivedRequest`
* `{component name}.{command name}.Response`
* `{component name}.{command name}.ReceivedResponse`

The GDK also includes a [`CommandSystem`]({{urlRoot}}/api/core/command-system) containing several methods that are used for sending and receiving command requests and responses.

## How to send command requests

To send a command request, the [`CommandSystem`]({{urlRoot}}/api/core/command-system) provides a `SendRequest` method, which requires a command request object of type `{component name}.{command name}.Request` as its argument.

The example below shows how to send a command request.

```csharp
// This ensures all command requests that you want to send are
// created before the `SpatialOSSendSystem` is run.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class SendSpawnCubeRequestSystem : ComponentSystem
{
    private CommandSystem commandSystem;

    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        commandSystem = World.GetExistingSystem<CommandSystem>();

        query = GetEntityQuery(
            ComponentType.ReadOnly<SpatialEntityId>(),
            ComponentType.ReadOnly<ShouldSpawnCube>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref SpatialEntityId spatialEntityId, ref ShouldSpawnCube cubeTrigger) =>
            {
                var targetEntityId = spatialEntityId.EntityId;

                // Create the request you want to send
                var cubeSpawnRequest = new CubeSpawner.SpawnCube.Request
                (
                    targetEntityId,
                    new Empty()
                );

                // Add to the set of command requests that the GDK
                // will send at the end of the current update loop
                commandSystem.SendCommand(cubeSpawnRequest);
            });
    }
}
```

## How to handle command requests and send responses

The [`CommandSystem`]({{urlRoot}}/api/core/command-system) has a `GetRequests<T>` method, which retrieves a list of received command requests of type T received by the worker. In this case, T must be of type `{component name}.{command name}.ReceivedRequest`. Iterate through the list returned from `GetRequests<T>` to handle each received command request.

To send a response back, the [`CommandSystem`]({{urlRoot}}/api/core/command-system) provides a `SendResponse` method, which requires a response object of type `{component name}.{command name}.Response` as its argument.

The example below shows how to handle a command request and send a command response back.

```csharp
// This ensures all received command requests are handled before being cleaned up
// by the `InternalSpatialOSCleanGroup`, and command responses that you want to
/// send are created before the `SpatialOSSendSystem` is run.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class HandleSpawnCubeRequestSystem : ComponentSystem
{
    private CommandSystem commandSystem;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        commandSystem = World.GetExistingSystem<CommandSystem>();
    }

    protected override void OnUpdate()
    {
        var spawnCubeRequests = commandSystem.GetRequests<CubeSpawner.SpawnCube.ReceivedRequest>();

        for (var i = 0; i < spawnCubeRequests.Count; i++)
        {
            var spawnCubeRequest = spawnCubeRequests[i];

            // Handle each SpawnCube request you received
            // ...

            // Create a SpawnCube response
            var spawnCubeResponse = new CubeSpawner.SpawnCube.Response
            (
                spawnCubeRequest.RequestId,
                new Empty()
            );

            // Add to the set of command responses that the GDK
            // will send at the end of the current update loop
            commandSystem.SendResponse(spawnCubeResponse);
        }
    }
}
```

## How to handle command responses

The [`CommandSystem`]({{urlRoot}}/api/core/command-system) has a `GetResponses<T>` method, which retrieves a list of received command responses of type T received by the worker. In this case, T must be of type `{component name}.{command name}.ReceivedResponse`. Iterate through the list returned from `GetResponses<T>` to handle each received command response.

The example below shows how to handle a command response.

```csharp
// This ensures all received command responses are handled before being cleaned up
// by the `InternalSpatialOSCleanGroup`.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class HandleSpawnCubeResponseSystem : ComponentSystem
{
    private CommandSystem commandSystem;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        commandSystem = World.GetExistingSystem<CommandSystem>();
    }

    protected override void OnUpdate()
    {
        var spawnCubeResponses = commandSystem.GetResponses<CubeSpawner.SpawnCube.ReceivedResponse>();

        for (var i = 0; i < spawnCubeResponses.Count; i++)
        {
            var spawnCubeResponse = spawnCubeResponses[i];

            if (spawnCubeResponse.StatusCode != StatusCode.Success)
            {
                // Handle command failure
                continue;
            }

            var responsePayload = spawnCubeResponse.ResponsePayload;
            var requestPayload = spawnCubeResponse.RequestPayload;

            // Handle SpawnCube response
        }
    }
}
```
