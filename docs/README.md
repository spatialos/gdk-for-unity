# SpatialOS GDK for Unity documentation

#### How to get started
We recommend you start by following the [Installation and setup guide](setup-and-installing.md) to set up the GDK Core and its accompanying example scene, the Playground. You can then explore what the SpatialOS GDK for Unity (GDK) Core does from there. Use the [SpatialOS GDK for Unity: Contents](#documentation) documentation, listed below, to explore how the GDK Core works.

Note that the GDK documentation assumes that you are familiar with SpatialOS concepts and the Unity ECS.

We want your feedback on the GDK, its documentation and its [roadmap (Trello board)](https://trello.com/b/29tMKyQC) - see [Give us feedback](../README.md#give-us-feedback).

## Questions?
Check out our [FAQs](content/faqs/faqs.md) or head over to our [discussion forums](../README.md#give-us-feedback).

## Documentation

#### SpatialOS
The GDK documentation assumes you are familiar with SpatialOS concepts. For guidance on SpatialOS concepts see the documentation on the [SpatialOS website](https://docs.improbable.io/reference/latest/shared/concepts/spatialos).

#### Entity Component System
The GDK currently uses the `0.0.12-preview.11` version of the entities preview packages.
The GDK documentation assumes you are familiar with the Unity Entity Component System. See:
* This overview [YouTube video](https://www.youtube.com/watch?v=_U9wRgQyy6s) from [Brackeys](http://brackeys.com/).
* The more detailed documentation from Unity on the ECS [GitHub repository](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/index.md).

#### SpatialOS GDK for Unity

If you're new to the SpatialOS GDK for Unity, check out the docs under [Getting started](#getting-started) and [Concepts](#concepts-and-features), and then [Configuring your project](#configuring-your-project).

If you're already set up, you can learn about using the GDK with [GameObjects and MonoBehaviours](#using-the-gdk-with-gameobjects-and-monobehaviours) or with [Unity ECS](#using-the-gdk-with-unity-ecs).

##### Getting started
* [Installation and setup](setup-and-installing.md)
* [Building and deploying your game](content/build-and-deploy.md)

##### Frequently asked questions (FAQs)
* [FAQs](content/faqs/faqs.md)

##### Concepts and features
* [SpatialOS concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) (on the SpatialOS docs site)
* [SpatialOS glossary](https://docs.improbable.io/reference/latest/shared/glossary) (on the SpatialOS docs site)
* [Core Module and Feature Modules overview](content/ecs/core-and-feature-module-overview.md)
* [Snapshots](content/snapshots.md)

##### Configuring your project
* [Starting workers](content/workers.md)
* [Setting up a new project](content/set-up-new-project.md)

##### Using the GDK with GameObjects and MonoBehaviours
* [Setting up GameObjects for interaction with SpatialOS](content/gameobject/set-up-gameobjects.md)
* [Reading and writing component data](content/gameobject/reading-and-writing-component-data.md)
* [Sending and receiving events](content/gameobject/sending-receiving-commands.md)
* [Sending and receiving commands](content/gameobject/sending-receiving-events.md)
* [World commands](content/gameobject/world-commands.md)
* [Logging and accessing the worker](content/gameobject/logging-and-accessing-the-worker.md)

##### Using the GDK with Unity ECS
* [System update order](content/ecs/system-update-order.md)
* [Components and component updates](content/ecs/component-data.md)
* [Sending and receiving events](content/ecs/events.md)
* [Sending and receiving commands](content/ecs/commands.md)
* [Receiving updates from SpatialOS: Reactive components](content/ecs/reactive-components.md)
* [Custom replication systems](content/ecs/custom-replication-system.md)
* [Authority](content/ecs/authority.md)
* [Creating entities](content/ecs/create-entity.md)
* [Entity checkout process](content/ecs/entity-checkout-process.md)
* [The code generator](content/ecs/code-generator.md)
* [Temporary components](content/ecs/temporary-components.md)
* [Logging](content/ecs/logging.md)
* [Accessing information about the worker during runtime](content/ecs/accessing-worker-info.md)

##### Contributions
We are currently not accepting public contributions. However, we are accepting issues and we do
 want your [feedback](../README.md#give-us-feedback).
* [Contributions policy](../.github/CONTRIBUTING.md)
* [Coding standards](contributions/unity-gdk-coding-standards.md)

---
#### Warning
This [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release of the GDK Core is for evaluation and feedback purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

----
&copy; 2018 Improbable