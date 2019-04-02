[//]: # (Doc of docs reference 16+27)

<%(TOC)%>
# Which workflow

When you use SpatialOS with Unity, you have the option of two workflows: the traditional MonoBehaviour workflow using GameObjects or the ECS workflow. You can also choose to use a mixture of the two workflows.

> ECS is an alternative Unity development paradigm, introduced by Unity in early 2018. Find out more on [Unity’s GitHub](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/workflows/ecs_in_detail.md#world).

[SpatialOS entities]({{urlRoot}}/reference/glossary#spatialos-entity) are the objects in your [SpatialOS world]({{urlRoot}}/reference/glossary#spatialos-world); how you create them depends on which workflow you are using.

## The two workflows

Most Unity game developers working with SpatialOS get started with the MonoBehaviour workflow, and then optimize their game with the Unity ECS workflow.

By representing what would traditionally be GameObjects in your game as ECS entities, you can speed up your game. GameObjects have a lot of data associated with them so by only using an ECS entity to represent them you store the only the exact data you need without having the overhead of GameObjects.

You can find out more about the benefits of using the ECS workflow in our blogpost: [What is ECS?](https://improbable.io/games/blog/unity-ecs-1)

This GDK documentation covers  how to create a SpatialOS game with Unity whether you are using an ECS workflow or a MonoBehaviour workflow and there are sections for each development approach.
The sections on _getting started_, _configuring your project_, and  _tools_ apply to both workflows.

> Whichever workflow you use, it’s essential to know what SpatialOS entities are, how you create them, and their relationship with GameObjects and ECS entities.

## SpatialOS entities

SpatialOS entities are the objects in your [SpatialOS world]({{urlRoot}}/reference/glossary#spatialos-world). The instance of your game world running on cloud servers (or locally on your development computer during game development) is called "the Runtime".

All of the data that you want to share between servers and clients needs to be stored in SpatialOS entities, using their components.

>You can read more about SpatialOS entities and their components in the [SpatialOS concept documentation](https://docs.improbable.io/reference/latest/shared/concepts/world-entities-components) and in the [GDK glossary]({{urlRoot}}/reference/glossary#spatialos-entity).

In order to create a SpatialOS entity, you need to set up SpatialOS workers. Workers are programs which you create; they work together, executing logic on your game’s SpatialOS world. They perform the computation associated with a world: they can read what’s happening, watch for changes, and make changes of their own. It is these workers which create the SpatialOS entities.

>You can read more about SpatialOS workers in the [SpatialOS concept documentation](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing), the [GDK glossary]({{urlRoot}}/reference/glossary#worker) and the GDK documentation on [workers]({{urlRoot}}/reference/concepts/worker).  

## GameObjects and SpatialOS entities

When working with the SpatialOS GDK using the MonoBehaviour workflow, you can think of SpatialOS entities as approximating to Unity GameObjects. However, while traditional Unity development with GameObjects involves creating a GameObject first and then adding various GameObject components to it to give it the functionality you want, to create SpatialOS entities, you do something different. Instead, you first make a SpatialOS entity and add its data, the GDK then creates a Unity ECS entity to store all the data, and then it creates a GameObject to represent this in your game (if you want it to; you may not want it to, which we discuss below.)

So, you set up a worker and it is the worker which sends a request to the [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) to create a SpatialOS entity. Once the Runtime has created the SpatialOS entity, that worker receives a notification which triggers it to create a corresponding ECS entity. It can also create a corresponding GameObject, if you have set it up to do this. In your Unity Project, the ECS entity and any corresponding GameObject represent the SpatialOS entity.

It’s worth noting that, even if you use only the MonoBehaviour workflow, the GDK still uses ECS under the hood.

#### Not all SpatialOS entities are GameObjects

You may want to have some SpatialOS entities which are not represented as GameObjects in your game. These are likely to be SpatialOS entities which don't need to use the Unity physics system and don’t need to exist for your game player in a "physical" sense in the game world. These entities might be data for a leaderboard or data about time in your game.

These SpatialOS entities still have a Unity ECS entity representation but they aren’t also represented by GameObjects. The reason to keep these entities "GameObject-less" is to increase the efficiency of your game by reducing the data storage and transfer overhead associated with representing these as GameObjects when they don't need to be.

#### Not all GameObjects are SpatialOS entities

Similarly, you only want GameObjects represented as SpatialOS entities if they need to exist in your SpatialOS game world; that is GameObjects that you want to be omnipresent in your servers and multiple gameplayer clients.

So, players, NPCs or even background objects like trees, that you want to exist across your servers and gameplayer clients need to be represented as a SpatialOS entity as well as a GameObject.  But some GameObjects, such as those for visual or audio effects only will not have a SpatialOS entity to represent them in the SpatialOS game world.

#### Further information on setting up SpatialOS entities with GameObjects

In the MonoBehaviour workflow, to represent your SpatialOS entity as a GameObject, you need to setup the GameObject Creation Feature Module. See the following documentation for more information:

- [GameObject Creation Feature Module overview]({{urlRoot}}/modules/game-object-creation/overview)
- [GameObject Creation Feature Module basic setup up]({{urlRoot}}/modules/game-object-creation/standard-usage)

## Unity ECS entities and SpatialOS entities

When working with the SpatialOS GDK using the Unity ECS workflow, you can think of SpatialOS entities as approximating to Unity ECS entities.

As with the MonoBehaviour workflow, you create a SpatialOS entity first which the GDK then represents as a Unity ECS entity.

So, it is the workers which you have set up which request the Runtime to create a SpatialOS entity. This triggers the GDK to create an ECS entity;  so the ECS entity represents the SpatialOS entity in your Unity project.

So, you set up a worker and it is the worker which sends a request to the [Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) to create a SpatialOS entity. Once the Runtime has created the SpatialOS entity, that worker receives a notification which triggers it to create a corresponding ECS entity. In your Unity Project, the ECS entity represents the SpatialOS entity.

#### SpatialOS entities are always ECS entities

A SpatialOS entity is always represented by an ECS entity.

#### Not all ECS entities are SpatialOS entities

You only want ECS entities represented as SpatialOS entities if they need to exist in your SpatialOS game world; that is ECS entities that you want to be omnipresent in your servers and multiple gameplayer clients. So, players, NPCs or even background objects like trees, that you want to exist across your servers and gameplayer clients need to be represented as a SpatialOS entity as well as an ECS entity.  But some ECS entities, such as player settings (volume, the key mapping of a controller) do not need a SpatialOS entity to represent them in the SpatialOS game world.

In a battle royale game, for example,  you could have an offline lobby, in which your game player is waiting to be matched to other players. This lobby could be a complete map with NPCs, all stored in ECS entities. As none of this needs to be synchronized with other game players, so you don’t need to represent these ECS entities as SpatialOS entities.

#### Further information on setting up SpatialOS entities and ECS entities

In the ECS workflow, to represent your SpatialOS entity as an ECS entity, you need to set up your workers to create SpatialOS entities using entity templates.  See the following documentation for more information:

* Both workflows - [SpatialOS entities: Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)

* Both workflows - [SpatialOS entities: How to create and delete SpatialOS entities]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/create-delete-spatialos-entities)
