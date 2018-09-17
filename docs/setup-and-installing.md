**Warning:** The alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

-----

# Short Version

1. Setup your machine by following the prerequisites. See the install guide for [Windows](#Windows) or [MacOS](#Mac).
2. Clone the UnityGDK repository: `git clone git@github.com:spatialos/UnityGDK.git` or `git clone https://github.com/spatialos/UnityGDK.git`
2. Open the `workers/unity` project in your Unity Editor. You may need to authenticate with SpatialOS.
3. In the Editor, open **Assets > Playground > Scenes > SampleScene**.
4. Launch a local deployment by either selecting **SpatialOS > Local launch** or press **Ctrl-L**.
5. Play the scene in the Unity Editor. A UnityGameLogic worker and a UnityClient worker should start and connect to the local deployment.

# Full Version

## Set up and get started with the SpatialOS GDK for Unity

### Setting up your machine

1. Install prerequisites:

    #### Windows

	1. [Unity 2018.2.0](https://unity3d.com/get-unity/download/archive) 
        - Make sure to select the following components:
            - **Linux Build Support**
            - **Mac Build Support**
	2. [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Verified with version `15.8.4`) or [Rider](https://www.jetbrains.com/rider/) (Verified with version `2018.2.1`)
	    - If you are using Visual Studio, within the Visual Studio Installer, on the Workloads tab:
            - Select **Game development with Unity** 
            - Select **.NET Core cross-platform development** 
            - In the summary on the right: 
                - Deselect **Unity 2017.2 64-bit Editor** (the SpatialOS GDK for Unity requires Unity 2018.2.0f2). 
                - Make sure **Visual Studio Tools for Unity** is selected.
        - If you are using Rider: 
            - Install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience.
    3. [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) (Verified with version `2.1.3xx` or `2.1.4xx`)
	4. SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/win). This installs:
		- the [`spatial` CLI](https://docs.improbable.io/reference/13.0/shared/spatial-cli-introduction)
		- the SpatialOS [Launcher](https://docs.improbable.io/reference/13.0/shared/operate/launcher)
		- the 32-bit and 64-bit Visual C++ Redistributables
	5. (Optional) [ReSharper](https://www.jetbrains.com/resharper/) if you are using Visual Studio
	    - It’s useful to have ReSharper installed if you want to contribute to the SpatialOS Unity GDK repository, as it makes it easy to stick to our [coding standards](contributions/unity-gdk-coding-standards.md). 
        - _We are currently not accepting public contributions - see our [contributions](../.github/CONTRIBUTING.md) policy._
    6. Reboot your machine to finish setup.

    > Warning: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.2.0 like: projects loading/unloading frequently and Intellisense breaking. If you do experience these issues, try updating Visual Studio 2017

# TODO - CHECK MAC STUFF

    #### Mac

	- [Unity 2018.2.0](https://unity3d.com/get-unity/download/archive) with Linux Build Support and Windows Build Support components selected during the installation process.
    - [Rider](https://www.jetbrains.com/rider/) (optional)
      <br>You can also use [Visual Studio 2017](https://www.visualstudio.com/downloads/) for development, however to lint your code according to our linting rules, you need to use Rider.
    - [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) Verified with version `2.1.3xx` and `2.1.4xx`
	- SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/mac)
	<br>This installs:
		- the [`spatial` CLI](https://docs.improbable.io/reference/13.0/shared/spatial-cli-introduction)
		- the SpatialOS [Launcher](https://docs.improbable.io/reference/13.0/shared/operate/launcher)
		- the 32-bit and 64-bit Visual C++ Redistributables

1. Clone the SpatialOS GDK for Unity repository:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/UnityGDK.git` |
    |HTTPS | `git clone https://github.com/spatialos/UnityGDK.git` | 
    

    The GDK repository is a SpatialOS project called `UnityGDK`. It contains:
    - a Unity project at `UnityGDK/workers/unity`, which you need to open to use the GDK
    - SpatialOS features, such as the schema and snapshot files
    - development code

### Running a game from your Unity Editor

You can now run a game from your Unity Editor, either using SpatialOS locally, or using SpatialOS in the cloud.

Currently, you can try this out using the `Playground`.

#### 1. Run the `Playground` locally using SpatialOS

1. Open Unity, and open the `unity` project.
    - Unity will automatically download several required SpatialOS libraries.
Unity may open a browser window prompting you to login to your SpatialOS account.
If this happens, please login.
This will only happen the first time you open you project, or if the version of the required libraries changes.

1. In the Unity Editor's Project window, open **Assets** > **Playground** > **Scenes** > **SampleScene**.

1. Choose **SpatialOS** > **Local launch**.
    - This launches a SpatialOS deployment locally in a new console window. You can open the [Inspector](https://docs.improbable.io/reference/13.0/shared/glossary#inspector) and see what’s happening in the game.
    > **It’s done when:** You see `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector` printed in the new console window

1. Click the play icon.
    - You can move around using the `W`, `A`, `S`, and `D` keys, and move the camera using your mouse.

1. When you want to stop the game:
    1. In the Editor, click the play icon again to stop your client.
    1. Stop the SpatialOS deployment by closing the console window.

#### 2. Run the `Playground` in the cloud using SpatialOS

1. Change the name of the project

    1. In the root directory of the project, open the `spatialos.json` file.
    1. Change the `name` field to the name of your project. You can find this in the [Console](https://console.improbable.io). It’ll be something like `beta_someword_anotherword_000`.

1. Build an assembly for the deployment
    - The first step of running a cloud deployment is uploading all the files that your game uses. This includes executable files for the clients and workers, and the assets your workers use (like models and textures used by the Unity client to visualise the game). We call that set of game files an assembly.
    - To build an assembly in the Unity Editor for your game to use while running in the cloud, select **SpatialOS** > **Build all workers for cloud**

1. Upload the assembly to the cloud
    - Open a terminal and navigate to the `UnityGDK` directory (the repository you’ve cloned). Run `spatial cloud upload <assembly name>`.
    - The `<assembly name>` is just a label so you can identify this assembly in the next step - for example, `MyGDKAssembly`.
    > **It’s done when:** You see `spatial upload <assembly name> succeeded` printed in your console output.

1. Launch a cloud deployment
    - The command `spatial cloud launch` deploys a project to the cloud. Its full syntax is:

        `spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>`

        where:

        - `<assembly name>` is the name of the assembly the deployment will use (the one you named in step 3).
        - `<launch configuration>` is the configuration file for the deployment. This project includes one called `default_launch.json`.
        - `<deployment name>` is a name of your choice, which you’ll use to identify the deployment. This must be in lowercase.
        - `<snapshot file>` is the snapshot of the world you want to start from. This project includes one called `snapshots/default.snapshot`.
    - In the same terminal window, run `spatial cloud launch <assembly name> cloud_launch.json <deployment name> --snapshot=snapshots/default.snapshot`
    - This command defaults to deploying to clusters located in the US. So if you’re in Europe, add the `--cluster_region=eu` flag for better performance.

    > **It’s done when:** You see `Successfully created deployment` printed in your console output.


1. Launch a game client:
    1. Open the [Console](https://console.improbable.io/projects). You’ll see the project and the deployment you just created.
    1. Click the deployment’s name to open the overview page.
    1. Click **Launch**.
        > You can ignore the prompt to install the Launcher, as it’s installed as part of the SpatialOS Installer.
    1. To get links to share the game with others, click **Share**.
    1. Once you’ve finished playing, click **Stop** in the Console.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../README.md#give-us-feedback).
