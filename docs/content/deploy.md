**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----
## How to deploy your game

When you want to try out your game, you need to run a deployment of the game. This means launching SpatialOS itself. SpatialOS sets up the game world and optionally starts up the server-workers needed to run the game world. Once the deployment is running, you can connect clients to it in order to play the game. You can run a deployment on your development machine (a "local deployment" or in the cloud (a "cloud deployment").

Before you deploy your game, you need to build its workers; see documentation on [building workers](build.md).
This page contains:
* [How to deploy your game](#how-to-deploy-your-game) - Use this if you want to deploy your own game.
* [Quickstart: How to deploy the `Playground` project](#quickstart-how-to-deploy-the-playground-project)- Use this if you want to experiment using a deployment.

#### Configuring your deployment

To ensure the SpatialOS Runtime starts [server-workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing#server-workers) correctly, you need to ensure the launch configuration file has the worker set up correctly. See the [Worker launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration) in the SpatialOS documentation for guidance on how to define the worker launch configurations for both server-workers and client-workers.

#### Local deployment

To start a local deployment, either:

* In the Unity Editor, from the menu select **SpatialOS** > **Local Launch**. (This runs the default launch configuration.)

    or
* Open a terminal window and from the root of your game project directory run `spatial local launch` in the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

(See the [SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) for details of the launch configurations.)

This starts a local version of the SpatialOS Runtime on your development machine together with all server-workers specified in the launch configuration you used.

To start your client-workers, from a terminal window, in any directory, run `spatial local worker launch <YourExternalWorker> <YourLaunchConfig>`,  replacing the `<example-content>` with names relevant to your game. Use the [Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector)  to look at the current state of your game world.

#### Cloud deployment

You are allocated an empty SpatialOS project in the cloud when you sign up to SpatialOS; you use this to deploy your game, but to do this you need to tell the GDK the name of your allocated SpatialOS project so it knows where to deploy your game to.  
<br/> To start a cloud deployment:
1.  Open the `spatialos.json` file in the root folder of your game.
1. Change the `name` field so it matches the name of your SpatialOS project.  You can find this in the SpatialOS [Console](https://console.improbable.io). It’ll be something like `beta_someword_anotherword_000`.
1. Build and upload a game assembly for the deployment
    - The assembly  includes executable files for the client-workers and server-workers, and the assets both types of workers use (such as the models and textures used by the client-server - that is, the game executable code - to visualise the game).
    1. To build an assembly; in the Unity Editor, select **SpatialOS** > **Build all workers for cloud**.

    2. To upload an assembly; open a terminal and navigate to the directory in which your game is in (the repository you’ve cloned). Run `spatial cloud upload <assembly name>`.
    - The `<assembly name>` is a label you create so you can identify this assembly in the next step - for example you could call it `MyGDKAssembly`.
    > **It’s finished uploading when:** You see `spatial upload <assembly name> succeeded` printed in your terminal output.
1. Launch a cloud deployment
    - In the same terminal window, run `spatial cloud launch <assembly name> cloud_launch.json <deployment name> --snapshot=snapshots/default.snapshot`
    - This command defaults to deploying to clusters located in the US. So if you’re in Europe, add the `--cluster_region=eu` flag for better latency.
    > **It’s done when:** You see `Successfully created deployment` printed in your Unity Editor console output.

 - **About the `spatial cloud launch` command**
    - `spatial cloud launch` deploys a project to the cloud. Its full syntax is:
        `spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>`
        where:
        - `<assembly name>` is the name of the assembly the deployment will use (the one you named above).
        - `<launch configuration>` is the configuration file for the deployment.
        - `<deployment name>` is the name you choose as you enter the command; , you’ll use this to identify the deployment. This must be in lowercase.
        - `<snapshot file>` is the snapshot of the world you want to start from.  See the the GDK documentation for further information on [snapshots](snapshots.md).

1. Launch a game client:
    1. Open the SpatialOS  [Console](https://console.improbable.io/projects). You’ll see the project and the deployment you just created.
    1. In the SpatialOS Console, select the deployment’s name to open the overview page.
    1. Select **Launch**.
        > You can ignore the prompt to install the Launcher, as it’s installed as part of the SpatialOS Installer.
    1. To get links to share the game with others, select **Share**.
    1. Once you’ve finished playing, select **Stop** in the Console.


## Quickstart: How to deploy the `Playground ` project

This guide walks you through starting a SpatialOS game from the Unity Editor, either using SpatialOS running locally on your computer, or using SpatialOS in the cloud.
It uses the `Playground` as an example; the `Playground` comes with the GDK. It is a basic Unity project which uses SpatialOS.

### Prerequisites

Before following this guide - make sure you have followed the [setup guide](../setup-and-installing.md).

####  Run the `Playground` locally using SpatialOS

1. Open the Unity Editor, and open the project at `UnityGDK/workers/unity`
    - Unity downloads several required SpatialOS libraries.
    <br/>Unity may open a browser window prompting you to log in to your SpatialOS account. If this happens, please log in.
    <br>Note: This only happens the first time you open you project, or if the version of the required libraries changes.
1. In the Unity Editor's Project window, open **Assets** > **Playground** > **Scenes** > **SampleScene**.
1. Select **SpatialOS** > **Local launch**.
    - This launches a SpatialOS deployment locally in a new terminal window. You can open the [Inspector](https://docs.improbable.io/reference/latest/shared/glossary#inspector) and see what’s happening in the game.
    > **It’s done when:** You see `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector` printed in the new console window.
1. In the Editor, select the play icon.
    - You can move around in the Scene using `W`, `A`, `S`, and `D` on your keyboard, and move the camera using your mouse.
1. When you want to stop the game:
    1. In the Editor, select the play icon to stop your client.
    1. Stop the SpatialOS deployment by closing the Unity Editor console window.

#### Run the `Playground` in the cloud using SpatialOS
1. Tie the `Playground`  project with a SpatialOS project in the cloud.
<br/>You are allocated an empty SpatialOS project in the cloud when you sign up to SpatialOS; you use this to deploy the `Playground`  project, but to do this you need to tell the GDK the name of your allocated SpatialOS project so it knows where to deploy `Playground` to.  
<br/> To do this:

    1.  Open the `spatialos.json` file in the root folder of the `Playground` project.
    1. Change the `name` field so it matches the name of your SpatialOS project.  You can find this in the SpatialOS [Console](https://console.improbable.io). It’ll be something like `beta_someword_anotherword_000`.
1. Build and upload a game assembly for the deployment
    - The assembly  includes executable files for the client-workers and server-workers, and the assets both types of workers use (such as the models and textures used by the client-server - that is, the game executable code - to visualise the game).
    1. To build an assembly; in the Unity Editor, select **SpatialOS** > **Build all workers for cloud**.

    2. To upload an assembly; open a terminal and navigate to the `UnityGDK` directory (the repository you’ve cloned). Run `spatial cloud upload <assembly name>`.
    - The `<assembly name>` is a label you create so you can identify this assembly in the next step - for example you could call it `MyGDKAssembly`.
    > **It’s finished uploading when:** You see `spatial upload <assembly name> succeeded` printed in your terminal output.
1. Launch a cloud deployment
    - In the same terminal window, run `spatial cloud launch <assembly name> cloud_launch.json <deployment name> --snapshot=snapshots/default.snapshot`
    - This command defaults to deploying to clusters located in the US. So if you’re in Europe, add the `--cluster_region=eu` flag for better latency.
    > **It’s done when:** You see `Successfully created deployment` printed in your Unity Editor console output.

1. Launch a game client:
    1. Open the SpatialOS  [Console](https://console.improbable.io/projects). You’ll see the project and the deployment you just created.
    1. In the SpatialOS Console, select the deployment’s name to open the overview page.
    1. Select **Launch**.
        > You can ignore the prompt to install the Launcher, as it’s installed as part of the SpatialOS Installer.
    1. To get links to share the game with others, select **Share**.
    1. Once you’ve finished playing, select **Stop** in the Console.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)
