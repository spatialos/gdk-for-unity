<%(TOC)%>

# MonoBehaviours: Worker connectors

<%(Callout message="
Before reading this document, make sure you have read:

  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
  * [Connection flows]({{urlRoot}}/reference/concepts/connection-flows)
")%>

To expose the [`Worker`]({{urlRoot}}/api/core/worker) class in the MonoBehaviour workflow, the GDK provide an abstract [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) MonoBehaviour.

This class provides methods for connecting and managing the lifecycle of your worker-instance. You can derive from `WorkerConnector` and use those MonoBehaviours to create multiple server-worker or client-worker instances in one Scene by adding it to multiple GameObjects, each GameObject creating a different worker instance.

<%(#Expandable title="GameObject transform translation")%>
When you have multiple worker instances represented as GameObjects in a Scene, you are likely to have multiple SpatialOS entities [checked out]({{urlRoot}}/reference/glossary#authority) to each worker instance. To make sure you don’t have GameObjects that are linked to the same SpatialOS entity in different workers in the same location, we use an offset for each GameObject against the origin of the worker GameObject. We call the offset of the worker GameObject origin, the “translation”.
<%(/Expandable)%>

## How to create your own WorkerConnector

You can inherit from the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) class to create your own connection logic.

<%(#Expandable title="Example worker connector implementation")%>

```csharp
public class ClientWorkerConnector : WorkerConnector
{
    private async void Start()
    {
        // Create the default connection parameters.
        var connParams = CreateConnectionParameters(WorkerType);

        // Change the network protocol to Kcp for this worker.
        connParams.Network.ConnectionType = NetworkConnectionType.Kcp;

        // Create the builder and set the connection parameters
        var builder = new SpatialOSConnectionHandlerBuilder()
            .SetConnectionParameters(connParams);

        if (!Application.isEditor)
        {
            // If we aren't in the Editor, I want to configure my connection via the command line.
            var initializer = new CommandLineConnectionFlowInitializer();

            // There are different connection flows for different scenarios.
            switch (initializer.GetConnectionService())
            {
                case ConnectionService.Receptionist:
                    builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType), initializer));
                    break;
                case ConnectionService.Locator:
                    builder.SetConnectionFlow(new LocatorFlow(initializer));
                    break;
                case ConnectionService.AlphaLocator:
                    builder.SetConnectionFlow(new AlphaLocatorFlow(initializer));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            // If we are in the Editor, simply connect via the Receptionist.
            builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType)));
        }

        // Asynchronously connect to the Runtime.
        await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
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
        // Need to call base.Dispose() to properly clean up
        // the ECS world and the connection.
        base.Dispose();
    }
}
```

<%(/Expandable)%>

## How to modify the connection configuration

There are a number of parameters to consider when configuring the connection. The GDK provides a small framework for composing these parameters, which you can then extend to build reusable configurations.

The `SpatialOSConnectionHandlerBuilder` object is central to this framework. It allows you to declare which connection flow to use, what parameters the connection should use, and how these parameters are populated.

### What connection parameters should I use

The [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector) class has a `CreateConnectionParameters(string workerType)` method which returns reasonable defaults for your `ConnectionParameters`. You can then tweak these to fit your use case. For example:

- If you are implementing a client worker, you may want to use the `KCP` networking stack.
- If you are debugging the low level networking of a worker, you may want to enable [protocol logging](https://docs.improbable.io/reference/latest/shared/debugging#protocol-logging).

### How do I populate my connection flow parameters

Each of the [connection flows]({{urlRoot}}/reference/concepts/connection-flows) have parameters that need to be configured to ensure a successful connection.

The GDK provides the `IConnectionFlowInitializer<TConnectionFlow>` interface as part of the small framework mentioned above. This interface represents an object which knows how to initialize a connection flow of type `TConnectionFlow`. The GDK provides two implementations:

- `CommandLineConnectionFlowInitializer` which can initialize the `ReceptionistFlow,` `LocatorFlow`, and `AlphaLocatorFlow` from command line parameters.
- `MobileConnectionFlowInitializer` which can initialize the `ReceptionistFlow` and `AlphaLocatorFlow`.

## How do I customize the connection flows

The Locator and AlphaLocator connection flows both have callbacks to the user during the connection process. The `LocatorFlow` and `AlphaLocatorFlow` objects provide a reasonable default implementation for these callbacks, but you may want to customize this to suit your connection & authentication logic. 

To customize these flows, simply inherit from the relevant flow object and override the methods that you want to tweak. Please refer to the API documentation for more detail on the [`LocatorFlow`]({{urlRoot}}/api/core/locator-flow) and [`AlphaLocatorFlow`]({{urlRoot}}/api/core/alpha-locator-flow).

## Example implementation

In the GDK’s [Blank project](https://github.com/spatialos/gdk-for-unity-blank-project), we provide an example implementation of a server-worker, client-worker, and mobile client-worker derived from the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector). There are three example scripts and four example Scenes.

### Scripts

The scripts are stored inside `workers/unity/Assets/Scripts/Workers`:

* `UnityClientConnector`: This is a sample implementation to connect as a client-worker on Windows or MacOS. (Note that the client-worker is sometimes called `UnityClient`.)
* `UnityGameLogicConnector`: This is a sample implementation to connect as a server-worker.
* `MobileClientWorkerConnector`: This is a sample implementation to connect as a client-worker on an Android or iOS device.

### Scenes

* `ClientScene`: This Scene contains only the `ClientWorker` GameObject which you can use to build a client-worker instance for cloud deployments (for a game client on Windows or MacOS)
* `MobileClientScene`: This Scene contains only the `ClientWorker` GameObject which you can use to build your client-worker for local and cloud deployments (for a game client on Android or iOS).
* `GameLogicScene`: This Scene contains only the `GameLogicWorker` GameObject which you can use to build a server-worker instance for cloud deployments (as it’s a server-worker instance, the game client platform you are using is irrelevant).
* `DevelopmentScene`: This Scene contains both the `ClientWorker` GameObject representing the client-worker instance (on Windows or MacOS) and the `GameLogicWorker` GameObject representing the server-worker instance and starts both worker instances as soon as you load the Scene.