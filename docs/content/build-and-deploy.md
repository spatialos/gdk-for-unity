**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----
## Building workers and deploying your game

Before running a deployment of your game, you need to configure your [workers](https://docs.improbable.io/reference/latest/shared/glossary#worker), configure the build of your workers and also build the workers. This applies to both server-workers and client-workers.

### Configuring your workers
To create the correct configurations for each of your server-workers and client-workers, you need to use the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli) (SpatialOS documentation).

To do this, either:

* From the Unity Editor menu, select **SpatialOS** > **Spatial** > **Build worker  configs**.

    or
* In a terminal window, from the root of your game project, run:  `spatial build build-config` in the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

### Configuring the build of your workers
To configure the build of your server-workers and client-workers you can create a new [Scriptable object](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) (Unity documentation) containing the information needed to build your workers correctly. 

To do this:
1. In your Unity Editor, go to the Project window and select menu:  **Create** > **SpatialOS** > **Build Configuration**. This creates a Unity Asset called `SpatialOS Build Configuration`. 
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

### Run your built-out workers

To ensure the SpatialOS Runtime starts [server-workers](https://docs.improbable.io/reference/latest/shared/concepts/workers##server-worker) correctly, you need to ensure the launch configuration file to has the worker set up correctly. See the [Worker launch configuration](https://docs.improbable.io/reference/latest/shared/worker-configuration/launch-configuration#worker-launch-configuration) in the SpatialOS documentation for guidance on how to define the worker launch configurations for both server-workers and client-workers workers.

### Deployment
When you want to try out your game, you need to run a deployment of the game. This means launching SpatialOS itself. SpatialOS sets up the game world and starts up the workers needed to run the game world. Once the deployment is running, you can connect clients to it. You can then use the clients to play the game.
You can run a deployment on your development machine (a [local deployment](https://docs.improbable.io/reference/latest/shared/deploy/deploy-local) - SpatialOS documentation) or in the cloud (a [cloud deployment](https://docs.improbable.io/reference/latest/shared/deploy/deploy-cloud#deploying-to-the-cloud)).

#### Local deployment

To start a local deployment, either:

* In the Unity Editor, from the menu select **SpatialOS** > **Local Launch**. (This runs the default launch configuration.)

    or 
* Open a terminal window and from the root of your game project directory run `spatial local launch` in the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli).

(See the [SpatialOS documentation](https://docs.improbable.io/reference/latest/shared/spatial-cli/spatial-local-launch) for details of the launch configurations.)

This starts a "local" version of the SpatialOS Runtime on your development machine together with all server-workers (sometimes called “managed workers”) specified in the launch configuration you used. 

To start your client-workers (sometimes called "external workers"), from a terminal window, in any directory, run `spatial local worker launch <YourExternalWorker> <YourLaunchConfig>`,  replacing the `<example-content>` with names relevant to your game.
Use the [Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector)  to look at the current state of your game world. 

### Cloud deployment
<br/>You are allocated an empty SpatialOS project in the cloud when you sign up to SpatialOS; you use this to deploy your game but to do this you need to tell the GDK the name of your allocated SpatialOS project so it knows where to deploy your game to.  
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
   
 - About the `spatial cloud launch` command 
    - `spatial cloud launch` deploys a project to the cloud. Its full syntax is:
        `spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>`
        where:
        - `<assembly name>` is the name of the assembly the deployment will use (the one you named above).
        - `<launch configuration>` is the configuration file for the deployment.
        - `<deployment name>` is the name you choose as you enter the command; , you’ll use this to identify the deployment. This must be in lowercase.
        - `<snapshot file>` is the snapshot of the world you want to start from.  See the the GDK documentation for further information on [snapshots](./content/snapshots.md).

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