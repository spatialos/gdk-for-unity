[//]: # (Doc of docs reference 15.a)
[//]: # (TODO - find out whether WorkerSystem is ECS and how it fits into a generic workflow.)
<%(TOC)%>
# Workers: API - Worker

_This document relates to both [MonoBehaviour and  ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)_

Before reading this document, see the documentation on [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk) and [Connecting to the SpatialOS Runtime]({{urlRoot}}/content/connecting-to-spatialos).

We provide the `Worker` class to bootstrap the creation of your workers.

Note that the connection between your game and the SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime), depends on the successful creation of your workers.  During their creation, workers attempt to connect to the SpatialOS Runtime. Only a successful connection leads to the creation of a worker.

Upon successfully connecting to the SpatialOS Runtime and creating your worker and its corresponding [ECS world]({{urlRoot}}/content/glossary#unity-ecs-world), the following systems are added by the `Worker` class your [worker’s world]({{urlRoot}}/content/glossary#worker-s-world). These systems  ensure that any changes in any SpatialOS entity is correctly synchronized between the SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) and the [worker's view]({{urlRoot}}/content/glossary#worker-s-view).

## Systems ensuring synchronization between the Runtime and workers

  * [WorkerSystem]({{urlRoot}}/content/workers/api-worker-system) - A system storing the worker information for easy access from any system in the same ECS world.

  * `SpatialOSSendSystem` - A system that sends all pending ECS component updates, events and commands to the SpatialOS Runtime on every update.

  * `SpatialOSReceiveSystem` - A system that receives all ECS component updates, events and commands from the SpatialOS Runtime on every update.

  * `CleanReactiveComponentsSystem` - A system that cleans up all [reactive components]({{urlRoot}}/content/ecs/reactive-components) and [temporary components]({{urlRoot}}/content/ecs/temporary-components) on every update.

  * `WorldCommandsCleanSystem` - A system that cleans up all responses received by using [World commands]({{urlRoot}}/content/ecs/world-commands) on every update.

  * `WorldCommandsSendSystem` -  A system that sends all pending world commands on every update.

  * `CommandRequestTrackerSystem` - A system that keeps track of all command requests that have been sent but a response has not been received.

  * `WorkerDisconnectCallbackSystem` - A system that triggers an `OnDisconnect` callback when the worker is disconnected.

##### Add definitions of your worker

To add more systems directly to the worker after it is already instantiated, you can use the following code snippet:

**Example: adding definitions to your worker**

```csharp
worker.World.GetOrCreateManager<YourSystem>();

// after adding all additional systems, ensure you add the following line
// to make sure all existing ECS worlds are correctly updated
ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
```

You can use the following fields, event callbacks, and methods, with `worker.World.GetOrCreateManager<YourSystem>()`.

**Fields**

| Field             | Type                   | Description                    |
|-------------------|------------------------|--------------------------------|
| Connection    | [Connection]({{urlRoot}}/content/connecting-to-spatialos) | The connection to the SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime). You can use it to send data and messages. |
| World         | `World`                  | The ECS world that this worker is associated with. |
| WorkerId      | `string`                 | The ID of this worker. |
| WorkerType    | `string`                 | The [type of this worker]({{urlRoot}}/content/glossary#worker-types). |
| Origin        | `Vector3`                | The vector by which we translate all ECS entities added to a worker. This is useful when running multiple workers in the same Scene. You can choose to set a [worker origin]({{urlRoot}}/content/glossary#worker-origin) to be large enough so that entities that are visible to or checked out by different workers don’t interact with each other. |
| LogDispatcher | `ILogDispatcher`         | A reference to the [logger]({{urlRoot}}/content/ecs/logging) that you can use to send logs to the Unity Console and the SpatialOS Runtime. |

**Events**

```csharp
event Action<string> OnDisconnect;
```

Register to this callback to get notified when the worker gets disconnected from the SpatialOS Runtime.

Callback parameters:

  * `string`: Contains the reason for the disconnect.

**Methods**

```csharp
static async Task<Worker> CreateWorkerAsync(ReceptionistConfig config, ILogDispatcher logger, Vector3 origin);
```

Parameters:

  * `ReceptionistConfig config`: The connection configuration used to connect via the [Receptionist]({{urlRoot}}/content/connecting-to-spatialos#receptionist-service-connection-flow).

  * `ILogDispatcher logger`: A reference to the `ILogDispatcher` object that the worker uses for [logging]({{urlRoot}}/content/ecs/logging).

  * `Vector3 origin`: The origin of this worker.

Returns: a `Task` (see [the Microsoft documentation on `Task`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=netframework-4.7.2))  that returns a `Worker` object on completion.

Throws: a `ConnectionFailedException` if it fails in creating a connection and therefore fails to instantiate a `Worker` object.

```csharp
static async Task<Worker> CreateWorkerAsync(LocatorConfig config, Func<DeploymentList, string> deploymentListCallback, ILogDispatcher logger, Vector3 origin);
```

Parameters:

* `LocatorConfig config`: The connection configuration used to connect to the [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) via the [Locator]({{urlRoot}}/content/connecting-to-spatialos#locator-connection-flow) flow.

* `Func<DeploymentList, string> deploymentListCallback`: The callback used to retrieve the correct [deployment]({{urlRoot}}/content/glossary#deploying) name given a list of deployments.

* `ILogDispatcher logger`: A reference to the `ILogDispatcher` object that object that the worker uses for [logging]({{urlRoot}}/content/ecs/logging).

* `Vector3 origin`: The origin of this worker.

Returns: a `Task` (see [the Microsoft documentation on `Task`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=netframework-4.7.2)) that will upon completion return a `Worker` object

Throws: a `ConnectionFailedException`, if it fails in creating a connection and therefore fails to instantiate a `Worker` object.
