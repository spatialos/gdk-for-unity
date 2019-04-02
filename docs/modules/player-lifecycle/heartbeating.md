<%(TOC)%>
# Heartbeating

Heartbeating is how a server-worker continually checks that a client-worker has not disconnected from SpatialOS.

The server-worker sends a `PlayerHeartbeat` request at regular intervals. If the server-worker doesn’t receive any responses back from a client-worker within a given period of time, the server-worker will assume the client has died. The server-worker then proceeds to delete the Player entity associated with that client-worker.

## How does it work in the GDK?

### Initialisation

[Initialisation](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/PlayerHeartbeatInitializationSystem.cs)

### Sending PlayerHeartbeat requests

[Sending heartbeat requests](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/SendPlayerHeartbeatRequestSystem.cs)

### Handling PlayerHeartBeat requests

[Handling heartbeat requests](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatRequestSystem.cs)

### Handling an unresponsive client

[Sending heartbeat requests](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Systems/PlayerHeartbeat/HandlePlayerHeartbeatResponseSystem.cs)
