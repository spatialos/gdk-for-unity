[//]: # (Doc of docs reference 14.1)
[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)

<%(TOC)%>
# Build your workers

Before running a [deployment of your game]({{urlRoot}}/content/deploy) locally or in the cloud, you need to:

1. Build the bridge configuration and launch configuration of your [workers]({{urlRoot}}/content/workers/workers-in-the-gdk).
1. Prepare the build configuration of your workers.
1. Build your workers.

## Building the bridge and launch configurations of your workers

A [SpatialOS deployment]({{urlRoot}}/content/glossary#deploying) requires [bridge](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config) and [launch](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration) configurations (commonly referred to as “worker configs”) to be built for all of its server-workers and client-workers.

Worker configs are built using information contained within [worker configuration files](https://docs.improbable.io/reference/latest/shared/glossary#worker-configuration-worker-json). You can find examples of worker configurations for [client-workers](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/spatialos.UnityClient.worker.json) and [server-workers](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/spatialos.UnityGameLogic.worker.json) in the [Unity project](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity) in this repository.

To build the worker configs, you can either:

* With your project open in your Unity Editor, select **SpatialOS** > **Build worker configs**.

    or
* In a terminal window, from the root of your project, run: `spatial build build-config` via the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

> **It’s finished building when:** there is a message stating `'spatial build build-config' succeeded`.

<%(Callout type="alert" message="If you encounter build errors, you might not have selected the build supports your game needs during your Unity setup. <br/><br/>
* You need **Linux** build support. This is because server-workers in a cloud deployment run in a Linux environment. In the `Assets/Fps/Config/BuildConfiguration`, do not change the `UnityGameLogic Cloud Environment` from Linux.<br/> <br/>
* You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
* You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
Unity gives you build support for your development machine (Windows or Mac) by default.")%>


## Preparing the build configuration of your workers

To prepare the build of your server-workers and client-workers you can create a new [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly.

To do this:

1. With your project open in your Unity Editor, go to the Project window and select **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.
1. Select this Asset to view it in your Unity Editor Inspector. Here you can configure each server-worker and client-worker for both local and cloud deployments. You can configure:
    * Which Scenes each worker should contain in its build.
    * Which platforms you want to build it for.
    * Which build options you want enabled. You can select the following build options:
      * [Development](https://docs.unity3d.com/ScriptReference/BuildOptions.Development.html)
      * [Server Build](https://docs.unity3d.com/ScriptReference/BuildOptions.EnableHeadlessMode.html)
      * [Compression](https://docs.unity3d.com/Manual/BuildSettings.html)

## Building your workers

To build your workers:

* With your project open in your Unity Editor, select **SpatialOS** > **Build for local** > **`<WorkerType>`** or **SpatialOS** > **Build for cloud** > **`<WorkerType>`**, where **`<WorkerType>`** is the type of worker you want to build.

This starts the build process for the worker and deployment type (cloud or local) you selected.

> **It’s finished building when:** there is a message stating `Completed build for Local/Cloud target`.

You can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.

#### TIP: Speed up development iteration with worker quick-run

 When you are developing with the GDK, you don't need to build out workers all the time, you can use quick-run to run multiple workers in your Unity Editor using Ctrl+L (Windows) or Cmd+L (Mac).

During development you use a local deployment rather than a cloud deployment. In a local deployment, you can either build your workers to run locally or use quick-run. With quick-run you can run multiple workers in your Unity Editor, so you don't have to keep building out workers during development iteration.  
 
 To use quick-run:

1. With your project open in your Unity Editor, on your computer’s keyboard, input Ctrl+L (Windows) or Cmd+L (Mac).
1. Wait until you see a message in the Editor’s Console window that SpatialOS is ready. The message is: `SpatialOS ready. Access the inspector at http://localhost:21000/inspector.`
1. In your Unity Editor, play your game's Scene.
