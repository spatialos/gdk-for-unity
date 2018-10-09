# Set up the SpatialOS GDK for Unity

<%(TOC)%>

To get set you'll need to create a SpatialOS account, install some dependencies and clone the GDK repository.

### Sign up for a SpatialOS account

You'll need a SpatialOS account in order to run the SpatialOS GDK for Unity.
Follow [this link](https://improbable.io/get-spatialos) to sign up for free.

### Set up your machine

#### Windows

| Step | Requirement | |
| --- | --- | --- |
| 1 | [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive) | Make sure to download the **Installer** version, and select the following components during installation: **Linux Build Support**, **Mac Build Support**<%(#Expandable title="How will I know when Unity is installed? (Expand for image)")%>Once Unity has been installed on your machine, the Download Assistant should prompt you to click *Finish* to close the installer.![]({{assetRoot}}assets/setup/windows/unity.png)<%(/Expandable)%> |
| 2 | [.NET Core SDK (x64)](https://www.microsoft.com/net/download/dotnet-core/2.1) | Verified with versions `2.1.3xx` and `2.1.4xx`<%(#Expandable title="How will I know when .NET Core SDK is installed? (Expand for image)")%>Once the .NET Core SDK has been installed, a window should appear stating that installation was successful and to prompt you to close the installer.![]({{assetRoot}}assets/setup/windows/dotnetcoresdk.png)<%(/Expandable)%> |
| 3 | <a href="https://console.improbable.io/installer/download/stable/latest/win" data-track-link="SpatialOS Installer Downloaded\|product=Docs\|platform=Win" target="_blank">SpatialOS installer</a> | This installs the `spatial CLI`, the `SpatialOS Launcher`, and 32-bit and 64-bit Visual C++ Redistributables<%(#Expandable title="How will I know when SpatialOS is installed? (Expand for image)")%>Once SpatialOS has been installed, the installer should prompt you to click *Finish* to complete the installation.![]({{assetRoot}}assets/setup/windows/spatialos.png)<%(/Expandable)%> |
| 4 | A code editor | We recommend either [Visual Studio 2017](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/).<%(#Expandable title="How will I know when Visual Studio is installed? (Expand for image)")%>Once Visual Studio has been installed, the installer should prompt you to restart your computer.![]({{assetRoot}}assets/setup/windows/visualstudio.png)<%(/Expandable)%><%(#Expandable title="How will I know when Rider is installed? (Expand for image)")%>Once Rider is installed, the installer should prompt you to click *Finish* to close Setup.![]({{assetRoot}}assets/setup/windows/rider.png)<%(/Expandable)%> |

> After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

##### Visual Studio

If you are using Visual Studio, find the **Workloads** tab in the Visual Studio Installer by clicking on the **More** option for *Visual Studio Build Tools*, and selecting **Modify** from the drop-down menu.

![]({{assetRoot}}assets/setup/windows/visualstudioworkloads.png)

On the **Workloads** tab:

* Select **.NET Core cross-platform development**

- Select **Game development with Unity**
  - In the **Summary** on the right:
    - Deselect any options that mention a Unity Editor (e.g. Unity 2017.2 64-bit Editor or Unity 2018.1 64-bit Editor)
    - The SpatialOS GDK for Unity requires **Unity 2018.2.8**, which should already be installed if you have followed the setup guide correctly.
  - Make sure **Visual Studio Tools for Unity** is included (there should be a tick next to it)

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.2.8 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to a newer version of Visual Studio 2017.

##### Rider

If you are using Rider, install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience

**You can now skip ahead to [validating your installation](#validate-your-installation).**

---

#### Mac

| Step | Requirement | |
| --- | --- | --- |
| 1 | [Unity 2018.2.8](https://unity3d.com/get-unity/download/archive) | Make sure to download the **Installer** version, and select the following components during installation: **Linux Build Support**, **Windows Build Support**<%(#Expandable title="How will I know when Unity is installed? (Expand for image)")%>Once Unity has been installed on your machine, the Download Assistant should tell you that installation was successful.![]({{assetRoot}}assets/setup/mac/unity.png)<%(/Expandable)%> |
| 2 | [.NET Core SDK (x64)](https://www.microsoft.com/net/download/dotnet-core/2.1) | Verified with versions `2.1.3xx` and `2.1.4xx`<%(#Expandable title="How will I know when .NET Core SDK is installed? (Expand for image)")%>Once the .NET Core SDK has been installed, a window should appear stating that installation was successful and to prompt you to close the installer.![]({{assetRoot}}assets/setup/mac/dotnetcoresdk.png)<%(/Expandable)%> |
| 3 | <a href="https://console.improbable.io/installer/download/stable/latest/mac" data-track-link="SpatialOS Installer Downloaded\|product=Docs\|platform=Mac" target="_blank">SpatialOS installer</a> | This installs the `spatial CLI`, the `SpatialOS Launcher`, and 32-bit and 64-bit Visual C++ Redistributables<%(#Expandable title="How will I know when SpatialOS is installed? (Expand for image)")%>Once SpatialOS has been installed, the installer should should tell you that installation was successful.![]({{assetRoot}}assets/setup/mac/spatialos.png)<%(/Expandable)%> |
| 4 | A code editor | We recommend either [Visual Studio](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/).<%(#Expandable title="How will I know when Visual Studio is installed? (Expand for image)")%>Once Visual Studio has been installed, the installer should should tell you that installation was successful.![]({{assetRoot}}assets/setup/mac/visualstudio.png)<%(/Expandable)%><%(#Expandable title="How will I know when Rider is installed? (Expand for image)")%>When installing Rider, the installer will prompt you to drag the Rider icon into your `Applications` folder.![]({{assetRoot}}assets/setup/mac/rider.png)If no errors are thrown, then your installation was successful.<%(/Expandable)%> |

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

| --- | --- |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |

The GDK repository is a SpatialOS project called `gdk-for-unity`. It contains:

- a Unity project at `gdk-for-unity/workers/unity`, which you need to open to use the GDK
- SpatialOS features, such as the schema and snapshot files
- development code

### Licensing

*Your access to and use of the Unity Engine is governed by the Unity Engine End User License Agreement. Please ensure that you have agreed to those terms before you access or use the Unity Engine. For more about licensing for the SpatialOS GDK see the [licensing]({{urlRoot}}/license) section.*

### What's next?

Once you've got the GDK set up on your computer, get back to [deploying a game to the cloud]({{urlRoot}}/welcome)!
