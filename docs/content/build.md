[//]: # (Doc of docs reference 14.1)
[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)

# How to build your game

Before running a [deployment of your game]({{urlRoot}}/content/deploy) locally or in the cloud, you need to:

1. Build the bridge configuration and launch configuration of your [workers]({{urlRoot}}/content/workers/workers-in-the-gdk).
1. Prepare the build configuration of your workers.
1. Build your workers.


## Building the bridge and launch configurations of your workers
A [SpatialOS deployment](glossary#spatialos-deployment) requires [bridge](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config) and [launch](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration) configurations (“worker configs” in short) to be built for all of its server-workers and client-workers.

The worker configs are built using information contained within [worker configuration files](link to something about jsons here). You can find examples of worker configurations in the [example project](link).

To build the worker configs, you can either:


* With your project open in the Unity Editor, select **SpatialOS** > **Build worker configs**.

    or
* In a terminal window, from the root of your project, run: `spatial build build-config` via the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

> **It’s finished building when:** there is a message stating `'spatial build build-config' succeeded`.

## Preparing the build configuration of your workers

To prepare the build of your server-workers and client-workers you can create a new [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly.

To do this:

1. With your project open in the Unity Editor, go to the Project window and select **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.
2. Select this Asset to view it in the Unity Editor Inspector. Here you can configure each server-worker and client-worker for both local and cloud deployments. You can configure:
    * Which scenes each worker should contain in its build.
    * Which platforms you want to build it for.
    * Which build options you want enabled. You can select the following build options:
      * [Development Build](https://docs.unity3d.com/ScriptReference/BuildOptions.Development.html)
      * [Headless Mode](https://docs.unity3d.com/ScriptReference/BuildOptions.EnableHeadlessMode.html): This is only available for Linux builds. If you want to reproduce the same behaviour for Windows or OSX, start your built-out workers with the following parameters: `-batchmode -nographics`. This can be configured in the [launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration).

## Building your workers

To build your workers:

* With your project open in the Unity Editor, select **SpatialOS** > **Build for local** > **`<WorkerType>`** or **SpatialOS** > **Build for cloud** > **`<WorkerType>`**, where **`<WorkerType>`** is the type of worker you want to build.

This starts the build process for the worker and deployment type (cloud or local) you selected.

> **It’s finished building when:** there is a message stating `Completed build for Local/Cloud target`.

You can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.
