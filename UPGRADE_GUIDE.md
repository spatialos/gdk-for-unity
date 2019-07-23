# Upgrade Guide

## From `0.2.5` to `0.2.6`

### General changes

#### `SpatialOSEntity.TryGetComponent<T>(out T component)`

Where previously, you may have had code like:

```csharp
SpatialOSEntity entity; // Assume constructed correctly.

if (!entity.HasComponent<MyComponent>())
{
    return;
}

var myComponent = entity.GetComponent<MyComponent>();
```

You may now write:

```csharp
SpatialOSEntity entity; // Assume constructed correctly.

if (!entity.TryGetComponent<MyComponent>(out var myComponent))
{
    return;
}
```

#### `Dynamic` API changes

The `DynamicSnapshot` and `DynamicConverter` classes have been collapsed into `Dynamic`.

The delegates that were previously passed into the `Accept` method for the respective handler interface are now inside the `Dynamic.VTable<TData, TUpdate, TSnapshot>` struct. This means that you can still access the exact same methods as before through the `VTable`.

Before:

```csharp
private class MyDynamicHandler : DynamicSnapshot.ISnapshotHandler 
{
    public void Accept<T>(uint componentId, DynamicSnapshot.SnapshotDeserializer<T> deserializeSnapshot, 
        DynamicSnapshot.SnapshotSerializer<T> serializeSnapshot) 
        where T : struct, ISpatialComponentSnapshot
    {
        var redundantComponentId = DynamicSnapshot.GetComponentSnapshotId<T>();
        deserializeSnapshot(...); 
        serializeSnapshot(...); 
    }
}
```

After: 

```csharp
private class MyDynamicHandler : Dynamic.IHandler 
{
    public void Accept<TData, TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TData, TUpdate, TSnapshot> vtable)
        where TData : struct, ISpatialComponentData
        where TUpdate : struct, ISpatialComponentUpdate
        where TSnapshot : struct, ISpatialComponentSnapshot
    {
        var redundantComponentId = Dynamic.GetComponentSnapshotId<TSnapshot>();
        vtable.DeserializeSnapshot(...); 
        vtable.SerializeSnapshot(...); 
    }
}
```

## From `0.2.4` to `0.2.5`

### NPM Packages

From release `0.2.5` onward, all GDK packages and feature modules will be delivered through the Unity Package Manager. To upgrade your project to use these packages do the following:

1. Open the `Packages/manifest.json` file inside your Unity project.
2. Add the following JSON snippet to the this file, under the `"registry: "https://packages.unity.com"` line:
    ```json
    "scopedRegistries": [
      {
        "name": "Improbable",
        "url": "https://npm.improbable.io/gdk-for-unity/",
        "scopes": [
          "io.improbable"
        ]
      }
    ]
    ```
3. Under the `dependencies` field, find all `com.improbable.*` packages and change the names to `io.improbable.*`.
4. For all these packages, change the value from `file:<something>` to `0.2.5`.

> **Note:** You no longer need to checkout the `gdk-for-unity` repository side-by-side your project to sideload these packages.

### Build system changes

When calling the build system from the command line, you must change the `buildTarget` argument to `buildEnvironment`. Previously you may have called the build system like so:

```bash
Unity.exe <arguments> \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityGameLogic" \
    +buildTarget "cloud" \
    +scriptingBackend "mono"
```

You must change this to:

```bash
Unity.exe <arguments> \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityGameLogic" \
    +buildEnvironment "cloud" \
    +scriptingBackend "mono"
```

## From `0.2.3` to `0.2.4`

### General changes

When constructing a `ForwardingDispatcher`, you must now provide a `UnityEngine.LogType` instead of a `Improbable.Worker.CInterop.LogLevel`.

### Transform Synchronization changes

Several utility methods in the `TransformUtils` class have been made `internal` to the Transform Synchronization package. We highly recommend that **if you were using the `TransformInternal` component in your logic, to use `UnityEngine.Transform` instead.**

You must no longer use the `TransformInternal.Snapshot` constructor.

```csharp
// Sample of INVALID code - the `Location` and `Quaternion` schema types have been removed.
var transform = new TransformInternal.Snapshot
{
    Location = new Location((float) coords.X, (float) coords.Y, (float) coords.Z),
    Rotation = new Quaternion(1, 0, 0, 0),
    TicksPerSecond = 1f / Time.fixedDeltaTime
};
```

Instead, you must use the `TransformUtils.CreateTransformSnapshot` static method. This is to ensure that the contents of an entity's `TransformInternal` component are compressed using the Transform Synchronization module's new compression scheme.

```csharp
// This is the correct way of initialising a `TransformInternal` snapshot manually.
var transform = TransformUtils.CreateTransformSnapshot(Coordinates.Zero, Quaternion.identity, Vector3.forward)
```

If you use the `AddTransformSynchronizationComponents` method in the `TransformSynchronizationHelper` class, you do not need to update code.

**You must regenerate any snapshots which contain entities that make use of the Transform Synchronization feature module.**

### WorkerConnector changes

This release brought a number of changes to the `WorkerConnector` and its derived classes. A crucial difference is now that the logic for connecting a worker is described by composing objects in a builder-like pattern rather than class level methods on the `WorkerConnector` methods.

As a result of this the `DefaultWorkerConnector` and `DefaultMobileWorkerConnector` have been removed, with their logic preserved in other objects. More about that later.

