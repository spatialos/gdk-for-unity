<%(TOC max="2")%>

# 1.1 - What is SpatialOS?

**SpatialOS** is a platform-as-a-service that runs and manages online games in the cloud.

But while it runs your game and manages the infrastructure for you, SpatialOS also enables something more than that. It runs games in a way that lets them scale further, be more complex, and have long-living persistence.

## How does it actually work?

The traditional ways to develop large online games mean that you’re either limited by the capacity of a single game server, or you have to shard your game world.

![Traditional client-server architecture]({{assetRoot}}assets/concepts/trad-client-server.png)

SpatialOS works differently: it brings together many servers so they’re working as one. But it does this in a way that makes a single world which looks seamless to players.

![SpatialOS architecture]({{assetRoot}}assets/concepts/deployment.png)

To make that work, you need to do some things differently.

You’ll need to build your game using an entity-component-worker architecture, writing server-side code in a way that enables SpatialOS to stitch servers together. Instead of writing a single game server, you’ll write **server-workers**: server-side programs that are only responsible for handling a part of the world at a time. The part of the world is their area of **authority**.

![Server-workers: area of authority]({{assetRoot}}assets/concepts/authority-areas.png)

In order to be able to simulate the world properly, server-worker instances need to see a slightly bigger area than the area they’re responsible for:

![Server-workers: area of interest]({{assetRoot}}assets/concepts/interest-areas.gif)

## SpatialOS world

In a game running on SpatialOS, the **game world** is a core concept. By this we mean the SpatialOS world: the canonical source of truth about things in your game.

### Entities and components

Entities are the objects in your game. All of the data that you want to share between worker instances has to be stored in entities. Each entity is made up of components; it's the components which store this data.

For example, in a world with rabbits and lettuces, you'd have `Rabbit` entities and `Lettuce` entities, each with certain components. These components in turn would have certain properties:

![Entities example]({{assetRoot}}assets/concepts/component-details.png)

As a developer working with SpatialOS, you will:

* define components, i.e. what data entities can be composed of (write a **schema**)
* place entities into your world to form a starting point for your game (create a **snapshot**)

The GDK generates code from your schema to make it easier when writing code for your workers that interact with entities and their component values.

The components on entities act as the data of the game world.

### Why this is necessary

The reason for having this separately-defined world is to store the state of the game world in such a way that many instances of server-workers and client-workers can access and change it, without needing to communicate with each other.

A major reason to use SpatialOS is to exceed those limits: instead of one server looking after the server-side of the game world, SpatialOS coordinates multiple programs (server-workers) to do that.

## Server-workers

In SpatialOS, you have a separately-defined game world, outside of any code you write. This is because SpatialOS is designed to exceed the limits of the game world a single server could hold. So instead, SpatialOS coordinates multiple programs to do that. We call these programs **server-workers**.

![Server-workers on the world]({{assetRoot}}assets/concepts/workers-world.png)

As a developer using SpatialOS, you will write the game code that runs within server-worker instances.

SpatialOS will run instances of server-workers, and use their combined computation to simulate the whole world. This means that the server-worker instances don’t know anything about each other - they might not even be on the same machine in the cloud. So when you’re writing the code for server-workers, you need to write code that can cope with only knowing about part of the world.

![Server-workers with the world]({{assetRoot}}assets/concepts/workers-machines.png)

This - writing code to deal with an arbitrary, not-necessarily-contiguous part of the world - is the largest paradigm change when using SpatialOS, the biggest difference from standard game development. This idea will have the biggest impact on the way you architect features for your game.

All of these server-worker instances run using a copy of the same binary.

### Splitting up the world, or “load balancing”

One of the decisions you need to make as a developer is, “How many server-worker instances does my world need?”.

To decide this, you’re working out how much computation your world needs, and how many server-worker instances you need to do that work. When you decide this, you split the world up - there are a few different ways you can do this - into regions; each region will be the area of authority of a server-worker instance.

[Read more about authority](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/concepts/authority-and-interest)

For local development, one instance might be enough. However, we definitely recommend trying to scale and test your game early on with at least two server-worker instances running your world.

While there’s lots of stuff SpatialOS does for you, it can be hard to reason about how to architect your game properly to deal with re-assignments of write access authority from one server-worker instance to another. And some problems won’t be obvious until you have multiple server-worker instances behind your game world.

### Deployments

SpatialOS hosts games for you. We call an instance of a running game a **deployment**.

As you learned above, you decide how many server-worker instances your world needs, and how to organise them across the world. In a deployment, SpatialOS starts those server-worker instances for you, running them on machines in the cloud (and orchestrating this for you - you don’t need to interact with the machines directly).

SpatialOS also mediates client-worker connections.

![Deployment diagram]({{assetRoot}}assets/concepts/deployment.png)

### Client-workers

Because each client-worker instance is tied to a player, and runs on the player's local machine, SpatialOS doesn't manage a client-worker's workload in the same way as it manages a server-worker's workload. This means that during game development, you set up client-workers and server-workers differently. The main difference is around how you sync data to and from the game world.

Like server-workers, client-workers can only see a part of the world. However, client-workers can see across server-worker boundaries, as shown in the diagram below:

![Client-worker diagram]({{assetRoot}}assets/concepts/client-workers.png)

## Persistence, scale, and complexity

**Persistence**: The game world is stored as a scalable database of entities, providing the canonical definition of the world that server-worker instances use and make changes to.

**Scale**: Stitching server-worker instances together allows you to create a huge game world, and distribute the workload among multiple servers.

**Complexity**: You don’t just have to have one type of server-worker. You can have many types, looking after many different systems in your game world, letting you layer up functionality without overloading your servers. (You’re not limited to one system per server-worker type - each server-worker type can potentially look after several systems.)

![Layers of server-workers: flat view]({{assetRoot}}assets/concepts/layers-load-balancing.png)

## What we provide

We run your game in the cloud for you, managing all the infrastructure, so you don't have to worry about how to make your game run on a distributed architecture. We also provide a [collection of tools](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/tools-overview) to help test, deploy and run your games.

The GDK for Unity is an integration built on top of a low-level [SDK](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/sdks-and-data-overview), enabling developers to more naturally create Unity games for SpatialOS.

#### Next: [Project walkthrough]({{urlRoot}}/projects/blank/tutorial/1/project-walkthrough)
