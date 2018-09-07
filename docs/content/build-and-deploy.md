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
To start a cloud deployment, follow these steps: 
1. Upload your server-worker and client-worker assemblies. To do this, in a terminal window, from any directory, run `spatial cloud upload <assembly-name>` from the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/glossary#the-spatial-command-line-tool-cli). 
</br>You can choose any assembly name you want (but note that the name can only contain alpha-numerical characters, hyphens, dots and underscores, and must be between 5 and 64 characters long).
2. Once the server-worker and client-worker assemblies are successfully uploaded, from the same directory run `spatial cloud launch <assembly-name> <path/to/launch-config.json> <deployment-name> --snapshot=<path/to/snapshot.snapshot>`, replacing the `<example-content>` with file path and file names relevant to your game. For more information on snapshots, see [Snapshots](snapshots.md).
3. The `spatial cloud launch` command automatically opens the SpatialOS [Console](https://docs.improbable.io/reference/latest/shared/glossary#console) in your browser window once the deployment has started. Note that this process might take a couple of minutes. You can then view what’s happening in your game’s deployment in real time.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

[//]: # (Document the options UTY-1168)
[//]: # (Document the options UTY-1170)