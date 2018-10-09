[//]: # (Doc of docs reference 15.a)
[//]: # (TODO - find out whether WorkerSystem is ECS and how it fits into a generic workflow.)
# Workers: API - Worker

_This document relates to both [GameObject-MonoBehaviour and  ECS workflows](../intro-workflows-spos-entities.md)_

Before reading this document, see the documentation on [workers in the GDK](workers-in-the-gdk.md) and [Connecting to the SpatialOS Runtime](connecting-to-spos.md).

We provide the `Worker` class to bootstrap the creation of your workers.

Note that the connection between your game and the SpatialOS [Runtime](../glossary.md#spatialos-runtime), depends on the successful creation of your workers.  During their creation, workers attempt to connect to the SpatialOS Runtime. Only a successful connection leads to the creation of a worker.

Upon successfully connecting to the SpatialOS Runtime and creating your worker and its corresponding [ECS world](../glossary.md#unity-ecs-world), the following systems are added by the `Worker` class your [worker’s world](../glossary.md#workers-world). These systems  ensure that any changes in any SpatialOS entity is correctly synchronized between the SpatialOS [Runtime](../glossary.md#spatialos-runtime) and the [worker's view](../glossary.md#workers-view).

## Systems ensuring synchronization between the Runtime and workers

  * [WorkerSystem](./api-workers-system.md) - A system storing the worker information for easy access from any system in the same ECS world.

  * SpatialOSSendSystem - A system that sends all pending ECS component updates, events and commands to the SpatialOS Runtime on every update.

  * SpatialOSReceiveSystem - A system that receives all ECS component updates, events and commands from the SpatialOS Runtime on every update.

  * CleanReactiveComponentsSystem - A system that cleans up all [reactive components](ecs/reactive-components.md) and [temporary components](ecs/temporary-components.md) on every update.

  * WorldCommandsCleanSystem - A system that cleans up all responses received by using [World commands](ecs/world-commands.md) on every update.

  * WorldCommandsSendSystem -  A system that sends all pending world commands on every update.

  * CommandRequestTrackerSystem - A system that keeps track of all command requests that have been sent but a response has not been received.

  * WorkerDisconnectCallbackSystem - A system that triggers an `OnDisconnect` callback when the worker is disconnected.

##### Add definitions of your worker

To add more systems directly to the worker after it is already instantiated, you can use the following code snippet:

**Example: adding definitions to your worker**

```csharp
worker.World.GetOrCreateManager<YourSystem>();

// after adding all additional systems, ensure you add the following line
// to make sure all existing ECS worlds are correctly updated
ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
```

You can use the following fields, event callbacks, and methods, with `worker.World.GetOrCreateManager<YourSystem>()`

**Fields**</br>

| Field         	| Type               	| Description                	|

|-------------------|------------------------|--------------------------------|

| Connection	| [Connection](../connecting-to-spos.md) | The connection to the SpatialOS [Runtime](../glossary.md#spatialos-runtime). You can use it to send data and messages. |

| World     	| World              	| The ECS world that this worker is associated with. |

| WorkerId  	| string             	| The ID of this worker. |

| WorkerType	| string             	| The [type of this worker](../glossary.md#type-of-worker). |

| Origin    	| Vector3            	| The vector by which we [translate](../glossary.md#translate) all ECS entities added to a worker. This is useful when running multiple workers in the same scene. You can choose to set a worker origin to be large enough so that entities that are visible to or checked out by different workers don’t interact with each other. |

| LogDispatcher | ILogDispatcher     	| A reference to the [logger](../ecs/logging.md) that you can use to send logs to the Unity Console and the SpatialOS Runtime. |

** Events ** </br>

```csharp

event Action<string> OnDisconnect;

```

Register to this callback to get notified when the worker gets disconnected from the SpatialOS Runtime.

Callback parameters:

  * `string`: Contains the reason for the disconnect.

** Methods ** </br>

```csharp

static async Task<Worker> CreateWorkerAsync(ReceptionistConfig config, ILogDispatcher logger, Vector3 origin);

```

Parameters:

  * `ReceptionistConfig config`: The connection configuration used to connect via the [Receptionist](../connecting-to-spos.md).

  * `ILogDispatcher logger`: A reference to the `ILogDispatcher` object that the worker uses for [logging](../ecs/logging.md).

  * `Vector3 origin`: The origin of this worker.

Returns: a `Task` (see the Microsoft documentation on [`Task`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=netframework-4.7.2)  that returns a `Worker` object on completion.

Throws: a `ConnectionFailedException` if it fails in creating a connection and therefore fails to instantiate a `Worker` object.

```csharp

static async Task<Worker> CreateWorkerAsync(LocatorConfig config, Func<DeploymentList, string> deploymentListCallback, ILogDispatcher logger, Vector3 origin);

```

Parameters:

* `LocatorConfig config`: The connection configuration used to connect to the [Runtime](../glossary.md#spatialos-runtime) via the [Locator](../connecting-to-spos.md) flow.

* `Func<DeploymentList, string> deploymentListCallback`: The callback used to retrieve the correct [deployment](../glossary.md#deploying) name given a list of deployments.

* `ILogDispatcher logger`: A reference to the `ILogDispatcher` object that object that the worker uses for [logging](../ecs/logging.md].

* `Vector3 origin`: The origin of this worker.

Returns: a `Task` (see the Microsoft documentation on [`Task`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=netframework-4.7.2) that will upon completion return a `Worker` object

Throws: a `ConnectionFailedException`, if it fails in creating a connection and therefore fails to instantiate a `Worker` object.
