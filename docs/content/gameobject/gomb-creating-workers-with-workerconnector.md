[//]: # (Doc of docs reference 15.1)

# (GameObject-MonoBehaviour) Creating workers with WorkerConnector
_This document relates to the [GameObject-MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spos-entities)._

See first the documentation on:

* [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)  
* [Worker API]({{urlRoot}}/content/workers/api-worker)

To demonstrate use of the `Worker` class, the GDK contains an example implementation
of how to create your workers. We have implemented this in
the `WorkerConnector` class which you can extend further by creating classes which inherit from it.
The `WorkerConnector` is a MonoBehaviour script. You can use it to create multiple workers
in one Scene by adding it to multiple GameObjects, each GameObject creating a different worker.

When you have multiple workers represented as GameObjects in a scene, you are likely to have multiple ECS entities [checked out]({{urlRoot}}/content/glossary#authority) to that worker. To make sure you don’t have all the ECS entities which are checked out to a worker/GameObject in the same (x, y, z) location, we use an offset for each ECS entity against the origin of the worker/GameObject.  We call the offset of the worker/GameObject origin, the “translation”.


## How to use worker prefabs

In the GDK’s [`Playground` project](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Assets/Playground), we provide
an example implementation of a client-worker and a server-worker connecting using the `WorkerConnector`.
These are stored as prefabs, so that you can use them directly in your Scenes.
The prefabs are stored inside `Playground/Resources/Prefabs/Worker`:

  * `ClientWorker`: This prefab has the `ClientWorkerConnector` attached to it. This is a sample implementation to connect as a client-worker. (Note that the client-worker is sometimes called `UnityClient`.)
  * `GameLogicWorker`: This prefab has the `GameLogicWorkerConnector` attached to it. This is a sample implementation to connect as a server-worker.

We provide three sample Scenes:

* `SampleScene`: This Scene contains both the `ClientWorker` and the `GameLogicWorker` prefabs and starts both workers as soon as you load the scene.
* `ClientScene`: This Scene contains only the `ClientWorker` prefab which you can use to build your client-worker for cloud deployments.
* `GameLogicScene`: This Scene contains only the `GameLogicWorker` prefab which you can use to build your server worker for cloud deployments.


## How to create your own WorkerConnector
You can inherit from the `WorkerConnector `class to create your own connection logic,
dependent on the [type of the worker]({{urlRoot}}/content/glossary#worker-types) that you want to create.

**Example**</br>
Showing what your implementation, inheriting from `WorkerConnector`, could look like.

```csharp
public class ClientWorkerConnector : WorkerConnector
{
	private async void Start()
	{
    	// try to connect as a worker of type UnityClient
    	await Connect("UnityClient", new ForwardingDispatcher())
        	.ConfigureAwait(false);
	}

	protected override string SelectDeploymentName(DeploymentList deployments)
	{
    	// return the name of the first active deployment that you can find
    	return deployments.Deployments[0].DeploymentName;
	}

	protected override void HandleWorkerConnectionEstablished()
	{
    	// add all the systems that you want on your workers
    	Worker.World.GetOrCreateManager<YourSystem>();
	}

	protected override void HandleWorkerConnectionFailure()
	{
    	Debug.Log("Failed to create worker.");
	}

	public override void Dispose()
	{
    	// need to call base.Dispose() to properly clean up
    	// the ECS world and the connection
    	base.Dispose();
	}
}
```

## How to modify the connection configuration

When inheriting from the `WorkerConnector`, you can override `GetLocatorConfig` and
`GetReceptionistConfig` to modify the connection configuration used to connect to the
Locator or Receptionist. See [Connecting to SpatialOS]({{urlRoot}}/content/connecting-to-spos) to find out more about the connection flows for client-workers and server-workers.

**Example** </br>
Override the connection Receptionist connection flow configuration.

```csharp
protected override ReceptionistConfig GetReceptionistConfig(string workerType)
{
	return new ReceptionistConfig
	{
    	WorkerType = workerType,
    	WorkerId = CreateNewWorkerId(workerType),
    	UseExternalIp = UseExternalIp
	};
}
```

## How to decide whether to use the Locator or the Receptionist flow
(See [Connecting to SpatialOS]({{urlRoot}}/content/connecting-to-spos) to find out more about the connection flows for client-workers and server-workers)

The `WorkerConnector` provides a default implementation to decide which connection
flow to use based on whether the application is running in the Unity Editor and whether it
can find a login token. You can change the behavior by overriding the `ShouldUseLocator` method.

**Example** </br>
Overriding the Locator connection flow configuration.

```csharp
protected override bool ShouldUseLocator()
{
	// Your worker will always connect via the Receptionist.
	// This is the expected behaviour for any server-worker.
	return false;
}
```
