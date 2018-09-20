**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----
## Building workers

Before running a deployment of your game, you need to configure your [workers](https://docs.improbable.io/reference/latest/shared/glossary#worker), configure the build of your workers and also build the workers. This applies to both server-workers and client-workers.

### Building the configuration of your workers
To create the correct configurations for each of your server-workers and client-workers, cou can do either:

* From the Unity Editor menu, select **SpatialOS** > **Build worker configs**.
    or
* In a terminal window, from the root of your game project, run:  `spatial build build-config` in the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

### Configuring the build of your workers
To configure the build of your server-workers and client-workers you can create a new [Scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly.

To do this:
1. In your Unity Editor, go to the Project window and select  **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.
1. Click on this Asset to view it in the Unity Editor Inspector. Here you can configure each server-worker and client-worker and for both local and cloud deployments. You can configure:
    * Which Scenes each worker should contain in its build.
    * Which platforms you want to build it for. (We currently support Windows, Linux and OSX.)
    * Which build options you want enabled.

[//]: # (Document the options UTY-1168)

> Note: You cannot configure the build of your workers via the  [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

### Building your workers
To build your workers:
From the Unity Editor menu, select **SpatialOS** > **Build <WorkerName> for local** or  **Build <WorkerName> for  cloud** where `<WorkerName>` is the name of any worker you could build.
This starts the build process for the worker you selected and deployment type (cloud or local). When the build is done, there is a message in the Unity Editor Console, you can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity  project.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)
