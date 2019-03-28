[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)

<%(TOC)%>
# Deploy your game

When you want to try out your game, you need to run a deployment of the game. This means launching SpatialOS itself. SpatialOS sets up the game world and optionally starts up the server-workers needed to run the game world. Once the deployment is running, you can connect clients to it in order to play the game. You can run a deployment on your development machine (a "local deployment") or in the cloud (a "cloud deployment").

Before you deploy your game, you need to build its workers; see documentation on [building workers]({{urlRoot}}/projects/myo/build).

## Configuring your deployment

To ensure the SpatialOS Runtime starts [server-workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing#server-workers) correctly, you need to ensure the launch configuration file has the worker set up correctly. See the [Worker launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration) in the SpatialOS documentation for guidance on how to define the worker launch configurations for both server-workers and client-workers.

## Local deployment

To start a local deployment, either:

* In your Unity Editor, from the menu select **SpatialOS** > **Local Launch**. (This runs the default launch configuration.)

    or
* Open a terminal window and from the root of your game project directory run `spatial local launch` via the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

(See the [SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) for details of the launch configurations.)

This starts a local version of the SpatialOS Runtime on your development machine together with all server-workers specified in the launch configuration you used.

It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.

To start your client-workers, from a terminal window, in any directory, run `spatial local worker launch <YourExternalWorker> <YourLaunchConfig>`, replacing the `<example-content>` with names relevant to your game. Use the [Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector) to look at the current state of your game world.

## Cloud deployment

Follow this section to start a cloud deployment.

### Setup the SpatialOS project name

You are allocated an empty SpatialOS project in the cloud when you sign up to SpatialOS; you use this to deploy your game, but to do this you need to tell the GDK the name of your allocated SpatialOS project so it knows where to deploy your game to.

1. Open the `spatialos.json` file in the root folder of your game.
1. Change the `name` field so it matches the name of your SpatialOS project. You can find this in the SpatialOS [Console](https://console.improbable.io). It’ll be something like `beta_someword_anotherword_000`.

### Build and upload the game assembly

The assembly includes executable files for the client-workers and server-workers, as well as the assets both types of workers use. (The assets might be the models and textures that the client-servers - that is, the game executable code - use to visualise the game.)

  1. To build an assembly; in your Unity Editor, select **SpatialOS** > **Build all workers for cloud**.
  1. To upload an assembly; open a terminal and navigate to the directory your game is in (the repository you’ve cloned). Run `spatial cloud upload <assembly name>`. The `<assembly name>` is a label you create so you can identify this assembly in the next step - for example you could call it `MyGDKAssembly`.

  > **It’s finished uploading when:** You see a successful upload report printed in your terminal output, and your automatically opens [https://console.improbable.io](https://console.improbable.io) displaying the deployment.

### Launch a cloud deployment

1. Launch a cloud deployment

   * In the same terminal window, run `spatial cloud launch <assembly name> cloud_launch.json <deployment name> --snapshot=snapshots/default.snapshot`
   * This command defaults to deploying to clusters located in the US. If you’re in Europe, add the `--cluster_region=eu` flag for lower latency.
   * It’s done when you see `Deployment launched successfully` printed in your terminal output. This might take a couple of minutes.

**About the `spatial cloud launch` command**   
`spatial cloud launch` deploys a project to the cloud. Its full syntax is:

```
spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>
```

where:

  * `<assembly name>` identifies the worker assemblies to use. The name needs to conform to the following regex: `[a-zA-Z0-9_.-]{5,64}`
  * `<launch configuration>` is the configuration file for the deployment. This project includes `default_launch.json`, which is intended for use with local deployments, and `cloud_launch.json`, which is for cloud deployments.
  * `<deployment name>` is a name of your choice, which is used to identify the deployment. The name needs to conform to the following regex: `[a-z0-9_]{2,32}`
  * `<snapshot file>` is the snapshot of the world you want to start from. See this [documentation]({{urlRoot}}/content/snapshots) for further information.

### Launch the game client

  1. Open the SpatialOS [Console](https://console.improbable.io/projects). You’ll see the project and the deployment you just created.
  1. In the SpatialOS Console, select the deployment’s name to open the overview page.
  1. Select **Launch**.
  
  > You can ignore the prompt to install the Launcher, as it’s installed as part of the SpatialOS Installer.
  
  1. To get links to share the game with others, select **Share**.
  1. Once you’ve finished playing, select **Stop** in the Console.
