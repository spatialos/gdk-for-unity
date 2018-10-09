# Set up the SpatialOS GDK for Unity

<%(TOC)%>

To get set you'll need to create a SpatialOS account, install some dependencies and clone the GDK repository.

### Sign up for a SpatialOS account

You'll need a SpatialOS account in order to run the SpatialOS GDK for Unity.
Follow [this link](https://improbable.io/get-spatialos) to sign up for free.

### Set up your machine

#### Windows

Follow each of these steps:

| Step | Requirement | |
| --- | --- | --- |
| 1 | [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive) | Make sure to download the **Installer** version, and select the following components during installation: **Linux Build Support**, **Mac Build Support** |
| 2 | [.NET Core SDK (x64)](https://www.microsoft.com/net/download/dotnet-core/2.1) | Verified with versions `2.1.3xx` and `2.1.4xx` |
| 3 | <a href="https://console.improbable.io/installer/download/stable/latest/win" data-track-link="SpatialOS Installer Downloaded\|product=Docs\|platform=Win" target="_blank">SpatialOS installer</a> | This installs the `spatial CLI`, the `SpatialOS Launcher`, and 32-bit and 64-bit Visual C++ Redistributables |
| 4 | A code editor | We recommend either [Visual Studio 2017](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/). |

> After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

##### Visual Studio

If you are using Visual Studio, within the Visual Studio Installer, on the **Workloads** tab:

- Select **Game development with Unity**
- Select **.NET Core cross-platform development**
- In the **Summary** on the right:
  - Deselect **Unity 2017.2 64-bit Editor** (the SpatialOS GDK for Unity requires Unity 2018.2.8)
  - Make sure **Visual Studio Tools for Unity** is selected

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.2.8 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to a newer version of Visual Studio 2017.

##### Rider

If you are using Rider, install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience

#### Mac

Follow each of these steps:

| Step | Requirement | |
| --- | --- | --- |
| 1 | [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive) | Make sure to download the **Installer** version, and select the following components during installation: **Linux Build Support**, **Windows Build Support** |
| 2 | [.NET Core SDK (x64)](https://www.microsoft.com/net/download/dotnet-core/2.1) | Verified with versions `2.1.3xx` and `2.1.4xx` |
| 3 | <a href="https://console.improbable.io/installer/download/stable/latest/mac" data-track-link="SpatialOS Installer Downloaded\|product=Docs\|platform=Win" target="_blank">SpatialOS installer</a> | This installs the `spatial CLI`, the `SpatialOS Launcher`, and 32-bit and 64-bit Visual C++ Redistributables |
| 4 | A code editor | We recommend either [Visual Studio](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/). |

> After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

##### Visual Studio

If you are using Visual Studio, within the Visual Studio Installer, select **.NET Core + ASP .NET Core**

##### Rider

If you are using Rider, install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience

### Validate your installation

We **thoroughly** recommend validating your installation using the SpatialOS CLI.

In a terminal, run: `spatial diagnose` and hit enter.

The SpatialOS CLI will check whether the installation dependencies are where it expects them to be, and alert you of any problems.

<%(#Expandable title="Having problems?")%>
On Windows you can use `powershell` as your terminal.

On Mac you can use `terminal`.

The SpatialOS installer should have added the SpatialOS CLI to your `path`, allowing you to use `spatial` commands from your terminal window of choice. You can test whether this is installed correctly by just typing `spatial`. Sometimes it is necessary to close and re-open your terminal after an installation in order to use the newly installed functionality.
<%(/Expandable)%>

### Clone the SpatialOS GDK for Unity repository

|     |     |
| --- | --- |
| SSH | `git clone git@github.com:spatialos/UnityGDK.git` |
| HTTPS | `git clone https://github.com/spatialos/UnityGDK.git` |

The GDK repository is a SpatialOS project called `UnityGDK`. It contains:

- a Unity project at `UnityGDK/workers/unity`, which you need to open to use the GDK
- SpatialOS features, such as the schema and snapshot files
- development code

#### Licensing

*Your access to and use of the Unity Engine is governed by the Unity Engine End User License Agreement. Please ensure that you have agreed to those terms before you access or use the Unity Engine. For more about licensing for the SpatialOS GDK see the [licensing]({{urlRoot}}/license) section.*

### What's next?

Once you've got the GDK set up on your computer, dive right in with the FPS starter project and [deploy a game to the cloud]({{urlRoot}}/projects/template-fps/cloud-fake-clients)!
