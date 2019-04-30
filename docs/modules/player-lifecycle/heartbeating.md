<%(TOC)%>
# Heartbeats

Heartbeating is a technique used to ensure that client-workers are still connected. Unresponsive or disconnected clients are periodically removed from the game world.

## Why it's useful

Client-workers typically create a new player entity at some point after connecting to SpatialOS. Without the heartbeats, it is impossible for a server-worker to tell whether the client-worker is still connected and responsive.

By introducing heartbeats, we ensure that a player entity belonging to a disconnected or unresponsive client-worker is deleted from the world. This assures us that in a stable deployment each player entity in the world is "owned" by a client-worker that is connected or has been connected in the last N seconds, where N is the maximum number of failed heartbeats allowed multiplied by the heartbeat interval.

## How it works

A server sends a heartbeat request at regular intervals to the clients. If the server continually receives failures or the request times out, the server assumes that the client has disconnected. The server then deletes the player associated with that client.

## How it's implemented in the GDK

### Initialisation

On the server-worker, the [`PlayerHeartbeatInitializationSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/PlayerHeartbeatInitializationSystem.cs) checks for ECS entities in the worker's view that have a `PlayerHeartbeatClient` component, but do not yet have the [`HeartbeatData`]({{urlRoot}}/api/player-lifecycle/heartbeat-data) component. For all these entities, a [`HeartbeatData`]({{urlRoot}}/api/player-lifecycle/heartbeat-data) component is added to the ECS entity at the end of the current frame.

The [`HeartbeatData`]({{urlRoot}}/api/player-lifecycle/heartbeat-data) component is used to determine which entities need to be sent `PlayerHeartbeat` requests and to track how many consecutive failed heartbeats a player has.

### How to send PlayerHeartbeat requests

At intervals set by `PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds`, the [`SendPlayerHeartbeatRequestSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/SendPlayerHeartbeatRequestSystem.cs) finds all entities fulfilling the following constraints:

* the server-worker has authority over the `PlayerHeartbeatServer` component.
* the entity has the [`HeartbeatData`]({{urlRoot}}/api/player-lifecycle/heartbeat-data) component.

It sends a `PlayerHeartbeat` request to each of those entities. These requests need to be handled by the client-worker that the player entity belongs to.

Note that there are numerous ways that a request may fail, as outlined [here](https://docs.improbable.io/reference/latest/shared/design/commands#failure-modes).

### How to handle PlayerHeartbeat requests

The client-worker runs the [`HandlePlayerHeartbeatRequestSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatRequestSystem.cs), which sends a response back whenever it receives a `PlayerHeartbeat` request.

### How to handle PlayerHeartbeat responses

The [`HandlePlayerHeartbeatResponseSystem`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatResponseSystem.cs) on the server-worker iterates through all `PlayerHeartbeat` responses received. If at least one successful response was received from a player entity, the `NumFailedHeartbeats` inside this player entity's [`HeartbeatData`]({{urlRoot}}/api/player-lifecycle/heartbeat-data) component is set to 0.

If no responses were received with a `Success` status code from a Player entity, the `NumFailedHeartbeats` field is incremented. If an entity has more failed heartbeats than the number configured in `PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats`, a request to delete the SpatialOS entity is sent to the SpatialOS Runtime.
