# Command line interface

The build system also exposes a command line interface (CLI) that you can use to start worker builds. This is useful for building your project on a headless machine or in a continuous integration context.

We expose a single static method: [`Improbable.Gdk.BuildSystem.WorkerBuilder.Build`]({{urlRoot}}/api/build-system/worker-builder) for use in the CLI. It accepts the following arguments:

- `buildWorkerTypes`: REQUIRED. The type of the worker to build.
- `buildTarget`: REQUIRED. The target for the worker build. Either `local` or `cloud`.
- `scriptingBackend`: Optional. Which scripting backend to use for the build. Either `mono` or `il2cpp`. If ommitted, this defaults to `mono`.

## Example

To invoke this method, you need to invoke Unity over the command line like the following:

```bash
Unity.exe -projectPath "${GDK_PROJECT_PATH}" \
    -batchmode \
    -quit \
    -logfile ${LOG_FILE_PATH} \
    -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityClient" \
    +buildTarget "cloud" \
    +scriptingBackend "mono"
```

This command builds the `UnityClient` worker type for cloud with the `mono` scripting backend.

> **Note:** `Unity.exe` may not be in your path. You may need to call it via a fully qualified path.