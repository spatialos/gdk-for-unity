<%(TOC max="2")%>

# 1.3 - What the Blank Project does

The Blank Project offers the basic ability to launch a deployment, connect a client-worker and server-worker to it, and let the Player Lifecycle module create a player entity to represent the client.

On this page, we’ll walk you through this functionality, and introduce the tooling and workflows that you should become familiar with along the way.

## Launch a local deployment

First, hit `Ctrl+L` on Windows, or `Cmd+L` on Mac, to launch a local deployment. This opens a new terminal window and launches a local instance of SpatialOS using the `default_launch.json` launch configuration (located at the root of your SpatialOS project).

> **Note: you can only have one instance of SpatialOS running on your local machine at a time.**

<%(#Expandable title="What is inside default_launch.json?")%>

This `default_launch.json` includes a load balancing configuration where SpatialOS expects at most one `UnityGameLogic` worker to be spawned in the world. The `manual_worker_connection_only` argument is specified to tell your local SpatialOS instance that you will manually connect a worker of that type to the deployment.

<%(/Expandable)%>

When your local instance of SpatialOS is ready, you should see the following message in the window:

```text

    SpatialOS ready. (6.2s)
    Access the Inspector at http://localhost:21000/inspector
    Access the new modular Inspector at http://localhost:21000/inspector-v2

```

## Connect workers to your local deployment

Open the `DevelopmentScene` found in the `Assets/Scenes/` folder. You should find a `ClientWorker` and `GameLogicWorker` present in the Scene hierarchy.

<img src="{{assetRoot}}assets/blank/tutorial/1/development-scene-hierarchy.png" style="margin: 0 auto; width: 25%; display: block;" />

Select the Play button. This connects both a client-worker and a server-worker to your local deployment.

You should notice some output in the terminal window indicating that your workers connected to the deployment.

```text

    Handling connection request for worker of type UnityGameLogic trying to log in: UnityGameLogic-XYZ
    The worker UnityGameLogic-XYZ connected to SpatialOS successfully.

    Handling connection request for worker of type UnityClient trying to log in: UnityClient-ABC
    The worker UnityClient-ABC connected to SpatialOS successfully.

```

## The Inspector

Eagle-eyed observers would have paid attention to this line from earlier:

```text

    Access the Inspector at http://localhost:21000/inspector

```

The Inspector is a web-based tool that you use to explore the current state of a SpatialOS world. It provides a real-time view of what’s happening in your deployment, either locally or in the cloud. Using this tool, you can see:

* Which SpatialOS entities are in the world, where they are and their component values.
* The worker instances connected to the deployment.
* The authority and interest regions for each worker instance.

You can learn more about the Inspector [here](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/inspector).

In a browser of your choice, navigate to `localhost:21000/inspector`. You should be able to validate that two workers are connected to the deployment:

<img src="{{assetRoot}}assets/blank/tutorial/1/inspector-workers-list.png" style="margin: 0 auto; width: 50%; display: block;" />

Looking further down, notice that there are four entities in your world:

<div style="text-align:center">
<img src="{{assetRoot}}assets/blank/tutorial/1/inspector-entities-hover.png" style="margin: 0 auto; width: 27%; display: inline-block;" />
<img src="{{assetRoot}}assets/blank/tutorial/1/inspector-entities-list.png" style="margin: 0 auto; width: 55%; display: inline-block;" />
</div>

The `UnityGameLogic-worker` and `UnityClient-worker` are worker entities. These types of entities are spawned and deleted automatically by SpatialOS when a worker of that type connects or disconnects. We will not cover worker entities or system entities in this tutorial, but you can read more about them [here](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/system-entities).

`PlayerCreator` entities are used by the PlayerLifecycle module to handle player creation requests. If your world does not contain at least one `PlayerCreator` entity, the PlayerLifecycle module will not work. To meet this minimum requirement, the default snapshot in the Blank Project includes a `PlayerCreator` entity.

The `Player` entity is then created by the Player Lifecycle module as soon as the client-worker connects to the deployment.

#### Next: [GDK Player lifecycle]({{urlRoot}}/projects/blank/tutorial/1/player-lifecycle)
