**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

-----

# Set up and get started with the SpatialOS Unity GDK

## Short version (for SpatialOS local deployment only)

1. Set up your machine by installing: [Unity 2018.1.1](https://unity3d.com/get-unity/download/archive), [Visual Studio 2017](https://www.visualstudio.com/downloads/), [SpatialOS](https://console.improbable.io/installer/download/stable/latest/win), [ReSharper](https://www.jetbrains.com/resharper/) (optional), and [ReSharper Command Line Tools](https://www.jetbrains.com/resharper/download/index.html#section=resharper-clt) (optional).

1. Clone the repository: `git clone git@github.com:spatialos/UnityGDK.git`  or `git clone  https://github.com/spatialos/UnityGDK.git`

2. Run `bash prepare-workspace.sh`.

1. Run `spatial local launch`.

1. Open the `unity` project in your Unity Editor.

1. In the Editor, open **Assets** > **Playground** > **Scenes** > **SampleScene**.

## Full version

### Setting up your machine

1. Install prerequisites:
	- [Unity 2018.2.0b10](https://unity3d.com/unity/beta-download)
	- [Visual Studio 2017](https://www.visualstudio.com/downloads/)
	    > Within Visual Studio Installer, on the Workloads tab, select **Game development with Unity**. In the summary on the right, deselect **Unity 2017.2 64-bit Editor** (the SpatialOS Unity GDK requires Unity 2018.1.1). Make sure **Visual Studio Tools for Unity** is selected.
	- SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/win)
	<br>This installs:
		- the [`spatial` CLI](https://docs.improbable.io/reference/13.0/shared/spatial-cli-introduction)
		- the SpatialOS [Launcher](https://docs.improbable.io/reference/13.0/shared/operate/launcher)
		- the 32-bit and 64-bit Visual C++ Redistributables
	- (Optional) [ReSharper](https://www.jetbrains.com/resharper/)
	<br>It’s useful to have ReSharper installed if you want to contribute to the SpatialOS Unity GDK repository, as it makes it easy to stick to our [coding standards](contributions/unity-gdk-coding-standards.md). 
		> We are currently not accepting public contributions - see our [contributions](../.github/CONTRIBUTING.md) policy.
    - (Optional) [ReSharper Command Line Tools](https://www.jetbrains.com/resharper/download/index.html#section=resharper-clt) 
    <br> You'll need this if you want to lint your code. Add it to your `PATH` environment variable.

1. Clone the SpatialOS Unity GDK repository:

	SSH: `git clone git@github.com:spatialos/UnityGDK.git`
    <br>HTTPS: `git clone https://github.com/spatialos/UnityGDK.git`

    The SpatialOS Unity GDK repository is a SpatialOS project called `UnityGDK`. It contains:
    - a Unity project at `UnityGDK/workers/unity`, which you need to open to use the GDK
    - SpatialOS features, such as the schema and snapshot files
    - development code
    
    The Unity project contains the core GDK code, the `TransformSynchronization` Feature Module (more feature modules to come), and the `Playground`, which is an example scene using the features of the GDK.

    The diagram below shows the structure of the `UnityGDK` SpatialOS project:

	![UnityGDK structure](assets/UnityGDK-structure.png)

### Running a game from your Unity Editor 

You can now run a game from your Unity Editor, either using SpatialOS locally, or using SpatialOS in the cloud.

Currently, you can try this out using the `Playground`.

#### 1. Get your workspace ready

1. Open a terminal and navigate to the `UnityGDK` directory (the repository you’ve just cloned).
1. Run `bash prepare-workspace.sh`.
<br>This fetches additional packages and generates the code for the SpatialOS workers.

#### 2. Run the `Playground` locally using SpatialOS

1. In the same terminal window, run `spatial local launch`. 
<br>This launches a SpatialOS deployment locally. You can open the [Inspector](https://docs.improbable.io/reference/13.0/shared/glossary#inspector) and see what’s happening in the game.
    > **It’s done when:** You see `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector` printed in your console output.

1. Open Unity, and open the `unity` project. 
<br>This causes Unity to generate a Visual Studio solution, `unity.sln`, within `UnityGDK/workers/unity`.

1. In the Unity Editor's Project window, open **Assets** > **Playground** > **Scenes** > **SampleScene**.

1. Click the play icon.
<br>You can move around using the `W`, `A`, `S`, and `D` keys, and move the camera using your mouse.

1. When you want to stop the game:
    1. In the Editor, click the play icon again to stop your client.
    1. In your terminal, stop the SpatialOS deployment by pressing `Ctrl + C`.

#### 3. Run the `Playground` in the cloud using SpatialOS

1. Change the name of the project

    1. In the root directory of the project, open the `spatialos.json` file.
    1. Change the `name` field to the name of your project. You can find this in the [Console](https://console.improbable.io). It’ll be something like `beta_someword_anotherword_000`.

1. Build an assembly for the deployment
<br>The first step of running a cloud deployment is uploading all the files that your game uses. This includes executable files for the clients and workers, and the assets your workers use (like models and textures used by the Unity client to visualise the game). We call that set of game files an assembly.
<br><br> To build an assembly for your game to use while running in the cloud, either:
    - In the Unity Editor, select **Improbable** > **Build all workers for cloud**
    - Or, close the Unity Editor and, in the root directory of the project, run `spatial build`
        > If you don’t close Unity before running `spatial build`, the command will report an error.
    
        **It’s done when:** You see `spatial build UnityClient UnityGameLogic succeeded` printed in your console output.

1. Upload the assembly to the cloud
    
    In the same terminal window as before, run `spatial cloud upload <assembly name>`.
    > The `<assembly name>` is just a label so you can identify this assembly in the next step - for example, `MyGDKAssembly`.

     **It’s done when:** You see `spatial upload MyGDKAssembly succeeded` printed in your console output.

1. Launch a cloud deployment
<br>The command `spatial cloud launch` deploys a project to the cloud. Its full syntax is:

    `spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>`

    where:

    - `<assembly name>` is the name of the assembly the deployment will use (the one you named in step 3).
    - `<launch configuration>` is the configuration file for the deployment. This project includes one called `default_launch.json`.
    - `<deployment name>` is a name of your choice, which you’ll use to identify the deployment. This must be in lowercase.
    - `<snapshot file>` is the snapshot of the world you want to start from. This project includes one called `snapshots/default.snapshot`.

    In the same terminal window, run `spatial cloud launch <assembly name> cloud_launch.json <deployment name> --snapshot=snapshots/default.snapshot`

    This command defaults to deploying to clusters located in the US. So if you’re in Europe, add the `--cluster_region=eu` flag for better performance.
    
    **It’s done when:** You see `Successfully created deployment` printed in your console output.


1. Launch a game client:
    1. Open https://console.improbable.io/projects.
    <br>You’ll see the project and the deployment you just created.

    1. Click the deployment’s name to open the overview page.

    1. To get links to share the game with others, click **Share**.

    1. Click **Launch**.
        > You can ignore the prompt to install the Launcher, as it’s installed as part of SpatialOS.

    1. Once you’ve finished playing, click **Stop** in the Console.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../README.md#give-us-feedback).
