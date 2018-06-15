**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

## Sending and receiving commands

Commands are SpatialOS's equivalent of remote procedure calls.

> For more information about what commands are and what their purpose is, see [this section on commands](https://docs.improbable.io/reference/13.0/shared/design/commands#component-commands) in the SpatialOS documentation.

### Sending command requests

A worker instance can send a command using a `CommandRequestSender<T>` ECS component, where `T` is the SpatialOS component the command is defined in.

Note that a worker instance _does not need_ authority over the relevant SpatialOS component to send commands to a SpatialOS entity.

Because of this, the Unity GDK attaches a `CommandRequestSender<T>` (where `T` is the SpatialOS component the command is defined in) for each SpatialOS component with a command to all ECS entities. This means any ECS entity can send any command to any SpatialOS entity. 

Given this schema:

```
package demogame;
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

The Unity GDK generates these types:

* `BuildRequest` - Equivalent of the schema type.
* `BuildResponse` - Equivalent of the schema type.
* `BuildWallRequest` - Represents the `build_wall` command request. Holds metadata associated with the request (for example, the caller's attribute set) and a `BuildRequest` object. Implements `IIncomingCommandRequest`.
* `BuildWallResponse` - Represents the `build_wall` command response. Holds metadata associated with the response (e.g. status code) and a `BuildResponse` object. Implements `IIncomingCommandResponse`.

The corresponding ECS component for sending commands is `CommandRequestSender<Builder>`, which has the function `SendBuildWallRequest(long targetEntityId, BuildRequest buildRequest)` to send the command request.

Here's an example of sending a command request:

```csharp
public class BuildSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<SpatialOSBuilder> Builders;
        public ComponentDataArray<CommandRequestSender<SpatialOSBuilder>> BuilderRequestSender;
        public ComponentDataArray<SpatialEntityId> EntityIds;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var requestSender = data.BuilderRequestSender[i];
            var entityId = data.EntityIds[i];
            
            BuildRequest buildRequest = new BuildRequest
            {
                Location = new Location(...),
                Rotation = new Rotation(...)
            }
            
            requestSender.SendBuildWallRequest(entityId, buildRequest);
        }
    }
}
```

This system runs on a client. It injects all SpatialOS entities in the client's view that have the `Builder` SpatialOS component, and sends a `build_wall` command to each SpatialOS entity (which will be received by the managed worker).

To send a `build_wall` command in a system, you need to inject `CommandRequestSender<Builder>` into the system like any other ECS component.

### Responding to command requests

When a worker instance receives a command request, the command request is represented with reactive ECS components. 

The Unity GDK attaches a `CommandRequests<T>` ECS component to the specified ECS entity (where `T` implements `IIncomingCommandRequest`). `CommandRequests<T>` holds a list of `T`. The Unity GDK cleans it up at the end of the tick.

You can use the object in the list itself to respond to that particular request.

> **Note**: `CommandRequests<T>` is a `Component`, not an `IComponentData`. This means you must use a `ComponentArray<T>` for injection rather than a `ComponentDataArray<T>`.

Here's an example of receiving a command request and acting on it, using the same schema as above:

```csharp
public class BuildWallHandlerSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<CommandRequests<BuildWallRequest>> BuildWallRequests;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var requests = data.BuildWallRequests[i];

            foreach (var request in requests.Buffer)
            {
                // Do something with the request
                var buildRequest = request.Request;
                
                BuildResponse buildResponse = new BuildResponse
                {
                    ...
                }
                
                request.SendBuildWallResponse(buildResponse);
            }
        }
    }
}
```

`build_wall` command requests are on a `CommandRequests<BuildWallRequest>` ECS component. The worker instance should respond to the request using the `SendBuildWallResponse(BuildResponse buildResponse)` method on the request itself.

### Receiving command responses

Like requests, when an ECS entity receives a command response, the Unity GDK attaches a `CommandResponses<T>` ECS component to the ECS entity (where `T` implements `IIncomingCommandResponse`).

The ECS entity that sent the request receives the response. The response object includes the payload of the response and the request payload that originally send the command. **This payload is null** when no payload is sent back.

> **Note**: `CommandResponses<T>` is a `Component`, not an `IComponentData`. This means you must use a `ComponentArray<T>` for injection rather than a `ComponentDataArray<T>`.

Here's an example of receiving a command response, using the same schema as above:

```csharp
public class BuildWallResponseHandler : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<CommandResponses<BuildWallResponse>> BuildWallResponses;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var responses = data.BuildWallResponses[i];

            foreach (var response in responses.Buffer)
            {
                if (response.StatusCode != StatusCode.Success)
                {
                    // Something bad happened!
                    continue;
                }
                
                var responsePayload = response.Response; // guaranteed to not be null at this point
                var requestPayload = response.Request; // original request payload

                // Do something
            }
        }
    }
}
```

`build_wall` command responses are on a `CommandResponses<BuildWallResponse>` ECS component.

### World commands

World commands are RPCs to request specific things from the SpatialOS. 

> They're different to component commands (which the sections above this cover), which are user-defined in schema. For more information, see [World commands](https://docs.improbable.io/reference/13.0/shared/design/commands#world-commands)in the SpatialOS documentation.

Every ECS entity has a `WorldCommandSender` ECS component for sending world commands. For each world command, there is a corresponding method to send the command and response object.

* ReserveEntityIdsRequest

        Use this method to reserve entity IDs if you want to use them in a `SendCreateEntityRequest`.

        `timeoutMillis` is optional. 
    * Response - `ReserveEntityIdsResponse`
* CreateEntityRequest
    * Method - `SendCreateEntityRequest(Worker.Entity entity, long entityId = 0, uint timeoutMillis = 0)`

        `entityId` and `timeoutMillis` are optional.

        If you do specify an `entityId`, you need to get this from `SendReserveEntityIdsRequest`.
    * Response - `CreateEntityResponse`
* DeleteEntityRequest
    * Method - `SendDeleteEntityRequest(long entityId, uint timeoutMillis = 0)` 

        `timeoutMillis` is optional. 
    * Response - `DeleteEntityResponse`
* EntityQueryRequest
    * Method - `SendEntityQueryRequest(Worker.Query.EntityQuery entityQuery, uint timeoutMillis = 0)`

        For more information, see [entity queries](https://docs.improbable.io/reference/13.0/shared/glossary#queries) in the SpatialOS documentation.

        `timeoutMillis` is optional. 
    * Response - `EntityQueryResponse`

When a response is received, the Unity GDK attaches a `CommandResponses<T>` ECS component to the ECS entity that originally sent the request. `T` is the corresponding response type.

Here's an example of creating a SpatialOS entity:

```csharp
public class CreateEntitySystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<Foo> Foo;
        public ComponentDataArray<WorldCommandSender> WorldCommandSender;
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

            data.WorldCommandSender[i].SendCreateEntityRequest(entity);
        }
    }
}
```

This system iterates through every entity with a `Foo` SpatialOS component and sends a create entity request.

For more information on how to compose an entity definition using the `EntityBuilder`, see the [creating an entity](create-entity.md#create-an-entity-definition-using-the-entitybuilder) page.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).