<%(TOC)%>
# Heartbeating 101

## When is a player entity deleted?

To ensure that player entities of disconnected client-workers instances get deleted correctly, the server-worker instances responsible for managing the player lifecycle sends a `PlayerHeartBeat` [command]({{urlRoot}}/reference/world-component-commands-requests-responses) to the different player entities to check whether they are still connected. If a player entity fails to send a response three times in a row, the server-worker instance sends a request to the SpatialOS Runtime to delete this entity.
