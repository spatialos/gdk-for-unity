[//]: # (Doc of docs reference 30)
[//]: # (TODO: Add examples of additional data you might add)

# Connecting to the SpatialOS Runtime

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

The SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) manages your game world by keeping track of all SpatialOS entities and the current state of their components.
To execute any kind of logic on these entities, we use [workers]({{urlRoot}}/content/glossary#worker). We differentiate between [server-workers]({{urlRoot}}/content/glossary#server-worker) and [client-workers]({{urlRoot}}/content/glossary#client-worker).

(To find out more about workers in the GDK, see the introduction to [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk).)

It’s the workers which create a connection from your game to the SpatialOS Runtime. In order for them to do this, you need to set up the configuration of your workers as part of your game development. Then, when your game runs, it creates workers which connect to the SpatialOS Runtime.  During their creation, workers attempt to connect to the SpatialOS Runtime. If the connection fails, the creation of the worker fails.

If you are using the MonoBehaviour workflow or the ECS workflow, you can use the [Worker Connector]({{urlRoot}}/content/gameobject/linking-spatialos-entities) to get going with setting up your workers to connect to SpatialOS, along with the [Worker API]({{urlRoot}}/content/workers/api-worker).

(For information on the different workflows, see [MonoBehaviour workflow vs ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities).)

## Which connection flow to use

We provide two types of connection flow, depending on what kind of worker and what kind of [deployment]({{urlRoot}}/content/glossary#deploying) you want to connect to.
In all cases, your worker contains a reference to a `Connection` object after successfully connecting to the SpatialOS Runtime.

### Receptionist service connection flow
Use the Receptionist service connection flow in the following cases:
  * Connecting a server-worker or a client-worker to a local deployment.
  * Connecting server-workers to a cloud deployment.
  * The special case of connecting a client-worker to a cloud deployment from the Unity Editor for debugging.

**Note:** You usually connect client-workers to a cloud deployment via the Locator connection flow (outlined below) but you may want to use the Receptionist connection flow for debugging from your Unity Editor. In this case you use the Receptionist service connection flow via `spatial cloud connect external <deploymentname>`. See the SpatialOS documentation to find out more about [`spatial cloud connect external <deploymentname>`](https://docs.improbable.io/reference/13.3/shared/spatial-cli/spatial-cloud-connect-external#spatial-cloud-connect-externall).

### Locator connection flow
Use the Locator service connection flow
  * Connecting a client-worker to a cloud deployment via the SpatialOS Launcher - [see SpatialOS documentation on the Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher).

## How do I connect via the Receptionist service flow?

To connect via the Receptionist service connection flow, you need to pass in a `ReceptionistConfig` object when you call `Worker.CreateWorkerAsync` to create your worker.

The `ReceptionistConfig` class stores the following fields:

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| LinkProtocol| [NetworkConnectionType](https://docs.improbable.io/reference/latest/csharpsdk/api-reference#improbable-worker-networkconnectiontype-enum) | The type of networking to use (either TCP or [Raknet (Wikipedia link)](https://en.wikipedia.org/wiki/RakNet)). The default is Raknet. |
| ReceptionistHost| string | The host for connecting to the SpatialOS  [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) using the Receptionist service. The default is `127.0.0.1`. |
| ReceptionistPort| ushort | The port for connecting to the SpatialOS Runtime using the Receptionist service. The default is `7777`. |
| EnableProtocolLoggingAtStartup| bool | Specifies whether to enable [protocol logging](#what-is-protocol-logging). The default is `false`. |
| UseExternalIp| bool | If enabled, the worker won’t use the local IP to connect to the SpatialOS Runtime via the Receptionist service. You only need this when connecting client-workers to cloud deployments using the Receptionist service. The default is `false`. |
| WorkerId| string | The unique ID of the worker. |
| WorkerType| string | The type of the worker. |

## How to connect via the Locator service flow

To connect via the Locator service flow, you need to pass in a `LocatorConfig`  when calling `Worker.CreateWorkerAsync` to create your worker.
The `LocatorConfig` class stores the following fields:

| Field         	| Type 	| Description                        	|
|-------------------|----------|--------------------------------------|
| LinkProtocol| [NetworkConnectionType](https://docs.improbable.io/reference/latest/csharpsdk/api-reference#improbable-worker-networkconnectiontype-enum) | The type of networking that should be used (either TCP or [Raknet - Wikipedia link](https://en.wikipedia.org/wiki/RakNet)). The default is Raknet. |
| LocatorHost| string | The host for connecting to the SpatialOS  [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) using the Locator flow. The default is `locator.improbable.io`. You usually don’t need to change this. |
| LocatorParameters| [LocatorParameters](https://docs.improbable.io/reference/latest/csharpsdk/api-reference#improbable-worker-locatorparameters-class) | The parameters needed to connect using the Locator service flow. |
| EnableProtocolLoggingAtStartup| bool | Specifies whether to enable [protocol logging](#what-is-protocol-logging). The default is `false`.  |
| UseExternalIp| bool | If enabled, the worker won’t use the local IP to connect to the SpatialOS Runtime via the Receptionist service. You only need this when connecting client-workers to cloud deployments using the Receptionist service. The default is `true`. |
| WorkerId| string | The unique ID of the worker. |
| WorkerType| string| The [type of the worker]({{urlRoot}}/content/glossary#worker-types) |

## When to use the `Connection` object
Upon successfully connecting to the SpatialOS Runtime, your worker stores a `Connection` object.
Use this object for:
  * sending and receiving component updates and messages to and from the SpatialOS Runtime. This is done internally by the GDK and you shouldn't need to do it unless you create a [custom replication system]({{urlRoot}}/content/ecs/custom-replication-system)
  * accessing the ID of the worker
  * accessing the [worker flags]({{urlRoot}}/content/glossary#worker-flags)
  * accessing the used [worker attribute]({{urlRoot}}/content/glossary#worker-attribute)

## What is protocol logging?
You can use protocol logging to log additional data to the data your worker sends and receives while being connected to the SpatialOS Runtime.
[//]: # (TODO: Add examples of additional data you might add)
[//]: # (EG: The additional data can be <X or Y> which you could use for <A or B>. It logs data to <WHERE?>)
This is disabled by default as the logs can get very big in size, very fast.
