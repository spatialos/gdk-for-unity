<%(TOC)%>

# Glossary

>**Note:** This glossary **only** contains the concepts you need to understand in order to use the SpatialOS GDK for Unity. See the [core concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) and [glossary](https://docs.improbable.io/reference/latest/shared/glossary) sections for a full glossary of generic SpatialOS concepts.
>
>**Note:** There are many concepts in this glossary that mean different things in different contexts. When semantically overloaded words or phrases are unavoidable, we explicitly prefix them to avoid confusion. [.NET assemblies (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) and [SpatialOS assemblies](#spatialos-assembly) are an example of this.

### Access Control List (ACL)

In order to read or modify a [SpatalOS component](#spatialos-component), a [worker](#worker) needs to have access to that component. Whether a worker is allowed [read](#read-access) or [write access](#write-access) is defined by an access control list (ACL). ACLs are components themselves, and are required on every [SpatialOS entity](#spatialos-entity). The ACL determines:

* At an entity level, which workers have read access
* At a component level, which workers may have write access

In the SpatialOS GDK, the [EntityTemplate implementation]({{urlRoot}}/reference/concepts/entity-templates) allows you to add ACL components to entities.

> Related:
>
> * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)

### Authority
>Also known as “write access”.

Many [workers](#worker) can connect to a [SpatialOS world](#spatialos-world). Each component on a [SpatialOS Entity](#spatialos-entity) has no more than one worker that is authoritative over it. This worker is the only one able to modify the component’s state and handling commands for that component. Authority is sometimes called [write-access](#write-access).

Which types of workers can have authority (or write access) is governed by each entity’s [access control list (ACL)](#access-control-list-acl).
Which specific worker actually has write access is managed by SpatialOS, and can change regularly due to [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing).

> Related:
>
> * [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access#understanding-read-and-write-access-authority)

### Building

> The GDK does not support the [`spatial` command-line tool](#spatial-command-line-tool-cli) command `spatial worker build`.


If you want to start a [cloud deployment](#deploying), you must first build your game in order to be able to upload the resulting binaries to your cloud deployment.

The guide on [how to build your game]({{urlRoot}}/projects/myo/build) explains this process step by step.

### Checking out

Each individual [worker](#worker) checks out only part of the [SpatialOS world](#spatialos-world). This happens on a [chunk](https://docs.improbable.io/reference/latest/shared/glossary#chunk)-by-chunk basis. A worker “checking out a chunk” means that:

* The worker has a local representation of every [SpatialOS entity](#spatialos-entity) in that chunk
* The SpatialOS Runtime sends updates about those entities to the worker

A worker checks out all chunks that it is [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest).

> Related:
>
> * [Entity interest](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#entity-interest)

### Client-worker

> Not to be confused with [Game client](#game-client)

While the lifecycle of a [server-worker](#server-worker) is managed by the SpatialOS Runtime, the lifecycle of a client-worker is managed by the game client.


Client-workers are mostly tasked with visualizing what’s happening in the [SpatialOS world](#spatialos-world). They also deal with player input.

> Related:
>
> * [External worker (client-worker) launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#external-worker-launch-configuration)

### Code generation

> The GDK does not support the [`spatial` command-line tool](#spatial-command-line-tool-cli) command `spatial worker codegen`.

Generated code is generated from the [schema](#schema). It is used by [workers](#worker) to interact with [SpatialOS entities](#spatialos-entity). Using generated code, workers can:

* Read the state of SpatialOS entities and their [SpatialOS components](#spatialos-component).
* [Send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update) to SpatialOS components via the [SpatialOS Runtime](#spatialos-runtime).

Code generation automatically occurs when you open the [Unity Project](#unity-project) in your Unity editor. You can also manually trigger code generation from inside your Unity Editor by selecting **SpatialOS** > **Generate code**. You only need to do this when you have:

* Edited the schema
* Created a new worker

> Related:
>
> * [Generating code from the schema](https://docs.improbable.io/reference/latest/shared/schema/introduction#generating-code-from-the-schema)


### Connection

Before the [worker](#worker) can interact with the [SpatialOS world](#spatialos-world), the worker must connect to SpatialOS. The connection is established using the `Improbable.Gdk.Core.Worker` class. See [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows) for more information.

> Related:
>
> * [Creating workers with the `WorkerConnector`]({{urlRoot}}/reference/workflows/monobehaviour/creating-workers)
> * [Locator Connection Flow](#locator-connection-flow)
> * [Receptionist Connection Flow](#receptionist-connection-flow)

### Console

> Not to be confused with the [Unity Console Window](https://docs.unity3d.com/Manual/Console.html)

The [Console](https://console.improbable.io/) is the main landing page for managing [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). It shows you:

* Your [project name](#project-name)
* Your past and present [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment)
* All of the [SpatialOS assemblies](#spatialos-assembly) you’ve uploaded
* Links to the [Inspector](#inspector), [Launcher](#launcher), and the logs and metrics page for your deployments.

> Related:
>
> * [Logs](https://docs.improbable.io/reference/latest/shared/operate/logs#cloud-deployments)
> * [Metrics](https://docs.improbable.io/reference/latest/shared/operate/metrics)

### Core module

The [Unity project](#unity-project) inside the GDK consists of the core module and a number of [feature modules](#feature-modules). The core module is compulsory. It provides core functionality to enable your game to run on SpatialOS. It is located at `UnityGDK/workers/unity/Packages/com.improbable.gdk.core` and consists out of the following [Unity packages](#unity-packages):

 * SpatialOS GDK Core (com.improbable.gdk.core)
 * SpatialOS GDK Test Utils (com.improbable.gdk.testutils)
 * SpatialOS GDK Tools (com.improbable.gdk.tools)

These packages provide the the basic implementation to synchronize with the SpatialOS Runtime and additional tooling for generating code and testing your game.

> Related:
>
> * [Core and Feature modules]({{urlRoot}}/modules/core-and-feature-module-overview)

### Deploying


When you want to try out your game, you need to deploy it. This means
launching SpatialOS itself. SpatialOS sets up the [world](#spatialos-world) based on a [snapshot](#snapshot), then starts up the [server-workers](#worker) needed to run the world.
There are two types of deployment: local and cloud.

[Local deployments](https://docs.improbable.io/reference/cloud/shared/deploy/deploy-local) allow you to start the [SpatialOS Runtime](#spatialos-runtime) locally to test changes quickly.

As their name suggests, [cloud deployments](https://docs.improbable.io/reference/latest/shared/deploy/deploy-cloud) run in the cloud on [nodes](#node). They allow you to share your game with other people, and run your game at a scale not possible on one local machine. Once a cloud deployment is running, you can connect [game clients](#game-client) to it using the [Launcher](#launcher).

> Related:
>
> * [How to deploy your game]({{urlRoot}}/reference/concepts/deployments)

### Feature modules

The [Unity project](#unity-project) inside the GDK consists of the [Core module](#core-module) and a number of feature modules. Feature modules are optional features that you can choose to include or exclude from your game (player lifecycle, for example). They are intended both to give you a head-start in the development of your game, and act as reference material for best practices to use when writing your own features.


The core module and all feature modules are [Unity packages](#unity-packages) and located at `<path to GDK repo>/workers/unity/Packages/`.

> Related:
>
> * [Core and Feature modules]({{urlRoot}}/modules/core-and-feature-module-overview)

### Game client

> Not to be confused with [client-worker](#client-worker)

A game client is a binary. A [client-worker](#client-worker) is an object instantiated by said binary.

### GameObject

In the GDK, each [SpatialOS entity](#spatialos-entity) that a [worker](#worker) has checked out is represented as a [Unity ECS Entity](#unity-ecs-entity). Additionally, you can represent a SpatialOS entity as a GameObject. See [the GameObject Creation feature module]({{urlRoot}}/modules/game-object-creation/overview) documentation for more info.

> Related:
>
> * [Unity Manual: GameObject](https://docs.unity3d.com/Manual/class-GameObject.html)
> * [Unity Scripting API Reference: GameObject](https://docs.unity3d.com/Scriptreference/workflows/monobehaviour.html)

### Inject

The term “inject” refers to when a field is populated automatically, either by Unity or the SpatialOS GDK.

In the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle) the GDK performs injection via [reflection](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection) using the [`[Require]` attribute]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle) to allow you to interact with SpatialOS. A MonoBehaviour is only enabled when all of its dependencies are populated.

In the ECS workflow, Unity performs injection via [reflection](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection) using the [`Inject`](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/8f94d72d1fd9b8db896646d9d533055917dc265a/Documentation/reference/injection.md) attribute inside systems to iterate over all the [Unity ECS Entities](#unity-ecs-entity) matching a required [Unity ECS component](#unity-ecs-component) type.

### Inspector

The Inspector is a web-based tool that you use to explore the internal state of a [SpatialOS world](#spatialos-world).
It gives you a real time view of what’s happening in a [deployment](#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment)
or in the [cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). Among other things, it displays:

* which [workers](#worker) are connected to the [deployment](#deploying)
* how much [load](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing) the workers are under
* which [SpatialOS entities](#spatialos-entity) are in the world
* what their [SpatialOS components](#spatialos-component)' [properties](https://docs.improbable.io/reference/latest/shared/glossary#property) are
* which [workers](#worker) are authoritative over each [SpatialOS component](#spatialos-component)


> Related:
>
> * [The Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector)

### Launcher

The Launcher is a tool that can download and launch [game clients](#game-client) that connect to [cloud deployments](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment). It's available as an application for Windows and macOS. From the [Console](#console), you can use the Launcher to connect a game client to your own [cloud deployment](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment), or generate a share link so anyone with the link can download a game client and join your game.

The Launcher downloads the client executable from the [SpatialOS assembly](#spatialos-assembly) you uploaded.

> Related:
>
> * [The Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)

### Locator connection flow
From SpatialOS v13.5, there are two versions of the Locator connection. The new v13.5+ Locator, in alpha, has additional functionality to the existing v10.4+ Locator which is the stable version.


#### v10.4+ Locator connection flow (stable version)
Use this Locator connection flow to:
 * Connect a client-worker to a cloud deployment via the SpatialOS Launcher - [see SpatialOS documentation on the Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher)


#### New v13.5+ Locator connection flow (alpha version)
Use this Locator connection flow to:

* Connect a client-worker to a cloud deployment via the SpatialOS Launcher - [see SpatialOS documentation on the Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher)
* Connect a client-worker to a cloud deployment via your Unity Editor so that you can debug using the [development authentication flow](https://docs.improbable.io/reference/13.5/shared/auth/development-authentication). (Note that you can also use the Receptionist to connect in this situation.)


Note that there are [other ways](https://docs.improbable.io/reference/13.3/shared/deploy/connect-external) to connect a client-worker to a cloud deployment without using the Locator flow.

> Related:
>
> * [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows)
> * [Connection](#connection)
> * [Creating your own game authentication server](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk)
> * [Development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)


### Message

A [worker](#worker) can send and receive updates and messages to and from the [SpatialOS Runtime](#spatialos-runtime). While updates are simply property updates for any [SpatialOS component](#spatialos-component) that the worker is [interested](https://docs.improbable.io/reference/latestshared/glossary#interest) in, messages can be either

  * [Events](https://docs.improbable.io/reference/latest/shared/glossary#event): messages about something that happened to a SpatialOS entity that are broadcasted to all workers.
  * [Commands](https://docs.improbable.io/reference/latest/shared/glossary#command): Messages used to send a direct message to another worker that has [write access](#write-access) over the corresponding SpatialOS component.

> Related:
>
> * [Sending and receiving events using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/events)
> * [Sending and receiving events using ECS]({{urlRoot}}/reference/workflows/ecs/interaction/events)

### MonoBehaviour


A MonoBehaviour stores the data and logic that defines the behaviour of the GameObject they are attached to. We provide support to [interact with SpatialOS using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle). This allows you to use the traditional MonoBehaviour workflow using GameObjects from the very beginning, without having to worry about the ECS.

> Related:
>
> * [Unity Scripting API Reference: MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

### Node

> Not to be confused with [Worker](#worker)

A node refers to a single machine used by a [cloud deployment](#deploying). Its name indicates the role it plays in your [deployment](#deploying). You can see these on the advanced tab of your deployment details in the [Console](#console).

### Project name

> Not to be confused with [SpatialOS Project](#spatialos-project) or [Unity Project](#unity-project).

Your project name is randomly generated when you sign up for SpatialOS. It’s usually something like `beta_someword_anotherword_000`.

You must specify this name in the [spatialos.json](https://docs.improbable.io/reference/latest/shared/reference/file-formats/spatialos-json) file in the root of your [SpatialOS project](#spatialos-project) when you run a [cloud deployment](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

You can find your project name in the [Console](https://console.improbable.io/).

### Persistence
Persistence is a SpatialOS [component](#spatialos-component) in the standard [schema](#schema) library. It’s optional, but all SpatialOS [entities](#spatialos-entity) that you want to persist in the [world](#world) must have this component. Persistence means that entities are saved into [snapshots](#snapshot).
If an entity doesn’t have this component, it won’t be captured in snapshots. This is fine for transient entities. For example, you probably don’t want the entities associated with players to be saved into a snapshot you take of a deployment, because the players won’t be connected when you restart the deployment.
### Position
Position is a [component](#spatialos-component) in the standard [schema](#schema) library; all SpatialOS [entities](#spatialos-entity) must have this component. It lets SpatialOS know what the position of an entity in the [world](#world) is.
This is used by SpatialOS few specific purposes, like [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing) and [queries](https://docs.improbable.io/reference/latest/shared/glossary#queries).

### Reactive component

Reactive components are a special type of [Unity ECS component](#unity-ecs-component) that are used to react to changes and [messages](#message) received by the [SpatialOS Runtime](#spatialos-runtime). The SpatialOS Runtime attaches reactive components to a [Unity ECS Entity](#unity-ecs-entity) representing a SpatialOS entity for the duration of one [update](#update-loop).

An example reactive component is the `ComponentAdded` component. It is added to an Unity ECS Entity representing a SpatialOS entity when a new SpatialOS component has been added to the SpatialOS entity. The GDK adds the corresponding ECS component automatically to the ECS entity, however you might want to run additional systems to react to this change in the current update loop. The `ComponentAdded` component is removed when the `CleanReactiveComponentsSystem` runs.

Reactive components contain all updates and messages received during the last [update loop](#update-loop). In every update loop, the contents of a reactive component are processed by whichever [Unity ECS System](#unity-ecs-system) is set up to react to those state changes or messages.

> Related:
>
> * [Reactive components]({{urlRoot}}/reference/workflows/ecs/reactive-components)

### Receptionist connection flow

The Receptionist service allows for a direct connection to the SpatialOS runtime and is the standard flow used to
  * connect any type of worker in a local deployment
  * connecting a [server-worker](#server-worker) in a cloud deployment
  * connecting a [client-worker](#client-worker) when using `spatial cloud connect external`.

> Related:
>
> * [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows)
> * [Connection](#connection)

### Read access

[Access control lists](#access-control-list-acl) define which workers can have read access over an entity. Read access is defined at the entity level: if a worker can read from an entity, it is allowed to read from all components on that entity.

> Related:
>
> * [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)

### Replication

> Also referred to as "synchronization".

Replication is the process by which all workers in the [SpatialOS world](#spatialos-world) maintain a consistent model of that world. It can be achieved using [standard replication](#standard-replication).

### Server-worker

A server-worker is a [worker](#worker) whose lifecycle is managed by SpatialOS. When running a [deployment](#deploying), the SpatialOS Runtime starts and stops server-workers based on your chosen [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing) configuration.

Server-workers are usually tasked with implementing game logic and physics simulation.

You can have one server-worker connected to your [deployment](#deploying), or dozens, depending on the size and complexity of your
[SpatialOS world](#spatialos-world).

> Related:
>
> * [Managed worker (server-worker) launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#managed-worker-launch-configuration)

### Scene

In the GDK, a Unity Scene stores GameObjects that you might use to represent your SpatialOS entities. Each Scene can interact with multiple Unity ECS worlds.

Scenes are an abstraction used to represent the part of the [SpatialOS world](#spatialos-world) that the [workers](#worker) defined in the Scene have checked out.
> Related: [Unity Manual: Scenes](https://docs.unity3d.com/Manual/CreatingScenes.html)

### Schema

The schema is where you define all the [SpatialOS components](#spatialos-component) in your [SpatialOS world](#spatialos-world).

You define your schema in `.schema` files that are written in [schemalang](https://docs.improbable.io/reference/latest/shared/glossary#schemalang). Schema files are stored in the `schema` folder in the root directory of your SpatialOS project.

SpatialOS uses the schema to [generate code](#code-generation). You can use this generated code in your [workers](#worker) to interact with [SpatialOS entities](#spatialos-entity) in the SpatialOS world.

> Related:
>
> * [Introduction to schema](https://docs.improbable.io/reference/latest/shared/schema/introduction)
> * [Schema reference](https://docs.improbable.io/reference/latest/shared/schema/reference)

### Snapshot

A snapshot is a representation of the state of a [SpatialOS world](#spatialos-world) at some point in time. It stores each [persistent](#persistence) [SpatialOS entity](#spatialos-entity) and the values of their [SpatialOS components](#spatialos-component)' [properties](https://docs.improbable.io/reference/latest/shared/glossary#property).

You'll use a snapshot as the starting point (an [initial snapshot](https://docs.improbable.io/reference/latest/shared/glossary#initial-snapshot)) for your [SpatialOS world](#spatialos-world) when you [deploy your game](#deploying).

> Related:
>
> * [Snapshots](https://docs.improbable.io/reference/latest/shared/operate/snapshots)

### SpatialOS Assembly

> Not to be confused with [.NET assembly (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime).

A SpatialOS assembly is created when you build your workers. It contains all the files that your game uses at runtime. This includes the compiled code and an executable and the assets your [workers](#worker) use (like models and textures used by a client to visualize the game).

The SpatialOS assembly is stored locally at `build\assembly` in the root directory of your SpatialOS project. When you start a [cloud deployment]({{urlRoot}}/reference/concepts/deployments#cloud-deployment), your SpatialOS assembly is uploaded and becomes accessible from the [Console](https://console.improbable.io/).

> Related:
>
> * [spatial cloud upload](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-cloud-upload)
> * [Deploying to the cloud]({{urlRoot}}/reference/concepts/deployments#cloud-deployment)

### `spatial` command-line tool (CLI)

The `spatial` command-line tool provides a set of commands that you use to interact with a [SpatialOS project](#spatialos-project). Among other things, you use it to [deploy](#deploying) your game (using [`spatial local launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) or [`spatial cloud launch`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-cloud-launch)).

> Related:
>
> * [An introduction to the `spatial` command-line tool](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction). Note that the GDK does not support any `spatial worker` commands.
> * [`spatial` reference](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial)


### SpatialOS component

> Not to be confused with a [Unity ECS component](#unity-ecs-component)

A [SpatialOS entity](#spatialos-entity) is defined by a set of components. Common components in a game might be things like `Health`, `Position`, or `PlayerControls`. They're the storage mechanism for data about the [world](#spatialos-world) that you want to be shared between [workers](#worker).

Components can contain:

* [properties](https://docs.improbable.io/reference/latest/shared/glossary#property), which describe persistent values that change over time (for example, a property for a `Health` component could be “the current health value for this entity”.)
* [events](https://docs.improbable.io/reference/latest/shared/glossary#event), which are transient things that can happen to an entity (for example, `StartedWalking`)
* [commands](https://docs.improbable.io/reference/latest/shared/glossary#command) that another worker can call to ask the component to do something, optionally returning a value (for example, `Teleport`)

A SpatialOS entity can have as many components as you like, but it must have at least [`Position`](https://docs.improbable.io/reference/latest/shared/glossary#position) and
[`EntityAcl`](#access-control-list-acl). Most entities will have the [`Metadata`](https://docs.improbable.io/reference/latest/shared/glossary#metadata) component.


> Unlike Unity ECS components, it is not possible to add or remove SpatialOS components on already existing SpatialOS entities.

Components are defined as files in your [schema](#schema).

[Entity access control lists](#access-control-list-acl) govern which workers can [read from](#read-access) or [write to](#write-access) each component on an entity.

> Related:
>
> * [Designing components](https://docs.improbable.io/reference/latest/shared/design/design-components)
> * [Component best practices](https://docs.improbable.io/reference/latest/shared/design/component-best-practices)
> * [Introduction to schema](https://docs.improbable.io/reference/latest/shared/schema/introduction)

### SpatialOS entity

> Not to be confused with a [Unity ECS Entity](#unity-ecs-entity).

All of the objects inside a [SpatialOS world](#spatialos-world) are SpatialOS entities: they’re the basic building blocks of the world. Examples include players, NPCs, and objects in the world like trees.

SpatialOS entities are made up of [SpatialOS components](#spatialos-component), which store data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest).

For example, for client-workers built using Unity, you might want to have a prefab associated with each entity type, and spawn a GameObject for each entity the worker has [checked out](https://docs.improbable.io/reference/latest/shared/glossary#checking-out).

You can have other objects that are *not* entities locally on workers - like UI for a player - but no other worker will be able to see them, because they're not part of the [SpatialOS world](#spatialos-world).

> Related:
>
> * [SpatialOS Concepts:Entities](https://docs.improbable.io/reference/latest/shared/concepts/world-entities-components#entities)
> * [Designing SpatialOS entities](https://docs.improbable.io/reference/latest/shared/design/design-entities)


### SpatialOS project

> Also referred to as "your game".
>
> Not to be confused with [Unity Project](#unity-project) or [project name](#project-name).

A SpatialOS project is the source code of a game that runs on SpatialOS. We often use this to refer to the directory that contains the source code, the `UnityGDK` directory in this case.

A SpatialOS project includes (but isn't limited to):

* The source code of all [workers](#worker) used by the project
* The project’s [schema](#schema)
* Optional [snapshots](#snapshot) of the project’s [SpatialOS world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world)
* Configuration files, mostly containing settings for [deployments](#deploying) (for example, [launch configuration files](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration))

> Related:
>
> * [Project directory structure](https://docs.improbable.io/reference/latest/shared/reference/project-structure)

### SpatialOS Runtime

> Not to be confused with the [SpatialOS world](#spatialos-world)
> Also sometimes just called "SpatialOS".

A SpatialOS Runtime instance manages the [SpatialOS world](#spatialos-world) of each [deployment](#deploying) by storing all [SpatialOS entities](#spatialos-entity) and the current state of their [SpatialOS components](#spatialos-component). [Workers](#worker) interact with the SpatialOS Runtime to read and modify the components of an entity as well as send messages between workers.

#### New SpatialOS Runtime (available from SpatialOS version 13.4)
From SpatialOS version 13.4 there is a new SpatialOS Runtime.
It consists of three elements:

* The new [bridge](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#bridge-configuration).
* The new [load balancer](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing).
* The new entity database.


It also contains a new feature: [Query-based interest]https://docs.improbable.io/reference/13.5/shared/reference/query-based-interest).

The GDK for Unity version Alpha 0.1.4, uses the new SpatialOS Runtime (available from SpatialOS version 13.4); wherever the documentation refers to the “Runtime”, it means the new v13.4+ SpatialOS Runtime.

> Related:
>
> * [The new Runtime (blog post)](https://improbable.io/games/blog/the-new-runtime-is-here-with-a-new-feature-for-managing-areas-of-interest)
> * [Upgrade to the new Runtime](https://docs.improbable.io/reference/latest/releases/upgrade-guides/upgrade-runtime)

### SpatialOS world

> Not to be confused with the [Unity ECS World](#unity-ecs-world)

The SpatialOS world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world's data is stored within [SpatialOS entities](#spatialos-entity) - specifically, within their [SpatialOS components](#spatialos-component).

SpatialOS manages the world, keeping track of all entities and their state.

Changes to the world are made by [workers](#worker). Each worker has a view onto the world (the part of the world that they're [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest)), and SpatialOS sends them updates when anything changes in that view.

It's important to recognize this fundamental separation between the SpatialOS world and the subset view of that world that an individual worker [checks out](https://docs.improbable.io/reference/latest/shared/glossary#checking-out). This is why workers must [send updates](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update) to SpatialOS when they want to change the world: they don't control the canonical state of the world, they must use the [Worker SDK](https://docs.improbable.io/reference/latest/shared/glossary#worker-sdk) API to change it.

### Standard replication

> Also referred to as "automatic synchronization".

The GDK performs replication in the [Unity ECS world](#unity-ecs-world) via the `Improbable.Gdk.Core.SpatialOSSendSystem` system, which generates replication handlers for all [SpatialOS components](#spatialos-component) defined in the [schema](#schema).

When writing game logic you don't need to worry about synchronisation code. Standard replication handles this automatically.

### Starter project

The GDK has two starter projects, the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) and the [Blank Starter Project]({{urlRoot}}/projects/blank/overview). They provide a solid foundation for you to build an entirely new game on-top of SpatialOS GDK for Unity.

### Temporary Components

Temporary components are a special type of [Unity ECS components](#unity-ecs-component). By adding the `[RemoveAtEnd]` attribute to an ECS component, the GDK will remove components at the end of an [update](#update-loop).

### Unity Assembly Definition files

We use [.NET assemblies (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) to structure the GDK. Unity Assembly Definition (.`asmdef`) files define a set of scripts as a .NET assembly. They also define how, and under what circumstances, these .NET assemblies should be generated.

The benefits of using Unity assembly definition files are:

* A comprehensible [Unity project](#unity-project) structure.
* A guarantee that scripts will only run on the [platforms (Unity documentation)](https://docs.unity3d.com/Manual/UnityCloudBuildSupportedPlatforms.html) that they are intended for.
* A guarantee that scripts will only run when they are required. This minimizes build times.

> Related:
>
> * [Script compilation and assembly definition files (Unity documentation)](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)
> * [Assemblies in the Common Language Runtime (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime)

### Unity ECS

Unity's Entity Component System (ECS) is a way of writing code that focuses on the data and behavior that make up your game. It is distinct from Unity's existing [MonoBehaviour](#monobehaviour) based workflow.

The GDK uses the Unity ECS as the underlying implementation of its Core while enabling users to follow the traditional MonoBehaviour workflow.

> Related:
>
> * [Ask the Unity expert: what is the ECS?](https://improbable.io/games/blog/unity-ecs-1)
> * [Introduction to ECS (Unity documentation)](https://unity3d.com/learn/tutorials/topics/scripting/introduction-ecs)
> * [Unity ECS Samples and Documentation](https://github.com/Unity-Technologies/EntityComponentSystemSamples)
> [Job System & ECS (Unity documentation)](https://unity3d.com/unity/features/job-system-ECS)


### Unity ECS component

> Not to be confused with a [SpatialOS component](#spatialos-component)

Just as [Unity ECS Entities](#unity-ecs-entity) represent [SpatialOS entities](#spatialos-entity), Unity ECS components represent [SpatialOS components](#spatialos-component) in the [Unity ECS World](#unity-ecs-world).

Unity ECS components contain only data and are represented as structs rather than classes. This means that they are passed [by value instead of by reference](https://stackoverflow.com/questions/373419/whats-the-difference-between-passing-by-reference-vs-passing-by-value?answertab=votes#tab-top). Any behaviour that you want to associate with an ECS component needs to be defined in a [Unity ECS system](#unity-ecs-system).

The GDK generates ECS components from [schema](#schema). This enables you to interact with [SpatialOS components](#spatialos-component) using familiar workflows in your Unity Editor.

Generated Unity ECS components can be injected into systems, read, and modified just as normal Unity ECS components can. Additionally, the generated code enables you to send and receive updates and [messages](#message) from and to SpatialOS.

> Related:
>
> * [Unity ECS documentation: IComponentData](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/workflows/ecs_in_detail#icomponentdata)

### Unity ECS Entity

> Not to be confused with a [SpatialOS entity](#spatialos-entity)

In the GDK you represent a [SpatialOS entity](#spatialos-entity) as a Unity ECS Entity. Every [SpatialOS component](#spatialos-component) that belongs to a SpatialOS entity is represented as an [Unity ECS component](#unity-ecs-component) on the corresponding ECS Entity.


> Related:
>
> * [Unity ECS documentation: Entity](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/workflows/ecs_in_detail#entity)

### Unity ECS system

The code you use to perform operations on [Unity ECS Entities](#unity-ecs-entity) and their [components](#unity-ecs-component) exist in the form of Unity ECS Systems that can be added to ECS worlds. They act, in bulk, on all of the entities in the [Unity ECS world](#unity-ecs-world) that contain the components you tell them to act on. For example, a health system might iterate over all entities that have health and damage components, and decrement health components by the value of the damage components.

### Unity ECS world

> Not to be confused with the [SpatialOS world](#spatialos-world)


The Unity ECS world is a new abstraction introduced by Unity to enable their Entity Component Systems. An ECS world contains a collection of ECS entities and systems to perform logic on these entities.

In the GDK, ECS worlds (and everything in them) are an abstraction used to represent the part of the [SpatialOS world](#spatialos-world) that a worker has checked out. A worker is tightly coupled to its ECS world and defines which ECS entities belong to this world and which systems are run.


> Related:
>
> * [Unity ECS documentation: World](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/workflows/ecs_in_detail#world)

### Unity packages


In Unity there are two [types of packages (Unity documentation)](https://docs.unity3d.com/Manual/AssetPackages.html):
Asset packages, available on the Unity Asset Store, which allow you to share and re-use Unity Projects and collections of Assets.
Unity packages, available through the Package Manager window (Unity documentation). You can import a wide range of Assets, including plugins directly into Unity with this type of package.
In the GDK each feature module, and the core module, consists of Unity packages. They are located at `UnityGDK/workers/unity/Packages/`. This allows you to choose which feature modules you want to include in your game.

### Unity Project

> Not to be confused with [SpatialOS Project](#spatialos-project) or [project name](#project-name).

A Unity project is the source code and assets of a SpatialOS game's Unity [workers](#worker). We often use this to refer to the directory that contains Unity code and assets, `<path to the GDK repo>/workers/unity` in this case.

### Update Loop

The Unity ECS updates all systems on the main thread. The order in which they are updated is based on constraints that you can add to your systems.

> Related:
>
> * [System update order (Unity documentation)](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/workflows/ecs_in_detail)
> [System update order in the GDK]({{urlRoot}}/reference/workflows/ecs/system-update-order)

### Worker

The [SpatialOS Runtime](#spatialos-runtime) manages the [SpatialOS world](#spatialos-world): it keeps track of all the [SpatialOS entities](#spatialos-entity) and their
[SpatialOS components](#spatialos-component). But on its own, it doesn’t make any changes to the world.

Workers are programs that connect to a SpatialOS world. They perform the computation associated with a world:
they can read what’s happening, watch for changes, and make changes of their own.

There are two types of workers, [server-workers](#server-worker) and [client-workers](#client-worker).

In order to achieve huge scale, SpatialOS divides up the SpatialOS entities in the world between workers, balancing the work so none of them are overloaded. For each SpatialOS entity in the world, it decides which worker should have [write access](#write-access) to each SpatialOS component on the SpatialOS entity. To prevent multiple workers writing to a component at the same time, only one worker at a time can have write access to a SpatialOS component.

As the world changes over time, the position of SpatialOS entities and the amount of work associated with them changes. [Server-workers](#server-worker) report back to SpatialOS how much load they're under, and SpatialOS adjusts which workers have write access to components on which SpatialOS entities (and starts up new workers when needed). This is called [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing).

Around the SpatialOS entities they have write access to, each worker has an area of the world they are [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest).
A worker can read the current properties of the SpatialOS entities within this area, and SpatialOS sends [updates and messages](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update) about these SpatialOS entities to the worker.

If the worker has write access to a SpatialOS component, it can [send updates and messages](https://docs.improbable.io/reference/latest/shared/glossary#sending-an-update):
it can update [properties](https://docs.improbable.io/reference/latest/shared/glossary#property), send and handle [commands](https://docs.improbable.io/reference/latest/shared/glossary#command) and trigger [events](https://docs.improbable.io/reference/latest/shared/glossary#event).


See also the [Workers in the GDK]({{urlRoot}}/reference/concepts/worker) documentation.

> Related:
>
> * [Concepts: Workers and load balancing](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing)



### Worker attribute

Worker attributes are used to denote a [worker’s](#worker) capabilities. The [SpatialOS runtime](#spatialos-runtime) uses these attributes to delegate [authority](#authority) over [SpatialOS components](#spatialos-component) in combination with the defined [ACL](#access-control-list-acl). A worker’s attributes are defined in its [worker configuration JSON](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#worker-attribute-sets).

> Related:
>  
> * [Worker attributes](https://docs.improbable.io/reference/latest/shared/glossary#worker-attribute)
> * [Bridge configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#worker-attribute-sets)

### Worker flags


A worker flag is a key-value pair that workers can access during runtime. Worker flags can be set in their [launch configuration](https://docs.improbable.io/reference/latest/shared/reference/file-formats/launch-config) or added/changed/removed at runtime through the SpatialOS console. [This SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/worker-configuration/worker-flags#worker-flags) describes how to define and change worker flags.

### Worker SDK

The GDK is built on top of a specialized version of the C# Worker SDK. This specialized C# Worker SDK is itself built on top of the [C API](https://docs.improbable.io/reference/latest/capi/introduction). The Worker SDK handles the implementation details of communicating to the SpatialOS runtime and provides a serialization library.

> Related:
>
> * [Worker SDK](https://docs.improbable.io/reference/latest/shared/glossary#worker-sdk)

### Worker Origin

The worker origin allows you to translate the origin of all SpatialOS entities checkout out by a given worker. When connecting multiple workers in the same scene, you can use this origin as an offset to ensure no unwanted physical interactions between those game objects occur.
### Worker types

There are two generic types of worker that define how you would want to connect these workers and what kind of capabilities they have:

  * [server-worker](#server-worker)
  * [client-worker](#client-worker)

Within these broad types, users can define their own worker sub-types to create more specialized workers.

### Worker’s view

A worker’s view consists of all [SpatialOS entities](#spatialos-entity) that a worker is currently [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest) and has [checked out](#checking-out). In the GDK, this view is used to populate the [worker’s world](#worker-s-world) and to synchronize any changes between the worker’s view and the worker’s world.

### Worker’s world

In the GDK, during the creation of a [worker](#worker), the worker connects to the [SpatialOS Runtime](#spatialos-runtime) and creates an [ECS world](#unity-ecs-world). The GDK populates this world with [ECS entities](#unity-ecs-entity) that represent [SpatialOS entities](#spatialos-entity) and are currently in the [worker’s view](#worker-s-view). Additionally, the worker adds [ECS systems](#unity-ecs-system) to this world to define the logic that should be run on those ECS entities. The GDK synchronizes this world with the worker’s view stored in the [SpatialOS runtime](#spatialos-runtime).
### World
The SpatialOS world, also known as “the world” and “the game world”.
The world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world’s data is stored within SpatialOS [entities](#spatialos-entity); specifically, within their [components](#spatialos-component).
SpatialOS manages the world, keeping track of all the SpatialOS entities and what state they’re in.
Changes to the world are made by [workers](#worker). Each worker has a view onto the world (the part of the world that they’re [interested in](https://docs.improbable.io/reference/latest/shared/glossary#interest)), and SpatialOS sends them updates when anything changes in that view.
It’s important to recognize this fundamental separation between the SpatialOS world and the view (or representation) of that world that a worker [checks out](#checking-out) locally. This is why workers must send updates to SpatialOS when they want to change the world: they don’t control the canonical state of the world, they must use SpatialOS APIs to change it.

### Write access

> Also referred to as "authority".

Many [workers](#worker) can connect to a [SpatialOS world](#spatialos-world). To prevent them from clashing, SpatialOS only allows one worker at a time to write to each [SpatialOS component](#spatialos-component). Write access is defined at the component level.

Which individual worker *actually has* write access is managed by SpatialOS, and can change regularly because of [load balancing](https://docs.improbable.io/reference/latest/shared/glossary#load-balancing). However, the list of workers that could *possibly* gain write access is constrained by the [access control lists](#access-control-list-acl).

> Related:
>
> * [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access)
