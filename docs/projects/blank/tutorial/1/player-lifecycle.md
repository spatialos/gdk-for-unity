<%(TOC)%>

# Player lifecycle

As you can see, the Player Lifecycle module creates an entity representing a player when a client-worker connects, and manages the lifecycle of this entity with the [Heartbeat system]({{urlRoot}}/modules/player-lifecycle/heartbeating) (which we’ll cover later).

At a high-level, the client-worker finds all PlayerCreator entities in the world, sends a request to one of them, and waits for a server-worker to create the requested player entity in the world. To do this, the module makes use of World Commands.

## World commands

World commands are built-in SpatialOS commands that allow you to create entities (`CreateEntity`), delete entities (`DeleteEntity`) and query the world for entities (`EntityQuery`).

An `EntityQuery` is used to initially find all PlayerCreator entities in the world. When handling a player creation request, the `CreateEntity` world command is called to create the Player entity. If the client-worker disconnects or becomes unresponsive, the Heartbeat system steps in to delete the Player entity using a `DeleteEntity` world command.

You can find out more about World Commands [here](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/commands#world-commands).

## Player creation

By default, the module sends a player creation request to a randomly chosen PlayerCreator entity as soon as the client-worker instance connects to SpatialOS. The server-worker instance responsible for simulating the PlayerCreator instance receives the request and then spawns a SpatialOS entity to represent the player. By choosing a random PlayerCreator entity for each request, you can distribute the load of player creation requests across multiple server-workers.

<%(#Expandable title="Custom player creation")%>

You can also customise this behaviour to not automatically create a Player entity, instead opting to manually send the player creation request. This option allows you to provide arbitrary serialized data that you can deserialize on a server-worker when handling the request. It also allows you to register a callback on the client-worker, which is run when it receives a player creation response.

You can find out more about custom player creation [here]({{urlRoot}}/modules/player-lifecycle/custom-player-creation).

<%(/Expandable)%>

## Heartbeats

To ensure that the deployment is not polluted with player entities of unresponsive or disconnected client-workers, the module implements a technique known as Heartbeating to periodically remove unresponsive or disconnected clients.

To demonstrate this, first look at the top-right corner of your local Inspector. You should see that a UnityClient and UnityGameLogic worker instance are connected. Select the UnityClient from the list of workers and then select the red `Stop worker` button.

<div style="text-align:center;">
<img src="{{assetRoot}}assets/blank/tutorial/1/inspector-workers-list-hover-client.png" style="margin: 0 auto; width: 50%; display: inline-block;" />
<img src="{{assetRoot}}assets/blank/tutorial/1/stop-worker-button.png" style="margin: 0 auto; width: 23%; display: inline-block;" />
</div>

A dialogue window should appear asking if you’re sure. We are - so select `stop worker` from the window to kill the client-worker. Immediately you should notice that the UnityGameLogic worker is still there and the UnityClient-worker entity has been removed, but the player entity is still in the world!

<div style="text-align:center">
<img src="{{assetRoot}}assets/blank/tutorial/1/stop-worker-window.png" style="margin: 0 auto; width: 50%; display: inline-block;" />
<img src="{{assetRoot}}assets/blank/tutorial/1/no-more-client.png" style="margin: 0 auto; width: 37%; display: inline-block;" />
</div>

Not to fear, because after a few seconds you’ll notice the player entity is swiftly removed from the world. The UnityClient-worker entity is managed by SpatialOS, which means that it is deleted as soon as the respective client-worker is disconnected.

The delay in removing the player entity exists because the Heartbeat system requires a minimum number of failed heartbeats before deleting the Player entity. This is to allow some failure tolerance from the client-worker, to avoid removing players for being a little slow at responding to heartbeat requests.

To understand more about how this works, read the [Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) documentation.
