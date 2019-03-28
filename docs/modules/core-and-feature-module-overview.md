[//]: # (Doc of docs reference 11)
[//]: # (TODO - which module is ECS or GO-MB specific and which is generic?)

<%(TOC)%>
# Feature Modules Overview
_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/which-workflow)._

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

### Build system module

The `Improbable.Gdk.BuildSystem` module provides an example implementation of how to build your workers. It creates a menu item in your Unity Editor toolbar (menu: **SpatialOS** > **Build...** ) that you can use to easily build your workers. It also generates a [ScriptableObject (Unity documentation)](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) that contains the build configuration for each worker.  This functionality is only available from your Unity Editor.

### Player lifecycle module

To access this module, use the   `Improbable.Gdk.PlayerLifecycle` namespace. It contains members which you use to implement player spawning and player heartbeats.

`Improbable.Gdk.PlayerLifecycle` is in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle).

The module consists of:

* `PlayerLifecycleHelper.AddClientSystems(world, autoRequestPlayerCreation)` -  in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs).<br/>
Call this to implement the player lifecycle module, adding all the necessary client systems to your client-worker. It also provides the option to disable automatic player creation when the worker connects to SpatialOS.<br/>
Call this when you create your [worker]({{urlRoot}}/reference/concepts/worker).

* `PlayerLifecycleHelper.AddServerSystems(world)` -  in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs).<br/>
Call this to implement the player lifecycle module, adding all the necessary server systems to your server-worker.<br/>
Call this when you create your [worker]({{urlRoot}}/reference/concepts/worker).

* `AddPlayerLifecycleComponents(entityTemplate, workerId, clientAccess, serverAccess)` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs).<br/>
Call this to add the SpatialOS components used by the player lifecycle module to your entity.<br/>
Call this during [entity template creation]({{urlRoot}}/reference/concepts/entity-templates).

Find out more in the [Player lifecycle feature module]({{urlRoot}}/modules/player-lifecycle-feature-module) documentation.

### Transform synchronization module

To access this module, use the `Improbable.Gdk.TransformSynchronization` namespace. It is a basic implementation of synchronizing the position and rotation of SpatialOS entities between client-workers and server-workers.

`Improbable.Gdk.TransformSynchronization` is in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.transformsynchronization).

This module consists of:

* `TransformSynchronizationHelper.AddSystems(world)` is in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.transformsynchronization).<br/>
Call this to implement the transform synchronization module, adding all the necessary systems to all workers.

* `AddTransformSynchronizationComponents(entityTemplate, writeAccess, location = default(Vector3), velocity = default(Vector3))` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformSynchronizationHelper.cs).<br/>
Call this to add the SpatialOS components used by the transform synchronization module to your SpatialOS entity template. <br/>
You can optionally pass in a `rotation`, `location` or `velocity`.

Call this during [entity templates creation]({{urlRoot}}/reference/concepts/entity-templates).

Find out more in the [Transform synchronization feature module]({{urlRoot}}/modules/transform-feature-module) documentation.

### GameObject creation module

To access this module, use the `Improbable.Gdk.GameObjectCreation` namespace. It offers a default implementation of spawning GameObjects for your SpatialOS entities.

`Improbable.Gdk.GameObjectCreation` is in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.gameobjectcreation).

This module consists of:

* `IEntityGameObjectCreator` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.gameobjectcreation/IEntityGameObjectCreator.cs).
<br/>This is an interface to implement your customized version of the [Creation Feature Module]({{urlRoot}}/reference/workflows/monobehaviour/linking/spatialos-entities) which you use for creating GameObjects and [linking them to SpatialOS entities]({{urlRoot}}/reference/workflows/monobehaviour/linking/spatialos-entities).
<br/>See the documentation on [How to link SpatialOS entities with GameObjects]({{urlRoot}}/reference/workflows/monobehaviour/linking/spatialos-entities).

* `GameObjectCreationHelper.EnableStandardGameObjectCreation(world)` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.gameobjectcreation/GameObjectCreationHelper.cs).<br/>
Use this to enable the default implementation or with the parameters below to change the default.

* `GameObjectCreationHelper.EnableStandardGameObjectCreation(world,  entityGameObjectCreator)` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.gameobjectcreation/GameObjectCreationHelper.cs).<br/>
Use this enable custom spawning by passing in parameters to change the default.


Call these during [entity templates creation]({{urlRoot}}/reference/concepts/entity-templates).

### Mobile support module

To access this module, use the `Improbable.Gdk.Mobile` namespace. It offers support to connect mobile [client-workers]({{urlRoot}}/reference/glossary#client-worker) to SpatialOS.

`Improbable.Gdk.Mobile` is in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.mobile).

This module consists of:

* `MobileWorkerConnector` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.mobile).<br/>
Inherit from this class to define your custom mobile worker connectors.

* `Improbable.Gdk.Mobile.Android` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.mobile/Android).
<br/>It provides additional functionality that you might need when developing for Android.

* `Improbable.Gdk.Mobile.iOS` - in the repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.mobile/iOS).
<br/>It provides additional functionality that you might need when developing for iOS.

Find out more in the [Mobile support]({{urlRoot}}/reference/mobile/overview) documentation.
