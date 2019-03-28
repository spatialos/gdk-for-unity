<%(TOC)%>
# Build your workers

Before running a [deployment of your game]({{urlRoot}}/content/deploy) locally or in the cloud, you need to setup a build configuration [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html).

**Step 1.** Create a build configuration asset.

With your project open in your Unity Editor, go to the Project window and select **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.

**Step 2.** Select this asset to view it in the Unity Editor Inspector.

**Step 3.** Configure your build.

The build configuration has a reasonable default configuration, however, you may want to make changes.

Each worker type has both a local and cloud configuration meant for local deployment development and cloud deployment development respectively.
You can configure:

* Which Unity Scenes each worker should contain in its build.
* Which platforms you want your worker to build for.
* A set of build options. Refer to the table for the description of these.

| | |
|---|---|
| **Build Option** | **Description** |
| Build | Denotes whether to build this target or not. |
| Required | Denotes whether a build failure while building this target should trigger a build-wide failure. |
| Development | Denotes whether the build should be a development build with debug symbols. |
| Server build | Denotes whether the worker is running in headless mode. |
| Compression | Which compression scheme to use in the build. |


> **Note:** All server-workers **must** have a Linux build target enabled for the cloud target because server-workers are ran on Linux machines in cloud deployments.

**Step 4.** Test building your workers.

With your project open in your Unity Editor, select: **SpatialOS** > **Build for local** > **All workers**.

<%(Callout message="**It’s finished building when:** there is a message stating `Completed build for local`.")%>

You can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.

For completeness, lets test building the cloud configuration as well; with your project open in your Unity Editor, select: **SpatialOS** > **Build for cloud** > **All workers**.

<%(Callout message="**It’s finished building when:** there is a message stating `Completed build for cloud`.")%>

# Next steps

This concludes setting up a new project with the SpatialOS GDK for Unity!

The next thing you'll want to do is get your workers to connect to the SpatialOS runtime. Check out [the WorkerConnector]({{urlRoot}}/content/gameobject/creating-workers-with-workerconnector) documentation to learn how to do this.