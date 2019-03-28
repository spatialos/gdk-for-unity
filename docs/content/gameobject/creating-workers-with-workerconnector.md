<%(TOC)%>
# Creating workers
_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

First see the documentation on:

* [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)

To demonstrate the use of the `Worker` class, the GDK contains an example implementation
of how to create a worker instance (server-worker or client-worker) as a `Worker` object and connect to SpatialOS. We provide an abstract `WorkerConnector` class and a `DefaultWorkerConnector` child class implementing the abstract methods to get you started quickly. You can extend it further by creating classes which inherit from it.
The `WorkerConnector` is a MonoBehaviour script. You can use it to create multiple server-worker or client-worker instances in one Scene by adding it to multiple GameObjects, each GameObject creating a different worker instance.

**ECS entity translation** <br/>
When you have multiple worker instances represented as GameObjects in a Scene, you are likely to have multiple ECS entities [checked out]({{urlRoot}}/content/glossary#authority) to each worker instance/GameObject. To make sure you don’t have all the ECS entities which are checked out to a worker instance/GameObject in the same (x, y, z) location, we use an offset for each ECS entity against the origin of the worker instance/GameObject.  We call the offset of the worker instance/GameObject origin, the “translation”.


## How to use worker prefabs

In the GDK’s [Blank project](https://github.com/spatialos/gdk-for-unity-blank-project), we provide an example implementation of a server-worker and client-worker on different platforms connecting using the `WorkerConnector`. There are four example scripts and five example Scenes.

#### Scripts
The scripts are stored inside `workers/unity/Assets/Scripts/Workers`:

* `UnityClientConnector`: This is a sample implementation to connect as a client-worker on Windows or MacOS. (Note that the client-worker is sometimes called `UnityClient`.)
* `UnityGameLogicConnector`: This is a sample implementation to connect as a server-worker.
* `AndroidClientWorkerConnector`: This is a sample implementation to connect as a client-worker on an Android device.
* `iOSClientWorkerConnector`: This is a sample implementation to connect as a client-worker on an iOS device.

#### Scenes

* `ClientScene`: This Scene contains only the `ClientWorker` GameObject which you can use to build a client-worker instance for cloud deployments (for a game client on Windows or MacOS)
* `AndroidClientScene`: This Scene contains only the `AndroidClient` GameObject which you can use to build your client-worker for local and cloud deployments (for a game client on Android).
* `iOSClientScene`: This Scene contains only the `iOSClient` GameObject which you can use to build your client-worker for local and cloud deployments (for a game client on iOS).
* `GameLogicScene`: This Scene contains only the `GameLogicWorker` GameObject which you can use to build a server-worker instance for cloud deployments (as it’s a server-worker instance, the game client platform you are using is irrelevant).
* `DevelopmentScene`: This Scene contains both the `ClientWorker` GameObject representing the client-worker instance and the `GameLogicWorker` GameObject representing the server-worker instance and starts both worker instances as soon as you load the Scene.

## How to create your own WorkerConnector
You can inherit from the `WorkerConnector` class to create your own connection logic, dependent on the [type of the worker]({{urlRoot}}/content/glossary#worker-types) that you want to create.

**Example**</br>
Showing what your implementation, inheriting from `DefaultWorkerConnector`, could look like.

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

When inheriting from the `WorkerConnector`, you can override `GetAlphaLocatorConfig`, `GetLocatorConfig` and
`GetReceptionistConfig` to modify the connection configuration a worker type uses to connect to the
Locator or Receptionist. See [Connecting to SpatialOS]({{urlRoot}}/content/connecting-to-spatialos) to find out more about the connection flows for client-workers and server-workers.

**Example** </br>
Overriding the Receptionist connection flow configuration.

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

## How to decide which connect service to use
First, see [Connecting to SpatialOS]({{urlRoot}}/content/connecting-to-spatialos) to find out more about the connection flows for client-workers and server-workers).

The `WorkerConnector` provides a default implementation which automatically decides which connection flow to use based on whether the application is running in your Unity Editor and whether it can find a login token. You can change the behavior by overriding the `GetConnectionService` method.

**Example** </br>
Overriding which connection service to choose.

```csharp
protected override ConnectionService GetConnectionService()
{
  // Your worker will always connect via the Receptionist.
  // This is the expected behavior for any server-worker.
  return ConnectionService.Receptionist;
}
```

## How to connect using the development authentication flow
We provide an integration with the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance) that you can use to connect your client-workers to a cloud deployment. This is especially important when working with [client-workers for mobile devices]({{urlRoot}}/content/mobile/overview). 

The `MobileWorkerConnector` already provides an implementation to use the development authentication flow connecting via the [new v13.5+ Locator connection flow]({{urlRoot}}/content/connecting-to-spatialos#locator-connection-flow).

However, you can use the development authentification but override using the new Locator, using the example code below.

**Example** </br>
How to override use the development authentication flow without using the new v13.5+ Locator connection flow configuration.

```csharp
protected override AlphaLocatorConfig GetAlphaLocatorConfig(string workerType)
{
  var pit = GetDevelopmentPlayerIdentityToken(DevelopmentAuthToken, GetPlayerId(), GetDisplayName());
  var loginTokenDetails = GetDevelopmentLoginTokens(workerType, pit);
  var loginToken = SelectLoginToken(loginTokenDetails);

  return new AlphaLocatorConfig
  {
    LocatorHost = RuntimeConfigDefaults.LocatorHost,
    LocatorParameters = new Worker.CInterop.Alpha.LocatorParameters
    {
      PlayerIdentity = new PlayerIdentityCredentials
      {
        PlayerIdentityToken  = pit,
        LoginToken = loginToken,
      },
      UseInsecureConnection = false,
    }
  };
}
```

