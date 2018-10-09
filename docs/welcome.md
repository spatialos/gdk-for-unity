# Welcome to the SpatialOS GDK for Unity!

The SpatialOS Game Development Kit (GDK) for Unity allows you to quickly and easily build and host Unity multiplayer games. These games can use multiple server-side game engines across one seamless world to create new kinds of gameplay.

<img src="{{assetRoot}}assets/GDK-Architecture-Diagram.jpg" style="float: right; width: 60%; margin: 0 0 0 0;" />

The GDK is composed of three layers:

* **The GDK Core:** a performant, data-driven integration with our cloud platform SpatialOS, based on the familiar Unity-native workflows.
* **The GDK Feature Modules:** a library of solutions for hard or common networked game development problems, such as Character Movement and Shooting.
* **The First Person Shooter Starter Project (FPS):** a starting sandbox project for the GDK that enables you and your friends to experience the true scale of SpatialOS, providing a solid foundation for entirely new games.

### Getting started

Get started with SpatialOS and Unity by launching a cloud deployment of our FPS Starter project. This will introduce you to the workflows and tooling you’ll use with SpatialOS and Unity to allow you to run your game at scale with up to 200 simulated player clients running in the cloud.

You’ll then be ready to learn how to build your own features. Our tutorial for adding Health Pickups functionality will introduce you to this development experience.

![Instant action in the FPS Starter Project]({{assetRoot}}assets/fps/GDK-FPS-Sights.png)

### Setup

<%(Callout type="info" message="Setting up SpatialOS, the GDK, the FPS Starter Project, and their dependencies should take you about 10 mins.")%>

Follow our [setup guide]({{urlRoot}}/setup-and-installing) to download and install the GDK and its dependencies.

Next, please clone the FPS starter project as follows:

