**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

---

## How to build your game

Before running a [deployment of your game](deploy.md) locally or in the cloud, you need to:

1. Build your workers.
2. Build the bridge configuration and launch configuration of your [workers](workers.md).
3. Prepare the build configuration of your workers.

### Building your workers

To build your workers:

* With your project open in the Unity Editor, select **SpatialOS** > **Build `<WorkerName>` for local** or **Build `<WorkerName>` for cloud** where `<WorkerName>` is the name of any worker you want to build.

This starts the build process for the worker you selected and deployment type (cloud or local).

> **It’s finished building when:** there is a message in the Unity Editor console.

You can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.

### Building the bridge and launch configurations of your workers

The [bridge](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-bridge-configuration) and [launch](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration) configurations are pre-configured for you. You don't need to understand how they work, you only need to build them. You do this for each of your [server-workers](https://docs.improbable.io/reference/latest/shared/glossary#server-worker) and [client-workers](https://docs.improbable.io/reference/latest/shared/glossary#client-worker). To do this you can either:

* With your project open in the Unity Editor, select **SpatialOS** > **Build worker configs**.

    or
* In a terminal window, from the root of your project, run: `spatial build build-config` via the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

> **It’s finished building when:** there is a message in the Unity Editor console.

### Preparing the build configuration of your workers

To prepare the build of your server-workers and client-workers you can create a new [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly.

To do this:

1. With your project open in the Unity Editor, go to the Project window and select **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.
2. Click on this Asset to view it in the Unity Editor Inspector. Here you can configure each server-worker and client-worker for both local and cloud deployments. You can configure:
    * Which Scenes each worker should contain in its build.
    * Which platforms you want to build it for.
    * Which build options you want enabled.

> Note: You cannot create the SpatialOS Build Configuration via the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).
> 
---
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)
