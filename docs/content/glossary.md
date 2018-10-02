**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

----
## Glossary

>This glossary **only** contains the terms you need to understand in order to use the SpatialOS GDK for Unity. See the [core concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) and [glossary](https://docs.improbable.io/reference/13.2/shared/glossary) sections of the SpatialOS documentation for a full glossary of generic terms related to SpatialOS.

### SpatialOS Project

Also referred to as "your game".

A SpatialOS project is the source code of a game that runs on SpatialOS. We often use this to refer to the directory that contains the source code, the `UnityGDK` directory in this case.

A SpatialOS project includes (but isn't limited to):

* The source code of all [workers](#worker) used by the project
* The project’s [schema](#schema)
* Optional [snapshots](#snapshot) of the project’s [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world)
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

> Related:
> * [spatial cloud upload]({{urlRoot}}/shared/spatial-cli/spatial-cloud-upload)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

## The `spatial` command-line tool (CLI)

Also known as the "`spatial` CLI".

The `spatial` command-line tool provides a set of commands that you use to interact with a
[SpatialOS project](#spatialos-project). Among other things, you use it to [deploy](#deploying) your game
(using [`spatial local launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) or
[`spatial cloud launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-cloud-launch)).

> Related:
> * [An introducion to the `spatial` command-line tool](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
> * [`spatial` reference documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial)

## Building

When you make changes to the code of a [worker](#worker), you need to build those changes in order to try them out in a [local](#local-deployment) or [cloud deployment](#cloud-deployment).

[How to build your game](build.md) explains this process step by step.

> Related:
> * [Building the bridge and launch configurations of your workers](build.md#building-the-bridge-and-launch-configurations-of-your-workers)
> * [Preparing the build configuration of your workers](build.md#preparing-the-build-configuration-of-your-workers)

## Deploying

When you want to try out your game, you need to deploy it. This means
launching SpatialOS itself. SpatialOS sets up the [world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) based on a [snapshot](#snapshot),
then starts up the [server-workers](#worker) needed to run the game world.

Once the deployment is running, you can connect [client workers](#client-worker) to it. People can then use these clients to play the game.

> Related:
> * [Deploying locally]({{urlRoot}}/shared/deploy/deploy-local)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

### Inspector

The Inspector is a web-based tool that you use to explore the internal state of a [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world).
It gives you realtime view of what’s happening in a [deployment](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment)
or in the [cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). Among other things, it displays:

* which [entities](#entity) are in the world
* what their [components](#component)' [properties](#property) are
* which [workers](#worker) are connected to the deployment
* how much [load](#load-balancing) the workers are under

> Related:
> * [The Inspector]({{urlRoot}}/shared/operate/inspector)

### Launcher

The Launcher is a tool that can download and launch [clients](#client-worker) that connect to [cloud deployments](#cloud-deployment). It's available as an application for Windows and macOS. From the [Console](#console), you can use the Laucnher to connect a local client to your own cloud deployment, or generate a share link so anyone with the link can join your game.

The Launcher downloads the client executable from the [assembly](#assembly) you uploaded.

> Related:
> * [The Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)

## Console

The Console ([console.improbable.io](https://console.improbable.io/)) is the main landing page for managing [cloud deployments](#cloud-deployment). It shows you:

* Your [project name](#project-name)
* You past and present [cloud deployments](#cloud-deployment)
* All the [assemblies](#assembly) you’ve uploaded
* Links to the [inspect](#inspector), [launch](#launcher), and view logs and metrics for your deployments.

> Related:
> * [Logs](https://docs.improbable.io/reference/latest/shared/operate/logs#cloud-deployments)
> * [Metrics](https://docs.improbable.io/reference/13.3/shared/operate/metrics)

## Worker

SpatialOS manages the [world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) itself: it keeps track of all the [entities](#entity) and their
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

> Related:
> * [Concepts: Workers and load balancing]({{urlRoot}}/shared/concepts/workers-load-balancing)

### Server-worker
_Also known as "managed worker", "server-side worker"._

A server-worker has its lifecycle managed by SpatialOS. That means in a running
[deployment](#deploying), SpatialOS is in charge of starting it up and stopping it, as part of
[load balancing](#load-balancing) - unlike a [client-worker](#client).

Server-workers are usually tasked with implementing game logic and physics simulation.

You might have just one server-worker connecting to a SpatialOS world, or dozens, depending on the size of the
[world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world).

You tell SpatialOS how to launch a worker as a server-worker in its [worker configuration file (worker.json)](#worker-configuration-worker-json).

> Related:
> * [Managed worker launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#managed-worker-launch-configuration)

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

> Related:
> * [External worker (server-worker) launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#external-worker-launch-configuration)

#### Node

Node refers to a single machine used by a cloud deployment. Its name indicates the role it plays in your deployment. You can see these on the advanced tab of your deployment details in the [Console](#console).

## Entity

All of the objects inside a [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) are entities: they’re the basic building
block of the world. Examples include players, NPCs, and objects in the world like trees.

Entities are made up of [components](#component), which store the data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](#interest). Workers can represent these
entities locally any way you like.

For example, for workers built using Unity, you might want to have a prefab associated with each entity type, and spawn a GameObject for each entity the worker has [checked out](#checking-out).

You can have other objects that are *not* entities locally on workers - like UI for a player - but no other
worker will be able to see them, because they're not part of the [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world).

> Related:
> * [Concepts: Entities]({{urlRoot}}/shared/concepts/world-entities-components)
> * [Designing entities]({{urlRoot}}/shared/design/design-entities)
> * Creating and deleting entities in:
>   * [C#]({{urlRoot}}/csharpsdk/using/creating-and-deleting-entities)
>   * [C++]({{urlRoot}}/cppsdk/using/creating-and-deleting-entities)
>   * [Java]({{urlRoot}}/javasdk/using/creating-and-deleting-entities)

### Component

An [entity](#entity) is defined by a set of components. Common components in a game might be things like `Health`,
`Position`, or `PlayerControls`. They're the storage mechanism for data about the [world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) that you
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

> Related:
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Component best practices]({{urlRoot}}/shared/design/component-best-practices)
> * [Introduction to schema]({{urlRoot}}/shared/schema/introduction)

## Schema

The schema is where you define all the [components](#component) in your [world](#spatialos-world).

Schema is defined in `.schema` files, written in [schemalang](https://docs.improbable.io/reference/latest/shared/glossary#schemalang). Schema files are stored in the
`schema/` directory of your [project](#project).

SpatialOS uses the schema to [generate code](#code-generation) in various languages (including C#, C++, and Java).
You can use this generated code in your [workers](#worker) to interact with [entities](#entity) in the world.

> Related:
> * [Introduction to schema]({{urlRoot}}/shared/schema/introduction)
> * [Schema reference]({{urlRoot}}/shared/schema/reference)

### Code generation

Use the [command](#the-spatial-command-line-tool-cli)
[`spatial worker codegen`]({{urlRoot}}/shared/spatial-cli/spatial-worker-codegen) to compile generated code from the
[schema](#schema) (in C#, C++ and Java).

This code is used by [workers](#worker) to interact with [entities](#entity): to read from their
[components](#component), and to [make changes](#sending-an-update) to them.

> Related:
> * [Generating code from the schema]({{urlRoot}}/shared/schema/introduction#generating-code-from-the-schema)

## Read and write access ("authority")

Many [workers](#worker) can connect to a [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world). To prevent them from clashing, only one
worker instance at a time is allowed to write to each [component](#component) on each [entity](#entity): ie,
given write access. Write access is sometimes also referred to as authority.

Which types of workers *can have* write access is governed by each entity's [access control list (ACL)](#acl).

Which specific worker instance *actually has* write access is managed by SpatialOS, and can change regularly
because of [load balancing](#load-balancing). However, the list of *possible* workers is constrained by the ACL.

ACLs also control which workers can have read access to an entity. Read access is at the entity level: if a worker
can read from an entity, it is allowed to read from all components on that entity.

> Related:
> * [Understanding write access]({{urlRoot}}/shared/design/understanding-access)

## Snapshot

A snapshot is a representation of the state of a [world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) at some point in time. It
stores each [persistent](#persistence) [entity](#entity) and the values of their [components](#component)'
[properties](#property).

You'll use a snapshot as the starting point (an [initial snapshot](#initial-snapshot)) for your world when you
[deploy](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment) or [in the cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

> Related:
> * [Snapshots]({{urlRoot}}/shared/operate/snapshots)

### SDK for Unity

The SpatialOS SDK for Unity was the predecessor to the SpatialOS Game Development Kit for Unity. It is not recommened for development.

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