As a general rule for this upgrade, the changes in the [`ClientWorkerConnector.cs`](workers/unity/Assets/Playground/Scripts/Worker/ClientWorkerConnector.cs), [`GameLogicWorkerConnector.cs`](workers/unity/Assets/Playground/Scripts/Worker/GameLogicWorkerConnector.cs), and [`MobileClientWorkerConnector.cs`](workers/unity/Assets/Playground/Scripts/Worker/MobileClientWorkerConnector.cs) classes between the `0.2.3` and `0.2.4` releases will be illustrative in the upgrade process. The upgrade process depends on how heavily you customized your worker connector.

Previously, your connection logic may have looked something like:

```csharp
private async void Connect()
{
    // Some custom logic..
    await Connect(WorkerUtils.MyWorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
}
```

All the details for how the connection was made and which flow it used were contained in the class methods like `GetConnectionService`, `GetReceptionistConfig`, etc.

Now, your connection logic will look something like:

```csharp
private async void Connect()
{
    var builder = new SpatialOSConnectionHandlerBuilder()
        .SetConnectionParameters(CreateConnectionParameters(WorkerUtils.MyWorkerType));

    var initializer = new CommandLineConnectionFlowInitializer();
    switch (initializer.GetConnectionService())
    {
        case ConnectionService.Receptionist:
            builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.MyWorkerType), initializer));
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

    await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
}
```

There are a few new things here:

- The `SpatialOSConnectionHandlerBuilder`. This object is used for configuring and creating a single SpatialOS connection.
- The `xyzFlow` objects. These encapsulate the configuration and implementation for connecting to SpatialOS via a specific connection flow.
- The `xyzFlowInitializer` objects. These describe how the configuration for a flow object is obtained.

#### Upgrading my worker connectors

For upgrading mobile worker connectors, see [below](#mobile-setup). We preserved the semantics of the `DefaultWorkerConnector` implementation in the `CommandLineConnectionFlowInitializer` and connection flow objects to make upgrading as easy as possible.

In order to replicate the `DefaultWorkerConnector` behaviour exactly:

1. Change your mobile worker connector implementation to inherit from `WorkerConnector` instead of `DefaultWorkerConnector`.
2. Use the following code snippet to connect:
    ```csharp
    private async void Connect()
    {
        var builder = new SpatialOSConnectionHandlerBuilder()
            .SetConnectionParameters(CreateConnectionParameters(WorkerUtils.MyWorkerType));

        var initializer = new CommandLineConnectionFlowInitializer();
        switch (initializer.GetConnectionService())
        {
            case ConnectionService.Receptionist:
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityClient), initializer));
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

        await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
    }
    ```


#### Customizing the connection flows

Previously, there were a set of virtual methods on the `WorkerConnector` class that changed the behaviour of some of the connection flows. These methods now live in their respective connection flow objects (and are still virtual). You can inherit from the flow object and override the methods to achieve the same behaviour. See the table below for a mapping of where these methods currently exist.

| Method | Class |
| --- | --- |
| `SelectDeploymentName(DeploymentList deployments)` | `LocatorFlow` |
| `GetPlayerId()` | `AlphaLocatorFlow` |
| `GetDisplayName()` | `AlphaLocatorFlow` |
| `string SelectLoginToken(List<LoginTokenDetails> loginTokens)` | `AlphaLocatorFlow` |
| `string GetDevelopmentPlayerIdentityToken(string authToken, string playerId, string displayName)` | `AlphaLocatorFlow` |
| `List<LoginTokenDetails> GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)` | `AlphaLocatorFlow` |

#### Mobile setup

The mobile setup is a little bit special. There are built-in initializers for the connection flows and connection parameters in the mobile package. These replicate the behaviour of the `DefaultMobileWorkerConnector`.

The `MobileConnectionFlowInitializer` constructor takes any number of `IMobileSettingsProvider` objects and will use these to get mobile specific settings (such as the host IP address for the receptionist). The order in which you provide these objects _does matter_ as the first one to return a valid setting for each setting will be used. We provide two implementations of this interface in the mobile package, one for command line arguments and one for player prefs.

In order to replicate the `DefaultMobileWorkerConnector` exactly:

1. Change your mobile worker connector implementation to inherit from `WorkerConnector` instead of `DefaultMobileWorkerConnector`.
2. Add a `[SerializeField] private string ipAddress;` field to your `WorkerConnector`.
3. Implement `MobileConnectionFlowInitializer.IMobileSettingsProvider` for your mobile worker connector:
    ```csharp
        public Option<string> GetReceptionistHostIp()
        {
            return string.IsNullOrEmpty(ipAddress) ? Option<string>.Empty : new Option<string>(ipAddress);
        }

        public Option<string> GetDevAuthToken()
        {
            var token = Resources.Load<TextAsset>("DevAuthToken")?.text.Trim();
            return token ?? Option<string>.Empty;
        }

        public Option<ConnectionService> GetConnectionService()
        {
            return Option<ConnectionService>.Empty;
        }
    ```
4. Use the following code snippet to connect:
    ```csharp
        var connParams = CreateConnectionParameters(WorkerUtils.MobileClient, new MobileConnectionParametersInitializer());

        var flowInitializer = new MobileConnectionFlowInitializer(
            new MobileConnectionFlowInitializer.CommandLineSettingsProvider(),
            new MobileConnectionFlowInitializer.PlayerPrefsSettingsProvider(),
            this);

        var builder = new SpatialOSConnectionHandlerBuilder()
            .SetConnectionParameters(connParams);

        switch (flowInitializer.GetConnectionService())
        {
            case ConnectionService.Receptionist:
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.MobileClient),
                    flowInitializer));
                break;
            case ConnectionService.AlphaLocator:
                builder.SetConnectionFlow(new AlphaLocatorFlow(flowInitializer));
                break;
            default:
                throw new ArgumentException("Received unsupported connection service.");
        }

        await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
    ```
