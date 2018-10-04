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

> Not to be confused with [Unity Console Window](https://docs.unity3d.com/Manual/Console.html)

The Console ([console.improbable.io](https://console.improbable.io/)) is the main landing page for managing [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). It shows you:

* Your [project name](#project-name)
* You past and present [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment)
* All of the [SpatialOS assemblies](#spatialos-assembly) you’ve uploaded
* Links to the [inspect](#inspector), [launch](#launcher), and view logs and metrics for your deployments.

> Related:
> * [Logs](https://docs.improbable.io/reference/latest/shared/operate/logs#cloud-deployments)
> * [Metrics](https://docs.improbable.io/reference/latest/shared/operate/metrics)

### SpatialOS world

> Not to be confused with [Unity ECS World](#unity-ecs-world)

The SpatialOS world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world's data is stored within [SpatialOS entities](#spatialos-entity) - specifically, within their [SpatialOS components](#spatialos-component).

SpatialOS manages the world, keeping track of all the entities and what state they’re in.

*Changes* to the world are made by [workers](#worker). Each worker has a view onto the world (the
part of the world that they're [interested](https://docs.improbable.io/reference/latest/shared/glossary#interest) in), and SpatialOS sends them updates when anything changes
in that view.

It's important to recognise this fundamental separation between the SpatialOS world and the subset view/representation of that world that an individual worker [checks out](https://docs.improbable.io/reference/latest/shared/glossary#checking-out). This is why workers must [send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update)
to SpatialOS when they want to change the world: they don't control the canonical state of the world, they must
use SpatialOS APIs to change it.

### SpatialOS Runtime

> Not to be confused with [SpatialOS world](#spatialos-world)

A SpatialOS Runtime instance manages the [SpatialOS world](#spatialos-world) of each [deployment](deploying).

### SpatialOS Entity

> Not to be confused with [Unity ECS entity](#unity-ecs-entity)

All of the objects inside a [SpatialOS world](#spatialos-world) are SpatialOS entities: they’re the basic building blocks of the world. Examples include players, NPCs, and objects in the world like trees.

SpatialOS entities are made up of [SpatialOS components](#spatialos-component), which store the data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest). Client-workers can represent these entities in any way they like.

For example, for client-workers built using Unity, you might want to have a prefab associated with each entity type, and spawn a GameObject for each entity the worker has [checked out](#https://docs.improbable.io/reference/latest/shared/glossary#checking-out).

You can have other objects that are *not* entities locally on workers - like UI for a player - but no other worker will be able to see them, because they're not part of the [SpatialOS world](#spatialos-world).

> Related:
> * [SpatialOS Concepts:Entities](https://docs.improbable.io/reference/latest/shared/concepts/world-entities-components#entities)
> * [Designing SpatialOS entities](https://docs.improbable.io/reference/latest/shared/design/design-entities)

### SpatialOS Component

> Not to be confused with [Unity ECS component](#unity-ecs-component)

A [SpatialOS entity](#spatialos-entity) is defined by a set of components. Common components in a game might be things like `Health`,
`Position`, or `PlayerControls`. They're the storage mechanism for data about the [world](#spatialos-world) that you
want to be shared between [workers](#worker).

Components can contain:

* [properties](https://docs.improbable.io/reference/latest/shared/glossary#property), which describe persistent values that change over time (for example, [`Position`](https://docs.improbable.io/reference/latest/shared/glossary#position))
* [events](https://docs.improbable.io/reference/latest/shared/glossary#event), which are things that can happen to an entity (for example, `StartedWalking`)
* [commands](https://docs.improbable.io/reference/latest/shared/glossary#command) that another worker can call to ask the component to do something, optionally returning
a value (for example, `Teleport`)

An entity can have as many components as you like, but it must have at least [`Position`](https://docs.improbable.io/reference/latest/shared/glossary#position) and
[`EntityAcl`](#access-control-list). Most entities will have the [`Persistence`](https://docs.improbable.io/reference/latest/shared/glossary#persistence) and [`Metadata`](https://docs.improbable.io/reference/latest/shared/glossary#metadata) components.

Components are defined as files in your [schema](#schema).

Which types of workers can [read from or write to](#read-and-write-access-authority) which components is governed by
[access control lists](#access-control-list).

> Related:
> * [Designing components](https://docs.improbable.io/reference/latest/shared/design/design-components)
> * [Component best practices](https://docs.improbable.io/reference/latest/shared/design/component-best-practices)
> * [Introduction to schema](https://docs.improbable.io/reference/latest/shared/schema/introduction)

### Unity ECS world

> Not to be confused with [SpatialOS world](#spatialos-world)

In the SpatialOS GDK for Unity you can represent a [SpatialOS world](#spatialos-world) using [Unity Sceces](scene) or Unity ECS worlds.

In Unity's ECS, Worlds are the equivalent of [Scenes](scenes). They are a set of ECS entities, components and systems. In the SpatialOS GDK for Unity, ECS worlds (and everything in them) are an abstraction used to represet all or part of the [SpatialOS world](spatialos-world), which is the canonical source of truth for the state of your game.

> Related: [Unity ECS documentation: World](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#world)

### Unity ECS entity

> Not to be confused with [SpatialOS entity](#spatialos-entity)

In the SpatialOS GDK for Unity you can represent a [SpatialOS entity](#spatialos-entity) as a [`GameObject`](gameobject) or a Unity ECS Entity.

A Unity ECS Entity is an abstraction used to represet a SpatialOS entity. It contains [Unity ECS components](unity-ecs-component), which represent [SpatialOS components](spatialos-component).

> Related:
> * [Unity ECS documentation: Entity](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#entity)

### Unity ECS component

> Not to be confused with [SpatialOS component](#spatialos-component)

Just as [Unity ECS entities](#unity-ecs-entity) represent [SpatialOS entities](spatialos-entity), Unity ECS components represent [SpatialOS components](spatialos-component) in the [Unity ECS World](unity-ecs-world).

Unity ECS components are [`IComponentData`](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#icomponentdata) structs. They are abstractions used to represent a stream of concrete, [blittable](blittable-type) data in Unity ECS. This stream of data parallels the data stored in SpatialOS components (remember, the [SpatialOS world](spatialos-world) is the canonical source of truth, the [Unity ECS world](unity-ecs-world) mirrors that truth).

The SpatialOS GDK for Unity generates ECS components from [schema](#schema). This enables you to interact with [SpatialOS components](spatialos-component) using familiar workflows in the Unity Editor.

Generated Unity ECS components can be injected into systems, read, and modified just as normal `IComponentData` structs can. The generated code handles updates from and to SpatialOS.

> Related: [Unity ECS documentation: IComponentData](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#icomponentdata)

### Unity ECS reactive component

A reactive component is a type of [Unity ECS component](unity-ecs-component). The [SpatialOS Runtime](spatialos-runtime) attaches it to a [Unity ECS entity](unity-ecs-entity) for the duration of one [update loop](update-loop). It is added to a Unity ECS Entity when `SpatialOSReceiveSystem` runs and is removed when `CleanReactiveComponentsSystem` runs.

You can think of the contents of a reactive component as the diff between the state of a SpatialOS Entity at the end of the last update loop, and its corresponding Unity ECS entity at the beginning of the next. As the canonical source of truth, the SpatialOS entity is always one update loop ahead of the Unity ECS entity that represents it, and reactive components are how the SpatialOS runtime delivers the state changes and messages that must be applied for the Unity ECS entity to maintain parity.

A reactive component contains all updates and messages received during the last [update loop](update-loop). In every update loop, the contents of a reactive component are processed by whichever [Unity ECS System](unity-ecs-system) that you want to react to those state changes or messages.

> Related: [Receiving entity updates from SpatialOS: reactive components](docs/content/ecs/reactive-components.md)


### Unity ECS system

> Not to be confused with [Worker](#worker)

The code you use to perform operations on [Unity ECS entities](unity-ecs-entity) and their [components](unity-ecs-component) exist in the form of Unity ECS Systems. Systems are scripts. They act, in bulk, on all of the entities in the [Unity ECS world](unity-ecs-world) that contain the components you tell them to act on. For example, a health system might iterate over all entities that have health and damage components, and decrement health components by the value of the damage components.

### Scene

In the SpatialOS GDK for Unity you can represent a [SpatialOS world](#spatialos-world) using Sceces or Unity ECS worlds.

Scenes (and almost everything in them), are an abstraction used to represet all or part of the [SpatialOS world](spatialos-world), which is the canonical source of truth for the state of your game.

> Related: [Unity Manual: Scenes](https://docs.unity3d.com/Manual/CreatingScenes.html)

### GameObject
In the SpatialOS GDK for Unity you can represent a [SpatialOS entity](#spatialos-entity) as a GameObject or a [Unity ECS Entity](unity-ecs-entity).

GameObjects are the fundamental objects in Unity's hybrid-ECS workflow. They represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for hybrid-ECS components such as Monobehaviours, which store data.

> Related:
> * [Unity Manual: GameObject](https://docs.unity3d.com/Manual/class-GameObject.html)
> * [Unity Scripting API Reference: GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)

### Monobehaviour

In the SpatialOS GDK for Unity you can represent a [SpatialOS component](#spatialos-entity) as a Monobehaviour or a [Unity ECS Component](#unity-ecs-component).

MonoBehaviour is the base class for storing data in Unity's hybrid-ECS workflow.

> Related: [Unity Scripting API Reference: MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

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

> Not to be confused with [Game client](#game-client)

A client-worker is a [worker](#worker) whose lifecycle is managed by the game client. You use it to connect to, interact with and disconnect from the [SpatialOS world](#spatialos-world).

Unlike a [server-worker](#server-worker), a client-worker’s lifecycle is not managed by SpatialOS. In a running [deployment](#deploying), there will be one client-worker per player.

Client-workers are mostly tasked with visualising what’s happening in the [SpatialOS world](#spatialos-world). They also dealing with player input. In general, you want to give client-workers
[write access](#read-and-write-access-authority) to as few components as possible,
to make cheating difficult.

> Related: [External worker (server-worker) launch configuration]({{urlRoot}}/shared/worker-configuration/launch-configuration#external-worker-launch-configuration)

### Game client

> Not to be confused with [client-worker](#client-worker)

A game client is a binary. A [client-worker](client-worker) is an object instantiated by said binary.

### Node

> Not to be confused with [Worker](#worker)

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
> * [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access#understanding-read-and-write-access-authority)

### Read access

[Access control lists](#access-control-list) control which workers can have read access to an entity. Read access is defined at the entity level: if a worker can read from an entity, it is allowed to read from all components on that entity.

> Related: [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)

### Write access

> Also referred to as "authority".

Many [workers](#worker) can connect to a [SpatialOS world](#spatialos-world). To prevent them from clashing, SpatialOS only allows one worker at a time to write to each [SpatialOS component](#spatialos-component). Write access is defined at the component level.

Which individual worker *actually has* write access is managed by SpatialOS, and can change regularly because of [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing). However, the list of workers that could *possibly* gain write access is constrained by the [access control lists](#access-control-list).

> Related: [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)

### Snapshot

A snapshot is a representation of the state of a [SpatialOS world](#spatialos-world) at some point in time. It stores each [persistent](#persistence) [SpatialOS entity](#spatialos-entity) and the values of their [SpatialOS components](#spatialos-component)'
[properties](#property).

You'll use a snapshot as the starting point (an [initial snapshot](#initial-snapshot)) for your world when you
[deploy](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment) or [in the cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

> Related: [Snapshots]({{urlRoot}}/shared/operate/snapshots)

### SpatialOS Assembly

> Not to be confused with [.NET assembly](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime).

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

### Unity packages

A Unity package is a container that holds any combination of Assets, Shaders, Textures, plug-ins, icons, and scripts. Each package in the SpatialOS GDK for Unity contains one or more [.NET assemblies](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) and contains one specific functionality that can be added to your game.

Unity packages are managed by the [Unity Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html).

### Core module

The core module is a [Unity package](#unity-package). It is located at `UnityGDK/workers/unity/Packages/com.improbable.gdk.core`.

The [Unity project](#unity-project) inside the SpatialOS GDK for Unity consists of the core module and a number of [feature modules](#feature-module). The core module is compulsory. It provides core functionality to enable your game to run on SpatialOS. It consists of [unity packages](#unity-package), tools (such as the code generator), dependency management, and tests.

### Feature modules

Feature modules are [Unity packages](#unity-package). They are located at `UnityGDK/workers/unity/Packages/`.

The [Unity project](#unity-project) inside the SpatialOS GDK for Unity consists of the [Core module](#core-module) and a number of feature modules. Feature modules are optional features that you can choose to include or exclude from your game (player lifecycle, for example). They are intended both to give you a head-start in the development of your game, and act as reference material for best practices to use when writing your own features.

### Inject

The term inject refers to when a field is populated automatically, either by Unity or the GDK.

In the [monobehaviour workflow] the GDK performs injection via [reflection]. It injects generated readers and writers using the `Improbable.Gdk.GameObjectRepresentation.RequireAttribute` method, and only enables the MonoBehaviour when all of its dependencies are populated.

In the [pure ECS workflow] Unity performs injection via [reflection]. It uses the `Unity.Entities.InjectAttribute` method to iterate over all the [Unity ECS entities] matching a required [component] type.

### Replication

> Also referred to as "synchronisation".

Replication is the process by which all workers in the [SpatialOS world](#spatialos-world) maintain a consistent model of that world. It can be achieved by eitehr [standard](#standard-replication) or [custom](#custom-replication) replication.

### Standard replication

> Also referred to as "automatic synchronisation".

The SpatialOS GDK for Unity performs replication in the [Unity ECS world](unity-ecs-world) via the `Improbable.Gdk.Core.SpatialOSSendSystem` system, which generates replication handlers for all [SpatialOS components](#spatialos-components) defined in the [schema](#schema).

When writing game logic you don't need to worry about synchronisation code. Standard replication handles this automatically.

### Custom replication

> Also referred to as "custom synchronisation".

[Standard replication](#standard-replication) is a powerful solution that keeps your [SpatialOS world](#spatialos-world) and your [Unity ECS World](#unity-ecs-world) sychronised. However, we recognise that it's not the perfect solution for everyone. Firstly, as a generic solution, it's not optimised for your specific game. You may wish to apply custom predicates and filters to minimise network traffic. Secondly, you may wish to update a non-spatial endpoint (such as a database).

Both of these use cases are made possible by custom replication, a system that gives you full control over how a [SpatialOS component](spatialos-component) is replicated. Take a look at `Improbable.Gdk.Core.CustomSpatialOSSendSystem` to see the implementation.

### Commands

### World commands

### Entity commands


### SpatialOS SDK for Unity

The SpatialOS SDK for Unity was the predecessor to the SpatialOS Game Development Kit for Unity. It is **not recommened for development**.

----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation - see [How to give us feedback](../../README.md#give-us-feedback).