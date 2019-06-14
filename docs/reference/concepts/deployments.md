<%(TOC)%>

# Deployments

When you want to try out your game, you need to run a deployment of the game. This means launching SpatialOS itself. SpatialOS sets up the game world and optionally starts up the server-workers needed to run the game world. 

Once the deployment is running, you can connect clients to it in order to play the game. 

You can run a deployment on your development machine (a "local deployment") or in the cloud (a "cloud deployment").

Before you deploy your game, you need to build its workers; see documentation on [building workers]({{urlRoot}}/modules/build-system/editor-menu).

## Configuring your deployment

To ensure the SpatialOS Runtime starts your [server-workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing#server-workers) correctly, you need to ensure the launch configuration file has the worker set up correctly.

 See the [Worker launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration) in the SpatialOS documentation for guidance on how to define the worker launch configurations for both server-workers and client-workers.

## Local deployment

To start a local deployment, in your Unity Editor, from the menu select **SpatialOS** > **Local Launch**.

This starts a local version of the SpatialOS Runtime on your development machine together with all server-workers specified in the launch configuration you used.

<%(Callout message="Its done when you see the following message in the console window which opens:<br/><br/>```SpatialOS ready. Access the Inspector at https://localhost:21000/inspector```")%>

### Start a standalone client worker

The GDK provides tooling for starting client workers from the Unity Editor. 

First, ensure that have built the worker by selecting **SpatialOS** > **Build for local** > **UnityClient**. This may take a few minutes depending on the size of your Unity project.

Then, in your Unity Editor, from the menu select **SpatialOS** > **Launch standalone client**.

## Cloud deployment

Check out our [Deployment Launcher feature module]({{urlRoot}}/modules/deployment-launcher/overview) for details on launching a cloud deployment.
