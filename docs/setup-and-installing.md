**Warning:** The alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

-----

# Set up the SpatialOS GDK for Unity

## Quick version

1. Sign up for a  [SpatialOS account](https://improbable.io/get-spatialos).
1. Set up your machine by installing the prerequisites. See the install guide for [Windows](#windows) or [MacOS](#mac).
1. Clone the UnityGDK repository: `git clone git@github.com:spatialos/UnityGDK.git` or `git clone https://github.com/spatialos/UnityGDK.git`.
1. Follow the [quickstart](./content/deploy.md#quickstart-how-to-deploy-the-playground-project) guide to run the example `Playground` project.

## Full version

### Sign up for a SpatialOS account

You'll need a SpatialOS account in order to run the SpatialOS GDK for Unity.
Follow [this link](https://improbable.io/get-spatialos) to sign up for free.

### Set up your machine

#### Windows

1. [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive)
    - Make sure to select the following components during installation:
        - **Linux Build Support**
        - **Mac Build Support**
1. [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Verified with version `15.8.4`) or [Rider](https://www.jetbrains.com/rider/) (Verified with version `2018.2.1`)
    - If you are using Visual Studio, within the Visual Studio Installer, on the **Workloads** tab:
        - Select **Game development with Unity**
        - Select **.NET Core cross-platform development**
        - In the **Summary** on the right:
            - Deselect **Unity 2017.2 64-bit Editor** (the SpatialOS GDK for Unity requires Unity 2018.2.8)
            - Make sure **Visual Studio Tools for Unity** is selected
    - If you are using Rider:
        - Install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience
1. [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) (Verified with version `2.1.3xx` or `2.1.4xx`)
1. SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/win). This installs:
    - the [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
    - the SpatialOS [Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)
    - the 32-bit and 64-bit Visual C++ Redistributables
1. Reboot your machine to finish the setup.

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.2.8 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to a newer version of Visual Studio 2017.

#### Mac

1. [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive)
    - Make sure to select the following components during installation:
        - **Linux Build Support**
        - **Windows Build Support**
1. [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Verified with version `7.6.5`) or [Rider](https://www.jetbrains.com/rider/) (Verified with version `2018.2.1`)
    - If you are using Visual Studio, within the Visual Studio Installer, select **.NET Core + ASP .NET Core**
1. [.NET Core SDK (x64)](https://www.microsoft.com/net/download/) (Verified with version `2.1.3xx` and `2.1.4xx`)
1. SpatialOS, using the the [SpatialOS installer](https://console.improbable.io/installer/download/stable/latest/mac). This installs:
    - The [`spatial` CLI](https://docs.improbable.io/reference/latest/shared/spatial-cli-introduction)
    - The SpatialOS [Launcher](https://docs.improbable.io/reference/latest/shared/operate/launcher)
    - The 32-bit and 64-bit Visual C++ Redistributables
1. Reboot your machine to finish the setup.

### Clone the SpatialOS GDK for Unity repository

|     |     |
| --- | --- |
| SSH | `git clone git@github.com:spatialos/UnityGDK.git` |
| HTTPS | `git clone https://github.com/spatialos/UnityGDK.git` |

The GDK repository is a SpatialOS project called `UnityGDK`. It contains:

- a Unity project at `UnityGDK/workers/unity`, which you need to open to use the GDK
- SpatialOS features, such as the schema and snapshot files
- development code

## Next steps

To learn how to build and run the included `Playground` project - follow the [quickstart](./content/deploy.md#quickstart-how-to-deploy-the-playground-project).

-----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../README.md#give-us-feedback).
