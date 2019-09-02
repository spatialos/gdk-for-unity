### 1. Sign up or log in

If you have not signed up before, you can do this [here](https://improbable.io/get-spatialos).
<br/>

If you have already signed up, make sure you are logged into [Improbable.io](https://improbable.io). If you are logged in, you should see your picture in the top right of this page; if you are not logged in, select __Sign in__ at the top of this page and follow the instructions.


### 2. Hardware

Ensure your machine meets the minimum system requirements for Unity development.

Refer to the [Unity system requirements](https://unity3d.com/unity/system-requirements) for further information about the minimum hardware requirements.

### 3. Network settings

To configure your network to work with SpatialOS, refer to the [SpatialOS network settings](https://docs.improbable.io/reference/latest/shared/setup/requirements#network-settings). 

### 4. Install the GDK dependencies

<%(#Expandable title="Setup for Windows")%>

**Step 1.** Install **Unity 2019.2.0**

Ensure you install this exact version as other Unity versions may not work with the GDK.

You can download **<a href="https://unity3d.com/get-unity/download/archive" data-track-link="Unity Download Link Clicked|product=Docs|platform=Win|label=Win" target="_blank"><strong>Unity 2019.2.0</strong></a>** from the Unity download archive.

Make sure you download the **Installer** version, and refer to the matrix below for which options to install :

| | | |
|---|---|---|
| **Option** | **Required?** | **Why?** |
| Linux Build Support | **Yes** | All server-workers in a cloud deployment run in a Linux environment. |
| Mac Build Support | No |  Only required if you wish to build clients compatible with MacOS. |
| Android Build Support | No | Only required if you wish to build mobile clients for Android. |
| iOS Build Support | No | Only required if you wish to build mobile clients for iOS. |

**Step 2.** Install **<a href="https://dotnet.microsoft.com/download/dotnet-core/2.2" data-track-link=".NET Core Download Link Clicked|product=Docs|platform=Win|label=Win" target="_blank"><strong>.NET Core SDK (x64)</strong></a>**

- Verified with version `2.2.2xx`.

> **Note:** After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

**Step 3.** Run the **<a href="https://console.improbable.io/installer/download/stable/latest/win" data-track-link="SpatialOS Installer Downloaded|product=Docs|platform=Win|label=Win" target="_blank">SpatialOS Installer</a>**

- This installs the [SpatialOS CLI]({{urlRoot}}/reference/glossary#spatial-command-line-tool-cli) , the [SpatialOS Launcher]({{urlRoot}}/reference/glossary#launcher), and 32-bit and 64-bit Visual C++ Redistributables.

**Step 4.** Install a **code editor** if you don't have one already

- We recommend either [Visual Studio 2017](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/). See _**Code editor set up**_, below.

**Step 5.** Install Git

The SpatialOS GDK for Unity source code is hosted on GitHub. You need to download and install [Git](https://git-scm.com/downloads) or [GitHub Desktop](https://desktop.github.com/) in order to clone the GDK repositories.

> If you do not want to use Git you can also download a .zip file containing the GDK repo from the [GDK GitHub Page](https://github.com/spatialos/gdk-for-unity). However, you will not be able to easily download updates to the GDK if you do not use Git to clone the repository.

**Step 6.** Restart your machine.

> If you do not restart your machine, you may experience errors when opening a GDK project.

#### Code editor set up

 **Using Visual Studio?**

You need to install the **.NET Core cross-platform development** and **Game development with Unity** workloads. To to this:

- As you install [Visual Studio](https://www.visualstudio.com/downloads/), select the **Workloads** tab in the Installer. If you already have Visual Studio installed, you can find the **Workloads** tab by opening Visual Studio Installer and, in the **Products** section, selecting **Modify** for Visual Studio 2017. If you can't see the **Modify** option, select **More**.

![Click Modify to find the Workloads tab.]({{assetRoot}}assets/setup/windows/visualstudioworkloads.png)

Once you have navigated to the **Workloads** tab:

- Select **.NET Core cross-platform development**.
- Select **Game development with Unity**:
  - Deselect any options in the **Summary** on the right that mention a Unity Editor (for example, Unity 2017.2 64-bit Editor or Unity 2018.1 64-bit Editor).
  - The SpatialOS GDK for Unity requires **Unity 2019.2.0**, which you already installed in step 1.
  - Make sure **Visual Studio Tools for Unity** is included (there should be a tick next to it).

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2019.2.0 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to the latest version of Visual Studio 2017.

**Using Rider?**

Once you have installed [Rider](https://www.jetbrains.com/rider/), install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience.

<%(/Expandable)%>

<%(#Expandable title="Setup for Mac")%>

**Step 1.** Install **Unity 2019.2.0**

Ensure you install this exact version as other Unity versions may not work with the GDK.

You can download **<a href="https://unity3d.com/get-unity/download/archive" data-track-link="Unity Download Link Clicked|product=Docs|platform=Mac|label=Mac" target="_blank"><strong>Unity 2019.2.0</strong></a>** from the Unity download archive.

Make sure you download the **Installer** version, and refer to the matrix below for which options to install :

| | | |
|---|---|---|
| **Option** | **Required?** | **Why?** |
| Linux Build Support | **Yes** | All server-workers in a cloud deployment run in a Linux environment. |
| Windows Build Support | No |  Only required if you wish to build clients compatible with Windows. |
| Android Build Support | No | Only required if you wish to build mobile clients for Android. |
| iOS Build Support | No | Only required if you wish to build mobile clients for iOS. |

**Step 2.** Install **<a href="https://dotnet.microsoft.com/download/dotnet-core/2.2" data-track-link=".NET Core Download Link Clicked|product=Docs|platform=Mac|label=Mac" target="_blank"><strong>.NET Core SDK (x64)</strong></a>**

- Verified with version `2.2.1xx`.

> **Note:** After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

**Step 3.** Run the **<a href="https://console.improbable.io/installer/download/stable/latest/mac" data-track-link="SpatialOS Installer Downloaded|product=Docs|platform=Mac|label=Mac" target="_blank">SpatialOS installer</a>**

- This installs the [SpatialOS CLI]({{urlRoot}}/reference/glossary#spatial-command-line-tool-cli) , the [SpatialOS Launcher]({{urlRoot}}/reference/glossary#launcher), and 32-bit and 64-bit Visual C++ Redistributables.

**Step 4.** Install a **code editor** if you don't have one already

- We recommend either [Visual Studio](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/). See _**Code editor set up**_, below.

**Step 5.** Install Git

The SpatialOS GDK for Unity source code is hosted on GitHub. You need to download and install [Git](https://git-scm.com/downloads) or [GitHub Desktop](https://desktop.github.com/) in order to clone the GDK repositories.

> If you do not want to use Git you can also download a .zip file containing the GDK repo from the [GDK GitHub Page](https://github.com/spatialos/gdk-for-unity). However, you will not be able to easily download updates to the GDK if you do not use Git to clone the repository.

**Step 6.** Restart your machine.

> If you do not restart your machine, you may experience errors when opening a GDK project.

#### Code editor set up

**Using Visual Studio?**

Once you have installed [Visual Studio](https://www.visualstudio.com/downloads/), within the Visual Studio Installer, select **.NET Core + ASP .NET Core**.

**Using Rider?**

Once you have installed [Rider](https://www.jetbrains.com/rider/), install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience.

<%(/Expandable)%>
