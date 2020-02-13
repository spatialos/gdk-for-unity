# Command line interface

The build system exposes a command line interface (CLI) that you can use to build your GDK for Unity workers. This is useful for building your project on a headless machine or in a continuous integration context.

We expose a single static method: [`Improbable.Gdk.BuildSystem.WorkerBuilder.Build`]({{urlRoot}}/api/build-system/worker-builder) for use in the CLI. It accepts the following arguments:

- `buildWorkerTypes`: REQUIRED. The type of the worker to build.
- `buildEnvironment`: REQUIRED. The target for the worker build. Either `local` or `cloud`.
- `scriptingBackend`: Optional. Which scripting backend to use for the build. Either `mono` or `il2cpp`. If omitted, this defaults to your current configuration.
- `buildTargetFilter`: Optional. A comma delimited list of build targets to filter for. For example, "win,macos". If ommitted, this defaults to all targets for the given environment.

## Example

To invoke this method, you need to invoke Unity over the command line like the following:

```bash
Unity.exe -projectPath "${GDK_PROJECT_PATH}" \
    -batchmode \
    -quit \
    -logfile ${LOG_FILE_PATH} \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityClient" \
    +buildEnvironment "cloud" \
    +scriptingBackend "mono"
```

This command builds the `UnityClient` worker type for cloud with the `mono` scripting backend.

> **Note:** `Unity.exe` may not be in your path. You may need to call it via a fully qualified path.

### Filtering build targets

To build workers for a specific build target of a build environment, you need to provide the `buildTargetFilter` argument like the following:

```bash
Unity.exe -projectPath "${GDK_PROJECT_PATH}" \
    -batchmode \
    -quit \
    -logfile ${LOG_FILE_PATH} \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityClient" \
    +buildEnvironment "cloud" \
    +scriptingBackend "mono" \
    +buildTargetFilter "macos"
```

This command builds the `UnityClient` worker type for cloud with the `mono` scripting backend, but only for MacOS. A `UnityClient` for Windows is _not_ built.

> **Note:** The build target filter must be a subset of the available build targets for a given worker type on a given environment. Otherwise, an error will be thrown.

### Enforcing iOS SDK target

When building for iOS you can use the `targetiOSSdk` argument to set the target SDK to `simulated` or `device`.

The argument can be provided as following:

```bash
Unity.exe -projectPath "${GDK_PROJECT_PATH}" \
    -batchmode \
    -quit \
    -logfile ${LOG_FILE_PATH} \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "MobileClient" \
    +buildEnvironment "cloud" \
    +scriptingBackend "il2cpp" \
    +buildTargetFilter "ios" \
    +targetiOSSdk "simulator"
```

> **Note:** This argument is only valid when building iOS workers and will be ignored otherwise.
