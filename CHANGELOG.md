# Changelog

## Unreleased

### Breaking Changes

- Your generated code Assembly Definition file now needs to have `allow unsafe code` selected to compile. [#1255](https://github.com/spatialos/gdk-for-unity/pull/1255)
- The `RedirectedProcess.WithArgs` API will now concatenate arguments, instead of replacing the previously provided arguments. [#1260](https://github.com/spatialos/gdk-for-unity/pull/1260)
- Building for Android clients now requires the Android NDK to be installed and configured on your machine. [#1265](https://github.com/spatialos/gdk-for-unity/pull/1265)

### Added

- Added public toolkit for writing code generators. [#1240](https://github.com/spatialos/gdk-for-unity/pull/1240) [#1243](https://github.com/spatialos/gdk-for-unity/pull/1243) [#1244](https://github.com/spatialos/gdk-for-unity/pull/1244) [#1245](https://github.com/spatialos/gdk-for-unity/pull/1245) [#1250](https://github.com/spatialos/gdk-for-unity/pull/1250)
- Added support for the `cn-production` environment.
- Added `RedirectedProcess.Spatial` wrapper for calling the `spatial` CLI. This wrapper automatically uses the current project environment. [#1260](https://github.com/spatialos/gdk-for-unity/pull/1260)
- Added generation of code generator run configurations for Jetbrains Rider, Visual Studio, and the `dotnet` CLI. [#1256](https://github.com/spatialos/gdk-for-unity/pull/1256)
    - This means you no longer need to keep the Unity Editor constantly open to iterate on generators.
- The code generator now logs an error when an input schema source directory does not exist. [#1256](https://github.com/spatialos/gdk-for-unity/pull/1256)

### Changed

- Upgrade to Worker SDK v14.4.0. [#1260](https://github.com/spatialos/gdk-for-unity/pull/1260)
  - The `Raknet`, `Tcp`, and `Kcp` network protocols have been deprecated. Please use `ModularKcp` and `ModularTcp` instead.
  - `ModularUdp` has been renamed to `ModularKcp`.

### Fixed

- Fixed a bug where build targets which were _not_ marked as required were not skipped if the user did not have the build support installed. [#1257](https://github.com/spatialos/gdk-for-unity/pull/1257)
- Fixed a bug where code generation would fail due to `dotnet new` failing to run. [#1262](https://github.com/spatialos/gdk-for-unity/pull/1262)

### Internal

- Implemented a new CodeWriter in the code generator which provides a fluent interface for generating C# code. [#1237](https://github.com/spatialos/gdk-for-unity/pull/1237)
    - The `CodeGenerationLib` has been migrated to the new `CodeWriter`.
- Added support for defining namespaces, structs, classes, enums and methods in the new CodeWriter. [#1239](https://github.com/spatialos/gdk-for-unity/pull/1239)
- Ported test-project to new CodeWriter. [#1241](https://github.com/spatialos/gdk-for-unity/pull/1241)
- Ported build system module to new CodeWriter. [#1242](https://github.com/spatialos/gdk-for-unity/pull/1242)
- Ported gameobject creation module to new CodeWriter. [#1247](https://github.com/spatialos/gdk-for-unity/pull/1247)
- Ported core module to new CodeWriter. [#1247](https://github.com/spatialos/gdk-for-unity/pull/1247) [#1248](https://github.com/spatialos/gdk-for-unity/pull/1248) [#1249](https://github.com/spatialos/gdk-for-unity/pull/1249) [#1251](https://github.com/spatialos/gdk-for-unity/pull/1251) [#1252](https://github.com/spatialos/gdk-for-unity/pull/1252) [#1253](https://github.com/spatialos/gdk-for-unity/pull/1253)
- Removed all Text Template Transformation Toolkit (T4) references and dependencies. [#1254](https://github.com/spatialos/gdk-for-unity/pull/1254)
- Simplified dirtyBits logic and code generation [#1255](https://github.com/spatialos/gdk-for-unity/pull/1255)
- The DeploymentLauncher now uses Platform SDK v14.4.0. [#1260](https://github.com/spatialos/gdk-for-unity/pull/1260)
- `init.sh` and `init.ps1` now support the `--china` and `-china` flag respectively to download from the `cn-production` environment. [#1261](https://github.com/spatialos/gdk-for-unity/pull/1261)
- Simplified code generation for Command classes and relevant interfaces. [#1263](https://github.com/spatialos/gdk-for-unity/pull/1263)
- Added `--force` flag to the CodeGenerator project to skip the dirty checks and re-generate everything. [#1263](https://github.com/spatialos/gdk-for-unity/pull/1263)

## `0.3.2` - 2019-12-23

### Added

- Added support for Unity build callbacks such as `IPostprocessBuildWithReport`. [#1228](https://github.com/spatialos/gdk-for-unity/pull/1228)
    - During a build you can now access the `WorkerBuilder.CurrentContext` field which contains all SpatialOS related build information.
- Enabled support for QBI frequency in the QBI Helper Module. [#1231](https://github.com/spatialos/gdk-for-unity/pull/1231)
    - Use the `WithMaxFrequencyHz` method when building an `InterestQuery` to define your query's frequency.

### Fixed

- Fixed a bug where empty list, maps, or options in a component update would not be applied correctly. [#1229](https://github.com/spatialos/gdk-for-unity/pull/1229)
- Fixed missing log path error when printing the Unity PlayerConnection debug port. [#1232](https://github.com/spatialos/gdk-for-unity/pull/1232)

## `0.3.1` - 2019-11-25

### Added

- A `WorkerSystem` now exposes the underlying Worker's `IsConnected` property. [#1217](https://github.com/spatialos/gdk-for-unity/pull/1217)

### Fixed

- Fixed an issue where the Deployment Launcher window would feel unresponsive due to saving changes after every input. [#1219](https://github.com/spatialos/gdk-for-unity/pull/1219)
    - It will now wait for at least 1 second to elapse after the last change before writing the configuration back to disk.
- Fixed issues ([#957](https://github.com/spatialos/gdk-for-unity/issues/957), [#958](https://github.com/spatialos/gdk-for-unity/issues/958)) where valid schema would generate invalid code due to name clashes. [#1222](https://github.com/spatialos/gdk-for-unity/pull/1222)
    - The offending schema properties will no longer be generated and are now logged in the Unity Editor.

### Internal

- Changed the default Locator port from 444 to 443. [#1220](https://github.com/spatialos/gdk-for-unity/pull/1220)

## `0.3.0` - 2019-11-11

### Breaking Changes

- Reactive components have been removed completely. [#1195](https://github.com/spatialos/gdk-for-unity/pull/1195)
  - If you are using reactive components, please see our documentation on the [ECS workflow](https://docs.improbable.io/unity/alpha/workflows/overview#ecs-centric-workflow).
- Codegen for the GameObjectCreation package has been moved into the package. If the package is not used, readers and writers will no longer be generated. [#1196](https://github.com/spatialos/gdk-for-unity/pull/1196)
- Empty component updates will no longer trigger callbacks when received. [#1211](https://github.com/spatialos/gdk-for-unity/pull/1211)

### Added

- Added the ability to select which Android device or emulator to launch an application on. [#1194](https://github.com/spatialos/gdk-for-unity/pull/1194)

### Changed

- Generated Worker ID's for local development are now smaller and easier to read for debugging. [#1197](https://github.com/spatialos/gdk-for-unity/pull/1197)
- Upgraded to Worker SDK 14.2.1. [#1208](https://github.com/spatialos/gdk-for-unity/pull/1208)
  - Versioning scheme for the SDK and Mobile SDK packages have now changed to align with the GDK for Unity version.
- Changed the default network type to ModularUDP with packet compression. [#1212](https://github.com/spatialos/gdk-for-unity/pull/1212)

### Fixed

- Fixed a bug where the Deployment Launcher window would accept tags with 33 characters. [#1202](https://github.com/spatialos/gdk-for-unity/pull/1202)
- Fixed a small memory leak with command response callbacks using MonoBehaviours. [#1205](https://github.com/spatialos/gdk-for-unity/pull/1205)
- Fixed an issue where events would trigger the `OnUpdate` callback on readers and writers. [#1211](https://github.com/spatialos/gdk-for-unity/pull/1211)

### Internal

- Cleaned up Subscriptions and Callbacks. [#1200](https://github.com/spatialos/gdk-for-unity/pull/1200)
  - Replaced usage of `GuardedAuthorityCallbackManagerSet` with more generic `GuardedCallbackManagerSet`.
  - Removed unused `EntitySubscriptions` class.
  - Formatting pass on all Subscriptions and Callbacks files.
- Moved the CompressedQuaternion and FixedPointVector partials to the transform synchronization package. [#1201](https://github.com/spatialos/gdk-for-unity/pull/1201)

## `0.2.10` - 2019-10-14

### Breaking Changes

- The `GetComponentId<T>()` and `GetSnapshotComponentId<T>()` methods have been moved from `Dynamic` to `ComponentDatabase`. [#1173](https://github.com/spatialos/gdk-for-unity/pull/1173)

### Added

- Added commands and world commands tabs to the Network Analyzer window. [#1174](https://github.com/spatialos/gdk-for-unity/pull/1174)

### Fixed

- Fixed a bug where the `TransformSynchronization` MonoBehaviour would not reset when disabled. [#1169](https://github.com/spatialos/gdk-for-unity/pull/1169)
- Fixed a bug where the `UnlinkGameObjectFromEntity` method in the `EntityGameObjectLinker` would not cleanup the `gameObjectToComponentsAdded` dictionary. [#1169](https://github.com/spatialos/gdk-for-unity/pull/1169)
- Fixed a bug where the Network Analyzer would not properly render all components/commands in the scroll view. [#1175](https://github.com/spatialos/gdk-for-unity/pull/1175)
- Fixed a bug where the code generator would not re-generate files when changes were made to existing schema files. [#1181](https://github.com/spatialos/gdk-for-unity/pull/1181)

### Internal

- Added the `IComponentMetaclass` and `ICommandMetaclass` interfaces. Where we previously would use reflection to find instances of various component/command related types, we now lookup through generated metaclasses. [#1173](https://github.com/spatialos/gdk-for-unity/pull/1173)
- Code Generator now enforces platform line endings. [#1189](https://github.com/spatialos/gdk-for-unity/pull/1189)

## `0.2.9` - 2019-09-16

### Added

- Added network statistics collection for both sending and receiving messages. [#1135](https://github.com/spatialos/gdk-for-unity/pull/1135)
    - This adds a single ECS system `NetworkStatisticsSystem`. This system will only run when running your workers inside the Unity Editor.
    - Additionally, there are a set of data types supporting this system which can be found under the `Improbable.Gdk.Core.NetworkStats` namespace.
- Added the Network Analyzer window, which allows you to view live bandwidth usage per component. `SpatialOS->Window->Network Analyzer` [#1148](https://github.com/spatialos/gdk-for-unity/pull/1148)

### Fixed

- Fixed a bug where `MonoBehaviour`s with `WorkerType` attributes would not be enabled even if the owning worker's type was a match for the `WorkerType` attribute. [#1147](https://github.com/spatialos/gdk-for-unity/pull/1147)
- Fixed a bug where the mobile configuration would get reset whenever assembly got reloaded or you entered Playmode. [#1157](https://github.com/spatialos/gdk-for-unity/pull/1157)

### Internal

- Added test coverage for `WorkerType` attribute and its interplay with `[Require]` fields in the `test-project`. [#1147](https://github.com/spatialos/gdk-for-unity/pull/1147)
- Refactored internals of code generation for modular codegen. [#1151](https://github.com/spatialos/gdk-for-unity/pull/1151).
    - Use `dotnet new` to generate a skeleton project then link in the various modules from each package.
    - This project is created in the `build/codegen` directory and is then executed to actually generate the code.

## `0.2.8` - 2019-09-02

### Added

- Workers will now log their `PlayerConnection` ports to SpatialOS after connecting. This port can be used for connecting the Unity profiler to workers running in the cloud. [#1128](https://github.com/spatialos/gdk-for-unity/pull/1128)
    - Note that this will only happen if the worker was built as a "Development Build".
- Added a new Unity Editor window for forwarding a port from a worker that is running in the cloud. [#1133](https://github.com/spatialos/gdk-for-unity/pull/1133)
    - You can find this window in the Unity Editor menu at: **SpatialOS** > **Port Forwarding**.
    - This can be used to connect the Unity profile to workers running in the cloud.

### Changed

- Upgraded the project to be compatible with `2019.2.0f1`.
- Upgraded to Worker SDK 14.0.2.

### Fixed

- Fixed a bug where recursive options in schema types would cause a Mono hard crash. [#1131](https://github.com/spatialos/gdk-for-unity/pull/1131)
    - Any fields in a schema type that are a recursive option will now be _skipped_.
    - This is a workaround until full recursive option support is implemented.

### Internal

- Work-around for Mac Launcher using wrong executable name. Generating hardcoded `launcher_client_config.json` for Mac builds. [#1142](https://github.com/spatialos/gdk-for-unity/pull/1142)

## `0.2.7` - 2019-08-19

### Breaking Changes

- Renamed the public field `AnonymousAuthenticationPort` to `LocatorPort` on the `AlphaLocatorFlow` class and the `RuntimeConfigDefaults` static class. [#1105](https://github.com/spatialos/gdk-for-unity/pull/1105)
- Upgraded to Worker SDK `14.0.1`. This brings a number of breaking changes. [#1112](https://github.com/spatialos/gdk-for-unity/pull/1112)
    - `Vector3f` and `Vector3d` are no longer available in the schema standard library.
    - The `Improbable.Coordinates.ToSpatialVector3d()` method has been removed.
    - `LocatorFlow` and `AlphaLocatorFlow` have been merged.
    - The implementation of the old `LocatorFlow` has been removed.
    - The `ConnectionService.AlphaLocator` enum value has been removed.
- The `ProjectName`, `SteamDeploymentTag`, and `SteamTicket` constants have been removed from the `RuntimeConfigNames` static class.

### Added

- Added the ability to connect to an arbitrary host/port combo for the `AlphaLocatorFlow`. [#1105](https://github.com/spatialos/gdk-for-unity/pull/1105)
- Added a `SpatialdManager` class for managing local deployments with `SpatialD` into `io.improbable.gdk.testutils`. [#1104](https://github.com/spatialos/gdk-for-unity/pull/1104)
- Added the ability to specify a snapshot to be used when launching a deployment in the Editor. [#1098](https://github.com/spatialos/gdk-for-unity/pull/1098)
- Added the ability to select the modular UDP network type as part of the Worker SDK 14.0.1 upgrade. [#1112](https://github.com/spatialos/gdk-for-unity/pull/1112)

### Changed

- Schema compilation error messages are now concatenated within the `GenerateCode` class before shown in the Console, creating only one error message instead of several. [#1107](https://github.com/spatialos/gdk-for-unity/pull/1107)
- Changed some reactive components for events and commands that were not correctly encapsulated with the `USE_LEGACY_REACTIVE_COMPONENTS` symbol. [#1113](https://github.com/spatialos/gdk-for-unity/pull/1113)
- Improved performance in SendComponentSystem through better job handling.

### Fixed

- Fixed a bug where `RedirectedProcess.RunAsync()` could deadlock if you did not provide a `CancellationToken`. [#1102](https://github.com/spatialos/gdk-for-unity/pull/1102)
- Fixed a bug in the `AlphaLocatorFlow` where the default implementation of `GetDevelopmentLoginTokens` did not respect the host/port fields. [#1105](https://github.com/spatialos/gdk-for-unity/pull/1105)
- Fixed a bug where `AlphaLocatorFlow.CreateAsync()` could deadlock. [#1108](https://github.com/spatialos/gdk-for-unity/pull/1108)

### Internal

- Added tests for the `ReceptionistFlow` class. [#1095](https://github.com/spatialos/gdk-for-unity/pull/1095)
- Added tests for the `CommandLineConnectionFlowInitializer` class. [#1096](https://github.com/spatialos/gdk-for-unity/pull/1096)
- Added tests for the `CommandLineConnectionParametersInitializer` class. [#1103](https://github.com/spatialos/gdk-for-unity/pull/1103)
- Added `spot` downloading to `init.sh` & `init.ps` into the `io.improbable.worker.sdk` package. [#1104](https://github.com/spatialos/gdk-for-unity/pull/1104)
- Added tests for the `AlphaLocatorFlow` class. [#1108](https://github.com/spatialos/gdk-for-unity/pull/1108)
- `Option<T>` is now explicitly immutable as a `readonly struct`. [#1110](https://github.com/spatialos/gdk-for-unity/pull/1110)
- Removed unused arguments from worker configuration files. [#1112](https://github.com/spatialos/gdk-for-unity/pull/1112)

## `0.2.6` - 2019-08-05

### Breaking Changes

- Renamed `Improbable.Gdk.Core.EntityQuerySnapshot` to `Improbable.Gdk.Core.EntitySnapshot`. The `Improbable.Gdk.Core.Commands.WorldCommands.EntityQuery.ReceivedResponse` has been updated accordingly. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
- `DynamicSnapshot` and `DynamicConverter` have been collapsed into `Dynamic`. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
    - The delegates from all of these classes are now available in the `Dynamic.VTable<TData, TUpdate, TSnapshot>` struct.
    - `Dynamic.IHandler.Accept` now takes a `Dynamic.Vtable<TData, TUpdate, TSnapshot` parameter.
- Reactive components are now **opt in** instead of **opt out**. Use the scripting define `USE_LEGACY_REACTIVE_COMPONENTS` to re-enable them. [#1059](https://github.com/spatialos/gdk-for-unity/pull/1059)
    - Note that these will be removed in a future release.
- Removed the `CreateTransformSnapshot(Coordinates location, Quaternion rotation, Vector3 velocity)` method. [#1063](https://github.com/spatialos/gdk-for-unity/pull/1063)
- Removed the `WorkerOpFactory` from the `io.improbable.gdk.testutils` package. Please use the `MockConnectionHandler` in the `io.improbable.gdk.core` package instead. [#1085](https://github.com/spatialos/gdk-for-unity/pull/1085)
- Removed the `TestMonoBehaviour` from the `io.improbable.gdk.testutils` package. [#1085](https://github.com/spatialos/gdk-for-unity/pull/1085)

### Added

- Added a `bool TryGetComponent<T>(out T component);` method to the `SpatialOSEntity` struct. This can help reduce boilerplate when writing custom `IEntityGameObjectCreator` implementations. [#1049](https://github.com/spatialos/gdk-for-unity/pull/1049)
- Added the `void AddComponentSnapshot<T>(T componentSnapshot)` and `bool TryGetComponent<T>(out T componentSnapshot)` methods to the `Improbable.Gdk.Core.EntitySnapshot` struct. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
- Added a `EntitySnapshot GetEntitySnapshot()` method to the `Improbable.Gdk.Core.EntityTemplate` class. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
- Added methods for conversion of `Coordinates`, `EdgeLength`, `FixedPointVector3` and `CompressedQuaternion` to/from native Unity `Vector3` and `Quaternion` types. [#1063](https://github.com/spatialos/gdk-for-unity/pull/1063)
- Added basic arithmetic and equality operators for the `EdgeLength` standard library type. [#1063](https://github.com/spatialos/gdk-for-unity/pull/1063)
- Added a new `io.improbable.gdk.debug` package which contains an inspector extension for viewing `[Require]` states in the editor [#1082](https://github.com/spatialos/gdk-for-unity/pull/1082)

### Changed

- Upgraded to Worker SDK 13.8.2. [#1052](https://github.com/spatialos/gdk-for-unity/pull/1052)
    - The new [`Entity` schema type](https://docs.improbable.io/reference/13.8/shared/schema/reference#primitive-types) is deserialized as an `Improbable.Gdk.Core.EntitySnapshot`. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
- The conversion methods for `FixedPointVector3` and `CompressedQuaternion` have been moved from `TransformUtils` to their generated structs and are now public. [#1063](https://github.com/spatialos/gdk-for-unity/pull/1063)

### Fixed

- The world command sender reactive components and reactive component systems are now properly conditionally compiled. [#1059](https://github.com/spatialos/gdk-for-unity/pull/1059)
- Subscribing to the `World`, `ILogDispatcher`, `WorkerId`, and `LinkedGameObjectMap` types no longer cause different MonoBehaviour/GameObject subscriptions to cross-talk. This previously would result in Monobehaviours disabling unexpectedly. [#1071](https://github.com/spatialos/gdk-for-unity/pull/1071)

### Internal

- Added extension methods on the `SchemaObject` struct for easy serializing/deserializing of the `Entity` schema type. [#1053](https://github.com/spatialos/gdk-for-unity/pull/1053)
- Added options and functionality for serialization overrides for schema types only. [#1061](https://github.com/spatialos/gdk-for-unity/pull/1061)
- Laid the groundwork for 2D support in the Transform Synchronization Feature Module. [#1064](https://github.com/spatialos/gdk-for-unity/pull/1064)
- Removed `core-sdk.pinned` from the tools package.
- Moved `DisableAutoCreationTests` into the test project [#1085](https://github.com/spatialos/gdk-for-unity/pull/1085)

## `0.2.5` - 2019-07-18

### Breaking Changes

- Renamed the `buildTarget` command line argument to `buildEnvironment`. [#1012](https://github.com/spatialos/gdk-for-unity/pull/1012)
- All GDK packages now have an `io.improbable` prefix instead of `com.improbable`. [#894](https://github.com/spatialos/gdk-for-unity/pull/894)

### Added

- Added the `LinkedGameObjectMap` class for finding the `GameObject`(s) linked with a specified `EntityId`. [#1013](https://github.com/spatialos/gdk-for-unity/pull/1013)
    - This can be used with the `[Require]` annotation to inject it into your `MonoBehaviours` provided you are using the `GameObjectCreation` feature module. For example: `[Require] private LinkedGameObjectMap gameObjectMap;`
- Added the ability for the build system to build specific targets of a given build environment. [#1012](https://github.com/spatialos/gdk-for-unity/pull/1012)
    - Use the `buildTargetFilter` command line argument to pass in a comma delimited list of build targets to filter for. For example, `+buildTargetFilter win,macos`.
- Added two new GDK packages: `io.improbable.worker.sdk` and `io.improbable.worker.sdk.mobile` which contain the underlying Worker SDK packages for Windows/MacOS/Linux and Android/iOS respectively. [#894](https://github.com/spatialos/gdk-for-unity/pull/894)
- You may now `[Require]` a `WorkerId` in MonoBehaviours. For example: `[Require] private WorkerId workerId;`. [#1016](https://github.com/spatialos/gdk-for-unity/pull/1016)
- iOS builds now perform a post processing step on the XCode project to ensure x86_64 and arm libraries from the SpatialOS Worker SDK are separated. [#1040](https://github.com/spatialos/gdk-for-unity/pull/1040)

### Changed

- `PlayerLifecycleHelper.IsOwningWorker` will now return false instead of throwing an exception if the entity is not in your worker's view.

### Fixed

- Fixed a bug where `PlayerLifecycleHelper.IsOwningWorker` would throw an exception if the entity was in your worker's view.

### Internal

- Stopped throwing a `Test Exception` in playground. [#1011](https://github.com/spatialos/gdk-for-unity/pull/1011)
- The embedded `DownloadCoreSdk` `dotnet` project has been removed from `Improbable.Gdk.Tools`. [#894](https://github.com/spatialos/gdk-for-unity/pull/894)
- The `PluginPostprocessor` tool has been removed from `Improbable.Gdk.Tools`. [#894](https://github.com/spatialos/gdk-for-unity/pull/894)

## `0.2.4` - 2019-06-28

### Breaking Changes

- The constructor for the `ForwardingDispatcher` now accepts a `UnityEngine.LogType` instead of `Improbable.Worker.CInterop.LogLevel` as its parameter. [#987](https://github.com/spatialos/gdk-for-unity/pull/987)
- The schema for the Transform Synchronization feature module has been optimised to reduce bandwidth. If you use this module, please make use of the updated helper methods and regenerate your snapshot. [#990](https://github.com/spatialos/gdk-for-unity/pull/990)
    - The `Location` and `Velocity` schema types have been replaced by the `FixedPointVector3` type.
        - The location and velocity fields are now [`Q21.10`](https://en.wikipedia.org/wiki/Q_(number_format)) fixed point values.
    - The `Quaternion` schema type has been replaced by the `CompressedQuaternion` type.
        - Rotation is now compressed from 4 floats to a single uint32.
- The worker abstraction & connectors have been changed **significantly**. See the [Upgrade guide](UPGRADE_GUIDE.md) for more details on how to upgrade. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
    - The `DefaultWorkerConnector` and `DefaultMobileWorkerConnector` classes have been removed.
    - The `WorkerConnector` has had the following abstract methods removed:
        - `ConnectionService GetConnectionService()`
        - `ConnectionParameters GetConnectionParameters(string workerType, ConnectionService service)`
        - `LocatorConfig GetLocatorConfig()`
        - `AlphaLocatorConfig GetAlphaLocatorConfig(string workerType)`
        - `ReceptionistConfig GetReceptionistConfig(string workerType)`
    - The `WorkerConnector` has had the following virtual methods removed:
        - `string GetPlayerId()`
        - `string GetDisplayName()`
        - `string SelectDeploymentName(DeploymentList deployments)`
        - `string GetDevAuthToken()`
        - `string SelectLoginToken(List<LoginTokenDetails> loginTokens)`
        - `string GetDevelopmentPlayerIdentityToken(string authToken, string playerId, string displayName)`
        - `List<LoginTokenDetails> GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)`
    - The `WorkerConnector` has also had the following changes:
        - The `public Worker Worker;` field is now the `public WorkerInWorld Worker;` field.
        - The `public async Task Connect(string workerType, ILogDispatcher logger)` method is now `protected async Task Connect(IConnectionHandlerBuilder builder, ILogDispatcher logger)`.
    - The `Worker` class has had the following changes:
        - The `public Connection Connection { get; private set; }` property has been removed.
        - The `public World World { get; private set; }` property has been moved to the `WorkerInWorld` class.
        - The `public static async Task<Worker> CreateWorkerAsync(ReceptionistConfig parameters, ConnectionParameters connectionParameters, ILogDispatcher logger, Vector3 origin)` method has been removed.
        - The `public static async Task<Worker> CreateWorkerAsync(LocatorConfig parameters, ConnectionParameters connectionParameters, ILogDispatcher logger, Vector3 origin)` method has been removed.
        - The `public static async Task<Worker> CreateWorkerAsync(AlphaLocatorConfig  parameters, ConnectionParameters connectionParameters, ILogDispatcher logger, Vector3 origin)` method has been removed.
    - The `ReceptionistConfig`, `LocatorConfig`, and `AlphaLocatorConfig` structs have been removed.
    - The `WorkerSystem` no longer has a `public readonly Connection Connection;` field
- The `CommandLineUtility` static class has been replaced with a `CommandLineArgs` stateful class. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
- The `ILogDispatcher` interface now has a `public Worker Worker { get; set; }` property instead of a `public Connection Connection { get; set; }` property. This of course, propagates down to all implementations of the `ILogDispatcher` interface. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)

### Added

- Added a mobile launcher window containing all the settings and functionality to allow you to launch your apps for iOS and Android. To open it, in the Unity Editor, select **SpatialOS** > **Mobile Launcher**.
- Added a `IEnumerable<T> FilterOption<T>(this IEnumerable<Option<T>> enumerable)` LINQ extension. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
- Added a `public readonly string WorkerId;` field to the `WorkerSystem` class. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
- Added a `IConnectionFlow` interface which describes how a connection can be created. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
    - Added a `ReceptionistFlow`, `LocatorFlow`, and `AlphaLocatorFlow` which implement `IConnectionFlow`.
- Added a `IConnectionFlowInitializer<TConnectionFlow>` interface which describes how the parameters for a particular connection flow are initialized. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
    - Added a `CommandLineConnectionFlowInitializer` which implements `IConnectionFlowInitializer<ReceptionistFlow>`, `IConnectionFlowInitializer<LocatorFlow>`, and `IConnectionFlowInitializer<AlphaLocatorFlow>`.
    - Added a `MobileConnectionFlowInitializer` which implements `IConnectionFlowInitializer<ReceptionistFlow>` and `IConnectionFlowInitializer<AlphaLocatorFlow>`.
- Added a `IConnectionParameterInitializer` which describes how the parameters for a worker connector are initialized. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
    - Added a `CommandLineConnectionParameterInitializer` which implements `IConnectionParameterInitializer`.
    - Added a `MobileConnectionParametersInitializer` which implements `IConnectionParameterInitializer`.
- The `IConnectionHandler` interface now has the following methods: [#981](https://github.com/spatialos/gdk-for-unity/pull/981)
    - `string GetWorkerId();`
    - `List<string> GetWorkerAttributes();`
- Added a `WorkerInWorld` class which inherits from `Worker` and adds ECS specific implementation details to the `Worker`. [#981](https://github.com/spatialos/gdk-for-unity/pull/981)

### Changed

- Moved the configuration of the Local Runtime IP from the GDK tools configuration window to the mobile launcher window.

### Fixed

- Fixed a bug where invalid characters in your PATH elements would throw exceptions and break code generation. [#986](https://github.com/spatialos/gdk-for-unity/pull/986)
- Fixed a regression in the `SetKinematicFromAuthoritySystem` that failed to toggle a rigidbody's kinematic state on `TransformInternal` authority changes. [#988](https://github.com/spatialos/gdk-for-unity/pull/988)
- Fixed a regression in the code generator where spaces in your path would cause code generation failures. [#991](https://github.com/spatialos/gdk-for-unity/pull/991)

## `0.2.3` - 2019-06-12

### Breaking Changes

- Schema from packages are no longer copied into the root `schema` directory. [#953](https://github.com/spatialos/gdk-for-unity/pull/953)
    - Renamed the `Schema` directory within packages to `.schema`, to avoid generating unecessary `.meta` files.
    - Update feature module schema to the correct namespaces and folders within `.schema`.
    - If you use schema that imports from GDK packages, you will need to change how you import GDK schema.
        - Schema file Y in package `improbable.gdk.X` is imported using `import "improbable/gdk/X/Y.schema"`.
        - For example, `import "from_gdk_packages/com.improbable.gdk.core/common.schema";` now becomes `import "improbable/gdk/core/common.schema";`.
- Upgraded the Unity Entities package to `preview.33` from `preview.21`. [#963](https://github.com/spatialos/gdk-for-unity/pull/963), [#966](https://github.com/spatialos/gdk-for-unity/pull/966), [#967](https://github.com/spatialos/gdk-for-unity/pull/967)
    - See the Unity Entities package [changelog](https://docs.unity3d.com/Packages/com.unity.entities@0.0/changelog/CHANGELOG.html) for more information.
    - This has removed several APIs such as `[Inject]` and `ComponentDataArray`.
    - If you use generic `IComponentData` types, you must explicitly register them. Please view Unity's example on `RegisterGenericComponentType` in the changelog linked above.
    - System groups API has changed, systems without a group are automatically added to the `SimulationSystemGroup` which runs on `Update`.
        - The Unity Editor will print helpful warnings if any systems are not grouped properly.
- Removed `BlittableBool`, as `bool` is now blittable. [#965](https://github.com/spatialos/gdk-for-unity/pull/965)
- Fixed a bug where the generated `ReceivedUpdates` component was not correctly wrapped in the `DISABLED_REACTIVE_COMPONENTS` define. [#971](https://github.com/spatialos/gdk-for-unity/pull/971)
    - This means that if you have `DISABLE_REACTIVE_COMPONENTS` set, the `ReceivedUpdates` types will no longer be available.

### Changed

- Upgraded the project to be compatible with `2019.1.3f1`. [#951](https://github.com/spatialos/gdk-for-unity/pull/951)
- Moved Runtime IP from the `GdkToolsConfiguration.json` to the Editor Preferences. [#961](https://github.com/spatialos/gdk-for-unity/pull/961)
- Moved Dev Auth Token to the Player Preferences. [#961](https://github.com/spatialos/gdk-for-unity/pull/961)
    - Added a setting in `GdkToolsConfiguration` to let users configure whether a `DevAuthToken.txt` should be generated or not.
    - When launching Android cloud clients from the Editor, the DevAuthToken is now passed in as a command line argument.
- The schema descriptor is no longer deleted when you select `SpatialOS > Clean all workers` from the Unity Editor. [#969](https://github.com/spatialos/gdk-for-unity/pull/969)

### Fixed

- Fixed a bug where a worker's `World` could get disposed multiple times if you stopped the application inside the Editor while the worker is being created. [#952](https://github.com/spatialos/gdk-for-unity/pull/952)

### Internal

- Removed the workaround for a schema component update bug (WRK-1031). [#962](https://github.com/spatialos/gdk-for-unity/pull/962)
- All playground launch configuration files now use the `w2_r0500_e5` template instead of the `small` template which was deprecated. [#968](https://github.com/spatialos/gdk-for-unity/pull/968)
- Disabled Burst compilation for all platforms except for iOS, because Burst throws benign errors when building workers for other platforms than the one you are currently using. [#199](https://github.com/spatialos/gdk-for-unity-fps-starter-project/pull/199)
- Enabled Burst compilation for iOS, because disabling results in an invalid XCode project. [#197](https://github.com/spatialos/gdk-for-unity-fps-starter-project/pull/197)

## `0.2.2` - 2019-05-15

### Breaking Changes

- Removed the `Improbable.Gdk.Mobile.Android` and `Improbable.Gdk.Mobile.iOS` packages. All functionality is now available inside the `Improbable.Gdk.Mobile` package.

### Added

- Added support for Windows x86 builds.
- Added a user-friendly error message when the build system fails to find a SpatialOS Build Configuration instance.
- Added two menu items: `SpatialOS > Launch mobile device > Android on local` and `SpatialOS > Launch mobile device > Android on cloud`.
- Added a new, project-generic, deployment launcher feature module, `com.improbable.gdk.deploymentlauncher`. This editor-only module includes functionality to:
    - Upload assemblies from the editor.
    - Launch and stop deployments from the editor.
    - View basic information about live deployments (start time, region, number of connected workers) in the editor.

### Changed

- Added a `Improbable.Gdk.Core.Editor` asmdef.
    - Moved `SingletonScriptableObject<T>` from the build system feature module into this assembly and made it public.
    - Pulled out the `UiStateManager` from the `BuildConfigEditor` into this assembly and made it public.
- Exceptions thrown in user-code callbacks no longer cause other callbacks scheduled for that frame to not fire. Instead, the exceptions are caught and logged with Debug.LogException.
- Upgraded the Worker SDK version to `13.7.1`.
- Updated the default method of loading a Development Authentication Token to search for a `DevAuthToken.txt` asset at the root of any `Resources` folder.
- Removed the `AndroidClientWorkerConnector` and `iOSClientWorkerConnector` and their specific scenes. You can now use the `MobileClientWorkerConnector` and its `MobileClientScene` to connect to a mobile device.

### Fixed

- Fixed a bug where if an entity received an event and was removed from your worker's view in the same ops list, the event would not be removed.
- Fixed a bug where clicking on `SpatialOS` > `Generate Dev Authentication Token` would not always refresh the asset database correctly.
- Fixed a bug where requireables on a GameObject linked to the worker entity were not injected properly.
- Fixed a bug where the `DevAuthToken.txt` asset would be imported from an invalid AssetDatabase path.

## `0.2.1` - 2019-04-15

### Breaking Changes

- Removed `clientAccess` from the `AddPlayerLifecycleComponents` signature. We now construct the client access attribute within the helper.

### Added

- Added a static helper in the `EntityTemplate` class to construct worker access attributes.
- Added an optional callback as an argument to the `RequestPlayerCreation` method in `SendCreatePlayerRequestSystem`. This callback is invoked upon receiving a response to a player creation request.
- Added a new Query-based interest helper module, `com.improbable.gdk.querybasedinteresthelper`.
    - `InterestTemplate` provides functionality to ergonomically add, replace and clear queries from an Interest component.
    - `InterestQuery` reduces boilerplate code required to construct interest queries.
    - `Constraint` contains static methods to easily create constraints for an interest query.
- Added a `WithTimeout(TimeSpan timeout)` method to the `RedirectedProcess` class. This allows you to set a timeout for the underlying process execution.
- Added a `Improbable.Gdk.Core.Collections.Result<T, E>` struct to represent a result which can either contain a value `T` or an error `E`.
- Added Scripting Define Symbol `DISABLE_REACTIVE_COMPONENTS`. Using this symbol will disable all reactive componts and systems.
- Added a `WorkerFlagReader` which you can subscribe and `Require`. This allows you to:
    - Add callbacks for changes to worker flags.
    - Read the value of worker flags.

### Changed

- The player lifecycle module now dynamically queries for PlayerCreator entities, and sends requests to a random one each time. This removes the reliance on a hardcoded PlayerCreator Entity ID.
- Removed the `Type` suffix from player lifecycle schema types.
- `RedirectedProcess.RunAsync()` now takes a `CancellationToken?` as a parameter. This token can be used to cancel the underlying process.
- Updated the Unity version to `2018.3.11`.

### Fixed

- Fixed an issue where player creation requests could retry infinitely without logging failure.
- Fixed an issue where if you called `RedirectedProcess.Command(...)` in a non-main thread, it would throw an exception.
- Fixed an issue where having the same name for a schema package and a schema component would lead to generating invalid code.

### Internal

- Tools package now uses PackageManager API instead of parsing manifest.json.
- Updated default snapshot to have more than one PlayerCreator entity.
- Fixed package dependencies.
- Worker flag changes are propagated to the `ViewDiff`.
- Exposed `GetWorkerFlag(string name)` on the `View`.

## `0.2.0` - 2019-03-18

### Breaking Changes

- Changed the format of the BuildConfiguration asset. Please recreate, or copy it from `workers/unity/Playground/Assets/Config/BuildConfiguration.asset`.
- Command request and responses are no longer constructed from static methods `CreateRequest` and `CreateResponse`. Instead, they are constructors that take the same arguments.
- The `Require` attribute has moved from the `Improbable.Gdk.GameObjectRepresentation` namespace to the `Improbable.Gdk.Subscriptions` namespace.
- The generated Readers have been renamed from `{COMPONENT_NAME}.Requirable.Reader` to `{COMPONENT_NAME}Reader`.
- The Reader callback events' names have changed.
    - `On{EVENT_NAME}` is now `On{EVENT_NAME}Event`.
    - `{FIELD_NAME}Updated` is now `On{FIELD_NAME}Update`.
- The generated Writers have been renamed from` {COMPONENT_NAME}.Requirable.Writer` to `{COMPONENT_NAME}Writer`.
- The Writer send method names have changed.
    - `Send{EVENT_NAME}` is now `Send{EVENT_NAME}Event`.
    - `Send` is now `SendUpdate`.
 - The generated command senders in MonoBehaviours have also changed.
    - `{COMPONENT_NAME}.Requirable.CommandRequestSender` and `{COMPONENT_NAME}.Requirable.CommandResponseHandler` have been combined and are now called `{COMPONENT_NAME}CommandSender`.
    - `{COMPONENT_NAME}.Requirable.CommandRequestHandler` is now called `{COMPONENT_NAME}CommandReceiver`.
- When creating GameObjects, the `IEntityGameObjectCreator.OnEntityCreated` signature has changed from `GameObject OnEntityCreated(SpatialOSEntity entity)` to `void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)`.
- The signature of `IEntityGameObjectCreator.OnEntityCreated` has changed from `void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject)` to `void OnEntityRemoved(EntityId entityId)`.
    - All linked `GameObject` instances are still unlinked before this is called, however it is now your responsibility to track if a `GameObject` was created when the entity was added.
    - You should now call `linker.LinkGameObjectToSpatialOSEntity()` to link the `GameObject` to the SpatialOS entity.
    - You should also pass-in a list of `ComponentType` to `LinkGameObjectToSpatialOSEntity` which you wish to be copied from the `GameObject` to the ECS entity associated with the `GameObject`.
        - Note that for the Transform Synchronization feature module to work correctly, you need to set up a linked Transform Component on your GameObject. You also need to link any Rigidbody Component on your GameObject.
    - There is no limit on the number of GameObject instances that you can link to a SpatialOS entity. However, you cannot add a component type to a linked GameObject instance more than once.
    - Deleting a linked GameObject unlinks it from the SpatialOS entity automatically.
- `SpatialOSComponent` has been renamed to `LinkedEntityComponent`.
    - The field `SpatialEntityId` on the `LinkedEntityComponent` has been renamed to `EntityId`.
    - The field `Entity` has been removed.
- The `Improbable.Gdk.Core.Dispatcher` class has been removed.

### Added

- All generated schema types, enums, and types which implement `ISpatialComponentSnapshot` are now marked as `Serializable`.
    - Note that generated types that implement `ISpatialComponentData` are not marked as `Serializable`.
- Added the `DynamicConverter` class for converting a `ISpatialComponentSnapshot` to an `ISpatialComponentUpdate`.
- Added a generated ECS shared component called `{COMPONENT_NAME}.ComponentAuthority` for each SpatialOS component.
    - This component contains a single boolean which denotes whether a server-worker instance has write access authority over that component.
    - The component does not tell you about soft-handover (`AuthorityLossImminent`).
- You may now `[Require]` an `EntityId`, `Entity`, `World`, `ILogDispatcher`, and `WorldCommandSender` in MonoBehaviours.
- Added constructors for all generated component snapshot types.
- Added the ability to send arbitrary serialized data in a player creation request.
    - Replaced `Vector3f` position in `CreatePlayerRequestType` with a `bytes` field for sending arbitrary serialized data.
- Added `RequestPlayerCreation` to manually request for player creation in `SendCreatePlayerRequestSystem`.
- Added a menu item, navigate to **SpatialOS** > **Generate Dev Authentication Token**, to generate a TextAsset containing the [Development Authentication Token](https://docs.improbable.io/reference/latest/shared/auth/development-authentication).
- Added the ability to mark a build target as `Required` which will cause builds to fail in the Editor if the prerequisite build support is not installed.

### Changed

- Upgraded the Worker SDK version to `13.6.2`.
- Improved the UX of the BuildConfiguration inspector.
- Improved the UX of the GDK’s Tools Configuration window.
- Deleting a GameObject now automatically unlinks it from its ECS entity. Note that the ECS entity and the SpatialOS entity are _not_ also deleted.
- Changed the format of the BuildConfiguration asset. Please recreate, or copy it from `workers/unity/Playground/Assets/Config/BuildConfiguration.asset`.
- Building workers will not change the active build target anymore. The build target will be set back to whatever was set before starting the build process.

### Fixed

- Fixed a bug where, from the SpatialOS menu in the Unity Editor, running **SpatialOS ** > **Generate code** would always regenerate code, even if no files had changed.
- Fixed a bug where building all workers in our sample projects would fail if you have Android build support installed but didn't set the path to the Android SDK.
 - Fixed a bug where some prefabs would not be processed correctly, causing `NullReferenceExceptions` in `OnEnable`.

### Internal

- Changed the code generator to use the schema bundle JSON rather than AST JSON.
    - If you have forked the code generator, this may be a breaking change.
- Exposed annotations in the code generator model.
- Added a `MockConnectionHandler` implementation for testing code which requires the world to be populated with SpatialOS entities.
- Added tests for `StandardSubscriptionManagers` and `AggregateSubscription`.
 - Re-added tests for Reader/Writer injection criteria and MonoBehaviour enabling.
- Reactive components have been isolated and can be disabled.
- Subscriptions API has been added, this allows you to subscribe anything for which a manager has been defined.
     - This now backs the `Require` API in MonoBehaviours.
- Low-level APIs have been changed significantly.
- Added a View separate from the Unity ECS.
- Removed unnecessary `KcpNetworkParameters` overrides in `MobileWorkerConnector` where it matched the default values.


## `0.1.5` - 2019-02-18

### Changed

- Changed `RedirectedProcess` to have Builder-like API.
- Upgraded the project to be compatible with `2018.3.5f1`.

### Fixed

- Fixed a bug where launching on Android from the Unity Editor would break if you have spaces in your project path.
- Fixed a bug where a Unity package with no dependencies field in its `package.json` would cause code generation to throw exceptions.
- Fixed a bug where protocol logging would crash Linux workers.

## `0.1.4` - 2019-01-28

### Added

- Added support for the Alpha Locator flow.
- Added support for connecting mobile devices to cloud deployments via the anonymous authentication flow.
- Added option to build workers out via IL2CPP in the cmd.
- Added an example of handling disconnect for mobile workers.
- Added support for launching an Android client from the Editor over ADB.

### Changed

- Upgraded the Worker SDK version to `13.5.1`. This is a stable Worker SDK release! :tada:
- `Improbable.Gdk.EntityTemplate` is now mutable and exposes a set of APIs to add, remove, and replace component snapshots
    - This replaces the `Improbable.Gdk.Core.EntityBuilder` class.
    - These changes also allow you to reuse an `EntityTemplate` more than once.
- Upgraded the project to be compatible with `2018.3.2f1`.
- Upgraded the entities package to `0.0.12-preview.21`
- Disabled protocol logging on Linux workers to prevent crashes. This will be reverted once the underlying issue is fixed.
- Updated the `MobileWorkerConnector` to use the KCP network protocol by default.
- Changed the `mobile_launch.json` config to use the new Runtime.
- Updated all the launch configs to use the new Runtime.
- Changed the build process in the Editor such that it skips builds that don't have build support rather than canceling the entire build process.
    - Note that building via the `Improbable.Gdk.BuildSystem.WorkerBuilder.Build` static method is unchanged.

### Fixed

- `Clean all workers` now cleans worker configs in addition to built-out workers.
- Fixed a bug where you could start each built-out worker only once on OSX.
- Code generation now captures nested package dependencies, so the generated schema contains schema components from all required packages. Previously, code generation only generated schema for top-level dependencies, skipping nested packages.
- Fixed a bug where spaces in the path would cause code generation to fail on OSX.
- Fixed an issue in the TransformSynchronization module where an integer underflow would cause a memory crash.
- Fixed a bug where using `Coordinates`, `Vector3f`, or `Vector3d` in a command definition would cause the Code Generator to crash.

### Removed

- Removed the `Improbable.Gdk.Core.EntityBuilder` class as it was superceded by the updated functionality in `Improbable.Gdk.Core.EntityTemplate`.
    - Removed `CreateSchemaComponentData` from each generated component as it is no longer required by the `EntityBuilder`.
- Removed `com.unity.incrementalcompiler` package as a dependency of the `Core` package.

## `0.1.3` - 2018-11-26

### Added

- Added Frames Per Second (FPS) and Unity heap usage as metrics sent by `MetricSendSystem.cs`.
- Added a warning message to the top of schema files copied into the `from_gdk_packages` directory.
- Added an `ISnapshottable<T>` interface to all generated components. This allows you to convert a component to a snapshot.
- Added an `EntityId` property on the Readers/Writers to access the `EntityId` of the underlying SpatialOS entity.
- Added a `HasEntity` method to the `WorkerSystem`. This allows you to check if an entity is checked out on your worker.
- Added operators and conversion methods to `Coordinates`, `Vector3d`, and `Vector3f` in code generation.
    - This supercedes the `StandardLibraryUtils` feature module which was removed as a consequence.

### Changed

- Improved the method of calculating load and FPS.
- Updated test project Unity version to `2018.2.14f`.
- Upgraded the Worker SDK snapshot version. This entails the following changes:
    - `EntityId` is now in the `Improbable.Gdk.Core` namespace. (Previously `Improbable.Worker`).
    - `Dispatcher` is now in the `Improbable.Gdk.Core` namespace. (Previously `Improbable.Worker`).
    - The `Improbable.Worker.Core` namespace is now `Improbable.Worker.CInterop`.

### Fixed

- Fixed a bug where schema components with a field named `value` would generate invalid code.

### Removed

- Removed the `StandardLibraryUtils` feature module as it was superceded by inserting the methods during code generation.

## `0.1.2` - 2018-11-01

### Added

- Added the ability to acknowledge `AuthorityLossImminent` messages.
- Added an `Open Inspector` button to the `SpatialOS` menu in the Unity Editor.
- Added support for local mobile development.
- Added a changelog.
- Added field level dirty markers in components. This allows for partial automatic component updates to be sent.
- Added full support for `EntityQuery` world commands.
    - Added `Improbable.Gdk.Core.EntityQuerySnapshot` to hold the result of a single entity from a snapshot query.
    - Added `Improbable.Gdk.Core.ISpatialComponentSnapshot` to differentiate between a snapshot of component state and component data.

### Changed

- Changed the allocation type used internally for Unity ECS chunk iteration from `Temp` to `TempJob`
- Running a build in the Editor no longer automatically selects all scenes in the Unity build configuration
- `Improbable.Gdk.Core.Snapshot.AddEntity` now returns the `EntityId` assigned in the snapshot.
- Changed the `WorkerConnector` to be more generic and have an explicit `StandaloneWorkerConnector` for any workers running on OSX/Linux/Windows.
- Updated the default Unity version to `2018.2.14f1`.

### Fixed

- Fixed a bug where deserialising multiple events in a single component update only returned N copies of the last event received, where N is the number of events in the update.
- Fixed a broken link to the setup guide in an error message.

## `0.1.1` - 2018-10-19

### Added

- Better error messages when missing build support for a target platform.
- Better error messages for common problems when downloading the Worker SDK.

### Changed

- Position updates are now sent after all other updates.
- Simplified the heartbeating system in the `PlayerLifecycle` feature module.
- Updated the `README` and "Get Started" guide.

### Fixed

- The `GameLogic` worker is run in headless mode.
- The `Clean All Workers` menu item now works.

## `0.1.0` - 2018-10-10

The initial alpha release of the SpatialOS GDK for Unity.
