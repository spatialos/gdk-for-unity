<%(TOC)%>

# Feature Modules Overview

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/overview)._

The SpatialOS GDK for Unity consists of several modules: the Core Module and multiple Feature Modules. The Core Module provides the functionality to enable your game for SpatialOS, while Feature Modules provide functionality that is not needed to connect to the SpatialOS [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) but makes it easier to implement your game; such as player lifecycle or transform synchronization. Each module has helper functions which you can use to add the module’s functionality to a [worker](https://github.com/spatialos/UnityGDK/blob/master/docs/reference/workers).

See the documentation on [Workers in the GDK]({{urlRoot}}/reference/concepts/worker) for information on the relationship between workers and [ECS entities]({{urlRoot}}/reference/glossary#unity-ecs-entity).

## Core Module

The Core Module contains multiple packages (or "assemblies") that you need to use to set up your Unity-developed game to work with SpatialOS. The packages are:

* `Improbable.Gdk.Core` <br/>
This provides the basic implementation to use SpatialOS natively in Unity.

* `Improbable.Gdk.Tools`<br/>
 This contains the code generator which generates C# code from your schema file and the tools to download dependent SpatialOS packages. (See the SpatialOS documentation on [schema]({{urlRoot}}/reference/glossary#schema).)

* `Improbable.Gdk.TestUtils` <br/>
This provides both a testing framework, which you can use to test any other module and a test which you can use to validate the state of the overall project.

## Feature Modules

### Build System

This feature module provides tooling for building your GDK for Unity workers inside the Unity Editor. See our [build system documentation]({{urlRoot}}/modules/build-system/overview) for more details on installation and usage.

### GameObject Creation

This feature module contains a default implementation of spawning GameObjects for your SpatialOS entities and offers the ability to customize that process. See the [detailed documentation for set up and usage instructions]({{urlRoot}}/modules/game-object-creation/overview).

### Player Lifecycle

This feature module provides player creation functionality and a simple player lifecycle management implementation. See the [detailed documentation for set up and usage instructions]({{urlRoot}}/modules/player-lifecycle/overview).

### Query-based Interest

This feature module contains methods that enable you to easily define the `Interest` component used by Query-based interest. See the [detailed documentation for set up and usage instructions]({{urlRoot}}/modules/qbi-helper/overview).

### Transform Synchronization

This feature module contains functionality that will automatically synchronize your entities' transform. See our [Transform Synchronization documentation]({{urlRoot}}/modules/transform-sync/overview) for more details on installation and usage.

### Mobile

This feature module offers support to connect mobile [client-workers]({{urlRoot}}/reference/glossary#client-worker) to SpatialOS. See our [Mobile Support documentation]({{urlRoot}}/modules/mobile/overview) for more details on installation and usage.
