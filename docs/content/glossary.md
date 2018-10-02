**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

----
## Glossary

>This glossary **only** contains the terms you need to understand in order to use the SpatialOS GDK for Unity. See the [core concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) and [glossary](https://docs.improbable.io/reference/13.2/shared/glossary) sections of the SpatialOS documentation for a full glossary of generic terms related to SpatialOS.

### SpatialOS Project

A SpatialOS project is the source code of a game that runs on SpatialOS. We often use this to refer to the directory that contains the source code, the `UnityGDK` directory in this case.

A SpatialOS project includes (but isn't limited to):

* The source code of all [workers](#worker) used by the project
* The project’s [schema](#schema)
* (optional) [Snapshots](#snapshot) of the project’s [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world)
* Configuration files, mostly containing settings for [deployments](#deploying) (for example,
[launch configuration files](#launch-configuration-file))

> Related:
>
> * [Project directory structure](https://docs.improbable.io/reference/latest/shared/reference/project-structure)

### Unity Project

A Unity project is the source code of a SpatialOS game's Unity [workers](#worker). We often use this to refer to the directory that contains Unity code, `UnityGDK/workers/unity` in this case.

### Project name

Your project name is randomly generated when you sign up for SpatialOS. It’s usually something like `beta_someword_anotherword_000`.

>Note that your project name is **not** the same as the name of the
directory your [SpatialOS project](spatialos-project) is in.

You must specify this name in the [spatialos.json](#spatialos-json) file in the root of your SpatialOS project when you run a [cloud deployment](#cloud-deployment).

You can find your project name in the [Console](https://console.improbable.io/).

### Assembly

An assembly is what's created when you [build your workers](build.md#building-your-workers). It contains all the files that your game uses at runtime. This includes executable files for [client-workers](#client-worker) and [server-workers](#server-worker), and the assets your [workers](#worker) use (like models and textures used by a client to visualize the game).

The assembly is stored locally at `UnityGDK\build\assembly`. When you run a [cloud deployment](#cloud-deployment), your assembly is uploaded and becomes accessible from the [Console](https://console.improbable.io/).

> Related pages:
>
> * [spatial cloud upload]({{urlRoot}}/shared/spatial-cli/spatial-cloud-upload)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

## The `spatial` command-line tool (CLI)

Also known as the "`spatial` CLI".

The `spatial` command-line tool provides a set of commands that you use to interact with a
[SpatialOS project](#spatialos-project). Among other things, you use it to [deploy](#deploying) your game
(using [`spatial local launch`]({{urlRoot}}/shared/spatial-cli/spatial-local-launch) or
[`spatial cloud launch`]({{urlRoot}}/shared/spatial-cli/spatial-cloud-launch)).

> Related pages:
>
> * [An introducion to the `spatial` command-line tool](https://docs.improbable.io/reference/13.3/shared/spatial-cli-introduction)
> * [`spatial` reference documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial)

## Building

When you make changes to the code of a [worker](#worker), you need to build those changes in order to try them out in a [local](#local-deployment) or [cloud deployment](#cloud-deployment).

[How to build your game](build.md) explains this process step by step.

> Related pages:
>
> * [Building the bridge and launch configurations of your workers](build.md#building-the-bridge-and-launch-configurations-of-your-workers)
> * [Preparing the build configuration of your workers](build.md#preparing-the-build-configuration-of-your-workers)

## Deploying

When you want to try out your game, you need to run a deployment of the game/[project](#project). This means
launching SpatialOS itself. SpatialOS sets up the [world](#spatialos-world) based on a [snapshot](#snapshot),
then starts up the [workers](#worker) needed to run the game world.

Once the deployment is running, you can connect [clients](#client-worker) to it. You can then use
the clients to play the game.

You can run a deployment:

* [on your local development machine](#local-deployment)
* [in the cloud](#cloud-deployment)

> Related pages:
>
> * [Deploying locally]({{urlRoot}}/shared/deploy/deploy-local)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

### Inspector

The Inspector is a web-based tool that you use to explore the internal state of a [SpatialOS world](#spatialos-world).
It gives you realtime view of what’s happening in a [deployment](#deploying), [locally](#local-deployment)
or in the [cloud](#cloud-deployment). Among other things, it displays:

* which [entities](#entity) are in the world
* what their [components](#component)' [properties](#property) are
* which [workers](#worker) are connected to the deployment
* how much [load](#load-balancing) the workers are under

> Related pages:
>
> * [The Inspector]({{urlRoot}}/shared/operate/inspector)

### Launcher

You use the Launcher to connect [clients](#client-worker) to a [cloud deployment](#cloud-deployment).
You can run the Launcher from the [Console](#console), or (from the Console) generate share links so anyone
with the link can join.

The Launcher gets the executable for the client-worker from the [assembly](#assembly) you uploaded.

> Related pages:
>
> * [The Launcher]({{urlRoot}}/shared/operate/launcher)

## Console

The Console ([console.improbable.io](https://console.improbable.io/)) is the main landing page for managing
[cloud deployments](#cloud-deployment). It shows you:

* your [project name](#project-name)
* running and previous [cloud deployments](#cloud-deployment) using that project name
* all the [assemblies](#assembly) you’ve uploaded using that project name
* a page for each deployment, which shows its status, and gives you access to the
[Inspector](#inspector), Logs, and Metrics pages for that deployment.

> Related pages:
>
> * [The Inspector]({{urlRoot}}/shared/operate/inspector)
> * [Logs]({{urlRoot}}/shared/operate/logs)
> * [Metrics]({{urlRoot}}/shared/operate/metrics)

## Worker

SpatialOS manages the [world](#spatialos-world) itself: it keeps track of all the [entities](#entity) and their
[properties](#property). But on its own, it doesn’t make any changes to the world.

Workers are programs that connect to a SpatialOS world. They perform the computation associated with a world:
they can read what’s happening, watch for changes, and make changes of their own.

In order to achieve huge scale, SpatialOS divides up the entities in the world between workers, balancing the work
so none of them are overloaded. For each entity in the world, it decides which worker should have
[write access](#read-and-write-access-authority) to each [component](#component) on the entity. (To prevent multiple workers clashing, ony one worker at a time can have
write access to a component.)

As the world changes over time, where entities are and the amount of work associated with them changes.
[server-workers (also known as "managed workers")](#server-worker) report back to SpatialOS how much load they're under, and
SpatialOS adjusts which workers have write access to components on which entities (and starts up new workers when
needed). This is called [load balancing](#load-balancing).

Around the entities they have write access to, each worker has an area of the world they are [interested in](#interest).
A worker can read the current properties of the entities within this area, and SpatialOS sends
[updates](#sending-an-update) about these entities to the worker.

If the worker has [write access](#read-and-write-access-authority) to a component, it can [send updates](#sending-an-update):
it can update [properties](#property), and trigger [events](#event).

Workers can be [server-workers (managed workers)](#server-worker) or [client-workers (external workers)](#client-worker).

You can create workers using:

* the [worker SDKs](#worker-sdk) (in C++, C, Java, or C API) 
* Unity - via the SpatialOS for Unity SDK or GDK 
* Unreal - via the SpatialOS for Unreal SDK or GDK

> Related pages:
>
> * [Concepts: Workers and load balancing]({{urlRoot}}/shared/concepts/workers-load-balancing)

### Server-worker
_Also known as "managed worker", "server-side worker"._

A server-worker has its lifecycle managed by SpatialOS. That means in a running
[deployment](#deploying), SpatialOS is in charge of starting it up and stopping it, as part of
[load balancing](#load-balancing) - unlike a [client-worker](#client).

Server-workers are usually tasked with implementing game logic and physics simulation.

You might have just one server-worker connecting to a SpatialOS world, or dozens, depending on the size of the
[world](#spatialos-world).

You tell SpatialOS how to launch a worker as a server-worker in its [worker configuration file (worker.json)](#worker-configuration-worker-json).

> Related pages:
>
> * [Managed worker launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#managed-worker-launch-configuration)


#### Server-side worker
See [server-worker](#server-worker).

#### Managed worker
See [server-worker](#server-worker).

#### UnityWorker
The term for a [server-worker](#server-worker) in the [SDK for Unity](https://github.com/spatialos/UnitySDK).

#### UnrealWorker
The term for a [server-worker](#server-worker) in the [SDK for Unreal](https://github.com/spatialos/UnrealSDK).


### Client-worker
_Also known as "external worker", "client-side worker", or "client"._

A client-worker is a [worker](#worker) that is run by a player, in their game executable program; their game executable program uses it to connect
to and interact with the [SpatialOS world](#spatialos-world). 

You tell SpatialOS how to launch a worker as a client-worker in its [worker configuration file (worker.json)](#worker-configuration-worker-json). 

Unlike a [server-worker](#server-worker), a client-worker’s lifecycle is not managed by
SpatialOS. In a running [deployment](#deploying), there will be one
client-worker per player, and the players start and stop their client-worker.

Client-workers are mostly tasked with visualising what’s happening in the world. You’d also use them for dealing
with player input. In general, you want to give client-workers
[write access](#read-and-write-access-authority) to as few components as possible,
to make cheating difficult.

> Related pages:
>
> * [External worker (server-worker) launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#external-worker-launch-configuration)


#### Client
See [client-worker](#client-worker).

#### Client-side worker
See [client-worker](#client-worker).

#### External worker
See [client-worker](#client-worker).

### Unmanaged workers
If you want to test what would usually be a [server-worker](#server-worker) by stopping and starting it manually in your game development environment, you can make that worker unmanaged in a [local deployment](#local-deployment) or [cloud deployment](#cloud-deployment) of your game.  Note that these workers are not client-workers (external workers), even though client-workers are also not managed by SpatialOS. Unmanaged workers access the deployment in a different way to client-workers.

### Worker attribute

In its [bridge configuration]({{urlRoot}}/shared/worker-configuration/bridge-config), each [worker](#worker) specifies
attributes that say what kind of worker it is: what its capabilities are. These attributes are used in
[access control lists](#acl), which specify which attributes a worker must have in order to get
[read or write access](#read-and-write-access-authority) to a component.

> Related pages:
>
> * [Worker attributes and worker requirements]({{urlRoot}}/shared/worker-configuration/attributes)
> * [Bridge configuration: worker attribute sets]({{urlRoot}}/shared/worker-configuration/bridge-config#worker-attribute-sets)

### Worker type

Worker type is the label for a worker. It's mostly important when running worker instances, so you know what
kind of worker it is. For example, `UnityWorker` and `UnityClient` are worker types.

Each worker type has a [worker configuration](#worker-configuration-worker-json) file (`<worker_type>.worker.json`).

### Worker ID

Each [worker](#worker) instance has a worker ID, used to uniquely identify it. Workers automatically
have an [attribute](#worker-attribute) which includes their ID: `"workerId:<worker ID>"`.

> Related pages:
>
> * [Bridge configuration: worker attribute sets]({{urlRoot}}/shared/worker-configuration/bridge-config#worker-attribute-sets)

### Sending an update

If a worker has [write access](#read-and-write-access-authority) to a [component](#component),
it can send an update to it. An update can change the value of a [property](#property), or trigger an
[event](#event). You can change multiple properties and trigger multiple events in one update.

> Related pages:
>
> * Sending component updates in:
>   * [C++]({{urlRoot}}/cppsdk/using/sending-data)
>   * [C#]({{urlRoot}}/csharpsdk/using/sending-data)
>   * [Java]({{urlRoot}}/javasdk/using/sending-data)
> * Receiving component updates in:
>   * [C#]({{urlRoot}}/csharpsdk/using/sending-data)
>   * [C++]({{urlRoot}}/cppsdk/using/receiving-data)
>   * [Java]({{urlRoot}}/javasdk/using/receiving-data)

### Worker configuration (worker.json)

Each [worker](#worker) must have a worker configuration file, with the name `spatialos.<worker_type>.worker.json`:
for example, `spatialos.MyCSharpWorker.worker.json`. This file:

* sets the process used to [build](#building) the worker
* configures settings to do with how the worker communicates with SpatialOS, including information like what
[components](#component) the worker is [interested](#interest) in
* sets whether a worker is a [server-worker](#server-worker) or a[client-worker](#client-worker)
* for [server-workers](#server-worker), tells SpatialOS how to run the worker in the cloud
* for [client-workers](#client-worker), specifies how to launch the worker on the game player's local computer

> Related pages:
>
> * [Configuring workers]({{urlRoot}}/shared/worker-configuration/worker-configuration) and its sub-pages:
>   * [Build configuration]({{urlRoot}}/shared/worker-configuration/worker-build)
>   * [Bridge configuration]({{urlRoot}}/shared/worker-configuration/bridge-config)
>   * [Launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration)
>   * [Worker attributes]({{urlRoot}}/shared/worker-configuration/attributes)

### Load balancing

One of the features of SpatialOS is load balancing: dynamically adjusting how many [components](#component)
on [entities](#entity) in the [world](#spatialos-world) each [worker](#worker) has
[write access](#read-and-write-access-authority) to, so that workers don't get overloaded.

Load balancing only applies to [server-workers](#server-worker).

When an instance of a worker is struggling with a high workload, SpatialOS can start up new instances of
the worker, and give them write access to some components on entities. 

This means that an [entity](#entity) won't necessarily stay on the same worker instance, even if that entity
doesn't move. SpatialOS may change which components on which entities a worker instance has
write access to: so entities move "from" one worker instance to another.
Even though the entity may be staying still, the worker instance's [area of interest](#interest) is moving.

> Related pages:
>
> * [Configuring load balancing]({{urlRoot}}/shared/worker-configuration/loadbalancer-config)

### Interest

There are two types of interest: entity interest and component interest.

#### Entity interest

A worker is interested in all [chunks](#chunk) that contain [entities](#entity) it has
[write access](#read-and-write-access-authority) to a component on. It's *also* interested in chunks within a configurable
radius of those entities: this makes sure that workers are aware of entities nearby. You can set this radius
in the [worker configuration](#worker-configuration-worker-json).

The consequence of a worker being interested in a chunk is that it will [check out](#checking-out)
all the entities in that chunk.

#### Component interest

Each worker, in its [worker configuration (worker.json)](#worker-configuration-worker-json), specifies which components it is
interested in. A worker will *only* get sent updates about components it's interested in.

This means that, for entities inside a chunk that a worker is interested in,
it may only be interested in (and therefore only check out) particular components on that entity.

The component delivery settings in a [worker configuration](#worker-configuration-worker-json) specify which components a
worker is interested in. By default, a worker is *only* interested in the components with `checkout_initially`
*explicitly* set to `true` in its worker configuration.

Which components a worker is interested can change at runtime:

* If a worker is given [write access](#read-and-write-access-authority) to a component, it
    becomes interested in that component.
* You can manually update which components a worker is interested in at runtime.

> Related pages:
>
> * [Entity interest settings]({{urlRoot}}/shared/worker-configuration/bridge-config#entity-interest)
> * [Component delivery settings]({{urlRoot}}/shared/worker-configuration/bridge-config#component-delivery)
> * Changing a worker's component interest at runtime:
>   * [C++]({{urlRoot}}/cppsdk/using/sending-data#updating-component-interests)
>   * [C#]({{urlRoot}}/csharpsdk/using/sending-data#updating-component-interests)
>   * [Java]({{urlRoot}}/javasdk/using/sending-data#sending-component-interests)

### Checking out

Each individual [worker](#worker) checks out only part of the [world](#spatialos-world). This happens
on a [chunk](#chunk)-by-chunk basis. A worker "checking out a chunk" means that:

* it will have a local representation of every [entity](#entity) in that chunk
* SpatialOS will send it updates about those entities

A worker will check out all chunks that it is [interested](#interest) in.

> Related pages:
>
> * [Entity interest settings]({{urlRoot}}/shared/worker-configuration/bridge-config#entity-interest)

### Streaming queries

Streaming queries allow workers to get information about the [world](#spatialos-world)
outside the region they're [interested in](#interest), so that they know about entities that they don't have checked out
(for example, entities that are far away, or that don’t have a physical position).

<%(Callout type="tip" message="Streaming queries are useful if you need to get information about an entity periodically
 - for example, so that a player can see and interact with it.

If you just need information about an entity at one particular time, use [queries](#queries) instead.")%>

For more information on the advantages and limitations of streaming queries, go to 
[Bridge configuration: streaming queries]({{urlRoot}}/shared/worker-configuration/bridge-config#streaming-queries).

### Worker flag

Worker flags let you change values that [workers](#worker) use, either at the start of a [deployment](#deploying)
or dynamically while it's running. For example, you could vary the walking speed of your [entities](#entity) and see
how this affects the world.

Worker flags (and their values) are defined in the [launch configuration file](#launch-configuration-file).

A worker can query the value of a flag at any time. You can set the value of flags for a running deployment
from the [Console](#console), or using the [command-line](#the-spatial-command-line-tool-cli).

> Related pages:
>
> * [Worker flags]({{urlRoot}}/shared/worker-configuration/worker-flags)
> * [`spatial-cloud-runtime-flags`]({{urlRoot}}/shared/spatial-cli/spatial-cloud-runtime-flags)
> * [Launch configuration file]({{urlRoot}}/shared/reference/file-formats/launch-config)

#### Node

Node refers to a single machine used by a cloud deployment. Its name indicates the role it plays in your deployment. You can see these on the advanced tab of your deployment details in the [Console](#console).

## SpatialOS world
Also known as "the world" and "the game world".

The world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world's
data is stored within [entities](#entity) - specifically, within their [components](#component).

SpatialOS manages the world, keeping track of all the entities and what state they’re in.

*Changes* to the world are made by [workers](#worker). Each worker has a view onto the world (the
part of the world that they're [interested](#interest) in), and SpatialOS sends them updates when anything changes
in that view.

It's important to recognise this fundamental separation between the SpatialOS world and the view/representation of
that world that a worker [checks out](#checking-out) locally. This is why workers must [send updates](#sending-an-update)
to SpatialOS when they want to change the world: they don't control the canonical state of the world, they must
use SpatialOS APIs to change it.

### Layers

Layers are a new concept in SpatialOS, introduced as part of the
[new load balancer]({{urlRoot}}/releases/upgrade-guides/upgrade-load-balancer). They’re a concept that
organises both the [components]({{urlRoot}}/shared/glossary#component) in your
[game world]({{urlRoot}}/shared/glossary#spatialos-world), and the [workers]({{urlRoot}}/shared/glossary#worker) that
simulate the world.

For details, see the [Introducing layers]({{urlRoot}}/releases/upgrade-guides/layers) page.

### Chunk

A [world](#spatialos-world) is split up into chunks: the grid squares of the world. A chunk is the smallest
area of space the world can be subdivided into. Every [entity](#entity) is in exactly one chunk.

You set the size of chunks for your world in [launch configuration files](#launch-configuration-file).

> Related pages:
>
> * [Launch configuration file]({{urlRoot}}/shared/reference/file-formats/launch-config): the `chunk_edge_length_meters`
> setting

### World unit

World units are an arbitrary unit that workers can interpret as they choose.

Settings in world units:

* the size of [chunks](#chunk) is defined in world units
* the dimensions of the [world](#spatialos-world)
* the [entity interest](#interest) radius
* the radius of a [query](#queries)'s sphere

> Related pages:
>
> * [Launch configuration file]({{urlRoot}}/shared/reference/file-formats/launch-config): `chunk_edge_length_meters`
> and `dimensions` settings
> * [Entity interest radius]({{urlRoot}}/shared/worker-configuration/bridge-config#entity-interest)

## Queries

Queries allow [workers](#worker) to get information about the [world](#spatialos-world) outside the
region they're [interested in](#interest).

<%(Callout type="tip" message="Entity queries are useful if you need to get information about an entity
at a particular time. 

If you need regular updates about an entity, use [streaming queries](#streaming-queries) instead.")%>

Queries can search for [entities](#entity) with the following attributes:

* a specific [EntityId](#entityid)
* a specific [component](#component)
* within a specific sphere in the [world](#spatialos-world)

and any combination of the above, using `AND`/`OR`.

Based on the set of entities that match it, a query can return:

* snapshots of those entities, including all components
* snapshots of those entities, including only the components you specify
* the number of entities

You should keep queries as limited as possible. All queries hit the network and
cause a runtime lookup, which is expensive even in the best cases. This means you should:

* always limit queries to a specific sphere of the world
* only return the information you need from queries: specify which components you need to know about
* if you're looking for entities that are within your worker's [region of interest](#interest), search internally
on the worker instead of using a query

> Related pages:
>
> * [Querying the world]({{urlRoot}}/shared/design/commands#querying-the-world) in:
>   * [C++]({{urlRoot}}/cppsdk/using/sending-data#entity-queries)
>   * [C#]({{urlRoot}}/csharpsdk/using/sending-data#entity-queries)
>   * [Java]({{urlRoot}}/javasdk/using/sending-data#entity-queries)

## Entity

All of the objects inside a [SpatialOS world](#spatialos-world) are entities: they’re the basic building
block of the world. Examples include players, NPCs, and objects in the world like trees.

Entities are made up of [components](#component), which store the data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](#interest). Workers can represent these
entities locally any way you like.

For example, for workers built using Unity, you might want to have a prefab associated with each entity type, and spawn a GameObject for each entity the worker has [checked out](#checking-out).

You can have other objects that are *not* entities locally on workers - like UI for a player - but no other
worker will be able to see them, because they're not part of the [SpatialOS world](#spatialos-world).

> Related pages:
>
> * [Concepts: Entities]({{urlRoot}}/shared/concepts/world-entities-components)
> * [Designing entities]({{urlRoot}}/shared/design/design-entities)
> * Creating and deleting entities in:
>   * [C#]({{urlRoot}}/csharpsdk/using/creating-and-deleting-entities)
>   * [C++]({{urlRoot}}/cppsdk/using/creating-and-deleting-entities)
>   * [Java]({{urlRoot}}/javasdk/using/creating-and-deleting-entities)

### Component

An [entity](#entity) is defined by a set of components. Common components in a game might be things like `Health`,
`Position`, or `PlayerControls`. They're the storage mechanism for data about the [world](#spatialos-world) that you
want to be shared between [workers](#worker).

Components can contain:

* [properties](#property), which describe persistent values that change over time (for example, [`Position`](#position))
* [events](#event), which are things that can happen to an entity (for example, `StartedWalking`)
* [commands](#command) that another worker can call to ask the component to do something, optionally returning
a value (for example, `Teleport`)

An entity can have as many components as you like, but it must have at least [`Position`](#position) and
[`EntityAcl`](#acl). Most entities will have the [`Persistence`](#persistence) and [`Metadata`](#metadata) components.

Components are defined as files in your [schema](#schema).

Which types of workers can [read from or write to](#read-and-write-access-authority) which components is governed by
[access control lists](#acl).

> Related pages:
>
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Component best practices]({{urlRoot}}/shared/design/component-best-practices)
> * [Introduction to schema]({{urlRoot}}/shared/schema/introduction)

#### Property

Properties are one of the things that can be contained in a [component](#component). Properties describe
persistent values that change over time.

Property updates can be [sent](#sending-an-update) by the [worker](#worker) with [write access](#read-and-write-access-authority)
to the component. They're delivered to other workers that are [interested in](#interest) this component of the entity.

For [entities](#entity) they are [interested in](#interest), [workers](#worker) can:

* Read the current value of a property
* Watch for changes to the value of a property
* Send an update to the value of a property (if they have [write access](#read-and-write-access-authority))

> Related pages:
>
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Schema reference]({{urlRoot}}/shared/schema/reference)

#### Event

Events are one of the things that can be contained in a [component](#component). Unlike a
[property](#property), an event is not persistent (the data in it isn't saved).

Events let an [entity](#entity) broadcast a transient message about something that has happened to it.
In all other respects, events work the same way as [updates](#sending-an-update) to persistent properties.

Events can be [triggered](#sending-an-update) by the [worker](#worker) with [write access](#read-and-write-access-authority) to the
component. They're delivered to other workers that are [interested in](#interest) this component of the entity.

For entities they are [interested in](#interest), [workers](#worker) can:

* Watch for an event
* Send an update that triggers an event (if they have [write access](#read-and-write-access-authority))

> Related pages:
>
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Schema reference]({{urlRoot}}/shared/schema/reference)

#### Command

Commands are one of the things that can be contained in a [component](#component). They're essentially
a remote procedure call (RPC). Commands facilitate communication in the other direction to [events](#event)
and [properties](#property): they allow any [worker](#worker) to send a request to the worker with
[write access](#read-and-write-access-authority) to a specific component. The receiving worker can take action and should
respond to the request.

By default, commands are routed through SpatialOS.

Because which worker has write access to a component can [change regularly](#load-balancing), and commands must be
sent to the worker with write access, *commands can fail* if this worker changes between the time of sending and
the time of receiving.

This means that, for communication within a small area, it's better to model it using properties or events.
Commands are best suited when you don't know where the target entity is, or know that it's likely to be far away.
You can [short-circuit](#short-circuiting) commands that you think will be received by the same worker that sent
them, but that comes with a lot of [caveats]({{urlRoot}}/shared/design/commands#caveats).

[Workers](#worker) can:

* Send a command to a component on an entity
* Respond to a command sent (if they have [write access](#read-and-write-access-authority))

> Related pages:
>
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Schema reference]({{urlRoot}}/shared/schema/reference)
> * [Commands]({{urlRoot}}/shared/design/commands)

##### Short-circuiting

[Commands](#command) are routed through SpatialOS by default. You can choose to bypass this and
short-circuit commands when the [worker](#worker) sending the command has [write access](#read-and-write-access-authority)
to the target [component](#component).

This comes with a lot of [caveats]({{urlRoot}}/shared/design/commands#caveats), because if the worker loses write
access to the component, the command can fail.

> Related pages:
>
> * [Commands: caveats]({{urlRoot}}/shared/design/commands#caveats)

### Required components

#### Position

Position is a [component](#component) in the standard schema library; all [entities](#entity) must have this
component. It lets SpatialOS know what the position of an entity in the [world](#spatialos-world) is.

This is used by SpatialOS few specific purposes, like [load balancing](#load-balancing) and
[queries](#queries).

Note that there's no reason that this component _must_ be used to represent entity positions inside,
say, worker logic. For example, a 2D simulation could use a custom position component with only two
fields, and update `improbable.Position` at a lower frequency to save bandwidth.

> Related pages:
>
> * [The standard schema library]({{urlRoot}}/shared/schema/standard-schema-library)

#### Metadata

Metadata is a [component](#component) in the standard schema library.

It has a single [property](#property), `entity_type`, that's used to label the entity.

> Related pages:
>
> * [The standard schema library]({{urlRoot}}/shared/schema/standard-schema-library)

#### Persistence

Persistence is a [component](#component) in the standard schema library. It's optional, but
all [entities](#entity) that you want to persist in the [world](#spatialos-world) must have this component.
Persistence means that entities are saved into [snapshots](#snapshot).

If an entity doesn't have this component, it won't exist in snapshots. This is fine for transient entities.
For example, you probably don't want the entities associated with players to be saved into a snapshot you take of
a [deployment](#deploying), because the players won't be connected when you restart the deployment.

> Related pages:
>
> * [The standard schema library]({{urlRoot}}/shared/schema/standard-schema-library)

#### ACL

In order to read from a [component](#component), or make changes to a component, [workers](#worker) need to
have [access](#read-and-write-access-authority), which they get through an access control list.

Access control lists are a [component](#component) in the standard schema library: `EntityAcl`. Every
[entity](#entity) needs to have one. The ACL determines:

* which types of workers have read access to an entity
* for each component on the entity, which type of worker have write access

> Related pages:
>
> * [The standard schema library]({{urlRoot}}/shared/schema/standard-schema-library)
> * [Understanding write access]({{urlRoot}}/shared/design/understanding-access)

### EntityId

An EntityId uniquely identifies each entity in the [world](#spatialos-world) and in a [snapshot](#snapshot).

### Entity template

An entity template defines what [components](#component) an [entity](#entity) has. You use this template
when you create an entity.

Creating an entity template varies in different programming languages, but, in general, you create a new
`Entity` object, and then add components to it.

> Related pages:
>
> * Creating and deleting entities in:
>   * [C#]({{urlRoot}}/csharpsdk/using/creating-and-deleting-entities)
>   * [C++]({{urlRoot}}/cppsdk/using/creating-and-deleting-entities)
>   * [Java]({{urlRoot}}/javasdk/using/creating-and-deleting-entities)

## Schema

The schema is where you define all the [components](#component) in your [world](#spatialos-world).

Schema is defined in `.schema` files, written in [schemalang](#schemalang). Schema files are stored in the
`schema/` directory of your [project](#project).

SpatialOS uses the schema to [generate code](#code-generation) in various languages (including C#, C++, and Java).
You can use this generated code in your [workers](#worker) to interact with [entities](#entity) in the world.

> Related pages:
>
> * [Introduction to schema]({{urlRoot}}/shared/schema/introduction)
> * [Schema reference]({{urlRoot}}/shared/schema/reference)

### Schemalang

Schemalang is SpatialOS’s language for writing a [component](#component) [schemas](#schema).

> Related pages:
>
> * [Schema reference]({{urlRoot}}/shared/schema/reference)

### Code generation

Use the [command](#the-spatial-command-line-tool-cli)
[`spatial worker codegen`]({{urlRoot}}/shared/spatial-cli/spatial-worker-codegen) to compile generated code from the
[schema](#schema) (in C#, C++ and Java).

This code is used by [workers](#worker) to interact with [entities](#entity): to read from their
[components](#component), and to [make changes](#sending-an-update) to them.

> Related pages:
>
> * [Generating code from the schema]({{urlRoot}}/shared/schema/introduction#generating-code-from-the-schema)

## Read and write access ("authority")

Many [workers](#worker) can connect to a [SpatialOS world](#spatialos-world). To prevent them from clashing, only one
worker instance at a time is allowed to write to each [component](#component) on each [entity](#entity): ie,
given write access. Write access is sometimes also referred to as authority.

Which types of workers *can have* write access is governed by each entity's [access control list (ACL)](#acl).

Which specific worker instance *actually has* write access is managed by SpatialOS, and can change regularly
because of [load balancing](#load-balancing). However, the list of *possible* workers is constrained by the ACL.

ACLs also control which workers can have read access to an entity. Read access is at the entity level: if a worker
can read from an entity, it is allowed to read from all components on that entity.

> Related pages:
>
> * [Understanding write access]({{urlRoot}}/shared/design/understanding-access)

## Snapshot

A snapshot is a representation of the state of a [world](#spatialos-world) at some point in time. It
stores each [persistent](#persistence) [entity](#entity) and the values of their [components](#component)'
[properties](#property).

You'll use a snapshot as the starting point (an [initial snapshot](#initial-snapshot)) for your world when you
[deploy](#deploying), [locally](#local-deployment) or [in the cloud](#cloud-deployment).

> Related pages:
>
> * [Snapshots]({{urlRoot}}/shared/operate/snapshots)

### Initial snapshot

An initial snapshot is a snapshot that you use as the starting point of your [world](#spatialos-world) when you
[deploy](#deploying), [locally](#local-deployment) or [in the cloud](#cloud-deployment).

> Related pages:
>
> * [Creating a snapshot from scratch]({{urlRoot}}/shared/operate/snapshots#create-snapshots-from-scratch)

### Live snapshots/taking snapshots

You can take a snapshot from a running [cloud deployment](#cloud-deployment), which will capture the current
state of all of the [persistent](#persistence) [entities](#entity) in the [world](#spatialos-world).

> Related pages:
>
> * [Taking a snapshot]({{urlRoot}}/shared/operate/snapshots#take-a-snapshot-from-a-running-deployment)

## SDKs

## The Runtime
A Runtime instance manages the [game world](#spatialos-world) of each [deployment](#deploying).

## Worker SDK
Use the worker SDKs to create [server-workers](#server-worker) and [client-workers](#client-worker) which make your game work as a SpatialOS [deployment](#deploying). You can use these to:

* extend the functionality of the development kits for [Unity](#unity-with-spatialos) or [Unreal](#unreal-with-spatialos).
* create low-level workers for game logic that does not require a game engine; these could work _without_ any game engine or _with_ a game engine, complimenting the functionality of workers in any game engine, including Unity or unreal.

The C++, C# and Java worker SDKs have a very similar structure; the C API is lower-level and doesn’t include code generation.

### C# worker SDK

The toolkit for developing C# [workers](#worker) with SpatialOS. This is a fairly low-level set of APIs for interacting
with a [SpatialOS world](#spatialos-world), and with [snapshots](#snapshot).

> Related pages:
>
> * [Introduction to the C# SDK]({{urlRoot}}/csharpsdk/introduction)

### C++ worker SDK

The toolkit for developing C++ [workers](#worker) with SpatialOS. This is a fairly low-level set of APIs for interacting
with a [SpatialOS world](#spatialos-world), and with [snapshots](#snapshot).

> Related pages:
>
> * [Introduction to the C++ SDK]({{urlRoot}}/cppsdk/introduction)

### Java worker SDK

The toolkit for developing Java [workers](#worker) with SpatialOS. This is a fairly low-level set of APIs for interacting
with a [SpatialOS world](#spatialos-world), and with [snapshots](#snapshot).

> Related pages:
>
> * [Introduction to the Java SDK]({{urlRoot}}/javasdk/introduction)

### C API
The lowest level took kit for developing SpatialOS [workers](#worker).

## Unity with SpatialOS

### GDK for Unity
A Unity-native experience for developing games with SpatialOS. Available on [GitHub](https://github.com/spatialos/UnityGDK).

### SDK for Unity

The  SDK for Unity is a toolkit for developing [workers](#worker) using Unity, built on top of the [C# worker SDK](#c-worker-sdk).
It includes APIs for interacting with a [SpatialOS world](#spatialos-world). 

Until [SpatialOS 13.0](https://docs.improbable.io/reference/13.0/releases/release-notes-13), SpatialOS included the SDK for Unity. 
From SpatialOS 13.0 onwards, the SDK for Unity is released separately and hosted on GitHub: 
[github.com/spatialos/UnitySDK](https://github.com/spatialos/UnitySDK).

## Unreal with SpatialOS 

### GDK for Unreal
An Unreal-native experience for developing games with SpatialOS. Available on [GitHub](https://github.com/spatialos/UnrealGDK).

### SDK for Unreal

The SDK for Unity is a toolkit for developing [workers](#worker) using Unreal, built on top of the [C++ worker SDK](#c-worker-sdk-1).
It include APIs for interacting with a [SpatialOS world](#spatialos-world). 

Until [SpatialOS 13.0](https://docs.improbable.io/reference/13.0/releases/release-notes-13), SpatialOS included the SDK for Unreal. 
From SpatialOS 13.0 onwards, the SDK for Unreal is released separately and hosted on GitHub: 
[github.com/spatialos/UnitySDK](https://github.com/spatialos/UnrealSDK).

### Read and write access (“authority”)

https://docs.improbable.io/reference/13.2/shared/glossary#read-and-write-access-authority

Assembly
We use assemblies to structure the GDK. By using [assembly definition files](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html), we are able to define how these assemblies should be generated. Each assembly may contain a module or part of a depending on how we have structured it. To ensure that scripts that should only run in the Editor or certain platforms won’t be added to the resulting build of your workers by default, we define in the corresponding assembly definition file for which platforms the assembly should be included.

Attribute
[Deets here -https://docs.improbable.io/reference/13.2/shared/design/understanding-access#worker-attribute
Referenced in this doc here: https://docs.google.com/document/d/1qKi3ju6OMGMLlfHj9ufvo73kGBH9E3ZUOXB6i4Grsgk/edit - Ed]


Checking out

Code generation


Commands
World commands
Entity commands
Component
ECS component
ECS reactive component
ECS non-reactive component
SpatialOS component
GameObject and MonoBehaviour component (AKA “a MonoBehaviour” in our docs)


Core module

Deploy


ECS
Entity Component System
The Entity-Component System (ECS) is a data-oriented paradigm that Unity recently introduced as a preview package in their Engine.

Entity
SpatialOS entity
ECS entity


Events

Feature module

Interest

Package
Each package contains one or multiple assemblies and contains one specific functionality that can be added to your game to make the development of your SpatialOS game simpler.
Read access

Replication
Standard replication
Custom replication

The Runtime
A Runtime instance manages the [game world](link!) of each [deployment](link).

Schema
Schemalang


See also the SpatialOS documentation on concepts: schema and working with schema.

Snapshot


Worker

Server-worker
Clent-worker

World
The SpatialOS world, also known as “the world” and “the game world”.
The world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world’s data is stored within SpatialOS [entities](link!!); specifically, within their [components](link!!! to ECS components).
SpatialOS manages the world, keeping track of all the SpatialOS entities and what state they’re in.
Changes to the world are made by [workers](link). Each worker has a view onto the world (the part of the world that they’re [interested](link) in), and SpatialOS sends them updates when anything changes in that view.
It’s important to recognise this fundamental separation between the SpatialOS world and the view (or representation) of that world that a worker [checks out](link!!) locally. This is why workers must send updates to SpatialOS when they want to change the world: they don’t control the canonical state of the world, they must use SpatialOS APIs to change it.

Write access


----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation - see [How to give us feedback](../../README.md#give-us-feedback).