<%(TOC)%>
# Heartbeating

Heartbeating is how a server-worker continually checks that a client-worker has not disconnected from SpatialOS.

## How does it work?

Generally, a server sends a heartbeat request at regular intervals to clients. If the server continually receives failures or is unable to get a response back from a client, the server will assume the client has disconnected. The server then deletes the Player associated with that client.

## How is it implemented in the GDK?

### Initialisation

On the server-worker, the [`PlayerHeartbeatInitializationSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/PlayerHeartbeatInitializationSystem.cs) checks for ECS entities in the worker's view that have a `PlayerHeartbeatClient` component, but do not yet have the `HeartbeatData` component. For all these entities, a `HeartbeatData` component is added to the ECS entity at the end of the current tick.

The `HeartbeatData` component is used to determine which entities need to be sent `PlayerHeartbeat` requests and to track how many consecutive failed heartbeats a player has.

### Sending PlayerHeartbeat requests

At intervals set by `PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds`, the [`SendPlayerHeartbeatRequestSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/SendPlayerHeartbeatRequestSystem.cs) finds all entities where the server-worker has write authority over the `PlayerHeartbeatServer` component and the entity has the `HeartbeatData` component. It then sends a `PlayerHeartbeat` request to each entity, which is expected to by handled by the client-worker.

Note that there are numerous ways that the request may fail, as outlined [here](https://docs.improbable.io/reference/latest/shared/design/commands#failure-modes).

### Handling PlayerHeartbeat requests

The client-worker runs the [`HandlePlayerHeartbeatRequestSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatRequestSystem.cs), which sends a response back to each `PlayerHeartbeat` request that was sent to that specific client-worker.

### Handling PlayerHeartbeat responses

Back on the server-worker, the [`HandlePlayerHeartbeatResponseSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatResponseSystem.cs) iterates through the `PlayerHeartbeat` responses received. If at least one successful response was received from a particular Player entity, the `NumFailedHeartbeats` inside its `HeartbeatData` component is set to 0.

If no responses were received with a `Success` status code from a Player entity, the `NumFailedHeartbeats` field is incremented. In the case that an entity has more failed heartbeats than the number configured in `PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats`, a request to delete the SpatialOS entity is sent.
