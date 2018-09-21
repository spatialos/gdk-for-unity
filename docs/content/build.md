**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----
## Building your workers

Before running a [deployment of your game](deploy.md), you need to:
1. Build the bridge and launch configuration of your [workers](workers.md)
2. Prepare the build configuration of your workers
3. Build your workers

To build your workers, you don't need to know what the bridge and launch configurations are, but if you do want to know more about them, see the SpatialOS documentation on [bridge configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#bridge-configuration) and [launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration).

### Building the bridge and launch configuration of your workers
To build the bridge and launch configurations for each of your server-workers and client-workers, you can either:

* From the Unity Editor menu, select **SpatialOS** > **Build worker configs**.

    or
* In a terminal window, from the root of your game project, run:  `spatial build build-config` in the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

### Preparing the build configuration of your workers
To prepare the build of your server-workers and client-workers you can create a new [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly.

To do this:
1. In your Unity Editor, go to the Project window and select  **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.
1. Click on this Asset to view it in the Unity Editor Inspector. Here you can configure each server-worker and client-worker for both local and cloud deployments. You can configure:
    * Which Scenes each worker should contain in its build.
    * Which platforms you want to build it for.
    * Which build options you want enabled.

[//]: # (Document the options UTY-1168)

### Building your workers
To build your workers:

* From the Unity Editor menu, select **SpatialOS** > **Build <WorkerName> for local** or  **Build <WorkerName> for  cloud** where `<WorkerName>` is the name of any worker you want to build.

This starts the build process for the worker you selected and deployment type (cloud or local). When the build is done, there is a message in the Unity Editor console and you can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.

> Note: You cannot prepare the build of your workers via the  [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)
