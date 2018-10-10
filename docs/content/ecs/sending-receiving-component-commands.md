**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only.

-----
[//]: # (Doc of docs reference 34)
[//]: # (Tech writer review)
# (ECS) Sending and receiving component commands
 _This document relates to the [ECS workflow](../intro-workflows-spos-entities.md)._

Before reading this document, make sure you are familiar with:
* [World and component command requests and responses](../world-component-commands-requests-responses.md)
  * [Readers and Writers](./readers-writers.md)
  * [Read and write access](./glossary.md#authority)

## About commands
Commands are SpatialOS's equivalent of [remote procedure calls (Wikipedia)](https://en.wikipedia.org/wiki/Remote_procedure_call). You use commands to send messages between two [workers](../workers/workers-in-the-gdk.md). Commands are relevant to both [GameObject-MonoBehaviour and ECS workflows](../intro-workflow-spos-entities.md).<br/>

There are two types of commands in SpatialOS:
* **World commands** are pre-set commands for reserving, creating, deleting and requesting information about [SpatialOS entities](../glossary.md#spatialos-entities).
* **Component commands** you set up in your [schema](../glossary.md#schema) for workers to invoke on any SpatialOS entity’s components. 

This document is about GameObject-MonoBehaviour component commands. The commands documentation is:
* [GameObject-MonoBehaviour world commands](./gomb-world-commands)
* [ECS world commands](../ecs/ecs-world-commands.md)
* GameObject-Monobehaviour component commands, see [(GameObject-MonoBehaviour) Sending and receiving component commands](../gameobject/sending-receiving-commands.md)
* ECS component commands - this document.
* Both workflows - [world and component command requests and responses](../world-component-commands-requests-responses.md)


## How to send and receive component commands
The GDK generates the following ECS components to allow you to send and receive commands using the ECS flow:
  * `{name of component}.CommandSenders.{name of command}`: allows you to send command requests
  * `{name of component}.CommandResponders.{name of command}`: allows you to send command responses
  * [Command request and response structs](): allows you to create and handle incoming and outgoing command requests and responses

We use the following schema for all examples described in this documentation.
```
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
// This ensures all command requests that you want to send are added before the 
// SpatialOSSendSystem in which all commands will be sent
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class SendSpawnCubeRequestSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<SpatialEntityId> EntityIds;
        // This is a non-SpatialOS ECS component to trigger the spawning
        [ReadOnly] public ComponentDataArray<ShouldSpawnCube> DenotesShouldSpawnCube;
        public ComponentDataArray<CubeSpawner.CommandSenders.SpawnCube> SpawnCubeSenders;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var requestSender = data.SpawnCubeSenders[i];
            var targetEntityId = data.EntityIds[i];
            // create the request you want to send
            var request = new CubeSpawner.SpawnCube.CreateRequest
            (
                targetEntityId,
                new Empty()
            );
            // add it to the list of command requests to be sent at the end of the current update loop
            requestSender.RequestsToSend.Add(request);
            data.SpawnCubeSenders[i] = requestSender;
        }
    }
}
```

## How to handle received command requests

The following code snippet provides an example on how to process and respond to a received command request.
This example ECS system would run only on workers that have this system added to their ECS world and have write access over the corresponding component.

```csharp
// This ensures all command requests that you want to handle are processed before the 
// CleanReactiveComponentsSystem cleans them up.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class HandleSpawnCubeRequestSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<CubeSpawner.CommandRequests.SpawnCube> ReceivedSpawnCubeRequests;
        public ComponentDataArray<CubeSpawner.CommandResponders.SpawnCube> CommandResponders;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var spawnCubeRequests = data.ReceivedSpawnCubeRequests[i];
            var responder = data.CommandResponders[i];

            foreach (var spawnCubeRequest in requests.Requests)
            {
                // handle each SpawnCube request you received
                // ...
                // create a SpawnCube response
                var spawnCubeResponse = CubeSpawner.SpawnCube.CreateResponse
                (
                    spawnCubeRequest,
                    new Empty()
                );

                // add it to the list of command responses to be sent at the end of the current update loop
                responder.ResponsesToSend.Add(spawnCubeResponse);
            }
            data.CommandResponders[i] = responder;
        }
    }
}
```

## How to handle received command responses

The following code snippet provides an example on how to handle a command response.
This example ECS system would run on any worker that has this system added to its ECS world.

```csharp
// This ensures all command responses that you want to handle are processed before the 
// CleanReactiveComponentsSystem cleans them up.
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class HandleSpawnCubeResponseSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<CubeSpawner.CommandResponses.SpawnCube> ReceivedSpawnCubeResponses;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var responses = data.ReceivedSpawnCubeResponses[i];

            foreach (var response in responses.Responses)
            {
                if (response.StatusCode != StatusCode.Success)
                {
                    // Handle command failure
                    continue;
                }

                var responsePayload = response.ResponsePayload; 
                var requestPayload = response.RequestPayload;
                // Handle SpawnCube response
            }
        }
    }
}
