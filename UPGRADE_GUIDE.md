# Upgrade Guide

## From `0.3.5` to `0.3.6`

### WorkerConnector changes

There are two related changes that you will need to adjust for. Previously, the `WorkerConnector.Connect` class would trigger the `HandleWorkerConnectionFailure` callback if the connection failed. 

We've removed this callback, and `WorkerConnector.Connect` will now throw an exception for a failed connection. For example:

```csharp
public class MyWorkerConnector : WorkerConnector
{
    public async void Start()
    {
        // Setup connection flow.
        var builder = ...;

        await Connect(builder, new ForwardingDispatcher());
    }

    protected override void HandleWorkerConnectionFailure(string errorMessage)
    {
        Debug.LogError(errorMessage);
    }
}
```

Would change to:

```csharp
public class MyWorkerConnector : WorkerConnector
{
    public async void Start()
    {
        // Setup connection flow.
        var builder = ...;

        try
        {
            await Connect(builder, new ForwardingDispatcher());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
```

## From `0.3.4` to `0.3.5`

### Unity 2019.3 upgrade

You must upgrade your project to 2019.3 to be able to use this version, and future versions, of the GDK for Unity. Any patch release of 2019.3 should work, but we test against 2019.3.7f1.

### Unity Entities 0.9.1 upgrade

We've upgraded our dependency on Unity's entities package from 0.1.0 to 0.9.1. As a result, we have introduced a few breaking changes.

You'll have to add the built-in modules `com.unity.modules.assetbundle` and `com.unity.modules.uielements` to your project dependencies. This can be done through the package manager window.

If you are using the ECS, you will have to update your queries to change the `ComponentAuthority` component to `HasAuthority`, and remove the previous filtering.

For example:

```csharp
cubeGroup = GetEntityQuery(
    ComponentType.ReadWrite<CubeTargetVelocity.Component>(),
    ComponentType.ReadOnly<CubeTargetVelocity.ComponentAuthority>(),
    ComponentType.ReadWrite<Rigidbody>()
);
cubeGroup.SetFilter(CubeTargetVelocity.ComponentAuthority.Authoritative);
```

Would change into:

```csharp
cubeGroup = GetEntityQuery(
    ComponentType.ReadWrite<CubeTargetVelocity.Component>(),
    ComponentType.ReadOnly<CubeTargetVelocity.HasAuthority>(),
    ComponentType.ReadWrite<Rigidbody>()
);
```

## From `0.3.3` to `0.3.4`

### PlayerLifecycle feature module now provides an EntityId

The callback used for creating a player `EntityTemplate` has changed to provide an `EntityId` up front.
This player's Entity will have this `EntityId` after it is successfully spawned, and can be useful for defining QBI queries.

For example:

```csharp
public static EntityTemplate Player(string workerId, byte[] args)
{
    var template = new EntityTemplate();
    // ...
    return template;
}
```

Would change into:

```csharp
public static EntityTemplate Player(EntityId entityId, string workerId, byte[] args)
{
    var template = new EntityTemplate();
    // ...
    return template;
}
```

### `IEntityGameObjectCreator` changes

#### Implement `PopulateEntityTypeExpectations` method

If you have written custom GameObject creators implementing `IEntityGameObjectCreator`, you will now have to implement a `void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)` method.

The `EntityTypeExpectations` class provides two public methods for defining a set of components expected on an entity to be able create GameObjects for a given entity type:

- `void RegisterDefault(IEnumerable<Type> defaultComponentTypes = null)`
- `void RegisterEntityType(string entityType, IEnumerable<Type> expectedComponentTypes = null)`

For example, the `GameObjectCreatorFromMetadata` class implements the method like so:

```csharp
public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
{
    entityTypeExpectations.RegisterDefault(new[]
    {
        typeof(Position.Component)
    });
}
```

The `AdvancedEntityPipeline` in the FPS Starter Project makes use of both public methods on the `EntityTypeExpectations` class. This is done in order to wait for specific components when creating "Player" GameObjects, and a different set of components for any other entity type:

```csharp
public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
{
    entityTypeExpectations.RegisterEntityType(PlayerEntityType, new[]
    {
        typeof(OwningWorker.Component), typeof(ServerMovement.Component)
    });

    fallback.PopulateEntityTypeExpectations(entityTypeExpectations);
}
```

