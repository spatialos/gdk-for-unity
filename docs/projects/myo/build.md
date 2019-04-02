<%(TOC)%>
# Build your workers

Before running a [deployment of your game]({{urlRoot}}/reference/concepts/deployments) locally or in the cloud, you need to setup a build configuration [scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html).

**Step 1.** Create a build configuration asset.

With your project open in your Unity Editor, go to the Project window and select **Assets** > **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`.

**Step 2.** Select this asset to view it in the Unity Editor Inspector.

**Step 3.** Configure your build.

The build configuration has a reasonable default configuration, however, you may want to make changes.

Please see our [Build System Feature Module documentation]({{urlRoot}}/modules/build-system/overview) to learn how to configure the build.

**Step 4.** Test building your workers.

With your project open in your Unity Editor, select: **SpatialOS** > **Build for local** > **All workers**.

<%(Callout message="**It’s finished building when:** there is a message stating `Completed build for local`.")%>

You can now find the built-out workers in `build/assembly/worker` in the root of the SpatialOS GDK for Unity project.

For completeness, lets test building the cloud configuration as well; with your project open in your Unity Editor, select: **SpatialOS** > **Build for cloud** > **All workers**.

<%(Callout message="**It’s finished building when:** there is a message stating `Completed build for cloud`.")%>

# Next steps

This concludes setting up a new project with the SpatialOS GDK for Unity!

The next thing you'll want to do is get your workers to connect to the SpatialOS runtime. Check out [the WorkerConnector]({{urlRoot}}/reference/workflows/monobehaviour/creating-workers) documentation to learn how to do this.