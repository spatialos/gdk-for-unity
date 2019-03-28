[//]: # (TODO: Add examples of additional data you might add)

<%(TOC)%>
# Connection flows

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

The SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) manages your game world by keeping track of all [SpatialOS entities]({{urlRoot}}/content/glossary#spatialos-entity) and the current state of their [components]({{urlRoot}}/content/glossary#spatialos-component).
To execute any kind of logic on these entities, we use [workers]({{urlRoot}}/content/glossary#worker). You can set up many different types of workers but the two main types are  [server-workers]({{urlRoot}}/content/glossary#server-worker) and [client-workers]({{urlRoot}}/content/glossary#client-worker).

(To find out more about workers in the GDK, see the introduction to [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk).)

It’s the workers (both server-workers and client-workers) which create a connection between your game and the SpatialOS Runtime. In order for them to do this, you need to set up the configuration of your worker types as part of your game development. Then, when your game runs, it creates worker instances which connect to the SpatialOS Runtime.  During their creation, worker instances attempt to connect to the SpatialOS Runtime. If the connection fails, the creation of the worker instance fails.

If you are using the [MonoBehaviour workflow or the ECS workflow]{{urlRoot}}/content/intro-workflows-spatialos-entities), you can use the [Worker Connector]({{urlRoot}}/content/gameobject/linking-spatialos-entities) or the `Worker` to set up your workers (server-workers and client-workers) to connect to SpatialOS. Upon successfully connecting to the SpatialOS Runtime, your worker stores a `Connection` object.


## Which connection flow to use

We provide two types of connection flow, depending on what kind of worker and what kind of [deployment]({{urlRoot}}/content/glossary#deploying) you want to connect to.
In all cases, your worker contains a reference to a `Connection` object after successfully connecting to the SpatialOS Runtime.

### Receptionist service connection flow

Use the Receptionist service connection flow in the following cases:

  * Connecting a server-worker or a client-worker instance to a local deployment.
  * Connecting server-worker instances to a cloud deployment.
  * The special case of connecting a client-worker instance to a cloud deployment from your Unity Editor for debugging. (Note you can also use the Locator to connect in this situation.)  

**Notes:**

*  You usually connect client-worker instances to a cloud deployment via the Locator connection flow (outlined below) but you may want to debug your client-worker instance from your Unity Editor In this case, you can either use the Receptionist service connection flow via `spatial cloud connect external <deploymentname>` or the Locator service connection flow via the development-authentication.  See the SpatialOS documentation to find out more about [`spatial cloud connect external <deploymentname>`](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-cloud-connect-external#spatial-cloud-connect-externall).
* For mobile development, you need to use the new v13.5+ Locator connection flow to launch client-workers to the cloud from your Unity Editor.


### Locator connection flow
From SpatialOS v13.5, there are two versions of the Locator connection. The new v13.5+ Locator, in alpha, has additional functionality to the existing v10.4+ Locator which is the stable version. 


### v10.4+ Locator connection flow (stable version)

Use this Locator service connection flow for:

 * Connecting a client-worker to a cloud deployment via the SpatialOS Launcher - [see SpatialOS documentation on the Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher)


### New v13.5+ Locator connection flow (alpha version)

Use this Locator service connection flow for:

* Connecting a client-worker to a cloud deployment via the SpatialOS Launcher - [see SpatialOS documentation on the Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher)
* Connecting a client-worker instance to a cloud deployment from your Unity Editor for debugging via the [development authentication functionality](https://docs.improbable.io/reference/13.5/shared/auth/development-authentication). (Note that you can also use the Receptionist to connect in this situation.)

## When to use the `Connection` object

Upon successfully connecting to the SpatialOS Runtime, your worker stores a `Connection` object.
Use this object for:

  * Accessing the ID of the worker.
  * Accessing the [worker flags]({{urlRoot}}/content/glossary#worker-flags).
  * accessing the used [worker attribute]({{urlRoot}}/content/glossary#worker-attribute).

## Protocol logging
You can use protocol logging to log additional data to the data your worker sends and receives while connected to the SpatialOS Runtime.
[//]: # (TODO: Add examples of additional data you might add)
[//]: # (EG: The additional data can be <X or Y> which you could use for <A or B>. It logs data to <WHERE?>)
This is disabled by default as the logs can get very big in size, very fast.
