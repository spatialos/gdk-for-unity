<%(TOC max="3")%>

# Glossary

<%(Callout message="
This glossary only contains the concepts you need to understand in order to use the SpatialOS GDK for Unity.

See the [core concepts](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/concepts/spatialos) and [glossary](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary) sections for a full glossary of generic SpatialOS concepts.
")%>

## Access Control List (ACL)

A [worker](#worker) must have access to a [SpatalOS component](#spatialos-component) to read or modify it. An access control list (ACL) defines whether a worker is allowed [read](#read-access) or [write access](#write-access). The ACL determines:

* At an entity level, which workers have read access
* At a component level, which workers can have write access

The `EntityAcl` component describes a [SpatialOS entity's](#spatialos-entity) ACl and is required on every SpatialOS entity.

In the GDK for Unity, the [EntityTemplate class]({{urlRoot}}/reference/concepts/entity-templates) allows you to build an entity's ACL.

> **Related:**
>
> * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)

## Authority

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Also known as “write access”.</i></span>

Many [worker instances](#worker) can connect to a [SpatialOS world](#spatialos-world). However, at most one worker instance is authoritative over each component on a [SpatialOS Entity](#spatialos-entity). This worker instance is the only one that can modify the component’s state and handle commands for that component.

An entity’s [access control list (ACL)](#access-control-list-acl) governs which worker types can have authority (or write access) over each component.

SpatialOS manages which specific worker instance has write access over a component. This can change regularly due to [load balancing](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#load-balancing).

> **Related:**
>
> * [Authority and interest](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/authority-and-interest/introduction)

## Building

If you want to start a [cloud deployment](#deploying), you must first build your game in order to be able to upload the resulting binaries to your cloud deployment.

The guide on [how to build your game]({{urlRoot}}/projects/myo/build) explains this process step-by-step.

> **Note:** The GDK does not support the [`spatial` CLI](#spatial-command-line-tool-cli) command `spatial worker build`.

## Checking out

Each individual [worker](#worker) checks out only part of the [SpatialOS world](#spatialos-world). This happens on a [chunk](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#chunk)-by-chunk basis. A worker “checking out a chunk” means that:

* The worker has a local representation of every [SpatialOS entity](#spatialos-entity) in that chunk
* The SpatialOS Runtime sends updates about those entities to the worker

A worker checks out all chunks that it is [interested in](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#interest).

> **Related:**
>
> * [Entity interest](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/bridge-config#bridge-configuration)


## Code generation

The code generator generates code from [schema](#schema). [Worker instances](#worker) use generated code to interact with [SpatialOS entities](#spatialos-entity).

Code generation occurs when you open the [Unity Project](#unity-project) in your Unity editor. You can also manually trigger code generation from inside your Unity Editor by selecting **SpatialOS** > **Generate code**.

You only need to manually generate code when you have:

* Edited the schema
* Created a new worker type

> **Note:** The GDK does not support the [`spatial` CLI](#spatial-command-line-tool-cli) command `spatial worker codegen`.

## Deploying

When you want to try out your game, you need to deploy it. This means launching SpatialOS itself.

SpatialOS sets up the [world](#spatialos-world) based on a [snapshot](#snapshot), then starts up the [server-workers](#worker) needed to run the world.

There are two types of deployment: local and cloud.

1. Local deployments allow you to start the [SpatialOS Runtime](#spatialos-runtime) locally to test changes quickly.
2. Cloud deployments run in the cloud. They allow you to share your game with other people, and run your game at a scale not possible on one local machine. Once a cloud deployment is running, you can connect [game clients](#game-client) to it using the [Launcher](#launcher).

> **Related:**
>
> * [How to deploy your game]({{urlRoot}}/reference/concepts/deployments)
> * [Deployment Launcher]({{urlRoot}}/modules/deployment-launcher/overview)

## Feature modules

The GDK for Unity offers a number of [feature modules](#feature-modules). Feature modules are optional features that you can choose to include in your game (player lifecycle, for example).

They are intended to:

* give you a head-start in the development of your game
* act as reference material for best practices

> **Related:**
>
> * [Core and Feature modules]({{urlRoot}}/modules/core-and-feature-module-overview)

## Game client

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with [client-worker](#client-worker).</i></span>

A game client is a binary that you use to launch your game. This binary instantiates a [client-worker](#client-worker) to connect to SpatialOS.

## GameObject

In the GDK, each [SpatialOS entity](#spatialos-entity) that a [worker instance](#worker) has checked out can be represented as a GameObject.

See [the GameObject Creation feature module]({{urlRoot}}/modules/game-object-creation/overview) documentation for more info.

> **Related:**
>
> * [Unity Manual: GameObject](https://docs.unity3d.com/Manual/class-GameObject.html)
> * [Unity Scripting API Reference: GameObject](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

## Inject

The term “inject” refers to when a field is populated automatically by the SpatialOS GDK for Unity.

In the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle), the GDK performs injection on fields that have the [`[Require]` attribute]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle). A MonoBehaviour is only enabled when all those fields are populated.

## Inspector

The Inspector is a web-based tool that you use to explore the internal state of a [SpatialOS world](#spatialos-world).
It gives you a real time view of what’s happening in a [deployment](#deploying), both [locally](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#local-deployment)
or in the [cloud](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment).

Among other things, it displays:

* which [worker instances](#worker) are connected to the [deployment](#deploying)
* how much [load](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#load-balancing) each worker instance is under
* which [SpatialOS entities](#spatialos-entity) are in the world
* what their [SpatialOS components](#spatialos-component)' [properties](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#property) are
* which [worker instances](#worker) are authoritative over each [SpatialOS component](#spatialos-component)

> **Related:**
>
> * [The Inspector](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/inspector)

## Launcher

The Launcher is a tool that can download and launch [game clients](#game-client) that connect to [cloud deployments](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment) from the [SpatialOS assembly](#spatialos-assembly) you uploaded. It is available as an application for Windows and macOS.

From the [SpatialOS Console](#spatialos-console) for your [cloud deployment](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment), you can use the Launcher to download your game client or generate a share link so anyone with the link can download your game client and join your game.


> Related:
>
> * [The Launcher](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/launcher)

## Locator connection flow

Use this connection flow to:

* Connect a client worker instance to a cloud deployment via the [SpatialOS Launcher](#launcher).
* Connect a mobile worker instance to a cloud deployment.
* Connect a client worker instance to a cloud deployment via your Unity Editor so that you can debug using the [development authentication flow](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/development-authentication).

> **Related:**
>
> * [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows)
> * [Creating your own game authentication server](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/integrate-authentication-platform-sdk)
> * [Development authentication flow](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/development-authentication)

## Message

A [worker instance](#worker) can send and receive messages to and from the [SpatialOS Runtime](#spatialos-runtime). These messages can be:

* Component updates: Messages that describe property updates for any [SpatialOS component](#spatialos-component)
* [Events](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#event): Messages about something that happened to a SpatialOS entity that are broadcasted to all workers.
* [Commands](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#command): Messages that are sent directly to another worker that has [write access](#write-access) over the corresponding SpatialOS component.

> **Related:**
>
> * [Sending and receiving events using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/events)
> * [Sending and receiving events using ECS]({{urlRoot}}/reference/workflows/ecs/interaction/events)

## MonoBehaviour

A MonoBehaviour stores the data and logic that defines the behaviour of the GameObject they are attached to. We provide support to [interact with SpatialOS using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle).

> **Related:**
>
> * [Unity Scripting API Reference: MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

## Project name

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with [SpatialOS Project](#spatialos-project) or [Unity Project](#unity-project).</i></span>

Your project name is randomly generated when you sign up for SpatialOS. It’s usually something like `beta_someword_anotherword_000`. You can find your project name in the [Console](https://console.improbable.io/).

You must specify this name in the [spatialos.json](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/spatialos-json#project-definition-file-spatialos-json) file in the root of your [SpatialOS project](#spatialos-project).

## Persistence

This is a [SpatialOS component](#spatialos-component) that denotes whether an entity will be saved into a snapshot. If this component is present on an entity, the entity will be captured in a [snapshot](#snapshot). If it is not present, it will not be captured,

Not all entities belong in a snapshot. For example, you probably don’t want the entities associated with players to be saved into a snapshot you take of a deployment, because the players won’t be connected when you restart the deployment.

## Position

This is a [SpatialOS component](#spatialos-component) defining the position of an entity in the [SpatialOS world](#spatialos-world). Every SpatialOS [entities](#spatialos-entity) must have this component.

This is used by SpatialOS for a few specific purposes, like [load balancing](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#load-balancing) and [queries](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#queries).

## Reactive component

<%(Callout type="warn" message="Please note that reactive components will be removed in a future release. We recommend that you do not use them.")%>

Reactive components are a special type of [Unity ECS component](#unity-ecs-component) that are used to react to changes and [messages](#message) received from the [SpatialOS Runtime](#spatialos-runtime).


Reactive components contain all updates and messages received during the last frame. In every update loop, the contents of a reactive component are processed by whichever [Unity ECS System](#unity-ecs-system) is set up to react to those state changes or messages.

> **Related:**
>
> * [Reactive components]({{urlRoot}}/reference/workflows/ecs/interaction/reactive-components/overview)

## Receptionist connection flow

The Receptionist service allows for a direct connection to the SpatialOS Runtime and is the standard flow used to:

* connect any type of worker to a local deployment
* connect a [server-worker](#server-worker) to a cloud deployment
* connect a [client-worker](#client-worker) when using `spatial cloud connect external`.

> Related:
>
> * [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows)

## Read access

[Access control lists](#access-control-list-acl) define which workers can have read access over an entity. Read access is defined at the entity level: if a worker can read from an entity, it is allowed to read from all components on that entity.

> **Related:**
>
> * [Active read access](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/authority-and-interest/interest/active-read-access)

## Schema

Schema is how you define [SpatialOS components](#spatialos-component) in your [SpatialOS world](#spatialos-world).

You define your SpatialOS components in `.schema` files that are written in [schemalang](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#schemalang).

SpatialOS uses schema to [generate code](#code-generation). You can use this generated code in your [workers](#worker) to interact with [SpatialOS entities](#spatialos-entity) in the SpatialOS world.

> **Related:**
>
> * [Introduction to schema](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/introduction)
> * [Schema reference](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/reference)

## Simulated player

A simulated player is a client-worker instance that is controlled by simulated player logic as opposed to real player input. You can use simulated players to scale-test your game.

In the FPS Starter Project, simulated players are hosted in a separate deployment to ensure that they do not share resources with `UnityGameLogic` server-worker instances. They are managed by [simulated player coordinator](#simulated-player-coordinator) worker-instances.

## Simulated player coordinator

A simulated player coordinator is a server-worker responsible for launching simulated players, connecting them into a game deployment, and managing their lifetime.

## Snapshot

A snapshot is a representation of the state of a [SpatialOS world](#spatialos-world) at some point in time. It stores each [persistent](#persistence) [SpatialOS entity](#spatialos-entity) and the values of their [SpatialOS components](#spatialos-component)' [properties](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#property).

You use a snapshot as [the starting point](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#initial-snapshot) for your [SpatialOS world](#spatialos-world) when you [deploy your game](#deploying).

> **Related:**
>
> * [Snapshots](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/snapshots)

## SpatialOS Assembly

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with [.NET assembly (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime).</i></span>

A SpatialOS assembly is created when you build your workers. It contains all the files that your game uses at runtime. This includes the compiled code, an executable and the assets your [workers](#worker) require.

The SpatialOS assembly is stored locally at `build\assembly` in the root directory of your SpatialOS project. Any SpatialOS assembly that has been uploaded is accessible from the [Console](https://console.improbable.io/).

> **Related:**
>
> * [spatial cloud upload](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatial-cli/spatial-cloud-upload)
> * [Deploying to the cloud]({{urlRoot}}/reference/concepts/deployments#cloud-deployment)

## `spatial` command-line tool (CLI)

<%(Callout type="warn" message="We recommend that you use the in-Editor tooling for interacting with the `spatial` CLI. Some commands may not work out of the box otherwise.")%>

The `spatial` command-line tool provides a set of commands that you use to interact with a [SpatialOS project](#spatialos-project). Among other things, you use it to [deploy](#deploying) your game (using [`spatial local launch`](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatial-cli/spatial-local-launch) or [`spatial cloud launch`](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatial-cli/spatial-cloud-launch)).

> Related:
>
> * [An introduction to the `spatial` command-line tool](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatialos-cli-introduction). Note that the GDK does not support any `spatial worker` commands.
> * [`spatial` reference](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatial-cli/spatial)

## SpatialOS component

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with a [Unity ECS component](#unity-ecs-component).</i></span>

A [SpatialOS entity](#spatialos-entity) is defined by a set of components. Common components in a game might be things like `Health`, `Position`, or `PlayerControls`. They're the storage mechanism for data about your [entities](#spatialos-world) that you want to be shared between [workers](#worker). Components are defined as files in your [schema](#schema).

Components can contain:

* [Properties](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#property), which describe persistent values that change over time (for example, a property for a `Health` component could be “the current health value for this entity”.)
* [Events](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#event), which are transient things that can happen to an entity (for example, `StartedWalking`)
* [Commands](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#command) that another worker can call to ask the component to do something, optionally returning a value (for example, `Teleport`)

A SpatialOS entity can have as many components as you like, but it must have at least [`Position`](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#position) and
[`EntityAcl`](#access-control-list-acl). Most entities will have the [`Metadata`](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#metadata) component.

[Entity access control lists](#access-control-list-acl) govern which workers can [read from](#read-access) or [write to](#write-access) each component on an entity.

> **Related:**
>
> * [Designing components](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/design-components)
> * [Component best practices](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/component-best-practices)
> * [Introduction to schema](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/introduction)

## SpatialOS Console

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with the [Unity Console Window](https://docs.unity3d.com/Manual/Console.html).</i></span>

The [Console](https://console.improbable.io/) is the main landing page for managing [cloud deployments](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment). It shows you:

* Your [project name](#project-name)
* Your past and present [cloud deployments](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment)
* All of the [SpatialOS assemblies](#spatialos-assembly) you’ve uploaded
* Links to the [Inspector](#inspector), [Launcher](#launcher), and the logs and metrics page for your deployments.

> **Related:**
>
> * [Logs](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/logs#cloud-deployments)
> * [Metrics](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/metrics)

## SpatialOS entity

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with a [Unity ECS Entity](#unity-ecs-entity).</i></span>

All of the objects inside a [SpatialOS world](#spatialos-world) are SpatialOS entities: they’re the basic building blocks of the world. Examples include players, NPCs, and objects in the world like trees.

SpatialOS entities are made up of [SpatialOS components](#spatialos-component), which store data associated with that entity.

[Workers](#worker) can only see the entities they're [interested in](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#interest).

> **Related:**
>
> * [SpatialOS Concepts: Entities](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/concepts/world-entities-components#entities)
> * [Designing SpatialOS entities](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/design-entities)


## SpatialOS project


<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with [Unity Project](#unity-project) or [project name](#project-name).</i></span>

A SpatialOS project is the source code, assets, and configuration of a game that runs on SpatialOS.

A SpatialOS project includes (but isn't limited to):

* The source code of all [workers](#worker) used by the project
* The project’s [schema](#schema)
* Optional [snapshots](#snapshot) of the project’s [SpatialOS world](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#spatialos-world)
* Configuration files, mostly containing settings for [deployments](#deploying) (for example, [launch configuration files](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/launch-config))

> **Related:**
>
> * [Project directory structure](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/files-and-directories)

## SpatialOS Runtime

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Also known as "SpatialOS".</i></span>

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with the [SpatialOS world](#spatialos-world).</i></span>

A SpatialOS Runtime instance manages the [SpatialOS world](#spatialos-world) of its [deployment](#deploying) by storing all [SpatialOS entities](#spatialos-entity) and the current state of their [SpatialOS components](#spatialos-component).

[Workers](#worker) interact with the SpatialOS Runtime to read and modify the components of an entity as well as send messages between workers.

## SpatialOS world

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with the [Unity ECS World](#unity-ecs-world).</i></span>

The SpatialOS world is a central concept in SpatialOS. It’s the canonical source of truth about your game. All the world's data is stored within [SpatialOS entities](#spatialos-entity) - specifically, within their [SpatialOS components](#spatialos-component).

SpatialOS manages the world, keeping track of all entities and their state.

Changes to the world are made by [workers](#worker). Each worker has a [view](#worker-s-view) onto the world (the part of the world that they're [interested in](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#interest)) and SpatialOS sends them updates when anything changes in that view. It's important to recognize and understand the difference between the SpatialOS world and a [worker's view](#worker-s-view).

## Starter project

The GDK has two starter projects, the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) and the [Blank Starter Project]({{urlRoot}}/projects/blank/overview).

They provide a solid foundation for you to build an entirely new game on-top of SpatialOS GDK for Unity.

## Temporary Components

Temporary components are a special type of [Unity ECS components](#unity-ecs-component). By adding the `[RemoveAtEnd]` attribute to an ECS component, the GDK will remove components at the end of an [update](#update-order).

## Unity Assembly Definition files

We use [.NET assemblies (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime) to structure the GDK.

Unity Assembly Definition (.`asmdef`) files define a set of scripts as a .NET assembly. They also define how, and under what circumstances, these .NET assemblies should be generated.

The benefits of using Unity assembly definition files are:

* A comprehensible [Unity project](#unity-project) structure.
* A guarantee that scripts will only run on the [platforms (Unity documentation)](https://docs.unity3d.com/Manual/UnityCloudBuildSupportedPlatforms.html) that they are intended for.
* A guarantee that scripts will only run when they are required. This minimizes build times.

> **Related:**
>
> * [Script compilation and assembly definition files (Unity documentation)](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)
> * [Assemblies in the Common Language Runtime (.NET documentation)](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/assemblies-in-the-common-language-runtime)

## Unity ECS

Unity's Entity Component System (ECS) is a data-driven pattern of writing code. It is distinct from Unity's existing [MonoBehaviour](#monobehaviour) based workflow.

The GDK uses the Unity ECS as the underlying implementation of its Core while enabling users to follow the traditional MonoBehaviour workflow.

> **Related:**
>
> * [Ask the Unity expert: what is the ECS?](https://improbable.io/games/blog/unity-ecs-1)
> * [Introduction to ECS (Unity documentation)](https://unity3d.com/learn/tutorials/topics/scripting/introduction-ecs)
> * [Unity ECS Samples and Documentation](https://github.com/Unity-Technologies/EntityComponentSystemSamples)
> * [Job System & ECS (Unity documentation)](https://unity3d.com/unity/features/job-system-ECS)


## Unity ECS component

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with a [SpatialOS component](#spatialos-component).</i></span>

Just as [Unity ECS Entities](#unity-ecs-entity) represent [SpatialOS entities](#spatialos-entity), Unity ECS components represent [SpatialOS components](#spatialos-component) in the [Unity ECS World](#unity-ecs-world).

The GDK generates ECS components from [schema](#schema). This enables you to interact with [SpatialOS components](#spatialos-component) using familiar ECS workflows.

Generated Unity ECS components can be injected into systems, read, and modified just as normal Unity ECS components can.

> **Related:**
>
> * [Unity ECS documentation: IComponentData](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/component_data.html#icomponentdata)

## Unity ECS Entity

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with a [SpatialOS entity](#spatialos-entity).</i></span>

In the GDK you represent a [SpatialOS entity](#spatialos-entity) as a Unity ECS Entity. Every [SpatialOS component](#spatialos-component) that belongs to a SpatialOS entity is represented as an [Unity ECS component](#unity-ecs-component) on the corresponding ECS Entity.

> **Related:**
>
> * [Unity ECS documentation: Entity](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/ecs_entities.html)

## Unity ECS system

The code you write to perform operations on [Unity ECS Entities](#unity-ecs-entity) and their [components](#unity-ecs-component) exist in the form of Unity ECS Systems that can be added to ECS worlds.

They act, in bulk, on all of the entities in the [Unity ECS world](#unity-ecs-world) that contain the components you tell them to act on.

For example, a health system might iterate over all entities that have health and damage components, and decrement health components by the value of the damage components.

## Unity ECS world

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with the [SpatialOS world](#spatialos-world).</i></span>

An ECS world contains a collection of ECS entities and systems to perform logic on these entities.

In the GDK, ECS worlds (and everything in them) are an abstraction used to represent the part of the [SpatialOS world](#spatialos-world) that a worker has checked out. A worker is coupled to its ECS world.

> **Related:**
>
> * [Unity ECS documentation: World](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/world.html)

## Unity packages

In Unity there are two [types of packages](https://docs.unity3d.com/Manual/AssetPackages.html):

* Asset packages, available on the Unity Asset Store, which allow you to share and re-use Unity Projects and collections of assets.
* Unity packages, available through the Package Manager window. You can import a wide range of assets, including plugins directly into Unity with this type of package.

In the GDK, each feature module and the core packages are Unity packages.

## Unity Project

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Not to be confused with [SpatialOS Project](#spatialos-project) or [project name](#project-name).</i></span>

A Unity project using the SpatialOS GDK contains the source code and assets of a SpatialOS game's Unity [workers](#worker).

## Update Order

The Unity ECS updates all systems on the main thread. The order in which they are updated is based on constraints that you can add to your systems.

> **Related:**
>
> * [System update order](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/system_update_order.html)
> * [System update order in the GDK]({{urlRoot}}/reference/workflows/ecs/concepts/system-update-order)

## Worker

Workers are server-side and client-side software. They perform the computation associated with a [SpatialOS world](#spatialos-world): they are responsible for iterating on the game simulation and updating the definitive state of the game.

### Worker types and worker instances

When you develop your game, you set up worker types. These are like molds for the worker instances which do the actual computation.

Worker instances are the programs that compute and connect a SpatialOS world. In general, you use one or more server-worker instances to compute the world, and each player’s client software uses a client-worker instance to interact with the world.

### Client-worker

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Also known as “external worker”, “client-side worker”, or “client”.</i></span>

While the lifecycle of a [server-worker](#server-worker) is managed by the SpatialOS Runtime, the lifecycle of a client-worker is managed by the [game client](#game-client).

Client-workers are typically tasked with visualizing what’s happening in the [SpatialOS world](#spatialos-world) and handling player input.

### Server-worker

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Also known as “managed worker”, “server-side worker”.</i></span>

A server-worker is a [worker instance](#worker) whose lifecycle is managed by SpatialOS. When running a [deployment](#deploying), the SpatialOS Runtime starts and stops server-workers based on your chosen [load balancing](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#load-balancing) configuration.

Server-workers are typically tasked with running game logic and physics simulation.

> **Related:**
>
> * [Managed worker (server-worker) launch configuration](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/launch-configuration)

## Worker attribute

Worker attributes are used to denote a [worker’s](#worker) capabilities.

The [SpatialOS Runtime](#spatialos-runtime) uses these attributes to delegate [authority](#authority) over [SpatialOS components](#spatialos-component) in combination with the defined [ACL](#access-control-list-acl).

A worker’s attributes are defined in its [worker configuration JSON](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/bridge-config).

> **Related:**
>
> * [Worker attributes](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#worker-attribute-sets)
> * [Bridge configuration](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/bridge-config#bridge-configuration)

## Worker flags

A worker flag is a key-value pair that workers can access during runtime.

Worker flags can be set in their [launch configuration](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/launch-config).

Worker flags can be modified at runtime through:

* The [`spatial` CLI](#spatial-command-line-tool-cli) for local and cloud deployments.
* The [SpatialOS Console](#spatialos-console) for cloud deployments.
* The Platform SDK for cloud deployments.

> **Related:**
>
> * [Worker flag documentation](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/worker-configuration/worker-flags#worker-flags)
> * [`spatial` CLI documentation](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/spatialos-cli-introduction)
> * [Platform SDK API reference](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/platform-sdk/reference/home)

## Worker SDK

The GDK is built on top of a specialized version of the C# Worker SDK. This specialized C# Worker SDK is itself built on top of the [C API](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/csdk/introduction).

The Worker SDK handles the implementation details of communicating to the SpatialOS Runtime and provides a serialization library.

> **Related:**
>
> * [Worker SDK](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#worker-sdk)

## Worker origin

The worker origin allows you to translate the origin of all SpatialOS entities checked out by a given worker.

When connecting multiple workers in the same Scene, you can use this origin as an offset to ensure no unwanted physical interactions between those game objects occur.

## Worker’s view

A worker’s view consists of all [SpatialOS entities](#spatialos-entity) that a worker is [interested in](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#interest) and has [checked out](#checking-out).

In the GDK, this view is used to populate the [worker’s world](#worker-s-world) and to synchronize any changes between the worker’s view and the worker’s world.

## Worker’s world

In the GDK, during the creation of a [worker](#worker), the worker connects to the [SpatialOS Runtime](#spatialos-runtime) and creates an [ECS world](#unity-ecs-world).

The GDK populates this world with [ECS entities](#unity-ecs-entity) that represent [SpatialOS entities](#spatialos-entity) and are in the [worker’s view](#worker-s-view).

Additionally, the worker adds [ECS systems](#unity-ecs-system) to this world to define the logic that should be run on those ECS entities. The GDK synchronizes this world with the worker’s view stored in the [SpatialOS Runtime](#spatialos-runtime).

## Write access

<span style="display: block;margin-top: -20px;font-size: 10pt;"><i>Also known as "authority".</i></span>

Many [worker instances](#worker) can connect to a [SpatialOS world](#spatialos-world). However, at most one worker instance can have write acesss on a component on a [SpatialOS entity](#spatialos-entity). This worker instance is the only one that can modify the component’s state and handle commands for that component.

An entity’s [access control list (ACL)](#access-control-list-acl) governs which worker types can have write access (or authority) over each component.

SpatialOS manages which specific worker instance has write access over a component. This can change regularly due to [load balancing](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#load-balancing).

> **Related:**
>
> * [Understanding read and write access](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/authority-and-interest/introduction)