1. Follow the [machine setup guide]({{urlRoot}}/setup-and-installing#set-up-your-machine).
2. Clone the [GDK for Unity FPS Starter Project](https://github.com/spatialos/gdk-for-unity-fps-starter-project) repository:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/gdk-for-unity-fps-starter-project.git` |
    | HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-fps-starter-project.git` |
3. Setup dependencies by either:
    - Running the bundled scripts in the `gdk-for-unity-fps-starter-project` repository: `powershell scripts/powershell/setup.ps1` (Windows) or `bash scripts/shell/setup.sh` (Mac).
    - Manually by following the instructions below.

<%(#Expandable title="Manually setup dependencies")%>

1. Clone the [GDK for Unity](https://github.com/spatialos/gdk-for-unity) repository alongside the FPS Starter Project so that they sit side-by-side:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |
    | HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
    > The two repositories should share a common parent like the following:
    ```text
    <common_parent_directory>
        ├── gdk-for-unity-fps-starter-project
        ├── gdk-for-unity
    ```

2. Navigate to the `gdk-for-unity` directory and checkout the pinned version which can be found in the `gdk.pinned` file in the root of the `gdk-for-unity-fps-starter-project` directory.
    - `git checkout <pinned_version>`

<%(/Expandable)%>

#### Fix shader bugs (optional)

There is a bug in the current preview version of the [High Definition Render Pipeline](https://blogs.unity3d.com/2018/03/16/the-high-definition-render-pipeline-focused-on-visual-quality/), where shaders do not fully compile and appear visually darker than intended.

There is a quick fix however:

1. Open the FPS Starter Project in the Unity Editor.
2. In the Project panel, navigate to Assets > Fps > Art > Materials.
3. Right click on `Source_Shaders` and press Reimport.

<img src="{{assetRoot}}assets/shader-fix.jpg" style="margin: 0 auto; display: block;" />

### Building Workers

As you will be launching a cloud deployment, you need to build out the code executables which will be run by SpatialOS servers - these are called [workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing).

<!-- !!!! TODO: <Explain to people how to open the project in Unity - eg that you have to go to /workers to find the Unity project - just in case>
 !!!! -->

In the Unity Editor, you can build your workers from the SpatialOS menu by clicking **Build for cloud** > **All workers**.

<!-- TODO: Image of Unity editor SpatialOS menu  -->

(Note that while you are developing locally with the GDK you can skip building executables, since both of your workers can run in the editor.)

## Uploading

### Setting your project name (first time only)

Your SpatialOS account is associated with an organisation and a project which were created for you when you signed up, both with generated names.

You’ll need your generated project name to deploy the FPS game to the cloud. To find it, enter the [Web Console](https://console.improbable.io/projects) and look for the `beta_randomword_anotherword_randomnumber` project name:

<img src="{{assetRoot}}assets/project-page.png" style="margin: 0 auto; display: block;" />

Using a text editor of your choice, open the `gdk-for-unity-fps-starter-project/spatialos.json` file and replace the `unity_gdk` project name with the project name you were assigned in the Web Console. This will let the SpatialOS platform know which project you intend to upload to.

### Upload worker assemblies

An [assembly](https://docs.improbable.io/reference/latest/shared/glossary#assembly) is a bundle of code, art assets and other files necessary to run your game in the cloud.

To run a deployment in the cloud, you must upload the worker assemblies to your SpatialOS project. This can only be done in a terminal, via the [spatial CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli). You must also give the worker assemblies a name so that you can reference them when launching a deployment.

Using a terminal of your choice - for example, the Command Prompt for Windows - navigate to `gdk-for-unity-fps-starter-project/` and run `spatial cloud upload <assembly_name>`, where `assembly_name` is a name of your choice (e.g fps-assembly).

> **It’s finished uploading when:** You see an upload report printed in your terminal output, for example:

```
Upload report:
6 artifacts uploaded (3 successful, 3 skipped, 0 failed)
```

Based on your network speed, this may take a little while (1-10 mins) to complete.

## Launch a cloud deployment

The next step is to [launch a cloud deployment](https://docs.improbable.io/reference/latest/shared/deploy/deploy-cloud#5-deploy-the-project) using the assembly that you just uploaded. This can only be done through the spatial CLI.

When launching a cloud deployment you must provide three parameters:

* **the assembly name**, which identifies the worker assemblies to use
* **a launch configuration**, which declares the world and load balancing configuration
* **a name for your deployment**, which is used to label the deployment in the SpatialOS console.

Using a terminal of your choice, navigate to the root directory of your SpatialOS project and run `spatial cloud launch --snapshot=snapshots/default.snapshot <assembly_name> cloud_launch_large.json <deployment_name>` where `assembly_name` is the name you gave the assembly in the previous step and `deployment_name` is a name of your choice (e.g shootyshooty).

This command defaults to deploying to clusters located in the US. If you’re in Europe, add the `--cluster_region=eu` flag for lower latency.

> **It's finished when:** You see `Deployment launched successfully` printed in your terminal console output.

## Time to Play!

Back in your [Console](https://console.improbable.io/projects), you should now see the deployment that you just created appear under your project. Select it to get to the Overview page:

<img src="{{assetRoot}}assets/overview-page.png" style="margin: 0 auto; width: 100%; display: block;" />

Hit the Play button on the left, and then Launch (you can skip Step 1 - the SpatialOS Launcher was previously installed during setup). The SpatialOS Launcher will download the game client for this deployment and launch it.

<img src="{{assetRoot}}assets/launch.png" style="margin: 0 auto; display: block;" />

Once the client has launched, enter the game and fire a few celebratory shots - you are now playing in your first SpatialOS cloud deployment!

<img src="{{assetRoot}}assets/client-launched.png" style="margin: 0 auto; display: block;" />

It’s a bit lonely in there isn’t it? Keep your client running while we get this world populated.

## Inviting friends

To invite other players to this game, head back to the Deployment Overview page in your console, and select the Share button:

<img src="{{assetRoot}}assets/overview-page-share.png" style="margin: 0 auto; display: block;" />

This generates a short link to share with anyone who wants to join in for the duration of the deployment, providing them with Launcher download instructions and a button to join the deployment.

<img src="{{assetRoot}}assets/share-modal.png" style="margin: 0 auto; display: block;" />

## Inviting enemies...

**For more of a challenge, let’s now invite 200 enemies you can fight it out against!**

These enemies will be Unity Clients running in the cloud, mimicking real players of your game from a behaviour and load perspective. Their behaviour is currently quite simple, but could be extended to include additional gameplay features.

In fact, as far as SpatialOS is concerned, these Clients  are indistinguishable from real players, so this is a good approach for regular scale testing.

To get the legion of enemies started, we will use [Worker Flags](https://docs.improbable.io/reference/latest/shared/worker-configuration/worker-flags#worker-flags), which you can find from your Deployment Overview page:

<img src="{{assetRoot}}assets/overview-page-worker-flags.png" style="margin: 0 auto; display: block;" />

Modify the `fps_fake_clients_per_coordinator` flag value from 0 to 10 and hit save:

<img src="{{assetRoot}}assets/worker-flags-modification.png" style="margin: 0 auto; display: block;" />

What this will do is start up 10 simulated player clients per Fake Client Coordinator worker (of which there are 20 running in the deployment), and they will connect in every 2 seconds (dictated by the `fps_fake_client_creation_interval` flag).

Back in the game, you will soon see the new simulated player clients running. Try to find them before they find you…

<img src="{{assetRoot}}assets/getting-shot.png" style="margin: 0 auto; display: block;" />

## Observing your deployment

Let’s take a look at how many simulated player clients are now running around this world and how our deployment is performing, using the Inspector accessible from the Deployment Overview page:

<img src="{{assetRoot}}assets/overview-page-inspector.png" style="margin: 0 auto; display: block;" />

The [World Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector#inspector) provides a real time view of what’s happening in a deployment, from the [perspective of SpatialOS](https://docs.improbable.io/reference/latest/shared/concepts/spatialos): where all the entities are, what their components are, which workers are running and which entities they are reading from and writing to.

We can use it, for instance, to highlight where all the Simulated Players and Player Entities are in the world (note: not cool to identify where you friends are hiding)

<img src="{{assetRoot}}assets/inspector-fake-players.jpg" style="margin: 0 auto; display: block;" />

[The Logs app](https://docs.improbable.io/reference/latest/shared/operate/logs#logs), available one tab away, displays all your deployment’s logs (whether they come from the SpatialOS Runtime or the Worker code you have written) with useful filters.

<img src="{{assetRoot}}assets/logs-app.png" style="margin: 0 auto; display: block;" />

The [Metrics dashboards](https://docs.improbable.io/reference/latest/shared/operate/metrics#metrics), one more tab to the right, show a selection of useful metrics,with annotations to identify the health of your deployment.

<img src="{{assetRoot}}assets/metrics.png" style="margin: 0 auto; display: block;" />

Note that the Logs and Metrics are [available via an API](https://docs.improbable.io/reference/latest/shared/operate/byo-metrics) if you with to create your own dashboards or alerts.

## Well-done, and welcome to the GDK!

We hope you enjoyed getting started with the SpatialOS GDK for Unity: setting up, launching your first cloud deployment, configuring simulated player clients and taking a tour of the tooling provided by the platform.

We’d love to know what you think, and invite you to join our community on [our forums](https://forums.improbable.io/), or on [Discord](https://discordapp.com/invite/SCZTCYm).

To keep learning, here are a few suggestions!

* Follow our [Health pickups tutorial]({{assetRoot}}/projects/template-fps/tutorial)
* Read more about SpatialOS workers in our [Concepts documentation](https://docs.improbable.io/reference/latest/shared/get-started/learn-spatialos)
* Inspect the [FPS Starter Project]({{urlRoot}}/projects/template-fps/overview) code, and start making a mod!
