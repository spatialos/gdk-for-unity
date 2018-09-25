# SpatialOS GDK for Unity documentation

If you would like an overview of the GDK please see the main [readme](../README.md).

The GDK documentation assumes you are familiar with SpatialOS concepts. For guidance see the [SpatialOS website](https://docs.improbable.io/reference/latest/shared/concepts/spatialos).

If you're new to the SpatialOS GDK for Unity, check out the docs under [Getting started](#getting-started) and [Concepts](#concepts-and-features), and then look at the Playground project included in the repo and get a feel for using the GDK with either [GameObjects and MonoBehaviours](#using-the-gdk-with-gameobjects-and-monobehaviours) or with the [Unity ECS](#using-the-gdk-with-unity-ecs).

Once you are comfortable with the GDK and want to start a separate project, look at [Configuring your project](#configuring-your-project).

##### Getting started
* [Installation and setup](setup-and-installing.md)
* [Building your workers](content/build.md)
* [Deploying your game](content/deploy.md)

##### Concepts and features
* [SpatialOS concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) (on the SpatialOS docs site)
* [SpatialOS glossary](https://docs.improbable.io/reference/latest/shared/glossary) (on the SpatialOS docs site)
* [Core Module and Feature Modules overview](content/ecs/core-and-feature-module-overview.md)
* [Snapshots](content/snapshots.md)
* [Creating entity templates](content/entity-templates.md)

##### Configuring your project

- [Starting workers](content/workers.md)
- [Setting up a new project](content/set-up-new-project.md)

##### Using the GDK with GameObjects and MonoBehaviours
* [Setting up GameObjects for interaction with SpatialOS](content/gameobject/set-up-gameobjects.md)
* [Reading and writing component data](content/gameobject/reading-and-writing-component-data.md)
* [Sending and receiving events](content/gameobject/sending-receiving-events.md)
* [Sending and receiving commands](content/gameobject/sending-receiving-commands.md)
* [World commands](content/gameobject/world-commands.md)
* [Retrieving your EntityId and accessing the worker](content/gameobject/retrieving-your-entityid-and-accessing-the-worker.md)

##### Using the GDK with Unity ECS
The GDK currently uses the `0.0.12-preview.13` version of the entities preview packages.
The GDK documentation assumes you are familiar with the Unity Entity Component System (ECS). 

ECS documentation:

* This overview [YouTube video](https://www.youtube.com/watch?v=_U9wRgQyy6s) from [Brackeys](http://brackeys.com/).
* The more detailed documentation from Unity on the ECS [GitHub repository](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/index.md).

GDK documentation:

* [System update order](content/ecs/system-update-order.md)
* [Components and component updates](content/ecs/component-data.md)
* [Sending and receiving events](content/ecs/events.md)
* [Sending and receiving commands](content/ecs/commands.md)
* [World commands](content/ecs/world-commands.md)
* [Receiving updates from SpatialOS: Reactive components](content/ecs/reactive-components.md)
* [Custom replication systems](content/ecs/custom-replication-system.md)
* [Authority](content/ecs/authority.md)
* [Entity checkout process](content/ecs/entity-checkout-process.md)
* [The code generator](content/ecs/code-generator.md)
* [Temporary components](content/ecs/temporary-components.md)
* [Logging](content/ecs/logging.md)
* [Accessing information about the worker during runtime](content/ecs/accessing-worker-info.md)

##### Frequently asked questions (FAQs)
* [FAQs](content/faqs/faqs.md)

---
#### Warning
This [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release of the GDK Core is for evaluation and feedback purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

----
&copy; 2018 Improbable