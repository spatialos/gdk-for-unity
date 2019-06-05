<%(TOC)%>

# ECS: Component commands

 _This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/overview)._

Before reading this document, make sure you are familiar with:

* [Read and write access]({{urlRoot}}/reference/glossary#authority)

## About commands

Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers]({{urlRoot}}/reference/concepts/worker). Commands are relevant to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/overview).<br/>

There are two types of commands in SpatialOS:

* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities]({{urlRoot}}/reference/glossary#spatialos-entity).
* **Component commands** you set up in your [schema]({{urlRoot}}/reference/glossary#schema) for workers to invoke on any SpatialOS entity’s components.

## How to send and receive component commands

The GDK generates the following ECS components to allow you to send and receive commands using the ECS flow:

* `{name of component}.CommandSenders.{name of command}`: allows you to send command requests
* `{name of component}.CommandResponders.{name of command}`: allows you to send command responses
* Command request and response structs

We use the following schema for all examples described in this documentation.
```schemalang
package playground;

component CubeSpawner
{
    id = 12002;
    command improbable.common.Empty spawn_cube(improbable.common.Empty);
}
```

The GDK generates the following types in the `Playground` namespace:

  * `CubeSpawner.SpawnCube.Request`
  * `CubeSpawner.SpawnCube.ReceivedRequest`
  * `CubeSpawner.SpawnCube.Response`
  * `CubeSpawner.SpawnCube.ReceivedResponse`
  * `CubeSpawner.CommandRequests.SpawnCube`
  * `CubeSpawner.CommandResponses.SpawnCube`
  * `CubeSpawner.CommandSenders.SpawnCube`
  * `CubeSpawner.CommandResponders.SpawnCube`

## How to send command requests

The following code snippet provides an example on how to send a command request.
This example ECS system would run on any worker that has this system added to its ECS world.

```csharp
// This ensures all command requests that you want to send are added before
// the GDK send systems are run.
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

## How to handle received command requests

The following code snippet provides an example on how to process and respond to a received command request.
This example ECS system would run only on workers that have this system added to their ECS world and have write access over the corresponding component.

```csharp
// This ensures all command requests that you want to handle are processed
// before they get cleaned up.
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

## How to handle received command responses

The following code snippet provides an example on how to handle a command response.
This example ECS system would run on any worker that has this system added to its ECS world.

```csharp
// This ensures all command responses that you want to handle are processed
// before they get cleaned up.
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
