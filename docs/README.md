# SpatialOS GDK for Unity documentation

If you would like an overview of the GDK please see the main [readme](../README.md).

The GDK documentation assumes you are familiar with SpatialOS concepts. For guidance see the [SpatialOS website](https://docs.improbable.io/reference/latest/shared/concepts/spatialos).

If you're new to the SpatialOS GDK for Unity, check out the docs under [Getting started](#getting-started) and [Concepts](#concepts-and-features), and then look at the Playground project included in the repo and get a feel for using the GDK with either [GameObjects and MonoBehaviours](#using-the-gdk-with-gameobjects-and-monobehaviours) or with the [Unity ECS](#using-the-gdk-with-unity-ecs).

Once you are comfortable with the GDK and want to start a separate project, look at [Configuring your project](#configuring-your-project).

##### Getting started
* [Installation and setup]({{urlRoot}}/setup-and-installing)
* [Building your workers]({{urlRoot}}/content/build)
* [Deploying your game]({{urlRoot}}/content/deploy)

##### Concepts and features
* [SpatialOS concepts](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) (on the SpatialOS docs site)
* [SpatialOS glossary](https://docs.improbable.io/reference/latest/shared/glossary) (on the SpatialOS docs site)
* [Core Module and Feature Modules overview]({{urlRoot}}/content/ecs/core-and-feature-module-overview)
* [Snapshots]({{urlRoot}}/content/snapshots)
* [Creating entity templates]({{urlRoot}}/content/entity-templates)

##### Configuring your project

- [Starting workers]({{urlRoot}}/content/workers)
- [Setting up a new project]({{urlRoot}}/content/set-up-new-project)

##### Using the GDK with GameObjects and MonoBehaviours
* [Setting up GameObjects for interaction with SpatialOS]({{urlRoot}}/content/gameobject/set-up-gameobjects)
* [Reading and writing component data]({{urlRoot}}/content/gameobject/reading-and-writing-component-data)
* [Sending and receiving events]({{urlRoot}}/content/gameobject/sending-receiving-events)
* [Sending and receiving commands]({{urlRoot}}/content/gameobject/sending-receiving-commands)
* [World commands]({{urlRoot}}/content/gameobject/world-commands)
* [Retrieving your EntityId and accessing the worker]({{urlRoot}}/content/gameobject/retrieving-your-entityid-and-accessing-the-worker)

##### Using the GDK with Unity ECS
The GDK currently uses the `0.0.12-preview.13` version of the entities preview packages.
The GDK documentation assumes you are familiar with the Unity Entity Component System (ECS).

ECS documentation:

* This overview [YouTube video](https://www.youtube.com/watch?v=_U9wRgQyy6s) from [Brackeys](http://brackeys.com/).
* The more detailed documentation from Unity on the ECS [GitHub repository](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/index.md).

GDK documentation:

* [System update order]({{urlRoot}}/content/ecs/system-update-order)
* [Components and component updates]({{urlRoot}}/content/ecs/component-data)
* [Sending and receiving events]({{urlRoot}}/content/ecs/events)
* [Sending and receiving commands]({{urlRoot}}/content/ecs/commands)
* [World commands]({{urlRoot}}/content/ecs/world-commands)
* [Receiving updates from SpatialOS: Reactive components]({{urlRoot}}/content/ecs/reactive-components)
* [Custom replication systems]({{urlRoot}}/content/ecs/custom-replication-system)
* [Authority]({{urlRoot}}/content/ecs/authority)
* [Entity checkout process]({{urlRoot}}/content/ecs/entity-checkout-process)
* [The code generator]({{urlRoot}}/content/ecs/code-generator)
* [Temporary components]({{urlRoot}}/content/ecs/temporary-components)
* [Logging]({{urlRoot}}/content/ecs/logging)
* [Accessing information about the worker during runtime]({{urlRoot}}/content/ecs/accessing-worker-info)

##### Frequently asked questions (FAQs)
* [FAQs]({{urlRoot}}/content/faqs/faqs.md)

---
#### Warning
This [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release of the GDK Core is for evaluation and feedback purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

----
&copy; 2018 Improbable
