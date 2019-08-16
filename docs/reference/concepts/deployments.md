<%(TOC)%>

# Deployments

<%(Callout type="warn" message="Before you deploy your game, you need to build its workers. See the documentation on [building workers]({{urlRoot}}/modules/build-system/editor-menu).")%>

When you want to try out your game, you need to run a deployment of the game. This means launching SpatialOS itself. SpatialOS sets up the game world and optionally starts up the server-workers needed to run the game world.

Once the deployment is running, you can connect clients to it in order to play your game.

You can run a deployment on your development machine (a "local deployment") or in the cloud (a "cloud deployment").

## Configuring your deployment

Before you launch a deployment, you need to ensure that it is configured correctly. There are two parts to configuring a deployment: the launch configuration and the worker configurations.

### Launch configuration

The launch configuration file specifies the parameters of your game world as well as your load balancing configuration. The load balancing configuration determines the worker-instances that the SpatialOS Runtime starts and how they are balanced across your game world.

For more information, see the [comprehensive launch configuration documentation](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/launch-config#launch-configuration-file).

> **Note:** If the SpatialOS Runtime fails to start a worker in a deployment that you were expecting, double check that your load balancing configuration is correct.

### Worker configuration

The worker configuration file specifies the parameters of a specific worker-type as well as describing how the SpatialOS Runtime should start them. 

For more information, see the [comprehensive worker configuration documentation](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/introduction#configuration-file).

> **Note:** The `build` section of the worker configuration files are **not** used by the GDK for Unity.

## Local deployment

Before starting a local deployment, you can set a custom snapshot as your deployment's launch snapshot. To do so select **SpatialOS** > 

To start a local deployment, in your Unity Editor, select **SpatialOS** > **Local Launch**.

This starts a local version of the SpatialOS Runtime on your development machine together with all server-workers specified in the launch configuration you selected.

<%(Callout message="Its done when you see the following message in the console window which opens:<br/><br/>```SpatialOS ready. Access the Inspector at https://localhost:21000/inspector```")%>

> **Note:** You can only have one instance of a local deployment running at any given time. If you already have one instance running, you may see an error like: `listen tcp :22000: bind: Only one usage of each socket address (protocol/network address/port) is normally permitted.`

### Start a standalone client worker

The GDK provides tooling for starting client workers from the Unity Editor.

First, ensure that you have built the worker by selecting **SpatialOS** > **Build for local** > **UnityClient**. This may take a few minutes depending on the size of your Unity project.

Then, select **SpatialOS** > **Launch standalone client** to launch the built out client.

## Cloud deployment

Check out our [Deployment Launcher feature module]({{urlRoot}}/modules/deployment-launcher/overview) for details on launching a cloud deployment.