#### Add `string entityType` as argument to `OnEntityCreated`

The `EntityType` available the `Metadata` component is now provided as an argument when calling `OnEntityCreated` on your GameObject creator.

> This means that your entities must have the `Metadata` component to use the GameObject Creation Feature Module.

The `AdvancedEntityPipeline` in the FPS Starter Project makes use of it like so:

```csharp
public void OnEntityCreated(string entityType, SpatialOSEntity entity, EntityGameObjectLinker linker)
{
    switch (entityType)
    {
        case PlayerEntityType:
            CreatePlayerGameObject(entity, linker);
            break;
        default:
            fallback.OnEntityCreated(entityType, entity, linker);
            break;
    }
}
```

## From `0.3.2` to `0.3.3`

### Building for Android now requires the NDK

You can install the NDK from within the Unity Hub. Select "Add Modules" for your Unity install and ensure that the "Unity SDK & NDK" option is ticked underneath "Android Build Support".

See [Unity's documentation](https://docs.unity3d.com/Manual/android-sdksetup.html) for more information.

### Generated code now requires unsafe

Code generated from schema now contains code marked as `unsafe`, which needs to be specifically allowed in the relevant [Assembly Definition file](https://docs.unity3d.com/Manual/class-AssemblyDefinitionImporter.html).
If you follow the Blank Project, you can find this option on the `Assets\Generated\Improbable.Gdk.Generated.asmdef` asset.

## From `0.2.10` to `0.3.0`

### Reactive Components

Reactive components are no longer available. For documentation on the equivalent APIs please refer to:

* [`ComponentUpdateSystem`](https://documentation.improbable.io/gdk-for-unity/docs/api-core-componentupdatesystem)
* [`CommandSystem`](https://documentation.improbable.io/gdk-for-unity/docs/api-core-commandsystem)

## From `0.2.9` to `0.2.10`

### General

* Replace all usages of `Dynamic.GetComponentId<T>` with `ComponentDatabase.GetComponentId<T>`.
* Replace all usages of `Dynamic.GetSnapshotComponentId<T>` with `ComponentDatabase.GetSnapshotComponentId<T>`.
    ```csharp
        // Previously
        var componentId = Dynamic.GetComponentId<Position.Component>();
        var componentId = Dynamic.GetSnapshotComponentId<Position.Snapshot>();

        // Now
        var componentId = ComponentDatabase.GetComponentId<Position.Component>();
        var componentId = ComponentDatabase.GetSnapshotComponentId<Position.Snapshot>();
    ```


## From `0.2.6` to `0.2.7`

### Worker SDK `14.0.1` upgrade

The Worker SDK upgrade introduces breaking changes to the connection flow, and removes the `Vector3f` and `Vector3d` types from the standard schema library.

#### Removal of `Vector3f` and `Vector3d`

These two schema types are no longer available in the standard schema library. You can replace their definitions by first defining a schema file in your project:

```
package my_game;

type Vector3f {
    float x = 1;
    float y = 2;
    float z = 3;
}

type Vector3d {
    double x = 1;
    double y = 2;
    double z = 3;
}
```

You should then replace the import of `improbable/vector.schema` and usage of `improbable.Vector3f`/`improbable.Vector3d` with the schema file you defined.

> Note that methods such as `Vector3f.ToUnityVector();` are no longer available and you'll need to reimplement them yourself as extension/static methods. You can find the old implementations here: [`Vector3f`](https://github.com/spatialos/gdk-for-unity/blob/0.2.6/workers/unity/Packages/io.improbable.gdk.tools/.CodeGenerator/GdkCodeGenerator/Partials/Improbable.Vector3f) and [`Vector3d`](https://github.com/spatialos/gdk-for-unity/blob/0.2.6/workers/unity/Packages/io.improbable.gdk.tools/.CodeGenerator/GdkCodeGenerator/Partials/Improbable.Vector3d).
>
> You will be unable to reimplement the operators since C# lacks the ability to define operations via extension methods.
>
> Note that the `Coordinates` type can be used as a replacement for `Vector3d` as they are structurally the same.

#### Connection flow changes

The `AlphaLocatorFlow` and the `LocatorFlow` have been merged. This means that your worker connectors may require some changes. Wherever you were previously using the `AlphaLocatorFlow` or the `ConnectionService.AlphaLocator` enum value, you should now be using the `LocatorFlow` and the `ConnectionService.Locator` enum value.

For example:

```csharp
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
```

Would change into:

```csharp
var initializer = new CommandLineConnectionFlowInitializer();
switch (initializer.GetConnectionService())
{
    case ConnectionService.Receptionist:
        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityClient), initializer));
        break;
    case ConnectionService.Locator:
        builder.SetConnectionFlow(new LocatorFlow(initializer));
        break;
    default:
        throw new ArgumentOutOfRangeException();
}
```

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

#### Reactive components

> If you were not using reactive components previously, no action is required.

Reactive components are now **opt in** instead of **opt out**. To enable them, add the scripting define `USE_LEGACY_REACTIVE_COMPONENTS` to your project.

Reactive components will be removed in a future release, **we strongly recommend to migrate off reactive components**. You can find the APIs to replace reactive components in the [ECS workflows documentation](https://documentation.improbable.io/gdk-for-unity/docs/ecs-introduction).

### Improved GDK type utility methods

To better enforce the range of `FixedPointVector3`, the `CreateTransformSnapshot(Coordinates location, Quaternion rotation, Vector3 velocity)` method has been removed. This means that `CreateTransformSnapshot` requires `location` to be a `Vector3`.

Previously:
```csharp
var coords = new Coordinates(10, 20, 30);
var transformSnapshot = TransformUtils.CreateTransformSnapshot(
    coords,
    Quaternion.identity,
    Vector3.zero);
```

Now:
```csharp
var coords = new Coordinates(10, 20, 30);
var transformSnapshot = TransformUtils.CreateTransformSnapshot(
    coords.ToUnityVector(),
    Quaternion.identity,
    Vector3.zero);
```

#### Arithmetic and equality operators

A set of arithmetic and equality operators have been implemented for the standard library's `EdgeLength` type. This means you can now use `+`, `-`, `*`, `/` with this type.

The equality operators `==` and `!=` have also been implemented for the `EdgeLength`, `FixedPointVector3` and `CompressedQuaternion` types.

> Note: The `Coordinates` type already provided these operators.

#### Type conversion

New methods are exposed for conversion of `Coordinates`, `EdgeLength`, `FixedPointVector3` and `CompressedQuaternion` to and from native Unity `Vector3` and `Quaternion` types.

| Static Method                                            | Result Type            | Module Dependency |
|----------------------------------------------------------|------------------------|-------------------|
| `Coordinates.FromUnityVector(Vector3 v)`                 | `Coordinates`          | -                 |
| `EdgeLength.FromUnityVector(Vector3 v)`                  | `EdgeLength`           | -                 |
| `FixedPointVector3.FromUnityVector(Vector3 v)`           | `FixedPointVector3`    | Transform Sync    |
| `CompressedQuaternion.FromUnityQuaternion(Quaternion q)` | `CompressedQuaternion` | Transform Sync    |

| Type                   | Method                      | Result Type            | Module Dependency |
|------------------------|-----------------------------|------------------------|-------------------|
| `Vector3`              | `.ToCoordinates()`          | `Coordinates`          | Transform Sync    |
| `Vector3`              | `.ToFixedPointVector3()`    | `FixedPointVector3`    | Transform Sync    |
| `Vector3`              | `.ToEdgeLength()`           | `EdgeLength`           | QBI Helper        |
| `FixedPointVector3`    | `.ToUnityVector()`          | `Vector3`              | Transform Sync    |
| `FixedPointVector3`    | `.ToCoordinates()`          | `Coordinates`          | Transform Sync    |
| `Coordinates`          | `.ToUnityVector()`          | `Vector3`              | -                 |
| `EdgeLength`           | `.ToUnityVector()`          | `Vector3`              | -                 |
| `CompressedQuaternion` | `.ToUnityQuaternion()`      | `Quaternion`           | Transform Sync    |
| `Quaternion`           | `.ToCompressedQuaternion()` | `CompressedQuaternion` | Transform Sync    |

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
