**Warning:** The alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).
-----
# Set up and get started with the SpatialOS GDK for Unity

## Quick version

1. Sign up for a  [SpatialOS account](https://improbable.io/get-spatialos).
2. Setup your machine by installing the prerequisites. See the install guide for [Windows](#windows) or [MacOS](#mac).
3. Clone the UnityGDK repository: `git clone git@github.com:spatialos/UnityGDK.git` or `git clone https://github.com/spatialos/UnityGDK.git`
4. Open the `workers/unity` project in the Unity Editor. You may need to authenticate with SpatialOS.
5. In the Editor, open **Assets** > **Playground** > **Scenes** > **SampleScene**.
6. Launch a local deployment by either selecting **SpatialOS** > **Local launch** in the Editor or select **Ctrl-L** on your keyboard.
7. Play the scene in the Editor. A server-worker (UnityGameLogic) and a client-worker (UnityClient) start and connect to the local deployment.

## Full version

### Sign up for a SpatialOS account

You'll need a SpatialOS account in order to run the SpatialOS GDK for Unity.
Follow [this link](https://improbable.io/get-spatialos) to sign up for free.

### Set up your machine

#### Windows

1. [Unity 2018.2.0](https://unity3d.com/get-unity/download/archive) 
    - Make sure to select the following components:
        - **Linux Build Support**
        - **Mac Build Support**
2. [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Verified with version `15.8.4`) or [Rider](https://www.jetbrains.com/rider/) (Verified with version `2018.2.1`)
    - If you are using Visual Studio, within the Visual Studio Installer, on the **Workloads** tab:
        - Select **Game development with Unity** 
        - Select **.NET Core cross-platform development** 
        - In the **Summary** on the right: 
            - Deselect **Unity 2017.2 64-bit Editor** (the SpatialOS GDK for Unity requires Unity 2018.2.0f2)
            - Make sure **Visual Studio Tools for Unity** is selected
    - If you are using Rider: 
        - Install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience
3. [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) (Verified with version `2.1.3xx` or `2.1.4xx`)
4. SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/win). This installs:
    - the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
    - the SpatialOS [Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)
    - the 32-bit and 64-bit Visual C++ Redistributables
5. (Optional) [ReSharper](https://www.jetbrains.com/resharper/) if you are using Visual Studio
    - It’s useful to have ReSharper installed if you want to contribute to the SpatialOS Unity GDK repository, as it makes it easy to stick to our [coding standards](contributions/unity-gdk-coding-standards.md). 
    - _We are currently not accepting public contributions - see our [contributions](../.github/CONTRIBUTING.md) policy._
6. Reboot your machine to finish the setup.

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.2.0 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to a newer version of Visual Studio 2017.

#### Mac

1. [Unity 2018.2.0](https://unity3d.com/get-unity/download/archive) 
    - Make sure to select the following components:
        - **Linux Build Support** 
        - **Windows Build Support**
2. [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Verified with version `7.6.5`) or [Rider](https://www.jetbrains.com/rider/) (Verified with version `2018.2.1`)
    - If you are using Visual Studio, within the Visual Studio Installer, select **.NET Core + ASP .NET Core**
3. [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) (Verified with version `2.1.3xx` and `2.1.4xx`)
4. SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/mac). This installs:
    - The [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
    - The SpatialOS [Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)
    - The 32-bit and 64-bit Visual C++ Redistributables
5. Reboot your machine to finish the setup.

### Clone the SpatialOS GDK for Unity repository:

|     |     |
| --- | --- |
| SSH | `git clone git@github.com:spatialos/UnityGDK.git` |
| HTTPS | `git clone https://github.com/spatialos/UnityGDK.git` | 

The GDK repository is a SpatialOS project called `UnityGDK`. It contains:
- a Unity project at `UnityGDK/workers/unity`, which you need to open to use the GDK
- SpatialOS features, such as the schema and snapshot files
- development code

### Run a game from your Unity Editor

You can now run a game from the Unity Editor, either using SpatialOS running locally on your computer, or using SpatialOS in the cloud.
You can try this out using the `Playground` which is a basic Unity project using SpatialOS.

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
<br/>You are allocated an empty SpatialOS project in the cloud when you sign up to SpatialOS; you use this to deploy the `Playground`  project but to do this you need to tell the GDK the name of your allocated SpatialOS project so it knows where to deploy `Playground` to.  
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
    - About the `spatial cloud launch` command 
        - `spatial cloud launch` deploys a project to the cloud. Its full syntax is:
            `spatial cloud launch <assembly name> <launch configuration> <deployment name> --snapshot=<snapshot file>`
            where:
            - `<assembly name>` is the name of the assembly the deployment will use (the one you named above).
            - `<launch configuration>` is the configuration file for the deployment. The `playground` project includes one called `default_launch.json`.
            - `<deployment name>` you choose as you enter the command; , you’ll use this to identify the deployment. This must be in lowercase.
            - `<snapshot file>` is the snapshot of the world you want to start from.  See the the GDK documentation for further information on [snapshots](./content/snapshots.md).

1. Launch a game client:
    1. Open the SpatialOS  [Console](https://console.improbable.io/projects). You’ll see the project and the deployment you just created.
    1. In the SpatialOS Console, select the deployment’s name to open the overview page.
    1. Select **Launch**.
        > You can ignore the prompt to install the Launcher, as it’s installed as part of the SpatialOS Installer.
    1. To get links to share the game with others, select **Share**.
    1. Once you’ve finished playing, select **Stop** in the Console.
----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../README.md#give-us-feedback).
