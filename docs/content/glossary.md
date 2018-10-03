**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

----
## Glossary

>**Note:** This glossary **only** contains the concepts you need to understand in order to use the SpatialOS GDK for Unity. See the [core concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) and [glossary](https://docs.improbable.io/reference/13.2/shared/glossary) sections of the SpatialOS documentation for a full glossary of generic SpatialOS concepts.

>**Note:** There are many concepts in this glossary that mean different things in different contexts. When semantically overloaded words or phrases are unavoidable, we explicity prefix them to avoid confusion. [.NET assemblies](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) and [SpatialOS assemblies](#spatialos-assembly) are an example of this.

### SpatialOS Project

> Also referred to as "your game".
> 
> Not to be confused with [Unity Project](unity-project) or [project name](project-name).

A SpatialOS project is the source code of a game that runs on SpatialOS. We often use this to refer to the directory that contains the source code, the `UnityGDK` directory in this case.

A SpatialOS project includes (but isn't limited to):

* The source code of all [workers](#worker) used by the project
* The project’s [schema](#schema)
* Optional [snapshots](#snapshot) of the project’s [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world)
* Configuration files, mostly containing settings for [deployments](#deploying) (for example,
[launch configuration files](#launch-configuration-file))

> Related: [Project directory structure](https://docs.improbable.io/reference/latest/shared/reference/project-structure)

### Unity Project

> Not to be confused with [SpatialOS Project](spatialos-project) or [project name](project-name).

A Unity project is the source code of a SpatialOS game's Unity [workers](#worker). We often use this to refer to the directory that contains Unity code, `UnityGDK/workers/unity` in this case.

### Project name

> Not to be confused with [SpatialOS Project](spatialos-project) or [Unity Project](unity-project).

Your project name is randomly generated when you sign up for SpatialOS. It’s usually something like `beta_someword_anotherword_000`.

You must specify this name in the [spatialos.json](#spatialos-json) file in the root of your [SpatialOS project](spatialos-project) when you run a [cloud deployment](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

You can find your project name in the [Console](https://console.improbable.io/).

### The `spatial` command-line tool (CLI)

>Also known as the "`spatial` CLI".

The `spatial` command-line tool provides a set of commands that you use to interact with a
[SpatialOS project](#spatialos-project). Among other things, you use it to [deploy](#deploying) your game
(using [`spatial local launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) or
[`spatial cloud launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-cloud-launch)).

> Related:
> * [An introducion to the `spatial` command-line tool](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
> * [`spatial` reference documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial)

### Building

> The [`spatial` command-line tool](the-spatial-command-line-tool-cli) command `spatial worker build` does not function in the SpatialOS GDK for Unity.

When you make changes to the code of a [worker](#worker), you need to build those changes in order to try them out in a [local](#local-deployment) or [cloud deployment](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

[How to build your game](build.md) explains this process step by step.

> Related:
> * [Building the bridge and launch configurations of your workers](build.md#building-the-bridge-and-launch-configurations-of-your-workers)
> * [Preparing the build configuration of your workers](build.md#preparing-the-build-configuration-of-your-workers)

### Deploying

When you want to try out your game, you need to deploy it. This means
launching SpatialOS itself. SpatialOS sets up the [world](#spatialos-world) based on a [snapshot](#snapshot),
then starts up the [server-workers](#worker) needed to run the game world.

Once the deployment is running, you can connect [client-workers](#client-worker) to it. People can then use these clients to play the game.

> Related:
> * [Deploying locally]({{urlRoot}}/shared/deploy/deploy-local)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

### Inspector

The Inspector is a web-based tool that you use to explore the internal state of a [SpatialOS world](#spatialos-world).
It gives you realtime view of what’s happening in a [deployment](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment)
or in the [cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). Among other things, it displays:

* which [SpatialOS entities](#spatialos-entity) are in the world
* what their [SpatialOS components](#spatialos-component)' [properties](#property) are
* which [workers](#worker) are connected to the [deployment](#deploying)
* how much [load](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing) the workers are under

> Related: [The Inspector]({{urlRoot}}/shared/operate/inspector)

### Launcher

The Launcher is a tool that can download and launch [game clients](#game-client) that connect to [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). It's available as an application for Windows and macOS. From the [Console](#console), you can use the Launcher to connect a game client to your own [cloud deployment](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment), or generate a share link so anyone with the link can download a game client and join your game.

The Launcher downloads the client executable from the [SpatialOS assembly](#spatialos-assembly) you uploaded.

> Related: [The Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)

### Console

The Console ([console.improbable.io](https://console.improbable.io/)) is the main landing page for managing [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). It shows you:

* Your [project name](#project-name)
* You past and present [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment)
* All of the [SpatialOS assemblies](#spatialos-assembly) you’ve uploaded
* Links to the [inspect](#inspector), [launch](#launcher), and view logs and metrics for your deployments.

> Related:
> * [Logs](https://docs.improbable.io/reference/latest/shared/operate/logs#cloud-deployments)
> * [Metrics](https://docs.improbable.io/reference/13.3/shared/operate/metrics)

### SpatialOS world

The world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world's data is stored within [SpatialOS entities](#spatialos-entity) - specifically, within their [SpatialOS components](#spatialos-component).

SpatialOS manages the world, keeping track of all the entities and what state they’re in.

*Changes* to the world are made by [workers](#worker). Each worker has a view onto the world (the
part of the world that they're [interested](https://docs.improbable.io/reference/latest/shared/glossary#interest) in), and SpatialOS sends them updates when anything changes
in that view.

It's important to recognise this fundamental separation between the SpatialOS world and the subset view/representation of that world that an individual worker [checks out](https://docs.improbable.io/reference/latest/shared/glossary#checking-out). This is why workers must [send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update)
to SpatialOS when they want to change the world: they don't control the canonical state of the world, they must
use SpatialOS APIs to change it.

### SpatialOS Entity

All of the objects inside a [SpatialOS world](#spatialos-world) are SpatialOS entities: they’re the basic building blocks of the world. Examples include players, NPCs, and objects in the world like trees.

SpatialOS entities are made up of [SpatialOS components](#spatialos-component), which store the data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest). Client-workers can represent these entities in any way they like.

For example, for client-workers built using Unity, you might want to have a prefab associated with each entity type, and spawn a GameObject for each entity the worker has [checked out](#https://docs.improbable.io/reference/latest/shared/glossary#checking-out).

You can have other objects that are *not* entities locally on workers - like UI for a player - but no other worker will be able to see them, because they're not part of the [SpatialOS world](#spatialos-world).

> Related:
> * [SpatialOS Concepts:Entities](https://docs.improbable.io/reference/13.3/shared/concepts/world-entities-components#entities)
> * [Designing SpatialOS entities](https://docs.improbable.io/reference/13.3/shared/design/design-entities)

### SpatialOS Component

A [SpatialOS entity](#spatialos-entity) is defined by a set of components. Common components in a game might be things like `Health`,
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

> Related:
> * [Designing components]({{urlRoot}}/shared/design/design-components)
> * [Component best practices]({{urlRoot}}/shared/design/component-best-practices)
> * [Introduction to schema]({{urlRoot}}/shared/schema/introduction)

### Worker

SpatialOS manages the [SpatialOS world](#spatialos-world) itself: it keeps track of all the [SpatailOS entities](#spatialos-entity) and their
[properties](#property). But on its own, it doesn’t make any changes to the world.

Workers are programs that connect to a SpatialOS world. They perform the computation associated with a world:
they can read what’s happening, watch for changes, and make changes of their own.

There are two types of workers, [server-workers](#server-worker) and [client-workers](#client-worker).

In order to achieve huge scale, SpatialOS divides up the SpatialOS entities in the world between workers, balancing the work so none of them are overloaded. For each SpatialOS entity in the world, it decides which worker should have [write access](#read-and-write-access-authority) to each [SpatialOS component](#spatialos-component) on the SpatialOS entity. To prevent multiple workers clashing, ony one worker at a time can have write access to a SpatialOS component.

As the world changes over time, where SpatialOS entities are and the amount of work associated with them changes. [Server-workers](#server-worker) report back to SpatialOS how much load they're under, and SpatialOS adjusts which workers have write access to components on which SpatialOS entities (and starts up new workers when needed). This is called [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing).

Around the SpatialOS entities they have write access to, each worker has an area of the world they are [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest).
A worker can read the current properties of the SpatialOS entities within this area, and SpatialOS sends [updates](#https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update) about these SpatialOS entities to the worker.

If the worker has [write access](#read-and-write-access-authority) to a SpatialOS component, it can [send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update):
it can update [properties](#property), and trigger [events](#event).

> Related: [Concepts: Workers and load balancing](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing)

### Server-worker

A server-worker is a [worker](#worker) whose lifecycle is managed by SpatialOS. This means that, when a
[deployment](#deploying) is running, SpatialOS is in charge of starting and stopping server-workers, as part of
[load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing) - unlike [client-workers](#client), which are not managed by SpatialOS.

Server-workers are usually tasked with implementing game logic and physics simulation.

You can have one server-worker connected to your [deployment](#deploying), or dozens, depending on the size and complexity of the
[world](#spatialos-world).

### Client-worker

A client-worker is a [worker](#worker) whose lifecycle is managed by the game client. It is used to connect to, interact with and disconnect from the [SpatialOS world](#spatialos-world).

Unlike a [server-worker](#server-worker), a client-worker’s lifecycle is not managed by
SpatialOS. In a running [deployment](#deploying), there will be one
client-worker per player.

Client-workers are mostly tasked with visualising what’s happening in the [SpatialOS world](#spatialos-world). They also dealing with player input. In general, you want to give client-workers
[write access](#read-and-write-access-authority) to as few components as possible,
to make cheating difficult.

> Related: [External worker (server-worker) launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#external-worker-launch-configuration)

### Game client

A game client is a binary. A [client-worker](client-worker) is an object instantiated by said binary.

### Node

Node refers to a single machine used by a [cloud deployment](#deploying). Its name indicates the role it plays in your [deployment](#deploying). You can see these on the advanced tab of your deployment details in the [Console](#console).

### Schema

The schema is where you define all the [SpatialOS components](#spatialos-component) in your [world](#spatialos-world).

Schema is defined in `.schema` files and written in [schemalang](https://docs.improbable.io/reference/latest/shared/glossary#schemalang). Schema files are stored in
`UnityGDK\schema`.

SpatialOS uses the schema to [generate code](#code-generation). You can use this generated code in your [workers](#worker) to interact with [SpatialOS entities](#spatialos-entity) in the world.

> Related:
> * [Introduction to schema](https://docs.improbable.io/reference/latest/shared/schema/introduction)
> * [Schema reference](https://docs.improbable.io/reference/latest/shared/schema/reference)

### Code generation

> The [`spatial` command-line tool](the-spatial-command-line-tool-cli) command `spatial worker codegen` does not function in the SpatialOS GDK for Unity.

Generated code is compiled from the [schema](#schema) and used by [workers](#worker) to interact with [SpatialOS entities](#spatialos-entity): to read from their [SpatialOS components](#spatialos-component), and to [send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update) to them.

Code generation automatically occurs when you open the [Unity Project](#unity-project) in the Unity editor. You can also manually trigger code generation from inside the Unity editor by selecting **SpatialOS** > **Generate code**. You only need to do this when you have:

* Edited the schema
* Created a new worker

> Related: [Generating code from the schema](https://docs.improbable.io/reference/latest/shared/schema/introduction#generating-code-from-the-schema)

#### Access Control List

In order to read from or make changes to a [SpatialOS component](#spatialos-component), [workers](#worker) need to have [read](#read-access) and [write](#write-access) respectively. Workers are granted this through an access control list (ACL).

ACLs are a [SpatialOS component](#component) in the standard [schema](#schema) library: [`EntityAcl`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#entityacl-required). Every
[entity](#entity) needs to have one. The ACL determines:

* At an entity level, which workers have read access
* At a component level, which workers have write access

> Related pages:
>
> * [The standard schema library](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library)
> * [Understanding read and write access](https://docs.improbable.io/reference/13.3/shared/design/understanding-access#understanding-read-and-write-access-authority)

### Read access

[Access control lists](https://docs.improbable.io/reference/latest/shared/glossary#acl) control which workers can have read access to an entity. Read access is defined at the entity level: if a worker can read from an entity, it is allowed to read from all components on that entity.

> Related: [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)

### Write access

> Also referred to as "authority".

Many [workers](#worker) can connect to a [SpatialOS world](#spatialos-world). To prevent them from clashing, SpatialOS only allows one worker at a time to write to each [SpatialOS component](#spatialos-component). Write access is defined at the component level.

Which individual worker *actually has* write access is managed by SpatialOS, and can change regularly because of [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing). However, the list of workers that could *possibly* gain write access is constrained by the access control list.

> Related: [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)

### Snapshot

A snapshot is a representation of the state of a [SpatialOS world](#spatialos-world) at some point in time. It stores each [persistent](#persistence) [SpatialOS entity](#spatialos-entity) and the values of their [SpatialOS components](#spatialos-component)'
[properties](#property).

You'll use a snapshot as the starting point (an [initial snapshot](#initial-snapshot)) for your world when you
[deploy](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment) or [in the cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

> Related: [Snapshots]({{urlRoot}}/shared/operate/snapshots)

### SpatialOS Assembly

A SpatialOS assembly is what's created when you [build your workers](build.md#building-your-workers). It contains all the files that your game uses at runtime. This includes executable files for [client-workers](#client-worker) and [server-workers](#server-worker), and the assets your [workers](#worker) use (like models and textures used by a client to visualize the game).

The SpatialOS assembly is stored locally at `UnityGDK\build\assembly`. When you run a [cloud deployment](#cloud-deployment), your SpatialOS assembly is uploaded and becomes accessible from the [Console](https://console.improbable.io/).

> Related:
> * [spatial cloud upload]({{urlRoot}}/shared/spatial-cli/spatial-cloud-upload)
> * [Deploying to the cloud]({{urlRoot}}/shared/deploy/deploy-cloud)

### Unity Assembly Definition files

We use [.NET assemblies](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) to structure the SpatialOS GDK for Unity. Unity Assembly Definition (.`asmdef`) files define a set of scripts as a .NET assembly. They also define how, and under what circumstances, these .NET assemblies should be generated. 

The benefits of our using Unity assembly definition files are:
* A comprehensible [Unity project](#unity-project) structure.
* A guaruntee that scripts will only run on the [platforms](https://docs.unity3d.com/Manual/UnityCloudBuildSupportedPlatforms.html) that they are intended for.
* A guaruntee that scripts will only run when they are required. This minimises build times.

> Related:
> * [Unity documentation: Script compilation and assembly definition files](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)
> * [Microsoft documentation: Assemblies in the Common Language Runtime](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime)

### SpatialOS SDK for Unity

The SpatialOS SDK for Unity was the predecessor to the SpatialOS Game Development Kit for Unity. It is not recommened for development.
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
It’s important to recognise this fundamental separation between the SpatialOS world and the view (or representation) of that world that a worker [checks out](https://docs.improbable.io/reference/13.3/shared/glossary#checking-out) locally. This is why workers must send updates to SpatialOS when they want to change the world: they don’t control the canonical state of the world, they must use SpatialOS APIs to change it.

Write access


----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation - see [How to give us feedback](../../README.md#give-us-feedback).