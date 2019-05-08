<%(TOC)%>

# Creating workers

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/overview)._

<%(Callout message="
Before reading this document, make sure you have read:

  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

To demonstrate the use of the [`Worker`]({{urlRoot}}/api/core/worker) class, the GDK contains an example implementation of how to create a worker instance (server-worker or client-worker) and connect to SpatialOS. We provide an abstract [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) class and two child classes implementing the abstract methods to get you started quickly:

  * The [`DefaultWorkerConnector`]({{urlRoot}}/api/core/default-worker-connector) for your server-workers or client-workers on Windows or MacOS.
  * The [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) for your client-workers on Android or iOS.

The [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) is a MonoBehaviour script. You can use it to create multiple server-worker or client-worker instances in one Scene by adding it to multiple GameObjects, each GameObject creating a different worker instance.

**ECS entity translation** <br/>
When you have multiple worker instances represented as GameObjects in a Scene, you are likely to have multiple ECS entities [checked out]({{urlRoot}}/reference/glossary#authority) to each worker instance/GameObject. To make sure you don’t have all the ECS entities which are checked out to a worker instance/GameObject in the same (x, y, z) location, we use an offset for each ECS entity against the origin of the worker instance/GameObject.  We call the offset of the worker instance/GameObject origin, the “translation”.

## How to use worker prefabs

In the GDK’s [Blank project](https://github.com/spatialos/gdk-for-unity-blank-project), we provide an example implementation of a server-worker and client-worker on different platforms connecting using the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector). There are three example scripts and four example Scenes.

#### Scripts

The scripts are stored inside `workers/unity/Assets/Scripts/Workers`:

* `UnityClientConnector`: This is a sample implementation to connect as a client-worker on Windows or MacOS. (Note that the client-worker is sometimes called `UnityClient`.)
* `UnityGameLogicConnector`: This is a sample implementation to connect as a server-worker.
* `MobileClientWorkerConnector`: This is a sample implementation to connect as a client-worker on an Android or iOS device.

#### Scenes

* `ClientScene`: This Scene contains only the `ClientWorker` GameObject which you can use to build a client-worker instance for cloud deployments (for a game client on Windows or MacOS)
* `MobileClientScene`: This Scene contains only the `ClientWorker` GameObject which you can use to build your client-worker for local and cloud deployments (for a game client on Android or iOS).
* `GameLogicScene`: This Scene contains only the `GameLogicWorker` GameObject which you can use to build a server-worker instance for cloud deployments (as it’s a server-worker instance, the game client platform you are using is irrelevant).
* `DevelopmentScene`: This Scene contains both the `ClientWorker` GameObject representing the client-worker instance (on Windows or MacOS) and the `GameLogicWorker` GameObject representing the server-worker instance and starts both worker instances as soon as you load the Scene.

## How to create your own WorkerConnector

You can inherit from the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) class to create your own connection logic, dependent on the [type of the worker]({{urlRoot}}/reference/glossary#worker-types) that you want to create.

**Example**</br>
Showing what your implementation, inheriting from the [`DefaultWorkerConnector`]({{urlRoot}}/api/core/default-worker-connector), could look like.

```csharp
public class ClientWorkerConnector : DefaultWorkerConnector
{
    private async void Start()
    {
        // Try to connect as a worker of type UnityClient.
        await Connect("UnityClient", new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override string SelectDeploymentName(DeploymentList deployments)
    {
        // Return the name of the first active deployment that you can find.
        return deployments.Deployments[0].DeploymentName;
    }

    protected override void HandleWorkerConnectionEstablished()
    {
        // Add all the systems that you want on your workers.
        Worker.World.GetOrCreateManager<YourSystem>();
    }

    protected override void HandleWorkerConnectionFailure()
    {
        Debug.Log("Failed to create worker.");
    }

    public override void Dispose()
    {
        // Need to call base.Dispose() to properly clean up.
        // The ECS world and the connection.
        base.Dispose();
    }
}
```

## How to modify the connection configuration

When inheriting from the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector), you can override `GetAlphaLocatorConfig`, `GetLocatorConfig` and
`GetReceptionistConfig` to modify the connection configuration a worker type uses to connect to the
Locator or Receptionist. See [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows) to find out more about the connection flows for client-workers and server-workers.

## How to decide which connect service to use

First, see [Connecting to SpatialOS]({{urlRoot}}/reference/concepts/connection-flows) to find out more about the connection flows for client-workers and server-workers).

The [`DefaultWorkerConnector`]({{urlRoot}}/api/core/default-worker-connector) provides a default implementation on how to decide which connection flow to use based on the received command line arguments. You can change the behavior by overriding the `GetConnectionService` method.

## How to connect using the development authentication flow

We provide an integration with the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance) that you can use to connect your client-workers to a cloud deployment. This is especially important when working with [client-workers for mobile devices]({{urlRoot}}/reference/mobile/overview) or when you want your client to be able to choose the deployment it connects to.

The [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) provides an example implementation on how to use the development authentication flow connecting via the [new v13.5+ Locator connection flow]({{urlRoot}}/reference/concepts/connection-flows#locator-connection-flow).