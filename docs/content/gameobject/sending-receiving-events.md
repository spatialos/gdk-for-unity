[//]: # (Doc of docs reference 7)
[//]: # (TODO - Tech writer pass)

<%(TOC)%>
# Readers and Writers: Events

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities#spatialos-entities)._

Events are SpatialOS's equivalent of broadcasted messages. They allow you to send messages to all interested workers.

> For more information, see the documentation on [events](https://docs.improbable.io/reference/latest/shared/glossary#events).

We provide code-generated Readers and Writers for sending and receiving SpatialOS events. Before reading this document, make sure you are familiar with

* [Linking SpatialOS entities with GameObjects]({{urlRoot}}/content/gameobject/linking-spatialos-entities)
* [Reader and Writer]({{urlRoot}}/content/gameobject/readers-writers)
* [Read and write access]({{urlRoot}}/content/glossary#authority)
* [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)


We use the following [schema]({{urlRoot}}/content/glossary#schema) for all examples described in this documentation.
```
package playground;

type Location
{
    float x = 1;
    float y = 2;
    float z = 3;
}

component Bomb
{
    id = 12002;
    event Location explode;
}
```

This will generate the following classes in the `Playground` namespace:

  * `BombReader`
  * `BombWriter`

### How to send events

The following code snippet provides an example on how to send events. To send an event,  ensure that your worker has write authority over the corresponding SpatialOS component.

```csharp
using Improbable.Gdk.Subscriptions;
using Playground;

public class TriggerExplosion : MonoBehaviour
{
	[Require] private BombWriter bombWriter;

	private void Update()
	{
    	if (Input.GetKeyDown(KeyCode.Space))
    	{
        	bombWriter.SendExplodeEvent(new Location(1, 1, 1));
    	}
	}
}
```

### How to receive events

The following code snippet provides an example on how to receive events. Any worker that has your entity checked out and has read access over the corresponding SpatialOS component can receive events.

```csharp
using Improbable.Gdk.Subscriptions;
using Playground;

public class HandleExplosion : MonoBehaviour
{
	[Require] private BombReader bombReader;

	private void OnEnable()
	{
    	bombReader.OnChangeColorEvent += OnExplode;
	}

	private void OnDisable()
	{
    	// all registered callbacks are automatically deregistered when this script is disabled.
    	// Do not deregister your callback as it is an invalid operation.
	}

	private void OnExplode(Location location)
	{
    	// show the explosion
	}
}
```
