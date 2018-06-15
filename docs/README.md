# SpatialOS Unity GDK documentation

The Unity GDK will be made up of three parts: 

* the GDK Core, a performant, data-driven [SpatialOS](https://docs.improbable.io/reference/latest/shared/concepts/spatialos) multiplayer integration.

* a library of solutions for hard or common networked game development problems via Feature Modules.

* the example game, with shared source code which both tests and demonstrates the Feature Modules; for use as a starting point or resource for game development.

The GDK Core is based on the new Unity [Entity Component System (ECS)](https://unity3d.com/unity/features/job-system-ECS). For background on the Unity GDK's current development and plans, as well as our open development ethos, see our [Unity GDK deep-dive blogpost](https://improbable.io/games/blog/unity-gdk-deepdive-1).

#### Warning
This pre-alpha release of the GDK Core is for evaluation and feedback purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

#### How to get started
We recommend you start by following the [Installation and setup guide](setup-and-installing.md) to set up the GDK Core and its accompanying example scene, the Playground. You can then explore what the Unity GDK Core does from there. Use the [Unity GDK: Contents](#contents) documentation, listed below, to explore how the GDK Core works.

Note that the Unity GDK documentation assumes that you are familiar with SpatialOS concepts and the Unity ECS.

We want your feedback on the Unity GDK and its documentation - see [Give us feedback](../README.md#give-us-feedback).

## Documentation 

#### SpatialOS
The Unity GDK documentation assumes you are familiar with SpatialOS concepts. For guidance on SpatialOS concepts see the documentation on the [SpatialOS website](https://docs.improbable.io/reference/latest/shared/concepts/spatialos). 

#### Entity Component System
The Unity GDK documentation assumes you are familiar with the Unity Entity Component System. See:
* This overview [YouTube video](https://www.youtube.com/watch?v=_U9wRgQyy6s) from [Brackeys](http://brackeys.com/).
* The more detailed documentation from Unity on the ECS [GitHub repository](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/index.md).

#### Unity GDK

##### Getting started
* [Installation and setup](setup-and-installing.md)

##### Contributions
We are currently not accepting public contributions. However, we are accepting issues and we do
 want your [feedback](../README.md#give-us-feedback).
* [Contributions policy](https://github.com/spatialos/UnityGDK/blob/master/CONTRIBUTING.md)
* [Coding standards](contributions/unity-gdk-coding-standards.md)

##### Contents
* [Receiving updates from SpatialOS: Reactive components](content/reactive-components.md)
* [Authority](content/authority.md)
* [Sending and receiving events](content/events.md)
* [Sending and receiving commands](content/commands.md)
* [Custom replication systems](content/custom-replication-system.md)
* [The code generator](content/code-generator.md)
* [Creating entities](content/create-entity.md)
* [Entity checkout process](content/entity-checkout-process.md)

&copy; 2018 Improbable
