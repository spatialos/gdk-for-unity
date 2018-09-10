**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

## ECS: Sending and receiving commands

Commands are SpatialOS's equivalent of remote procedure calls.

> For more information about what commands are and what their purpose is, see [this section on commands](https://docs.improbable.io/reference/latest/shared/design/commands#component-commands) in the SpatialOS documentation.

### Sending command requests

A worker instance can send a command using a `ComponentName.CommandSenders.CommandName` ECS component, where `ComponentName` is the SpatialOS component the command is defined in and `CommandName` is the name of the command in schema.

Note that a worker instance _does not need_ authority over the relevant SpatialOS component to send commands to a SpatialOS entity.

Because of this, the SpatialOS GDK for Unity (GDK) attaches a `ComponentName.CommandSenders.CommandName` for each command to all ECS entities that represent a SpatialOS entity. This means any ECS entity can send any command to any SpatialOS entity.

Given this schema:

```
package playground;
import "improbable/transform.transform.schema"

type BuildRequest
{
    improbable.transform.Location location = 1;
    improbable.transform.Quaternion rotation = 2;
}

type BuildResponse
{
    bool success = 1;
    string error_message = 2;
    EntityId created_entity_id = 3;
}

component Builder
{
    id = 12002;
    command BuildResponse build_wall(BuildRequest);
}
```

The GDK generates these types:

* `BuildRequest` - Equivalent of the schema type.
* `BuildResponse` - Equivalent of the schema type.
* `Builder.BuildWall.Request` - Represents a `build_wall` command request that you wish to send. Holds metadata associated with the request (for example, the target EntityId) and a `BuildRequest` struct.
* `Builder.BuildWall.Response` - Represents the `build_wall` command response that you wish to send. We provide two helper methods for constructing these: `CreateResponse` for successful commands and `CreateFailure` for unsuccessful commands.
* `Builder.BuildWall.ReceivedRequest` - Represents a `build_wall` command request that you have received. Holds metadata associated with the request (for example, the caller attribute set) and a `BuildRequest` struct.
* `Builder.BuildWall.ReceivedResponse` - Represents a `build_wall` command response that you have received. Holds metadata associated with the response (for example, the status code). If the command was successful the `ResponsePayload` field will be set and if the command was unsuccessful the `Message` field will be set.

The corresponding ECS component for sending commands is `Builder.CommandSenders.BuildWall`, which has a list of `Builder.BuildWall.Request` objects on it. To send a command, add a new request object to the list.

Here's an example of sending a command request:

```csharp
public class BuildSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<ShouldSendBuildWallCommand> DenotesShouldSendCommand; // Non-SpatialOS component
        public ComponentDataArray<Builder.CommandSenders.BuildWall> BuildWallSender;
        public ComponentDataArray<SpatialEntityId> EntityIds;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var requestSender = data.BuildWallSender[i];
            var entityId = data.EntityIds[i];

            Builder.BuildWall.Request request = new Builder.BuildWall.Request
            {
                TargetEntityId = entityId,
                Payload = new BuildRequest
                {
                    Location = new Location(...),
                    Rotation = new Rotation(...)
                }
            };


            requestSender.RequestsToSend.Add(request);
        }
    }
}
```

This system is an example of sending command requests. Unity injects all ECS entities which have the `ShouldSendBuildWallCommand`, `Builder.CommandSenders.BuildWall`, and `SpatialEntityId` components into this system before `OnUpdate`. This system iterates over the injected entities and sends a `build_wall` command request to each of them.

To send a `build_wall` command in a system, you need to inject `Builder.CommandSenders.BuildWall` into the system like any other ECS component.

### Responding to command requests

When a worker instance receives a command request, the command request is represented with reactive ECS components.

The GDK attaches a `ComponentName.CommandRequests.CommandName` ECS component to the specified ECS entity: where `ComponentName` is the SpatialOS component the command is defined in, `CommandName` is the name of the command in schema. `ComponentName.CommandRequests.CommandName` contains a list of type `ComponentName.CommandName.ReceivedRequest`. The GDK cleans it up at the end of the tick.

To respond to the request, use `ComponentName.CommandResponders.CommandName` for that given command. This contains a list of type `ComponentName.CommandName.Response`. Create and add a `ComponentName.CommandName.Response` object to this list and the GDK will send the response for you.

Here's an example of receiving a command request and acting on it, using the same schema as above:

```csharp
public class BuildWallHandlerSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Builder.CommandRequests.BuildWall> BuildWallRequests;
        public ComponentDataArray<Builder.CommandResponders.BuildWall> BuildWallResponders;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var requests = data.BuildWallRequests[i];
            var responder = data.BuildWallResponders[i];

            foreach (var request in requests.Requests)
            {
                // Do something with the request
                var buildRequest = request.Request;

                Builder.BuildWall.Response buildResponse = Builder.BuildWall.CreateResponse
                (
                    buildRequest,
                    new BuildResponse(...)
                );

                responder.ResponsesToSend.Add(buildResponse);
            }
        }
    }
}
```

`build_wall` command requests are on a `Builder.CommandRequest.BuildWall` ECS component. The worker instance should respond to the request using the `Builder.CommandResponders.BuildWall` ECS component.

### Receiving command responses

Like requests, when an ECS entity receives a command response, the GDK attaches a `ComponentName.CommandResponses.CommandName` ECS component to the ECS entity, where `ComponentName` is the SpatialOS component the command is defined in and `CommandName` is the name of the command in schema.

`ComponentName.CommandResponses.CommandName` contains a list of `ComponentName.CommandName.ReceivedResponse`. The `ComponentName.CommandName.ReceivedResponse` includes the payload of the response and the request payload that was originally sent with the command. **This payload is null** when the command fails.

The ECS entity that sent the request receives the response.

Here's an example of receiving a command response, using the same schema as above:

```csharp
public class BuildWallResponseHandler : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Builder.CommandResponses.BuildWall> BuildWallResponses;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var responses = data.BuildWallResponses[i];

            foreach (var response in responses.Responses)
            {
                if (response.StatusCode != StatusCode.Success)
                {
                    // Something bad happened!
                    continue;
                }

                var responsePayload = response.ResponsePayload; // guaranteed to not be null at this point
                var requestPayload = response.RequestPayload; // original request payload

                // Do something
            }
        }
    }
}
```

`build_wall` command responses are on a `Builder.CommandResponses.BuildWall` ECS component.

### World commands

World commands are RPCs to request specific things from the SpatialOS.

> They're different to component commands (which the sections above this cover), which are user-defined in schema. For more information, see [World commands](https://docs.improbable.io/reference/latest/shared/design/commands#world-commands)in the SpatialOS documentation.

Each ECS entity that represents a SpatialOS entity has a set of components for sending world commands. For each world command, there is a component to send the command and receive the response.

* ReserveEntityIds
    * Sending a request - `WorldCommands.ReserveEntityIds.CommandSender`. This contains a list of `WorldCommands.ReserveEntityIds.Request` structs. Add a struct to the list to send the command.
        * `TimeoutMillis` is optional.
    * Receiving a response - `WorldCommands.ReserveEntityIds.CommandResponses`. This contains a list of `WorldCommands.ReserveEntityIds.ReceivedResponse` structs.
* CreateEntity
    * Sending a request - `WorldCommands.CreateEntity.CommandSender`. This contains a list of `WorldCommands.CreateEntity.Request` structs. Add a struct to the list to send the command.
        * `EntityId` and `TimeoutMillis` are optional.
        * If you do specify an `EntityId`, you need to get this from a `ReserveEntityIds` command.
    * Receiving a response - `WorldCommands.CreateEntity.CommandResponses`. This contains a list of `WorldCommands.CreateEntity.ReceivedResponse`.
* DeleteEntity
    * Sending a request - `WorldCommands.DeleteEntity.CommandSender`. This contains a list of `WorldCommands.DeleteEntity.Request` structs. Add a struct to the list to send the command.
        * `TimeoutMillis` is optional.
    * Receiving a response - `WorldCommands.DeleteEntity.CommandResponses`. This contains a list of `WorldCommands.DeleteEntity.ReceivedResponse`.
* EntityQuery
    * Sending a request - `WorldCommands.EntityQuery.CommandSender`. This contains a list of `WorldCommands.EntityQuery.Request` structs. Add a struct to the list to send the command
        * For more information, see [entity queries](https://docs.improbable.io/reference/latest/shared/glossary#queries) in the SpatialOS documentation.
        * `TimeoutMillis` is optional.
    * Receiving a response - `WorldCommands.EntityQuery.CommandResponses`. This contains a list of `WorldCommands.EntityQuery.ReceivedResponse`.


When a response is received, the GDK attaches an ECS component to the ECS entity that originally sent the request. The ECS component it attaches corresponds to the response it receives.

Here's an example of creating a SpatialOS entity:

```csharp
public class CreateEntitySystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Foo> Foo;
        public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var entity = EntityBuilder.Begin()
                ...
                ...
                .Build();

            var request = new WorldCommands.CreateEntity.Request
            {
                Entity = entity
            };

            data.CreateEntitySender[i].RequestsToSend.Add(request);
        }
    }
}
```

This system iterates through every entity with a `Foo` SpatialOS component and sends a create entity request.

For more information on how to compose an entity definition using the `EntityBuilder`, see the [creating an entity](create-entity.md#create-an-entity-definition-using-the-entitybuilder) page.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).
