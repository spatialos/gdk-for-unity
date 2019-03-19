<%(TOC)%>
# Get the dependencies

To start using the SpatialOS GDK for Unity you'll need to create a SpatialOS account, install some dependencies and clone the GDK repository.

## Sign up for a SpatialOS account, or make sure you are logged in

If you have already signed up, make sure you are logged into [Improbable.io](https://improbable.io). If you are logged in, you should see your picture in the top right of this page; if you are not logged in, select __Sign in__ at the top of this page and follow the instructions.

If you have not signed up before, you can do this [here](https://improbable.io/get-spatialos).
<br/>
<br/>

## Set up your machine

<%(#Expandable title="Setup for Windows")%>

**Step 1.** Install **<a href="https://unity3d.com/get-unity/download/archive" data-track-link="Unity Download Link Clicked|product=Docs|platform=Win|label=Win" target="_blank"><strong>Unity 2018.3.5</strong></a>**

Ensure you install this exact version as other Unity versions may not work with the GDK.

- Make sure you download the **Installer** version, and select the following components during installation:
  - **Linux Build Support**
  - **Mac Build Support**

> **Note:**
> Even though you are developing on a Windows PC, you need:<br/>
> **Linux Build Support** because all server-workers in a cloud deployment run in a Linux environment. <br/>
> **Mac Build Support** if you want to share your game with end-users on a Mac.<br/>
> Unity gives you Windows build support by default.

**Step 2.** Install **<a href="https://www.microsoft.com/net/download/dotnet-core/2.1" data-track-link=".NET Core Download Link Clicked|product=Docs|platform=Win|label=Win" target="_blank"><strong>.NET Core SDK (x64)</strong></a>**

- Verified with versions `2.1.3xx` and `2.1.4xx`

> **Note:** After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

**Step 3.** Run the **<a href="https://console.improbable.io/installer/download/stable/latest/win" data-track-link="SpatialOS Installer Downloaded|product=Docs|platform=Win|label=Win" target="_blank">SpatialOS Installer</a>**

- This installs the [SpatialOS CLI]({{urlRoot}}/content/glossary#spatial-command-line-tool-cli) , the [SpatialOS Launcher]({{urlRoot}}/content/glossary#launcher), and 32-bit and 64-bit Visual C++ Redistributables

**Step 4.** Install a **code editor** if you don't have one already

- We recommend either [Visual Studio 2017](https://www.visualstudio.com/downloads/) or [Rider](https://www.jetbrains.com/rider/). See _**Code editor set up**_, below.

**Step 5.** Install Git

The SpatialOS GDK for Unity source code is hosted on GitHub. You need to download and install [Git](https://git-scm.com/downloads) or [GitHub Desktop](https://desktop.github.com/) in order to clone the GDK repositories. 

> If you do not want to use Git you can also download a .zip file containing the GDK repo from the [GDK GitHub Page](https://github.com/spatialos/gdk-for-unity). However, you will not be able to easily download updates to the GDK if you do not use Git to clone the repository. 

**Step 6.** Restart your machine.

> If you do not restart your machine, you may experience errors when opening a GDK project.

#### Code editor set up

 **Using Visual Studio?**

As you install [Visual Studio](https://www.visualstudio.com/downloads/), navigate to the **Workloads** tab in the Installer. If you already have Visual Studio installed, you can find this by selecting the **More** option for *Visual Studio Build Tools* in the Visual Studio Installer, and selecting **Modify** from the drop-down menu.

![Click Modify to find the Workloads tab.]({{assetRoot}}assets/setup/windows/visualstudioworkloads.png)

Once you have navigated to the **Workloads** tab:

- Select **.NET Core cross-platform development**.
- After selecting **Game development with Unity**:
  - Deselect any options in the **Summary** on the right that mention a Unity Editor (for example, Unity 2017.2 64-bit Editor or Unity 2018.1 64-bit Editor).
  - The SpatialOS GDK for Unity requires **Unity 2018.3.5**, which should already be installed if you have followed the setup guide correctly.
  - Make sure **Visual Studio Tools for Unity** is included (there should be a tick next to it).

> **Warning**: Older versions of Visual Studio 2017 have been known to cause some issues with Unity 2018.3.5 - the issues are projects loading and unloading frequently, and Intellisense breaking. If you do experience these issues, try updating to a newer version of Visual Studio 2017.

**Using Rider?**

If you prefer to use [Rider](https://www.jetbrains.com/rider/), install the [.NET Framework 4.7.1 Developer Pack](https://dotnet.microsoft.com/download/visual-studio-sdks).

Additionally, install the [**Unity Support** plugin](https://github.com/JetBrains/resharper-unity) for a better experience.

<%(/Expandable)%>

<%(#Expandable title="Setup for Mac")%>

**Step 1.** Install **<a href="https://unity3d.com/get-unity/download/archive" data-track-link="Unity Download Link Clicked|product=Docs|platform=Mac|label=Mac" target="_blank"><strong>Unity 2018.3.5</strong></a>**

Ensure you install this exact version as other Unity versions may not work with the GDK.

- Make sure to download the **Installer** version, and select the following components during installation:
  - **Linux Build Support**
  - **Windows Build Support**

> **Note:**
> Even though you are developing on a Mac, you need:<br/>
> **Linux Build Support** because all server-workers in a cloud deployment run in a Linux environment. <br/>
> **Windows Build Support** if you want to share your game with end-users on a Windows PC.<br/>
> Unity gives you Mac build support by default.

**Step 2.** Install **<a href="https://www.microsoft.com/net/download/dotnet-core/2.1" data-track-link=".NET Core Download Link Clicked|product=Docs|platform=Mac|label=Mac" target="_blank"><strong>.NET Core SDK (x64)</strong></a>**

- Verified with versions `2.1.3xx` and `2.1.4xx`

> **Note:** After installing the .NET Core SDK, you should restart any Unity and Unity Hub processes. This will prevent errors where Unity cannot find the `dotnet` executable.

**Step 3.** Run the **<a href="https://console.improbable.io/installer/download/stable/latest/mac" data-track-link="SpatialOS Installer Downloaded|product=Docs|platform=Mac|label=Mac" target="_blank">SpatialOS installer</a>**

- This installs the [SpatialOS CLI]({{urlRoot}}/content/glossary#spatial-command-line-tool-cli) , the [SpatialOS Launcher]({{urlRoot}}/content/glossary#launcher), and 32-bit and 64-bit Visual C++ Redistributables.

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

#### Make sure you have chosen the right build support

- You need **Linux** build support. This is because server-workers in a cloud deployment run in a Linux environment.<br/>
- You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
- You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
- Unity gives you build support for your development machine (Windows or Mac) by default.

#### Android and iOS support

Mobile support is in pre-alpha. If you are developing a game for Android or iOS, refer to our GDK for Unity [mobile support documentation]({{urlRoot}}/content//mobile/overview)

#### Need some help?

If you need help using the GDK, come and talk to us about the software and the documentation via:

- **The SpatialOS forums** - Visit the [support section](https://forums.improbable.io/new-topic?category=Support&tags=unity-gdk) in our forums and use the unity-gdk tag.
- **Discord** - Find us in the [#unity channel](https://discord.gg/SCZTCYm). You may need to grab Discord [here](https://discordapp.com/).
- **Github issues** - Create an [issue](https://github.com/spatialos/gdk-for-unity/issues) in this repository.
  <br/>

### Clone the SpatialOS GDK for Unity repository

To run the GDK for Unity, you need to download the source code. To do this, you need to clone the GDK repository.

#### Using a terminal:

Clone the SpatialOS GDK for Unity repository using one of the following terminal commands:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |

#### Using GitHub Desktop

To clone the repository using GitHub desktop: navigate to **File > Clone Repository**, select the URL tab and enter `https://github.com/spatialos/gdk-for-unity` into the URL or username/repository field, then select Clone. 

The GDK repository is a SpatialOS project called `gdk-for-unity`. It contains:

  * a Unity project at `gdk-for-unity/workers/unity`, which you need to open in your Unity editor in order to use the GDK
  * SpatialOS features, such as the schema and snapshot files

