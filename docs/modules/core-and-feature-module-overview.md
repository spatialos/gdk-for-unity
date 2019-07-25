<%(TOC)%>

# Feature Modules Overview

The SpatialOS GDK for Unity consists of a set of modules: the core packages and Feature Modules.

## Core packages

The core packages provide the base implementation for:

* generating code from [schema]({{urlRoot}}/reference/glossary#schema)
* synchronizing your worker instance's local state with the SpatialOS Runtime
* exposing SpatialOS data in both the [ECS and MonoBehaviour workflows]({{urlRoot}}/reference/workflows/overview)
* providing in-Editor tooling for local development

The core packages are delivered as [UPM](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html) packages. You can add them to your project by opening the `Packages/manifest.json` file and add the following:

```json
{
  "dependencies": {
    "io.improbable.gdk.core": "<%(Var key="current_version")%>",
  },
  "scopedRegistries": [
    {
      "name": "Improbable",
      "url": "https://npm.improbable.io/gdk-for-unity/",
      "scopes": [
        "io.improbable"
      ]
    }
  ]
}
```

## Feature Modules

Feature Modules provide optional features such as player lifecycle management or transform synchronization. Feature Modules are intended to both:

* give you a head-start in the development of your game
* act as reference material for best practices

Feature Modules are also delivered as [UPM](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html) packages. Please see the feature module specific documentation for installation instructions.

### Build System

This feature module provides tooling for building your GDK for Unity workers inside the Unity Editor.

See the [build system documentation]({{urlRoot}}/modules/build-system/overview) for details on installation and usage.

### Deployment Launcher

This feature module contains Unity Editor tooling for uploading assemblies, and managing SpatialOS deployments.

See the [Deployment Launcher documentation]({{urlRoot}}/modules/deployment-launcher/overview) for details on installation and usage.

### GameObject Creation

This feature module contains a default implementation of spawning GameObjects for your SpatialOS entities and offers the ability to customize that process.

See the [GameObject Creation documentation]({{urlRoot}}/modules/game-object-creation/overview) for details on installation and usage.

### Mobile

This feature module offers support to connect mobile [client-workers]({{urlRoot}}/reference/glossary#client-worker) to SpatialOS.

See the [Mobile documentation]({{urlRoot}}/modules/mobile/overview) for details on installation and usage.

### Player Lifecycle

This feature module provides player creation functionality and a simple player lifecycle management implementation.

See the [Player Lifecycle documentation]({{urlRoot}}/modules/player-lifecycle/overview) for details on installation and usage.

### Query-based Interest Helper

This feature module contains methods that enable you to easily define the `Interest` component used by Query-based interest.

See the [Query-base Interest Helper documentation]({{urlRoot}}/modules/qbi-helper/overview) for details on installation and usage.

### Transform Synchronization

This feature module contains functionality that will automatically synchronize your entities' transform. 

See the [Transform Synchronization documentation]({{urlRoot}}/modules/transform-sync/overview) for details on installation and usage.
