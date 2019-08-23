# Overview

The SpatialOS GDK for Unity provides functionality to develop games in both of Unity's paradigms:

* Object-oriented: MonoBehaviours and GameObjects
* Data-oriented: Entity-Component-System (ECS)

You can stick to familiar MonoBehaviour-based development, check out the new ECS workflow, or try a combination of both.

> However you choose to develop your game, it is important to understand what [SpatialOS entities]({{urlRoot}}/reference/glossary#spatialos-entity) are and how they relate to each workflow.

## MonoBehaviour-centric workflow

In the GDK, SpatialOS entities can be linked to GameObjects by using the [Game Object Creation Feature Module]({{urlRoot}}/modules/game-object-creation/overview). [Readers and Writers]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/index) allow you to inspect and change the state of components on SpatialOS entities using MonoBehaviours that you add to their linked GameObjects.

You may find that not all SpatialOS entities need to be represented as GameObjects. The [Game Object Creation Feature Module]({{urlRoot}}/modules/game-object-creation/overview) allows you to customise the GameObject creation and linking process.

## ECS-centric workflow

ECS is an architectural pattern adopted by Unity in early 2018, allowing you to design your Unity game in a data-oriented fashion.

In the GDK, each SpatialOS entity checked out by a worker has a corresponding ECS entity. To inspect or change the state of components on SpatialOS entities, you must read or write to the components on corresponding ECS entities.

To learn more about ECS, check out:

* [Unity’s GitHub](https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/ECSSamples/Documentation)
* [DOTS: Entity Component System](https://blogs.unity3d.com/2019/03/08/on-dots-entity-component-system/)
* [What is the ECS?](https://improbable.io/blog/unity-ecs-1)
* [Choosing between the ECS and MonoBehaviours workflows](https://improbable.io/blog/unity-ecs-2)
